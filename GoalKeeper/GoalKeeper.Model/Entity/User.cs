namespace Model.Entity
{
    public class User
    {
        public Guid UID { get; set; }
        public string Name { get; set; }    
        public string? Mail { get; set; }        
        public string PasswordHash { get; set; }                
    }
}
