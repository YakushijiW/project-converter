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
            LoadProfiles();
            LoadUserPresets();
            LoadUserSettings();
            WireDragDrop();
        }

        private void LoadProfiles()
        {
            try
            {
                string jsonPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "project_profiles.json");
                string jsonString = File.ReadAllText(jsonPath);
                var loaded = JsonSerializer.Deserialize<Dictionary<string, ProjectProfile>>(jsonString, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                _profiles = loaded ?? new Dictionary<string, ProjectProfile>();

                comboProfiles.Items.Clear();
                comboProfiles.Items.AddRange(_profiles.Keys.ToArray());
                comboProfiles.Items.Add("自定义...");
                if (comboProfiles.Items.Count > 0)
                {
                    comboProfiles.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"加载预设文件失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                _profiles = new Dictionary<string, ProjectProfile>();
            }
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
                btnDeletePreset.Visible = false;
                return;
            }

            // 检查是否为用户自定义预设
            bool isUserPreset = _userPresets.ContainsKey(selected);
            btnAddPreset.Visible = false;
            btnDeletePreset.Visible = isUserPreset;

            if (_profiles.TryGetValue(selected, out var profile))
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
                    // 合并到下拉框
                    foreach (var key in _userPresets.Keys)
                    {
                        if (!_profiles.ContainsKey(key))
                        {
                            _profiles[key] = _userPresets[key];
                        }
                    }
                    // 重新填充下拉
                    comboProfiles.Items.Clear();
                    comboProfiles.Items.AddRange(_profiles.Keys.ToArray());
                    comboProfiles.Items.Add("自定义...");
                }
            }
            catch
            {
                // ignore
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
        /// 删除预设按钮点击事件处理
        /// </summary>
        private void btnDeletePreset_Click(object sender, EventArgs e)
        {
            var selected = comboProfiles.SelectedItem as string;
            if (string.IsNullOrWhiteSpace(selected) || !_userPresets.ContainsKey(selected))
            {
                return;
            }

            var result = MessageBox.Show($"确定要删除预设 '{selected}' 吗？", "确认删除", 
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            
            if (result == DialogResult.Yes)
            {
                // 从字典中移除
                _userPresets.Remove(selected);
                _profiles.Remove(selected);
                
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
                    FileName = "https://github.com/your/repo", // TODO: 替换为真实链接
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