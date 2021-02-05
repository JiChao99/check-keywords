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
        private CheckResult Result { get; set; }

        private string Param { get; set; }

        private bool ShowResultFragment { get; set; }

        private string ErrorInfo { get; set; }
        private string SwaggerUrl { get; set; }

        private RenderFragment ResultFragment { get; set; }

        private readonly RenderFragment CodelfSvg = (builder) => builder.AddMarkupContent(0, @"<svg version = '1.1' id='Layer_1' xmlns='http://www.w3.org/2000/svg' xmlns:xlink='http://www.w3.org/1999/xlink' x='0px' y='0px' width='1rem' height='1rem' viewBox='0 0 228 228' enable-background='new 0 0 228 228' xml:space='preserve'><image id = 'image0' x='0' y='0' href='data:image/png;base64,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
FKSY5j08s3odz23dEHqUWihIsc1BBfzr6rfTLzqhp9lxClJMc0BVwbEz389Lg/2hx9lxClLM8x7y
MmW2nYYeZccpSDHPOdjotxj4XaFH2XHRBBnXCViTxzvPufxQ6DF2XBRBOudV5ATI3DD0CDsuiiAT
50kU5FhLkoql5vOhx9j5+xl6gDo0koLEbR8ckPGUkNLtj0KPUcP9jEAjGdFMBqHHkDfIe7jmirM0
3eQvFhZJkAOmki5Ou61jxwOD3PPxo59m//R/hx5nx0UR5N7WMu87+EXSZPsXLOPDAUudPqOt5dCj
1CKKILMk57Y9x5lOeypyDN269wTXzvxD6DFqEUWQADfMn+Ddb3lcu61jxHtY6mzwvv33k7ki9Di1
iCZIgHuuvo+Z5lAbyTHhHNy4+BRHdx8PPUptogrywPTX+MChv8KhPddxsNTe5IcO/27oMWoVVZAA
P3L4Pt5z5WPbUapKk7yHzlTB+w/+Gfunnw89Tq2iC7KdbfCLR36Jo3tP6P2kUY3UcevuR/jQoU/R
zrZCj1Or6IIEmG2s8evv/GnuOvDQN6LU1jIs719dzTxzHF36Cr/89o/TSvuhx6qdW3lgMdqn4qBs
c+zMD/CFF+7hxe415KXHo/PQQ3DOccvSSd575ed51+6/ZbaxEXqkMI9DzEG+5uXBPtbzK3jo9N08
sXoH3bzN1mgap/Nfd4xzkLqKVtpnULb4iWvv4459D7HQfCX0aGEfFwUpYkeU7yFFrFKQIoYoSBFD
FKSIIQpSxBAFKWKIghQxREGKGKIgRQxRkCKGKEgRQxSkiCEKUsQQBSliiIIUMURBihiiIEUMUZAi
hihIEUMUpIghClLEEAUpYoiCFDFEQYoYoiBFDFGQIoYoSBFDFKSIIQpSxBAFKWKIghQxREGKGKIg
RQxRkCKGKEgRQxSkiCEKUsSQ/wFge37h7X0EMQAAACV0RVh0ZGF0ZTpjcmVhdGUAMjAyMC0xMC0y
NVQxMzozMDozOCswODowMAZtmrcAAAAldEVYdGRhdGU6bW9kaWZ5ADIwMjAtMTAtMjVUMTM6MzA6
MzgrMDg6MDB3MCILAAAAIHRFWHRzb2Z0d2FyZQBodHRwczovL2ltYWdlbWFnaWNrLm9yZ7zPHZ0A
AAAYdEVYdFRodW1iOjpEb2N1bWVudDo6UGFnZXMAMaf/uy8AAAAYdEVYdFRodW1iOjpJbWFnZTo6
SGVpZ2h0ADIyOEE6/9kAAAAXdEVYdFRodW1iOjpJbWFnZTo6V2lkdGgAMjI40suvhAAAABl0RVh0
VGh1bWI6Ok1pbWV0eXBlAGltYWdlL3BuZz+yVk4AAAAXdEVYdFRodW1iOjpNVGltZQAxNjAzNjAz
ODM43O6jRAAAABJ0RVh0VGh1bWI6OlNpemUAMzk2N0JCsdZDbgAAAEZ0RVh0VGh1bWI6OlVSSQBm
aWxlOi8vL2FwcC90bXAvaW1hZ2VsYy9pbWd2aWV3Ml85XzE2MDI1Nzg0NDk1NjYzMDIzXzcyX1sw
XfmtYl0AAAAASUVORK5CYII = '></image></svg>");

        private readonly List<string> LanguageNames = CheckKeywords.GetAllProgramNames();
        IEnumerable<string> SelectedLanguageNames { get; set; }
        public Index()
        {
            ShowResultFragment = false;
        }

        private void ChangShowResultFragment()
        {
            ShowResultFragment = !ShowResultFragment;
        }
        private async Task Check()
        {

            try
            {
                if (string.IsNullOrEmpty(Param))
                    return;

                if (Uri.IsWellFormedUriString(Param, UriKind.Absolute))
                {
                    SwaggerUrl = Param;
                    Param = "loading..";
                    Param = await Client.GetStringAsync(SwaggerUrl);
                }

                Result = CheckKeywords.Check(Param, SelectedLanguageNames.ToList());
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

                    //await JS.InvokeVoidAsync("csfunc.appendHtml", @"AAA", z);
                    ShowResultFragment = true;
                    ResultFragment = (builder) => builder.AddMarkupContent(0, z);
                }
                else
                {
                    ShowResultFragment = false;
                }
            }
            catch(HttpRequestException)
            {
                ErrorInfo = "error, please check your swagger url";
                Param = SwaggerUrl;
                SwaggerUrl = string.Empty;
                Result = new CheckResult
                {
                    Result = 0
                };
            }
            catch(Exception)
            {
                Result = new CheckResult
                {
                    Result = 0
                };
            }
        }
    }
}
