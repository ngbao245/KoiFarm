namespace Repository.Model.User
{
    public class ResponseUserModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string? Address { get; set; } 
        public string? Phone { get; set; }
        public string? RoleId { get; set; }
    }
}
