using System.ComponentModel.DataAnnotations.Schema;

namespace AD.Models
{
    public class GoogleOU
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        public string? OUPath { get; set; }
        public string? OUName { get; set; }
        public string? OUid { get; set; }
        //public TypeName? workerType { get; set; }
        //public ICollection<UserAccountNames>? accounts { get; set; }
        [NotMapped]
        public bool isChecked { get; set; }
    }

}
