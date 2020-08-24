using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Model
{
    public class CheckResult
    {
        /// <summary>
        /// 结果 1 完全可用 2 可用（有保留字） 3 不建议用（有关键字）
        /// </summary>
        public int Result { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public List<LanguageWordsItem> Items { get; set; }
    }
}
