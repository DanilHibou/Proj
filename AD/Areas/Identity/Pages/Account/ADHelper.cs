using System.DirectoryServices.AccountManagement;
using System.DirectoryServices;
using AD.Models;


namespace AD.Areas.Identity.Pages.Account
{
    public class ADHelper
    {
        public static bool ADLogin(string userName, string password)
        {
            using (PrincipalContext principalContext = new(ContextType.Domain))
            {
                bool isValidLogin = principalContext.ValidateCredentials(userName.ToUpper(), password);

                return isValidLogin;
            }            
        }  

    }
}
