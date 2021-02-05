using Shared.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shared.Handlers
{
    public class WordsHandler : CheckKeywordsHandler
    {
        public override List<string> BuildNeedCheckWords(string str)
        {
            return str.Split(',').Select(t=>t.Trim()).ToList();
        }
    }
}
