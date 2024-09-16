using Microsoft.AspNetCore.Mvc.Formatters;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace AD.Models
{
    public class ADDomain
    {        
            public int Id { get; set; }
            [Display(Name = "IP Адрес контроллера домена")]
            public string? DomainControllerAddress { get; set; }
            [Display(Name = "Название домена")]
            public string? DomainName { get; set; }
            [Display(Name = "Формат LDAP")]
            public string? LdapFormat
            {
                get
                {
                    if (DomainName == null)
                    {
                        return "Значение не установлено";
                    }
                    string[] domainComponents = DomainName.Split('.');
                    string ldapFormat = string.Join(",", domainComponents.Select(Component => "DC=" + Component));
                    return ldapFormat;
                }
                set { }
            }
            [Display(Name = "Успех подключения")]
            public bool? IsActive { get; set; }
            
        
    }
}
