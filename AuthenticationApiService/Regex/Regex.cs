using System.Runtime.CompilerServices;

namespace AuthenticationApiService.Regex
{
    public sealed class Regex
    {
        /// <summary>
        /// At least one digit.
        /// Has minimum 4 characters in length. 
        /// At least one uppercase English letter.
        /// At least one lowercase English letter
        /// </summary>
        public const string Password = @"^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9]).{4,}$";

        /// <summary>
        /// At least one letter or number
        /// Every character from the start to the end is a letter or numbe
        /// </summary>
        public const string Login = @"^[a-zA-Z0-9]+$";
    }
}
