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
        private string Param;

        private void Check()
        {
            try
            {

                Result = CheckKeywords.Check(Param);
            }
            catch
            {
                Result = new CheckResult
                {
                    Result = 0
                };
            }
        }
    }
}
