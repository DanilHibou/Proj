using System.ComponentModel.DataAnnotations.Schema;

namespace AD.Models
{
    public class ADGroups
    {
        

            [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
            public int id { get; set; }
            public string? GroupPath { get; set; }
            public string? GroupName { get; set; }
            //public TypeName? workerType { get; set; }
            public ICollection<UserAccountNames>? UserAccountNames { get; set; }
            [NotMapped]
            public bool isChecked { get; set; }


    }
}
