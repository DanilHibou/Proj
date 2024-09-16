namespace AD.Models
{
    public class Location
    {       
        public int id { get; set; }
        public string? Name { get; set; }        

        public ICollection<UserAccountNames>? UserAccountNames { get; set; }


    }
}
