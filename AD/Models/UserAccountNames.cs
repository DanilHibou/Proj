using Kerberos.NET.Entities.Pac;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace AD.Models
{
    public class UserAccountNames
    {
        public int id { get; set; }

        [Display(Name = "UserName")]
        [StringLength(15)]
        public string? UserName {  get; set; }        

        [Required]
        [Display(Name = "Имя")]
        [StringLength(15)] //ограничение длины имени
        public string? FirstName { get; set; } //имя

        [Required]
        [Display(Name = "Фамилия")]
        [StringLength(15)] //ограничение длины имени
        public string? LastName { get; set; } //фамилия

        [Required]
        [StringLength(15)] //ограничение длины имени
        [Display(Name = "Отчество")]
        public string? SurName { get; set; } //отчество

     
        public string? UserKey { get; set; }
        public bool? isADCreated { get; set; }
        public DateTime? ADCreated { get; set; }
        public bool? isGoogleCreated { get; set; }
        public DateTime? GoogleCreated { get; set; }       
        public int? organizationalDivisionsid { get; set; }
        public OrganizationalDivisions? organizationalDivisions { get; set; }

        public ICollection<ADGroups>? ADGroups { get; set; }
        public ICollection<GoogleGroups>? GoogleGroups { get; set; }
        //public ICollection<Location>? location { get; set; }

        [NotMapped]
        public bool isChecked { get; set; }       
    }
    //public enum TypeName
    //{
    //    [Display(Name = "Сотрудники")]
    //    Worker,
    //    [Display(Name = "Ученики")]
    //    Student,
    //    [Display(Name = "Преподаватели")]
    //    Teacher,
    //    [Display(Name = "Неопределено")]
    //    Undefined
    //}    

}