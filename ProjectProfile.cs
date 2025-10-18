using System.Collections.Generic;

namespace project_converter
{
    /// <summary>
    /// 项目配置文件类，用于存储项目的文件扩展名和排除目录等信息
    /// </summary>
    public class ProjectProfile
    {
        /// <summary>
        /// 预设配置名称
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// 需要处理的文件扩展名列表
        /// </summary>
        public List<string> Extensions { get; set; } = new List<string>();

        /// <summary>
        /// 需要排除的目录列表
        /// </summary>
        public List<string> ExcludeDirs { get; set; } = new List<string>();
    }
}