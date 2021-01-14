using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
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
        private string Param = string.Empty;
        string ShowDiv = "none";

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
                var z = Param;
                if (Result.Result != 1)
                {
                    foreach (var item in Result.Items)
                    {
                        item.Words.ForEach(t =>
                        {
                            z = z.Replace(t, "<code>" + t + "</code>");
                        });
                    }
                }
                if (!string.IsNullOrEmpty(z))
                {
                    await JS.InvokeVoidAsync("csfunc.appendHtml", @"AAA", z);
                    ShowDiv = "block";
                }
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
