using Microsoft.AspNetCore.Components;
using Shared;
using Shared.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace CheckKeywordsBlazor.Pages
{
    public partial class Index
    {
        [Inject]
        protected HttpClient Client { get; set; }
        private CheckResult Result;
        private string Param;

        private async Task Check()
        {
            try
            {
                if (Uri.IsWellFormedUriString(Param, UriKind.Absolute))
                {
                    var url = Param;
                    Param = "loading..";
                    Param = await Client.GetStringAsync(url);
                }

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
