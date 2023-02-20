using Microsoft.AspNetCore.WebUtilities;
using System.Net;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace SimpleAuthorizationCodeFlowWithPKCE
{
    public  class Utility
    {

        public const string ClientId = "your_client_secrect";

        public static string CodeVerifier;

        public static string CodeChallenge;
        public static void Init()
        {
            CodeVerifier = GenerateNonce();
            CodeChallenge = GenerateCodeChallenge(CodeVerifier);
        }
        private static string GenerateNonce()
        {
            const string chars = "abcdefghijklmnopqrstuvwxyz123456789";
            var random = new Random();
            var nonce = new char[128];
            for (int i = 0; i < nonce.Length; i++)
            {
                nonce[i] = chars[random.Next(chars.Length)];
            }

            return new string(nonce);
        }

        private static string GenerateCodeChallenge(string codeVerifier)
        {
            using var sha256 = SHA256.Create();
            var hash = sha256.ComputeHash(Encoding.UTF8.GetBytes(codeVerifier));
            var b64Hash = Convert.ToBase64String(hash);
            var code = Regex.Replace(b64Hash, "\\+", "-");
            code = Regex.Replace(code, "\\/", "_");
            code = Regex.Replace(code, "=+$", "");
            return code;
        }


        public  static  async Task<string> GetAuthorizationCodeWithPikce()
        {

            string code =string.Empty;
            var queryParams = new Dictionary<string, string>()
{
                    { "client_id",  "your_client_secrect" },
                    { "response_type", "code"},
                    { "scope", "openid profile phone display_name offline_access"},
                     { "redirect_uri", "your_client_redirect" },
                    { "state","vsf234tgfgbsq2356245yhsgh234"},
                    { "nonce" ,"c6gbmzxjnwl24fwf243124rfqwd21" },
                    { "code_challenge","d5aboMkkBsXXPvH1dmAUpDwII5p47IvTeaFUL7tvK9Y"},
                    { "code_challenge_method","S256" },
};
            var uri = QueryHelpers.AddQueryString("your_authorize_Adddress(get it from discovery address)", queryParams);

            var baseadress = new Uri("your_authorize_Adddress");
            var cookieContainer = new CookieContainer();
            cookieContainer.Add(baseadress, new Cookie("your_token_name", Cookies.Value));
            using (var handler = new HttpClientHandler() { CookieContainer = cookieContainer, AllowAutoRedirect = false })
            using (var client = new HttpClient(handler) { BaseAddress = baseadress })
            {

                var result = await client.GetAsync(uri);
                Uri myUri = new Uri(result.Headers.Location.ToString());
                code = HttpUtility.ParseQueryString(myUri.Query).Get("code");

            }

            return code;
        }

        public static async Task<string> GetTokens(string code)
        {

          string content=string.Empty;
            var baseadressFortoken = new Uri("your_token_Adddress(get it from discovery address)");
            var cookieContainerFortoken = new CookieContainer();
            cookieContainerFortoken.Add(baseadressFortoken, new Cookie("your_token_name", Cookies.Value));
            using (var handler = new HttpClientHandler() { CookieContainer = cookieContainerFortoken, AllowAutoRedirect = false })
            using (var client = new HttpClient(handler) { BaseAddress = baseadressFortoken })
            {
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var postBody = new Dictionary<string, string>
            {

                    { "grant_type", "authorization_code"},
                              { "client_id",  "hamdam-mobile" },
                     { "redirect_uri", "your_client_redirect" },
                    { "code",code },

                    {"code_verifier","wBGh6cNZwy7TzYFN9zIngrDMJ4hFpsyiG4PxQppRTsZyax9nguapDPMfBZwRC763" }
            };
                var urlEncodedParameters = new FormUrlEncodedContent(postBody);
                var req = new HttpRequestMessage(HttpMethod.Post, "your_token_Adddress(get it from discovery address)") { Content = urlEncodedParameters };
                //req.Headers.Add("authorization", "Bearer <access_token>");
                var response = await client.SendAsync(req);
                 content = response.Content.ReadAsStringAsync().Result; ;
            }
            return content;
        }
    }
}
