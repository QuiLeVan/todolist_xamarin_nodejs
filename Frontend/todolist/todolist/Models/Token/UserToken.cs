using Newtonsoft.Json;
namespace todolist.Models.Token
{
    public class UserToken
    {
        [JsonProperty("jwt")]
        public string AccessToken { get; set; }
    }
}
