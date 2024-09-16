using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AD.Models
{
    public class ADOU
    {

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        public string? OUPath { get; set; }
        public string? OUName { get; set; }
        //public TypeName? workerType { get; set; }
        //public List<UserAccountNames> UserAccountNames { get; set; } = new();
        //public ICollection<UserAccountNames>? accounts { get; set; }
        [NotMapped]
        public bool isChecked { get; set; }
       


    }
}
