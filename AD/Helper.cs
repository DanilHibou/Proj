using AD.Areas.Identity.Pages.Account;
using Azure;
using Google.Apis.Admin.Directory.directory_v1.Data;
using Google.Apis.Auth.OAuth2.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Shared;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;


namespace AD
{    
     
    public class Helper
    {
        private AD.Data.Identity _context;
        private CookieProtector _cookieProtector;
        private TokenProtector _tokenProtector;
        private readonly IHttpClientFactory _httpClientFactory;
        public Helper(AD.Data.Identity context, CookieProtector cookieProtector, TokenProtector tokenProtector, IHttpClientFactory httpClientFactory)
        {
            _cookieProtector = cookieProtector;
            _context = context;
            _tokenProtector = tokenProtector;
            _httpClientFactory = httpClientFactory;
        }
        public static class ActiveDirectoryConnection
        {
            public static string ADDomainIP {  get; set; }
            public static string ADDomainName { get; set; }
            public static PrincipalContext MyDomain { get; private set; }
                        
            public static void InitializeDomainConnection(string domainControllerAddress)
            {
                MyDomain = new PrincipalContext(ContextType.Domain, domainControllerAddress);
            }
        }    
        

        public async Task GetGoogleToken(ApplicationUser user, HttpContext httpContext)
        {
            HttpClient client = _httpClientFactory.CreateClient();            
            string refreshToken = string.Empty;
            string UnprotectedrefreshToken = string.Empty;
            var googleToken = await _context.GoogleOauthTokens.FirstOrDefaultAsync(x => x.UserId == user.Id);
            if (googleToken != null)
            {
                refreshToken = googleToken.refreshToken;
                UnprotectedrefreshToken = _tokenProtector.Unprotect(refreshToken);
            }
            else
            {
                
                return;
            }
            var clientId = "";
            var clientSecret = "";

            var request = new HttpRequestMessage(HttpMethod.Post, "https://oauth2.googleapis.com/token");
            request.Content = new FormUrlEncodedContent(new[]
            {
            new KeyValuePair<string, string>("client_id", clientId),
            new KeyValuePair<string, string>("client_secret", clientSecret),
            new KeyValuePair<string, string>("refresh_token", UnprotectedrefreshToken),
            new KeyValuePair<string, string>("grant_type", "refresh_token")
            });

            var response = await client.SendAsync(request);
            var responseContent = await response.Content.ReadAsStringAsync();
            var tokenResponse = JsonConvert.DeserializeObject<TokenResponse>(responseContent);            
            var cookieOptions = new CookieOptions
            {
                Expires = DateTimeOffset.UtcNow.AddSeconds(tokenResponse.expires_in), 
                HttpOnly = true, 
                Secure = true, 
                SameSite = SameSiteMode.Strict,                 

            };
            string token = tokenResponse.access_token;
            string encryptedAccessToken = _cookieProtector.Protect(token);
            httpContext.Response.Cookies.Append("access_token", encryptedAccessToken, cookieOptions);          

        }
        
        public class TokenResponse
        {
            public string? access_token { get; set; }
            public int expires_in { get; set; }
            public string? scope { get; set; }
            public string? token_type { get; set; }
        } 
        public async Task<bool> GoogleCheck(string email, string accessToken)
        {
            HttpClient client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            var request = new HttpRequestMessage(HttpMethod.Get, $"https://admin.googleapis.com/admin/directory/v1/users/{email}");
            HttpResponseMessage response = await client.SendAsync(request);

            return response.IsSuccessStatusCode;
        } 
        public string PasswordGen()
        {
            string lettersAndDigits = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            string specialChars = "!@#$%^&*()";
            Random random = new Random();

          
            string randomChars = new string(Enumerable.Repeat(lettersAndDigits, 10)
                                            .Select(s => s[random.Next(s.Length)]).ToArray());

            string randomSpecialChars = new string(Enumerable.Repeat(specialChars, 2)
                                                  .Select(s => s[random.Next(s.Length)]).ToArray());

            string password = new string(randomChars + randomSpecialChars);

            password = new string(password.ToCharArray().OrderBy(x => random.Next()).ToArray());

            return password;
        } 
        public class NameConverter
        {
            public static string FormatName(string transliteratedName)
            {
                string[] parts = transliteratedName.Split(' ');

                string lastName = parts[0].ToLower();

                string firstLetters = string.Join("", parts.Skip(1).Select(p => p.Length > 0 ? p.Substring(0, 1) : "")).ToLower();

                return $"{lastName}_{firstLetters}";
            }
            public static string ConvertFullNameToLatin(string fullName)
            {
                string[] words = fullName.Split(' ');

                for (int i = 0; i < words.Length; i++)
                {
                    words[i] = Converter.ConvertToLatin(words[i]);
                }

                return string.Join(" ", words);
            }
            public static class Converter
            {
                private static readonly Dictionary<char, string> ConvertedLetters = new Dictionary<char, string>
    {
        {'а', "a"},
        {'б', "b"},
        {'в', "v"},
        {'г', "g"},
        {'д', "d"},
        {'е', "e"},
        {'ё', "yo"},
        {'ж', "zh"},
        {'з', "z"},
        {'и', "i"},
        {'й', "j"},
        {'к', "k"},
        {'л', "l"},
        {'м', "m"},
        {'н', "n"},
        {'о', "o"},
        {'п', "p"},
        {'р', "r"},
        {'с', "s"},
        {'т', "t"},
        {'у', "u"},
        {'ф', "f"},
        {'х', "h"},
        {'ц', "c"},
        {'ч', "ch"},
        {'ш', "sh"},
        {'щ', "sch"},
        {'ъ', "j"},
        {'ы', "i"},
        {'ь', "y"},
        {'э', "e"},
        {'ю', "yu"},
        {'я', "ya"},
        {'А', "A"},
        {'Б', "B"},
        {'В', "V"},
        {'Г', "G"},
        {'Д', "D"},
        {'Е', "E"},
        {'Ё', "Yo"},
        {'Ж', "Zh"},
        {'З', "Z"},
        {'И', "I"},
        {'Й', "J"},
        {'К', "K"},
        {'Л', "L"},
        {'М', "M"},
        {'Н', "N"},
        {'О', "O"},
        {'П', "P"},
        {'Р', "R"},
        {'С', "S"},
        {'Т', "T"},
        {'У', "U"},
        {'Ф', "F"},
        {'Х', "H"},
        {'Ц', "C"},
        {'Ч', "Ch"},
        {'Ш', "Sh"},
        {'Щ', "Sch"},
        {'Ъ', "J"},
        {'Ы', "I"},
        {'Ь', "Y"},
        {'Э', "E"},
        {'Ю', "Yu"},
        {'Я', "Ya"}
    };
                public static string ConvertToLatin(string source)
                {
                    var result = new StringBuilder();
                    foreach (var letter in source)
                    {

                        if (ConvertedLetters.ContainsKey(letter))
                        {
                            result.Append(ConvertedLetters[letter]);
                        }
                        else
                        {
                            
                            result.Append(letter);
                        }
                    }
                    return result.ToString();
                }
            }
        } 

        public class ADCheck 
        {
            private static string ldap = "";
            public async Task<bool> UserExists(string username)
            {
                if (string.IsNullOrEmpty(username))
                {
                    return false;
                }

                using (DirectoryEntry entry = new DirectoryEntry(ldap))
                {
                    using (DirectorySearcher searcher = new DirectorySearcher(entry))
                    {
                        searcher.Filter = "(&(objectClass=user)(samAccountName=" + username + "))";
                        var results = await Task.Run(() => searcher.FindAll());
                        return results != null && results.Count > 0;
                    }
                }
            }
        }
        
    }
    public class TokenProtector
    {
        private readonly IDataProtector _protector;

        public TokenProtector(IDataProtectionProvider dataProtectionProvider)
        {
            _protector = dataProtectionProvider.CreateProtector("Token");
        }
        public string Protect(string token)
        {
            return _protector.Protect(token);
        }
        public string Unprotect(string protectedToken)
        {
            return _protector.Unprotect(protectedToken);
        }
    } 
    public class CookieProtector
    {
        private readonly IDataProtector _protector;
        public CookieProtector(IDataProtectionProvider dataProtectionProvider)
        {
            
            _protector = dataProtectionProvider.CreateProtector("Cookie");
        }
       
        public string Protect(string cookieValue)
        {
            return _protector.Protect(cookieValue);
        }
       
        public string Unprotect(string protectedCookieValue)
        {
            return _protector.Unprotect(protectedCookieValue);
        }
    } 


}
