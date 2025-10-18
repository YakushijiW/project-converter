namespace project_converter
{
    /// <summary>
    /// 用户设置类，用于保存用户的配置信息
    /// </summary>
    public class UserSettings
    {
        public string? TargetDir { get; set; }
        public string? OutputFileName { get; set; }
        public string? SourceFile { get; set; }
        public string? DecodeDir { get; set; }
        public string? LastProjectPath { get; set; }
        public string? LastExtractPath { get; set; }
        
        // 新增：记住打包页面的设置
        public string? LastPackProjectPath { get; set; }
        public string? LastPackOutputPath { get; set; }
        public string? LastSelectedPreset { get; set; }
        
        // 新增：记住解包页面的设置
        public string? LastUnpackInputFile { get; set; }
        public string? LastUnpackOutputPath { get; set; }
    }
}