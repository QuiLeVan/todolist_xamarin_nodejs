using Newtonsoft.Json;

namespace todolist.Models.User
{
    public class UserRegisterInfo
    {
        [JsonProperty("username")]
        public string UserName { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("password")]
        public string Password { get; set; }
    }
}
