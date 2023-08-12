﻿namespace AuthenticationApiService.Messages
{
    public class CreateUserDto
    {
        public Guid Id { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public DateTime CreatedOnUtc {  get; set; }
    }
}
