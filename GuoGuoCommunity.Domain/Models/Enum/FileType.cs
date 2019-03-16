using System.Collections.Generic;

namespace GuoGuoCommunity.Domain.Models.Enum
{
    /// <summary>
    /// 文件类型
    /// </summary>
    public class FileType
    {
        static FileType()
        {
            Image = new FileType { Name = "图片", Value = "Image" };
            Text = new FileType { Name = "文字", Value = "Text" };
        }

        public string Name { get; set; }

        public string Value { get; set; }

        public static FileType Image { get; set; }

        public static FileType Text { get; set; }

        public static IEnumerable<FileType> GetAll() => new List<FileType>() { Image, Text };

    }
}
