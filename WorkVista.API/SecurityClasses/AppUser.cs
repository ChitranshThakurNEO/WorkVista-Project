using System.Text.Json.Serialization;

namespace WorkVista.API.SecurityClasses
{
    public partial class AppUser
    {
        public AppUser()
        {
            UserId = Guid.NewGuid();
            UserName = string.Empty;
            Password = string.Empty;
            RoleName = string.Empty;
            IsAuthenticated = false;
        }

        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public string RoleName { get; set; }
        [JsonIgnore]
        public string Password { get; set; }
        public bool IsAuthenticated { get; set; }
    }
}
