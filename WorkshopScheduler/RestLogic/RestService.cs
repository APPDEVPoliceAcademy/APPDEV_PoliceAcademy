using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using WorkshopScheduler.Logic;
using WorkshopScheduler.Models;
using Xamarin.Forms;

namespace WorkshopScheduler.RestLogic
{
    public class RestService : IRestService
    {
        private const string RestUserInfo = "http://10.0.2.2:58165/api/user/me";
        private const string RestAuthUri = "http://10.0.2.2:58165/token";
        private const string RestWorkshopAllUri = "http://10.0.2.2:58165/api/workshops/all";
        private const string RestWorkshopMeUri = "http://10.0.2.2:58165/api/workshops/me";
        private const string RestWorkshops = "http://10.0.2.2:58165/api/workshops/";
        private const string RestAddUser = "http://10.0.2.2:58165/api/user/add";
        private HttpClient _client;

        public RestService()
        {
            _client = new HttpClient(new HttpClientHandler());
            _client.MaxResponseContentBufferSize = 256000;
        }

        public async Task<List<WorkshopDTO>> GetAllWorkshopsAsync()
        {
            var request = new HttpRequestMessage()
            {
                RequestUri = new Uri(RestWorkshopAllUri),
                Method = HttpMethod.Get
            };
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", TokenManager.GetToken());
            try
            {
                var response = await _client.SendAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<List<WorkshopDTO>>(responseContent);
                }
                else
                {
                    var content = await response.Content.ReadAsStringAsync();
                    Debug.WriteLine(@"ERROR {0}", content);

                    return null;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(@"ERROR {0}", ex.InnerException.Message);
                return null;
            }
        }


        public async Task<List<WorkshopDTO>> GetUserWorkshopAsynch()
        {
            var request = new HttpRequestMessage()
            {
                RequestUri = new Uri(RestWorkshopMeUri),
                Method = HttpMethod.Get
            };

            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", TokenManager.GetToken());
            try
            {
                var response = await _client.SendAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<List<WorkshopDTO>>(responseContent);
                }
                else
                {
                    var content = await response.Content.ReadAsStringAsync();
                    Debug.WriteLine(@"ERROR {0}", content);

                    return null;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(@"ERROR {0}", ex.InnerException.Message);
                return null;
            }
        }

        public async Task<Workshop> GetSingleWorkshop(int id)
        {
            var request = new HttpRequestMessage()
            {
                RequestUri = new Uri(RestWorkshops + id.ToString()),
                Method = HttpMethod.Get
            };

            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", TokenManager.GetToken());
            try
            {
                var response = await _client.SendAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var workshop = JsonConvert.DeserializeObject<Workshop>(responseContent);
                    return workshop;
                }
                else
                {
                    var content = await response.Content.ReadAsStringAsync();
                    Debug.WriteLine(@"ERROR {0}", content);

                    return null;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(@"ERROR {0}", ex.InnerException.Message);
                return null;
            }
        }

        public async Task<bool> UpdateUserInfo(UserInfo userInfo)
        {
            var request = new HttpRequestMessage()
            {
                RequestUri = new Uri(RestUserInfo),
                Method = HttpMethod.Post,
                Content = new StringContent(JsonConvert.SerializeObject(userInfo))
            };
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", TokenManager.GetToken());
            try
            {
                var response = await _client.SendAsync(request);
                if (response.IsSuccessStatusCode)
                    return true;
                else
                {
                    var content = await response.Content.ReadAsStringAsync();
                    Debug.WriteLine(@"ERROR {0}", content);
                    return false;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(@"ERROR {0}", ex.InnerException.Message);
                return false;
            }
        }

        public async Task<UserInfo> GetUserDetail()
        {
            var userInfo = new UserInfo();
            var request = new HttpRequestMessage()
            {
                RequestUri = new Uri(RestUserInfo),
                Method = HttpMethod.Get
            };
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", TokenManager.GetToken());
            try
            {
                var response = await _client.SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    var responseContet = await response.Content.ReadAsStringAsync();
                    var app = Application.Current as App;
                    return JsonConvert.DeserializeObject<UserInfo>(responseContet);
                }
                else
                {
                    var content = await response.Content.ReadAsStringAsync();
                    Debug.WriteLine(@"ERROR {0}", content);
                    return null;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(@"ERROR {0}", ex.InnerException.Message);
                return null;
            }
        }

        public async Task<TokenInfo> AuthenticateUser(string login, string password)
        {
            var tokenInfo = new TokenInfo();
            var uri = new Uri(string.Format(RestAuthUri, string.Empty));
            var requestContent = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("UserName", login),
                new KeyValuePair<string, string>("Password", password),
                new KeyValuePair<string, string>("grant_type", "password"),
            });
            try
            {
                Debug.WriteLine(@"ERROR {0}", "Before response");
                var response = await _client.PostAsync(uri, requestContent);
                Debug.WriteLine(@"ERROR {0}", "After response");
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    tokenInfo = JsonConvert.DeserializeObject<TokenInfo>(content);
                    return tokenInfo;
                }
                else
                {
                    var content = await response.Content.ReadAsStringAsync();
                    Debug.WriteLine(@"ERROR {0}", content);
                    return null;
                }
            }
            catch (HttpRequestException ex)
            {
                Debug.WriteLine(@"ERROR {0}", ex.InnerException.Message);
                return null;
            }
        }

        public async Task<TokenInfo> CreateUser(string login, string password)
        {
            var userInfo = new UserInfo();
            var request = new HttpRequestMessage()
            {
                RequestUri = new Uri(RestAddUser),
                Method = HttpMethod.Post,
                Content = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("Login", login),
                    new KeyValuePair<string, string>("Password", password),
                })
            };
            try
            {
                var response = await _client.SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    var responseContet = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<TokenInfo>(responseContet);
                }
                else
                {
                    var content = await response.Content.ReadAsStringAsync();
                    Debug.WriteLine(@"ERROR {0}", content);
                    return null;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(@"ERROR {0}", ex.InnerException.Message);
                return null;
            }
        }
    }
}