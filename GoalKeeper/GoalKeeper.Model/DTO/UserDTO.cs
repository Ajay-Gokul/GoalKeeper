namespace Model.DTO
{
    public class UserDTO
    {
        public Guid UID { get; set; }
        public string Mail { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public string Token { get; set; }
    }
}
