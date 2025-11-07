using System.Text;
using Hangfire.Annotations;
using Hangfire.Dashboard;

namespace WebApplication1
{
    public class MyAuthorizationFilter : IDashboardAuthorizationFilter
    {
        private readonly string _username;
        private readonly string _password;

        public MyAuthorizationFilter(IConfiguration configuration)
        {
            // Получение данных из файла
            _username = configuration["Hangfire:Username"];
            _password = configuration["Hangfire:Password"];
        }

        // Авторизация
        public bool Authorize(DashboardContext context)
        {
            var request = context.GetHttpContext().Request;
            var authorizationHeader = request.Headers["Authorization"].ToString();

            // Если заголовок авторизации пустой, то отправляем запрос на ввод логина и пароля
            if (string.IsNullOrEmpty(authorizationHeader))
            {
                context.GetHttpContext().Response.StatusCode = 401;
                context.GetHttpContext().Response.Headers.Add("WWW-Authenticate", "Basic realm=\"Hangfire Dashboard\"");
                return false;
            }

            // Разбиваем для проверки значения
            var authHeader = authorizationHeader.Substring("Basic ".Length).Trim();
            var credentialBytes = Convert.FromBase64String(authHeader);
            var credentials = Encoding.ASCII.GetString(credentialBytes).Split(':');

            var username = credentials[0];
            var password = credentials[1];

            return username == _username && password == _password;
        }
    }
}
