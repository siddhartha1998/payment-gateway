namespace PaymentGateway.Server.Application.Users.Queries
{
    public class CurrentUserDetail
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Fullname { get; set; }
        public string PhoneNumber { get; set; }
        public bool IsActive { get; set; }
    }
}
