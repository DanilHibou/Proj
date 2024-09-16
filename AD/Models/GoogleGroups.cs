using System.ComponentModel.DataAnnotations.Schema;

namespace AD.Models
{
    public class GoogleGroups
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }        
        public string? GroupName { get; set; }
        public string? Groupid { get; set; }
       // public TypeName? workerType { get; set; }
        public ICollection<UserAccountNames>? UserAccountNames { get; set; }
        [NotMapped]
        public bool isChecked { get; set; }
    }
}
