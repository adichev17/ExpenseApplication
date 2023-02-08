namespace AuthenticationApiService.Models.CommunicationModel
{
    public sealed class RegisterRequest
    {
        public required string Login { get; set; }
        public required string Password { get; set; }
    }
}
