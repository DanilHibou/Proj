namespace AD.Models
{
    public class OrganizationalDivisions
    {
        public int id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public int? ParentId { get; set; }
        public OrganizationalDivisions? Parent { get; set; }
        public ICollection<OrganizationalDivisions>? Children { get; set; }
        public int? ADOUid { get; set; }
        public ADOU? ADOU { get; set; }
        public int? GoogleOUid { get; set; }
        public GoogleOU? GoogleOU { get; set; }
        public ICollection<UserAccountNames>? accounts { get; set; }
    }
}
