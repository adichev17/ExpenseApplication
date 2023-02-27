namespace Authentication.Application.Common.Dtos
{
    public class RegisterResultDto
    {
        public int Id { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public DateTime CreatedOnUtc { get; set; }
    }
}
