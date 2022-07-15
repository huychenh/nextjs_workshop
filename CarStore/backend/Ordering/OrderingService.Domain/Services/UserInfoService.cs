using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderingService.AppCore.Services
{
    public class UserInfoService
    {
        private readonly HttpClient _httpClient = null!;

        public UserInfoService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string> GetUserEmail(Guid userId)
        {
            try
            {
                var result = await _httpClient.GetStringAsync($"api/users/get-user-email?userId={userId}");

                return result;
            }
            catch
            {
                return null;
            }
        }
    }
}
