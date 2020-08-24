using System;
using System.Collections.Generic;

namespace Shared
{
    public class KeyWords
    {
        /// <summary>
        /// 语言
        /// </summary>
        public string Language { get; set; }
        /// <summary>
        /// 来源地址
        /// </summary>
        public string Ref { get; set; }
        /// <summary>
        /// 关键字
        /// </summary>
        public List<string> Keywords { get; set; }

        /// <summary>
        /// 保留字
        /// </summary>
        public List<string> SaveWords { get; set; }
    }
}
