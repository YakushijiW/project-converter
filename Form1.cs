using project_converter;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;

namespace project_converter
{
    public partial class Form1 : Form
    {
        private Dictionary<string, ProjectProfile> _profiles = new();
        private const string UserPresetsFileName = "user_presets.json";
        private const string UserSettingsFileName = "user_settings.json";
        private Dictionary<string, ProjectProfile> _userPresets = new();
        private UserSettings _settings = new();
        private readonly List<string> _manualFiles = new();

        public Form1()
        {
            InitializeComponent();
            LoadProfiles(); // LoadProfiles内部会调用LoadUserPresets
            LoadUserSettings();
            WireDragDrop();
        }

        private void LoadProfiles()
        {
            try
            {
                string jsonPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "project_profiles.json");

                // 如果内置配置文件不存在，创建它
                if (!File.Exists(jsonPath))
                {
                    CreateDefaultProfiles(jsonPath);
                    MessageBox.Show("已创建默认预设配置文件，您可以根据需要修改。", "信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                // 1. 加载内置预设
                string jsonString = File.ReadAllText(jsonPath);
                var loaded = JsonSerializer.Deserialize<Dictionary<string, ProjectProfile>>(jsonString,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                if (loaded == null || loaded.Count == 0)
                {
                    throw new Exception("预设配置文件为空或格式错误");
                }
                _profiles = loaded;

                // 加载用户预设
                LoadUserPresets();

                // 重新填充下拉框
                RefreshProfileComboBox();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"加载预设文件失败: {ex.Message}\n将创建并使用默认预设。", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                // 尝试创建默认预设并重试加载
                try
                {
                    string jsonPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "project_profiles.json");
                    // 如果文件存在，先删除，确保是全新的
                    if (File.Exists(jsonPath)) File.Delete(jsonPath);
                    CreateDefaultProfiles(jsonPath);

                    // 再次加载内置和用户预设
                    string jsonString = File.ReadAllText(jsonPath);
                    _profiles = JsonSerializer.Deserialize<Dictionary<string, ProjectProfile>>(jsonString, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new Dictionary<string, ProjectProfile>();
                    LoadUserPresets();
                    RefreshProfileComboBox();
                }
                catch (Exception innerEx)
                {
                    MessageBox.Show($"创建默认预设也失败: {innerEx.Message}\n应用程序将继续运行，但预设功能可能不可用。", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    SetupMinimalDefaultProfiles(); // 使用最小配置作为最后手段
                }
            }
        }

        /// <summary>
        /// 刷新预设下拉框
        /// </summary>
        private void RefreshProfileComboBox()
        {
            // 创建一个临时的字典来合并预设，以 _profiles (内置)为基础
            var combinedProfiles = new Dictionary<string, ProjectProfile>(_profiles);

            // 用用户预设覆盖或添加条目，用户预设优先级更高
            foreach (var userPreset in _userPresets)
            {
                combinedProfiles[userPreset.Key] = userPreset.Value;
            }

            comboProfiles.Items.Clear();

            // 遍历合并后的最终预设列表
            foreach (var profileEntry in combinedProfiles)
            {
                var profileName = profileEntry.Key;
                var profileData = profileEntry.Value;

                // 检查这是否是一个被用户标记为"删除"的预设（即空预设）
                if (profileData.Extensions.Count == 0 && profileData.ExcludeDirs.Count == 0)
                {
                    continue; // 如果是删除标记，则跳过，不添加到下拉框
                }

                comboProfiles.Items.Add(profileName);
            }

            comboProfiles.Items.Add("自定义...");

            // 设置默认选中项
            if (comboProfiles.Items.Count > 1) // 至少要有一个预设 + "自定义..."
            {
                // 尝试选中上次使用的预设
                if (!string.IsNullOrEmpty(_settings?.LastSelectedPreset) && comboProfiles.Items.Contains(_settings.LastSelectedPreset))
                {
                    comboProfiles.SelectedItem = _settings.LastSelectedPreset;
                }
                else
                {
                    // 默认选中第一个
                    comboProfiles.SelectedIndex = 0;
                }
            }
            else
            {
                comboProfiles.SelectedItem = "自定义...";
            }
        }

        /// <summary>
        /// 创建默认的预设配置文件
        /// </summary>
        private void CreateDefaultProfiles(string filePath)
        {
            try
            {
                var defaultProfiles = new Dictionary<string, ProjectProfile>
                {
                    ["Unity 项目"] = new ProjectProfile
                    {
                        Name = "Unity 项目",
                        Extensions = new List<string> { ".cs", ".lua", ".shader", ".json", ".txt" },
                        ExcludeDirs = new List<string> { "Library", "Temp", "Logs", "Obj", ".vs", ".git", "Build", "WebGLBuilds" }
                    },
                    ["Unreal Engine 项目"] = new ProjectProfile // 新增
                    {
                        Name = "Unreal Engine 项目",
                        Extensions = new List<string> { ".h", ".cpp", ".ini", ".uproject", ".uplugin" },
                        ExcludeDirs = new List<string> { "Binaries", "DerivedDataCache", "Intermediate", "Saved", ".vs", ".vscode", ".git" }
                    },
                    ["Godot Engine 项目"] = new ProjectProfile // 新增
                    {
                        Name = "Godot Engine 项目",
                        Extensions = new List<string> { ".gd", ".cs", ".tscn", ".tres", "project.godot" },
                        ExcludeDirs = new List<string> { ".godot", ".vs", ".vscode", ".git", "export_presets.cfg" }
                    },
                    ["微信小程序"] = new ProjectProfile
                    {
                        Name = "微信小程序",
                        Extensions = new List<string> { ".wxml", ".js", ".wxss", ".json", ".wxs", ".ts" },
                        ExcludeDirs = new List<string> { "node_modules", "miniprogram_npm", ".git", "dist", "build" }
                    },
                    ["Python 项目"] = new ProjectProfile
                    {
                        Name = "Python 项目",
                        Extensions = new List<string> { ".py", ".pyw", ".txt", ".yml", ".yaml", ".cfg", ".ini" },
                        ExcludeDirs = new List<string> { "venv", ".venv", "__pycache__", ".git", ".idea", "dist", "build", ".pytest_cache" }
                    },
                    ["Winform 项目"] = new ProjectProfile
                    {
                        Name = "Winform 项目",
                        Extensions = new List<string> { ".cs", ".sln", ".json" },
                        ExcludeDirs = new List<string> { "bin", "obj", ".git", ".vs", ".vscode" }
                    },
                    ["Java 项目"] = new ProjectProfile
                    {
                        Name = "Java 项目",
                        Extensions = new List<string> { ".java", ".xml", ".properties", ".txt", ".yml", ".yaml", ".gradle" },
                        ExcludeDirs = new List<string> { "target", "build", ".git", ".idea", ".gradle", "out" }
                    },
                    ["PHP 项目"] = new ProjectProfile
                    {
                        Name = "PHP 项目",
                        Extensions = new List<string> { ".php", ".json", ".xml", ".txt", ".yml", ".yaml", ".twig" },
                        ExcludeDirs = new List<string> { "vendor", ".git", ".idea", "storage/logs", "bootstrap/cache" }
                    },
                    ["Go 项目"] = new ProjectProfile
                    {
                        Name = "Go 项目",
                        Extensions = new List<string> { ".go", ".mod", ".sum", ".txt", ".yml", ".yaml" },
                        ExcludeDirs = new List<string> { "vendor", ".git", ".idea", "bin", "pkg" }
                    },
                    ["Web 前端项目"] = new ProjectProfile
                    {
                        Name = "Web 前端项目",
                        Extensions = new List<string> { ".html", ".css", ".js", ".ts", ".vue", ".jsx", ".tsx", ".json", ".scss", ".sass", ".less" },
                        ExcludeDirs = new List<string> { "node_modules", "dist", ".git", ".idea", "build", ".next", ".nuxt" }
                    },
                    ["Android 项目"] = new ProjectProfile
                    {
                        Name = "Android 项目",
                        Extensions = new List<string> { ".wxml", ".js", ".wxss", ".json", ".wxs", ".ts" },
                        ExcludeDirs = new List<string> { "build", ".gradle", ".git", ".idea", "app/build", "gradle" }
                    },
                    ["iOS 项目"] = new ProjectProfile
                    {
                        Name = "iOS 项目",
                        Extensions = new List<string> { ".swift", ".m", ".h", ".plist", ".storyboard", ".xib", ".txt" },
                        ExcludeDirs = new List<string> { "build", "DerivedData", ".git", ".idea", "Pods", "xcuserdata" }
                    },
                    // ... 其他预设配置，按照上面优化的JSON格式继续添加
                };

                var json = JsonSerializer.Serialize(defaultProfiles, new JsonSerializerOptions { WriteIndented = true, Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping });
                File.WriteAllText(filePath, json);
            }
            catch (Exception ex)
            {
                throw new Exception($"创建默认预设文件失败: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// 设置最小的默认配置（当所有方法都失败时使用）
        /// </summary>
        private void SetupMinimalDefaultProfiles()
        {
            _profiles = new Dictionary<string, ProjectProfile>
            {
                ["通用项目"] = new ProjectProfile
                {
                    Name = "通用项目",
                    Extensions = new List<string> { ".cs", ".js", ".html", ".css", ".json", ".xml", ".txt", ".md" },
                    ExcludeDirs = new List<string> { "bin", "obj", "node_modules", ".git", ".vs" }
                }
            };

            RefreshProfileComboBox();
        }

        private void comboProfiles_SelectedIndexChanged(object sender, EventArgs e)
        {
            var selected = comboProfiles.SelectedItem as string;
            if (string.IsNullOrWhiteSpace(selected))
            {
                return;
            }

            if (selected == "自定义...")
            {
                txtIncludeExt.Clear();
                txtExcludeDirs.Clear();
                btnAddPreset.Visible = true;
                btnEditPreset.Visible = false;
                btnDeletePreset.Visible = false;
                return;
            }

            // 所有预设都可以修改和删除（除了"自定义..."选项）
            btnAddPreset.Visible = false;
            btnEditPreset.Visible = true;
            btnDeletePreset.Visible = true;

            // 优先从用户预设中查找，然后从内置预设中查找
            ProjectProfile? profile = null;
            if (_userPresets.TryGetValue(selected, out profile) || _profiles.TryGetValue(selected, out profile))
            {
                txtIncludeExt.Text = string.Join(", ", profile.Extensions);
                txtExcludeDirs.Text = string.Join(", ", profile.ExcludeDirs);
            }
        }

        // --- Browse Buttons ---
        private void btnBrowseProject_Click(object sender, EventArgs e)
        {
            using (var dialog = new FolderBrowserDialog())
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    txtProjectPath.Text = dialog.SelectedPath;
                }
            }
        }

        private void btnSaveFile_Click(object sender, EventArgs e)
        {
            using (var dialog = new FolderBrowserDialog())
            {
                dialog.Description = "选择输出文件夹";
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    txtOutputFile.Text = dialog.SelectedPath;
                }
            }
        }

        private void btnBrowseFile_Click(object sender, EventArgs e)
        {
            using (var dialog = new OpenFileDialog())
            {
                dialog.Filter = "Text Files|*.txt";
                dialog.Title = "选择要解包的文件";
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    txtInputFile.Text = dialog.FileName;
                }
            }
        }

        private void btnBrowseExtract_Click(object sender, EventArgs e)
        {
            using (var dialog = new FolderBrowserDialog())
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    txtExtractPath.Text = dialog.SelectedPath;
                }
            }
        }

        // --- Core Logic Buttons ---
        private async void btnPack_Click(object sender, EventArgs e)
        {
            // 优先按文件列表打包，否则按项目目录打包
            bool hasManualFiles = _manualFiles.Count > 0;
            if (!hasManualFiles && (string.IsNullOrWhiteSpace(txtProjectPath.Text) || string.IsNullOrWhiteSpace(txtOutputFile.Text)))
            {
                MessageBox.Show("请选择项目文件夹和输出文件夹路径。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var extensions = txtIncludeExt.Text.Split(new[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries).ToList();
            var excludes = txtExcludeDirs.Text.Split(new[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries).ToList();

            SetUIEnabled(false);
            var progress = new Progress<string>(status => UpdateStatus(status));
            progressBar.Value = 0;

            try
            {
                var packer = new ProjectPacker();
                
                // 生成输出文件的完整路径
                string outputFileName = string.IsNullOrWhiteSpace(txtDefaultOutputName.Text) ? "encoded_project.txt" : txtDefaultOutputName.Text.Trim();
                string outputFilePath = Path.Combine(txtOutputFile.Text, outputFileName);
                
                if (hasManualFiles)
                {
                    if (string.IsNullOrWhiteSpace(txtOutputFile.Text))
                    {
                        MessageBox.Show("请先设置输出文件夹路径。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    await packer.PackFilesAsync(_manualFiles, outputFilePath, progress);
                }
                else
                {
                    await packer.PackProjectAsync(txtProjectPath.Text, outputFilePath, extensions, excludes, progress);
                }
                progressBar.Value = 100;
                MessageBox.Show($"项目打包成功！\n输出文件：{outputFilePath}", "完成", MessageBoxButtons.OK, MessageBoxIcon.Information);
                
                // 自动保存当前设置
                SaveCurrentPackSettings();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"打包失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                lblStatus.Text = "状态：打包失败";
            }
            finally
            {
                SetUIEnabled(true);
            }
        }

        private async void btnUnpack_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtInputFile.Text) || string.IsNullOrWhiteSpace(txtExtractPath.Text))
            {
                MessageBox.Show("请选择要解包的文件和目标文件夹。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            SetUIEnabled(false);
            var progress = new Progress<string>(status => UpdateStatus(status));
            progressBar.Value = 0;

            try
            {
                var packer = new ProjectPacker();
                await packer.UnpackProjectAsync(txtInputFile.Text, txtExtractPath.Text, progress);
                progressBar.Value = 100;
                MessageBox.Show("项目解包成功！", "完成", MessageBoxButtons.OK, MessageBoxIcon.Information);
                
                // 自动保存当前设置
                SaveCurrentUnpackSettings();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"解包失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                lblStatus.Text = "状态：解包失败";
            }
            finally
            {
                SetUIEnabled(true);
            }
        }

        // --- UI Helper Methods ---
        private void SetUIEnabled(bool isEnabled)
        {
            tabControlMain.Enabled = isEnabled;
            // You can selectively disable buttons if you prefer
            // btnPack.Enabled = isEnabled;
            // btnUnpack.Enabled = isEnabled;
        }

        private void UpdateStatus(string status)
        {
            // 安全地从后台线程更新UI
            if (InvokeRequired)
            {
                Invoke(new Action<string>(UpdateStatus), status);
                return;
            }
            lblStatus.Text = $"状态：{status}";
            // 简单的进度条更新逻辑
            if (status.Contains("打包中") || status.Contains("解包中"))
            {
                // 解析 " (1/100):" 这样的格式来更新进度条
                var parts = status.Split('(', ')');
                if (parts.Length > 1)
                {
                    var progressParts = parts[1].Split('/');
                    if (progressParts.Length == 2 && int.TryParse(progressParts[0], out int current) && int.TryParse(progressParts[1], out int total))
                    {
                        progressBar.Value = (int)((double)current / total * 100);
                    }
                }
            }
        }

        // --- User Presets Persistence ---
        private void LoadUserPresets()
        {
            try
            {
                string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, UserPresetsFileName);
                if (File.Exists(path))
                {
                    var json = File.ReadAllText(path);
                    var dict = JsonSerializer.Deserialize<Dictionary<string, ProjectProfile>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                    _userPresets = dict ?? new Dictionary<string, ProjectProfile>();
                }
                else
                {
                    // 如果文件不存在，确保_userPresets是一个空字典而不是null
                    _userPresets = new Dictionary<string, ProjectProfile>();
                }
            }
            catch
            {
                // 加载失败时也确保是一个空字典，防止后续代码出错
                _userPresets = new();
            }
        }

        private void SaveUserPresets()
        {
            try
            {
                string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, UserPresetsFileName);
                var json = JsonSerializer.Serialize(_userPresets, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(path, json);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"保存用户预设失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 添加预设按钮点击事件处理
        /// </summary>
        private void btnAddPreset_Click(object sender, EventArgs e)
        {
            using var dlg = new PresetDialog();
            if (dlg.ShowDialog(this) == DialogResult.OK)
            {
                string presetName = dlg.PresetName;
                if (string.IsNullOrWhiteSpace(presetName)) return;

                // 从当前打包页面获取设置信息
                var profile = new ProjectProfile
                {
                    Name = presetName,
                    Extensions = txtIncludeExt.Text.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries).ToList(),
                    ExcludeDirs = txtExcludeDirs.Text.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries).ToList()
                };

                _userPresets[profile.Name] = profile;
                _profiles[profile.Name] = profile;
                SaveUserPresets();

                // 更新下拉并选中
                comboProfiles.Items.Insert(0, profile.Name);
                comboProfiles.SelectedItem = profile.Name;
            }
        }

        /// <summary>
        /// 修改预设按钮点击事件处理
        /// </summary>
        private void btnEditPreset_Click(object sender, EventArgs e)
        {
            var selected = comboProfiles.SelectedItem as string;
            if (string.IsNullOrWhiteSpace(selected))
            {
                return;
            }

            // 获取当前预设的配置
            ProjectProfile? currentProfile = null;
            if (!_userPresets.TryGetValue(selected, out currentProfile) && !_profiles.TryGetValue(selected, out currentProfile))
            {
                return;
            }

            using var dlg = new PresetDialog();
            dlg.PresetName = selected; // 设置当前预设名称
            if (dlg.ShowDialog(this) == DialogResult.OK)
            {
                string newPresetName = dlg.PresetName;
                if (string.IsNullOrWhiteSpace(newPresetName)) return;

                // 从当前UI获取设置信息
                var newProfile = new ProjectProfile
                {
                    Name = newPresetName,
                    Extensions = txtIncludeExt.Text.Trim().Split(',', StringSplitOptions.RemoveEmptyEntries)
                        .Select(s => s.Trim()).ToList(),
                    ExcludeDirs = txtExcludeDirs.Text.Trim().Split(',', StringSplitOptions.RemoveEmptyEntries)
                        .Select(s => s.Trim()).ToList()
                };

                // 如果预设名称改变了，需要删除旧的预设
                if (newPresetName != selected)
                {
                    // 删除旧预设
                    if (_userPresets.ContainsKey(selected))
                    {
                        _userPresets.Remove(selected);
                    }
                    else if (_profiles.ContainsKey(selected))
                    {
                        // 如果是内置预设，标记为删除
                        _userPresets[selected] = new ProjectProfile { Extensions = new List<string>(), ExcludeDirs = new List<string>() };
                    }
                    
                    // 从下拉框中移除旧预设
                    comboProfiles.Items.Remove(selected);
                }

                // 保存新预设
                _userPresets[newPresetName] = newProfile;
                SaveUserPresets();

                // 更新下拉框
                if (newPresetName != selected)
                {
                    comboProfiles.Items.Insert(0, newPresetName);
                }
                comboProfiles.SelectedItem = newPresetName;
            }
        }

        /// <summary>
        /// 删除预设按钮点击事件处理
        /// </summary>
        private void btnDeletePreset_Click(object sender, EventArgs e)
        {
            var selected = comboProfiles.SelectedItem as string;
            if (string.IsNullOrWhiteSpace(selected))
            {
                return;
            }

            var result = MessageBox.Show($"确定要删除预设 '{selected}' 吗？", "确认删除", 
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            
            if (result == DialogResult.Yes)
            {
                // 如果是内置预设，将其添加到用户预设中作为"已删除"标记
                if (_profiles.ContainsKey(selected) && !_userPresets.ContainsKey(selected))
                {
                    // 标记内置预设为已删除（通过在用户预设中添加空预设）
                    _userPresets[selected] = new ProjectProfile { Extensions = new List<string>(), ExcludeDirs = new List<string>() };
                }
                else if (_userPresets.ContainsKey(selected))
                {
                    // 删除用户自定义预设
                    _userPresets.Remove(selected);
                }
                
                // 从下拉框中移除
                comboProfiles.Items.Remove(selected);
                
                // 选择第一个项目或自定义
                if (comboProfiles.Items.Count > 0)
                {
                    comboProfiles.SelectedIndex = 0;
                }
                
                // 保存更改
                SaveUserPresets();
            }
        }

        /// <summary>
        /// 加载用户设置
        /// </summary>
        private void LoadUserSettings()
        {
            try
            {
                string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, UserSettingsFileName);
                if (File.Exists(path))
                {
                    var json = File.ReadAllText(path);
                    _settings = JsonSerializer.Deserialize<UserSettings>(json) ?? new UserSettings();
                }
                // 应用到UI
                txtDefaultOutputName.Text = _settings.OutputFileName ?? "encoded_project.txt";

                if (string.IsNullOrWhiteSpace(txtOutputFile.Text))
                {
                    txtOutputFile.Text = txtDefaultOutputName.Text;
                }
                
                // 使用上次的项目路径和解包路径
                if (string.IsNullOrWhiteSpace(txtProjectPath.Text) && !string.IsNullOrWhiteSpace(_settings.LastProjectPath))
                {
                    txtProjectPath.Text = _settings.LastProjectPath;
                }
                if (string.IsNullOrWhiteSpace(txtExtractPath.Text) && !string.IsNullOrWhiteSpace(_settings.LastExtractPath))
                {
                    txtExtractPath.Text = _settings.LastExtractPath;
                }
                
                // 加载打包页面的上次设置
                if (!string.IsNullOrWhiteSpace(_settings.LastPackProjectPath))
                {
                    txtProjectPath.Text = _settings.LastPackProjectPath;
                }
                if (!string.IsNullOrWhiteSpace(_settings.LastPackOutputPath))
                {
                    txtOutputFile.Text = _settings.LastPackOutputPath;
                }
                if (!string.IsNullOrWhiteSpace(_settings.LastSelectedPreset))
                {
                    // 尝试选中上次使用的预设
                    for (int i = 0; i < comboProfiles.Items.Count; i++)
                    {
                        if (comboProfiles.Items[i].ToString() == _settings.LastSelectedPreset)
                        {
                            comboProfiles.SelectedIndex = i;
                            break;
                        }
                    }
                }
                
                // 加载解包页面的上次设置
                if (!string.IsNullOrWhiteSpace(_settings.LastUnpackInputFile))
                {
                    txtInputFile.Text = _settings.LastUnpackInputFile;
                }
                if (!string.IsNullOrWhiteSpace(_settings.LastUnpackOutputPath))
                {
                    txtExtractPath.Text = _settings.LastUnpackOutputPath;
                }
            }
            catch
            {
                // ignore
            }
        }

        /// <summary>
        /// 保存用户设置
        /// </summary>
        private void SaveUserSettings()
        {
            try
            {
                _settings.OutputFileName = string.IsNullOrWhiteSpace(txtDefaultOutputName.Text) ? "encoded_project.txt" : txtDefaultOutputName.Text.Trim();
                
                // 保存当前使用的路径作为上次使用的路径
                _settings.LastProjectPath = string.IsNullOrWhiteSpace(txtProjectPath.Text) ? null : txtProjectPath.Text.Trim();
                _settings.LastExtractPath = string.IsNullOrWhiteSpace(txtExtractPath.Text) ? null : txtExtractPath.Text.Trim();

                // 保存打包页面的设置
                _settings.LastPackProjectPath = string.IsNullOrWhiteSpace(txtProjectPath.Text) ? null : txtProjectPath.Text.Trim();
                _settings.LastPackOutputPath = string.IsNullOrWhiteSpace(txtOutputFile.Text) ? null : txtOutputFile.Text.Trim();
                _settings.LastSelectedPreset = comboProfiles.SelectedItem?.ToString();
                
                // 保存解包页面的设置
                _settings.LastUnpackInputFile = string.IsNullOrWhiteSpace(txtInputFile.Text) ? null : txtInputFile.Text.Trim();
                _settings.LastUnpackOutputPath = string.IsNullOrWhiteSpace(txtExtractPath.Text) ? null : txtExtractPath.Text.Trim();

                string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, UserSettingsFileName);
                var json = JsonSerializer.Serialize(_settings, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(path, json);
                MessageBox.Show("设置已保存", "信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"保存设置失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 保存设置按钮点击事件
        /// </summary>
        private void btnSaveSettings_Click(object sender, EventArgs e)
        {
            SaveUserSettings();
        }

        /// <summary>
        /// 保存当前打包页面的设置
        /// </summary>
        private void SaveCurrentPackSettings()
        {
            try
            {
                _settings.LastPackProjectPath = string.IsNullOrWhiteSpace(txtProjectPath.Text) ? null : txtProjectPath.Text.Trim();
                _settings.LastPackOutputPath = string.IsNullOrWhiteSpace(txtOutputFile.Text) ? null : txtOutputFile.Text.Trim();
                _settings.LastSelectedPreset = comboProfiles.SelectedItem?.ToString();
                
                string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, UserSettingsFileName);
                var json = JsonSerializer.Serialize(_settings, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(path, json);
            }
            catch
            {
                // 静默失败，不影响用户体验
            }
        }

        /// <summary>
        /// 保存当前解包页面的设置
        /// </summary>
        private void SaveCurrentUnpackSettings()
        {
            try
            {
                _settings.LastUnpackInputFile = string.IsNullOrWhiteSpace(txtInputFile.Text) ? null : txtInputFile.Text.Trim();
                _settings.LastUnpackOutputPath = string.IsNullOrWhiteSpace(txtExtractPath.Text) ? null : txtExtractPath.Text.Trim();
                
                string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, UserSettingsFileName);
                var json = JsonSerializer.Serialize(_settings, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(path, json);
            }
            catch
            {
                // 静默失败，不影响用户体验
            }
        }

        private void linkGithub_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = "https://github.com/YakushijiW/project-converter",
                    UseShellExecute = true
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        // --- Drag & Drop support ---
        private void WireDragDrop()
        {
            // 文本框
            txtProjectPath.AllowDrop = true;
            txtProjectPath.DragEnter += (s, e) => OnDragEnterSetEffect(e, true);
            txtProjectPath.DragDrop += (s, e) => OnTextboxDropSetPath(txtProjectPath, e, expectDirectory: true);

            txtOutputFile.AllowDrop = true;
            txtOutputFile.DragEnter += (s, e) => OnDragEnterSetEffect(e, false);
            txtOutputFile.DragDrop += (s, e) => OnTextboxDropSetPath(txtOutputFile, e, expectDirectory: true);

            txtInputFile.AllowDrop = true;
            txtInputFile.DragEnter += (s, e) => OnDragEnterSetEffect(e, false);
            txtInputFile.DragDrop += (s, e) => OnTextboxDropSetPath(txtInputFile, e, expectDirectory: false);

            txtExtractPath.AllowDrop = true;
            txtExtractPath.DragEnter += (s, e) => OnDragEnterSetEffect(e, true);
            txtExtractPath.DragDrop += (s, e) => OnTextboxDropSetPath(txtExtractPath, e, expectDirectory: true);
        }

        private static void OnDragEnterSetEffect(DragEventArgs e, bool allowDir)
        {
            if (e.Data != null && e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Copy;
            }
        }

        private static void OnTextboxDropSetPath(TextBox box, DragEventArgs e, bool expectDirectory)
        {
            if (e.Data == null) return;
            var paths = e.Data.GetData(DataFormats.FileDrop) as string[];
            if (paths == null || paths.Length == 0) return;
            var path = paths[0];
            if (expectDirectory && Directory.Exists(path) || !expectDirectory && File.Exists(path))
            {
                box.Text = path;
            }
        }

        private void lstFiles_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data != null && e.Data.GetDataPresent(DataFormats.FileDrop)) e.Effect = DragDropEffects.Copy;
        }

        private void lstFiles_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data == null) return;
            var paths = e.Data.GetData(DataFormats.FileDrop) as string[];
            if (paths == null) return;
            AddFiles(paths);
        }

        private void btnAddFiles_Click(object sender, EventArgs e)
        {
            using var ofd = new OpenFileDialog
            {
                Filter = "所有文件|*.*",
                Multiselect = true
            };
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                AddFiles(ofd.FileNames);
            }
        }

        private void AddFiles(IEnumerable<string> files)
        {
            foreach (var f in files)
            {
                if (File.Exists(f) && !_manualFiles.Contains(f, StringComparer.OrdinalIgnoreCase))
                {
                    _manualFiles.Add(f);
                    lstFiles.Items.Add(f);
                }
            }
        }

        private void btnClearFiles_Click(object sender, EventArgs e)
        {
            _manualFiles.Clear();
            lstFiles.Items.Clear();
        }

        private void label9_Click(object sender, EventArgs e)
        {

        }
    }
}