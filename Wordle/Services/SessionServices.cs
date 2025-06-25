using Microsoft.AspNetCore.Http;
using System.Text.Json;

namespace Wordle.Services
{
    public class SessionServices
    {
        public const string Key = "Wordle";
        private readonly IHttpContextAccessor _httpContextAccessor;

        public SessionServices(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public void SetCookie(UserInfo value)
        {

            var options = new CookieOptions
            {
                Expires = DateTimeOffset.MaxValue,
                HttpOnly = false,
                Secure = true, // set to true in production!
                SameSite = SameSiteMode.None

            };

            var json = JsonSerializer.Serialize(value);
            _httpContextAccessor.HttpContext.Response.Cookies.Append(Key, json, options);

        }

        public UserInfo GetCookie()
        {
            if (_httpContextAccessor.HttpContext.Request.Cookies.TryGetValue(Key, out string json))
            {
                var result = JsonSerializer.Deserialize<UserInfo>(json);
                return result;
            }
            return null;

        }
    }

    public class UserInfo
    {
        public bool HasFinished { get; set; }
        public int GuessCount { get; set; } = 0;
        public DateTime GuessDate { get; set; }
    }
}
