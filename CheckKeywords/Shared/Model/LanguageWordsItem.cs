using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Model
{
    public class LanguageWordsItem
    {
        /// <summary>
        /// 语言
        /// </summary>
        public string Language { get; set; }

        /// <summary>
        /// 类型 1 关键字 2 保留字
        /// </summary>
        public int Type { get; set; }

        /// <summary>
        /// 来源地址
        /// </summary>
        public string Ref { get; set; }

        /// <summary>
        /// 包含词
        /// </summary>
        public List<string> Words { get; set; }
    }
}
