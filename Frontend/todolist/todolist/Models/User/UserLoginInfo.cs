using Newtonsoft.Json;

namespace todolist.Models.User
{
    public class UserLoginInfo
    {
        [JsonProperty("identifier")]
        public string Email { get; set; }

        [JsonProperty("password")]
        public string Password { get; set; }
    }
}
