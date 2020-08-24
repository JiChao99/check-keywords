using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using Shared;
using Shared.Model;
using System.Linq;

namespace CheckKeywordsGateway.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CheckController : ControllerBase
    {
        public bool IsCLI { get; set; }
        public string[] PLAIN_TEXT_AGENTS = {
            "curl",
            "httpie",
            "lwp-request",
            "wget",
            "python-requests",
            "openbsd ftp",
            "powershell"
        };
        public const string PLAIN_END = "\r\nFollow [44m@alec.ji[0m on [44mjichao.top[0m";

        [HttpGet("{str}")]
        public ActionResult Check(string str)
        {
            var result = CheckKeywords.Check(str);
            var agent = HttpContext.Request.Headers[HeaderNames.UserAgent][0].ToLower();

            if (PLAIN_TEXT_AGENTS.Any(t => agent.Contains(t)))
            {
                return Content(ConvertCLI(result));
            }
            return new JsonResult(result);
        }

        private string ConvertCLI(CheckResult checkResult)
        {
            var result = string.Empty;
            if (checkResult.Result == (int)EnumCheckResult.Success)
            {
                result = "[92mPerfect √[0m\r\n";
            }
            else if (checkResult.Result == (int)EnumCheckResult.Available)
            {
                result = "[34mUsable[0m\r\n\r\n[34mSavewords[0m\r\n";
                checkResult.Items.ForEach(
                    t =>
                    {
                        result += @$"Language:{t.Language}
SaveWords:{string.Join(',', t.Words)}
REF:{t.Ref}
";
                    });
            }
            else if (checkResult.Result == (int)EnumCheckResult.Disable)
            {
                result = "[31mDisable ×[0m\r\n\r\n[34mKeywords[0m\r\n";

                checkResult.Items.Where(t => t.Type == (int)EnumKeywordsType.Keywords).ToList().ForEach(t =>
                {
                    result += @$"Language:{t.Language}
Keywords:{string.Join(',', t.Words)}
REF:{t.Ref}
";
                });
                var saveWords = checkResult.Items.Where(t => t.Type == (int)EnumKeywordsType.SaveWords).ToList();
                if(saveWords.Any())
                {
                    result += "\r\n[34mSavewords[0m\r\n";
                    saveWords.ForEach(t =>
                     {
                         result += @$"Language:{t.Language}
SaveWords:{string.Join(',', t.Words)}
REF:{t.Ref}
";
                     });
                }
                
            }
            else
            {
                result = "[31mSystem Error[0m\r\n";
            }
            result += PLAIN_END;

            return result;
        }
    }
}
