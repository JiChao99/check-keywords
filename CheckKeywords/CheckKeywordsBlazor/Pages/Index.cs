using Shared;
using Shared.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CheckKeywordsBlazor.Pages
{
    public partial class Index
    {
        private CheckResult Result;
        private string param;

        private void Check()
        {
                var x = CheckKeywords.Check(param);
            Result = x;
        }
    }
}
