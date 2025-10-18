using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace project_converter
{
    public class ProjectPacker
    {
        private const string FileSeparator = "\n---[NextFile]---\n";
        private const string HeaderFormat = "[PATH]:{0}\n[HASH]:{1}\n[CONTENT]:\n";

        /// <summary>
        /// 计算文件的SHA256哈希值
        /// </summary>
        private static string GetSha256(string filePath)
        {
            try
            {
                using var sha256 = SHA256.Create();
                using var stream = File.OpenRead(filePath);
                var hash = sha256.ComputeHash(stream);
                return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task PackProjectAsync(string projectPath, string outputFile, List<string> includeExtensions, List<string> excludeDirs, IProgress<string> progress)
        {
            progress?.Report($"目标目录: {projectPath}");
            progress?.Report($"输出文件: {outputFile}");
            progress?.Report($"文件类型: {string.Join(", ", includeExtensions)}");
            progress?.Report($"排除目录: {string.Join(", ", excludeDirs)}");

            if (!Directory.Exists(projectPath))
            {
                throw new DirectoryNotFoundException($"目标目录不存在: {projectPath}");
            }

            // 删除已存在的输出文件
            if (File.Exists(outputFile))
            {
                File.Delete(outputFile);
                progress?.Report($"已删除旧文件: {outputFile}");
            }

            int fileCount = 0;
            int errorCount = 0;

            // 将扩展名列表转换为集合以提高查找效率，并确保包含点号
            var extensionsSet = new HashSet<string>(includeExtensions.Select(ext => 
                ext.StartsWith(".") ? ext : "." + ext), StringComparer.OrdinalIgnoreCase);
            var excludeDirsSet = new HashSet<string>(excludeDirs, StringComparer.OrdinalIgnoreCase);

            progress?.Report("开始搜索并处理文件...");

            // 使用Directory.EnumerateFiles递归遍历
            var allFiles = Directory.EnumerateFiles(projectPath, "*.*", SearchOption.AllDirectories)
                .Where(file =>
                {
                    var fileInfo = new FileInfo(file);
                    
                    // 检查是否在排除目录中
                    var relativePath = Path.GetRelativePath(projectPath, file);
                    var pathParts = relativePath.Split(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
                    if (pathParts.Any(part => excludeDirsSet.Contains(part)))
                    {
                        return false;
                    }

                    // 检查文件扩展名
                    return extensionsSet.Contains(fileInfo.Extension);
                })
                .ToList();

            using var outputWriter = new StreamWriter(outputFile, false, Encoding.UTF8);

            for (int i = 0; i < allFiles.Count; i++)
            {
                var file = allFiles[i];
                var relativePath = Path.GetRelativePath(projectPath, file).Replace('\\', '/');
                
                progress?.Report($"打包中 ({i + 1}/{allFiles.Count}): {Path.GetFileName(file)}");

                try
                {
                    var fileHash = GetSha256(file);
                    if (string.IsNullOrEmpty(fileHash))
                    {
                        progress?.Report($"警告: 无法计算文件哈希: {relativePath}");
                        errorCount++;
                        continue;
                    }

                    // 写入文件头信息（类似Python版本的格式）
                    await outputWriter.WriteLineAsync();
                    await outputWriter.WriteLineAsync("// ==========================================");
                    await outputWriter.WriteLineAsync($"// 文件: {relativePath}");
                    await outputWriter.WriteLineAsync($"// SHA256: {fileHash}");
                    await outputWriter.WriteLineAsync("// ==========================================");
                    await outputWriter.WriteLineAsync();

                    // 读取并写入文件内容
                    try
                    {
                        var content = await File.ReadAllTextAsync(file, Encoding.UTF8);
                        await outputWriter.WriteAsync(content);
                        fileCount++;
                    }
                    catch (Exception ex)
                    {
                        progress?.Report($"警告: 无法读取文件内容: {relativePath} - {ex.Message}");
                        errorCount++;
                    }
                }
                catch (Exception ex)
                {
                    progress?.Report($"严重错误: 处理文件时发生未知错误: {relativePath} - {ex.Message}");
                    errorCount++;
                }
            }

            progress?.Report($"处理完成 - 成功: {fileCount}, 错误: {errorCount}");
            progress?.Report($"输出文件已生成: {outputFile}");
        }

        public async Task PackFilesAsync(IEnumerable<string> files, string outputFile, IProgress<string> progress)
        {
            var fileList = files.Where(File.Exists).ToList();
            
            // 删除已存在的输出文件
            if (File.Exists(outputFile))
            {
                File.Delete(outputFile);
                progress?.Report($"已删除旧文件: {outputFile}");
            }

            int fileCount = 0;
            int errorCount = 0;

            using var outputWriter = new StreamWriter(outputFile, false, Encoding.UTF8);

            for (int i = 0; i < fileList.Count; i++)
            {
                var file = fileList[i];
                var fileName = Path.GetFileName(file);
                
                progress?.Report($"打包中 ({i + 1}/{fileList.Count}): {fileName}");

                try
                {
                    var fileHash = GetSha256(file);
                    if (string.IsNullOrEmpty(fileHash))
                    {
                        progress?.Report($"警告: 无法计算文件哈希: {fileName}");
                        errorCount++;
                        continue;
                    }

                    // 写入文件头信息
                    await outputWriter.WriteLineAsync();
                    await outputWriter.WriteLineAsync("// ==========================================");
                    await outputWriter.WriteLineAsync($"// 文件: {fileName}");
                    await outputWriter.WriteLineAsync($"// SHA256: {fileHash}");
                    await outputWriter.WriteLineAsync("// ==========================================");
                    await outputWriter.WriteLineAsync();

                    // 读取并写入文件内容
                    try
                    {
                        var content = await File.ReadAllTextAsync(file, Encoding.UTF8);
                        await outputWriter.WriteAsync(content);
                        fileCount++;
                    }
                    catch (Exception ex)
                    {
                        progress?.Report($"警告: 无法读取文件内容: {fileName} - {ex.Message}");
                        errorCount++;
                    }
                }
                catch (Exception ex)
                {
                    progress?.Report($"严重错误: 处理文件时发生未知错误: {fileName} - {ex.Message}");
                    errorCount++;
                }
            }

            progress?.Report($"处理完成 - 成功: {fileCount}, 错误: {errorCount}");
            progress?.Report($"输出文件已生成: {outputFile}");
        }

        /// <summary>
        /// 解包项目文件（匹配Python版本的解码逻辑）
        /// </summary>
        public async Task UnpackProjectAsync(string inputFile, string extractPath, IProgress<string> progress)
        {
            progress?.Report("========== 配置信息 ==========");
            progress?.Report($"源文件: {inputFile}");
            progress?.Report($"输出目录: {extractPath}");
            progress?.Report("==============================");
            progress?.Report("");

            if (!File.Exists(inputFile))
            {
                progress?.Report($"[错误] 源文件不存在: {inputFile}");
                return;
            }

            // 确保输出目录存在
            Directory.CreateDirectory(extractPath);

            progress?.Report("[信息] 开始解析源文件...");
            progress?.Report("");

            int fileCount = 0;
            var filesToVerify = new List<(string relativePath, string expectedHash, string fullPath)>();

            const string DELIMITER = "// ==========================================";
            const string FILE_PREFIX = "// 文件: ";
            const string SHA_PREFIX = "// SHA256: ";

            try
            {
                var lines = await File.ReadAllLinesAsync(inputFile, Encoding.UTF8);
                
                bool inHeader = false;
                string currentFilePath = null;
                string currentFileHash = null;
                var currentFileContent = new List<string>();

                foreach (var line in lines)
                {
                    var strippedLine = line.Trim();

                    if (strippedLine == DELIMITER)
                    {
                        if (inHeader) // 头部结束
                        {
                            inHeader = false;
                            if (!string.IsNullOrEmpty(currentFilePath))
                            {
                                // 将内容写入文件
                                var fullPath = Path.Combine(extractPath, currentFilePath);
                                var directoryPath = Path.GetDirectoryName(fullPath);
                                if (!string.IsNullOrEmpty(directoryPath))
                                {
                                    Directory.CreateDirectory(directoryPath);
                                }

                                await File.WriteAllLinesAsync(fullPath, currentFileContent, Encoding.UTF8);
                                
                                // 添加到验证列表
                                if (!string.IsNullOrEmpty(currentFileHash))
                                {
                                    filesToVerify.Add((currentFilePath, currentFileHash, fullPath));
                                }

                                currentFileContent.Clear();
                            }
                        }
                        else // 新文件开始
                        {
                            inHeader = true;
                            currentFilePath = null;
                            currentFileHash = null;
                        }
                        continue;
                    }

                    if (inHeader)
                    {
                        if (line.StartsWith(FILE_PREFIX))
                        {
                            var relativePath = line.Substring(FILE_PREFIX.Length).Trim();
                            currentFilePath = relativePath;
                            progress?.Report($"[创建] {relativePath}");
                            fileCount++;
                        }
                        else if (line.StartsWith(SHA_PREFIX))
                        {
                            currentFileHash = line.Substring(SHA_PREFIX.Length).Trim();
                        }
                    }
                    else if (!string.IsNullOrEmpty(currentFilePath))
                    {
                        // 不在头部，是文件内容
                        currentFileContent.Add(line);
                    }
                }

                progress?.Report("");
                progress?.Report("========== 解码完成 ==========");
                progress?.Report($"还原文件数: {fileCount}");
                progress?.Report($"输出目录: {extractPath}");
                progress?.Report("==============================");
                progress?.Report("");

                // 文件完整性校验
                progress?.Report("[校验] 开始进行文件完整性校验...");
                progress?.Report("");

                int successCount = 0;
                int failedCount = 0;

                foreach (var (relativePath, expectedHash, fullPath) in filesToVerify)
                {
                    progress?.Report($"[校验中] {relativePath.Replace('\\', '/')}");
                    
                    if (!File.Exists(fullPath))
                    {
                        progress?.Report("   └─ [失败] 文件未找到!");
                        failedCount++;
                        continue;
                    }

                    var actualHash = GetSha256(fullPath);

                    if (actualHash == expectedHash)
                    {
                        progress?.Report("   └─ [成功] 哈希匹配");
                        successCount++;
                    }
                    else
                    {
                        progress?.Report("   └─ [失败] 哈希不匹配!");
                        progress?.Report($"      ├─ 期望值: {expectedHash}");
                        progress?.Report($"      └─ 计算值: {actualHash}");
                        failedCount++;
                    }
                }

                progress?.Report("");
                progress?.Report("========== 校验完成 ==========");
                progress?.Report($"共校验文件数: {filesToVerify.Count}");
                progress?.Report($"成功: {successCount}");
                progress?.Report($"失败: {failedCount}");
                progress?.Report("==============================");
            }
            catch (Exception ex)
            {
                progress?.Report($"[严重错误] 解码过程中发生错误: {ex.Message}");
            }
        }
    }
}