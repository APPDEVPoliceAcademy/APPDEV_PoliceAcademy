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

        public async Task<RestResponse<List<WorkshopDTO>>> GetAllWorkshopsAsync()
        {
            var restResponse = new RestResponse<List<WorkshopDTO>>();
            var request = new HttpRequestMessage()
            {
                RequestUri = new Uri(RestWorkshopAllUri),
                Method = HttpMethod.Get
            };
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", TokenManager.GetToken());
            try
            {
                var response = await _client.SendAsync(request);
                restResponse.ResponseCode = response.StatusCode;
                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var workshops = JsonConvert.DeserializeObject<List<WorkshopDTO>>(responseContent);
                    restResponse.Value = workshops;
                }
                else
                {
                    var content = await response.Content.ReadAsStringAsync();
                    restResponse.ErrorMessage = content;
                }
            }
            catch (Exception ex)
            {
                restResponse.ErrorMessage = ex.Message;
            }
            return restResponse;
        }

        public async Task<RestResponse<List<WorkshopDTO>>> GetUserWorkshopAsynch()
        {
            var restResponse = new RestResponse<List<WorkshopDTO>>();
            var request = new HttpRequestMessage()
            {
                RequestUri = new Uri(RestWorkshopMeUri),
                Method = HttpMethod.Get
            };

            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", TokenManager.GetToken());
            try
            {
                var response = await _client.SendAsync(request);
                restResponse.ResponseCode = response.StatusCode;
                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var workshops = JsonConvert.DeserializeObject<List<WorkshopDTO>>(responseContent);
                    restResponse.Value = workshops;
                }
                else
                {
                    var content = await response.Content.ReadAsStringAsync();
                    restResponse.ErrorMessage = content;
                }
            }
            catch (Exception ex)
            {
                restResponse.ErrorMessage = ex.Message;
            }
  
            return restResponse;
        }

        public async Task<RestResponse<Workshop>> GetSingleWorkshop(int id)
        {
            var restResponse = new RestResponse<Workshop>();
            var request = new HttpRequestMessage()
            {
                RequestUri = new Uri(RestWorkshops + id.ToString()),
                Method = HttpMethod.Get
            };

            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", TokenManager.GetToken());
            try
            {
                var response = await _client.SendAsync(request);
                restResponse.ResponseCode = response.StatusCode;
                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var workshop = JsonConvert.DeserializeObject<Workshop>(responseContent);  
                    restResponse.Value = workshop;
                }
                else
                {
                    var content = await response.Content.ReadAsStringAsync();
                    restResponse.ErrorMessage = content;
                }
            }
            catch (Exception ex)
            {
                restResponse.ErrorMessage = ex.Message;
            }
            return restResponse;
        }

        public async Task<RestResponse<Boolean>> UpdateUserInfo(UserInfo userInfo)
        {
            var restResponse = new RestResponse<Boolean>();
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
                restResponse.ResponseCode = response.StatusCode;
                if (response.IsSuccessStatusCode)
                {
                    restResponse.Value = true;
                }
                else
                {
                    var content = await response.Content.ReadAsStringAsync();
                    restResponse.ErrorMessage = content;
                }
            }
            catch (Exception ex)
            {
                restResponse.ErrorMessage = ex.Message;
            }

            return restResponse;
        }

        public async Task<RestResponse<UserInfo>> GetUserDetail()
        {
            var restResponse = new RestResponse<UserInfo>();
            var request = new HttpRequestMessage()
            {
                RequestUri = new Uri(RestUserInfo),
                Method = HttpMethod.Get
            };
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", TokenManager.GetToken());
            try
            {
                var response = await _client.SendAsync(request);
                restResponse.ResponseCode = response.StatusCode;
                if (response.IsSuccessStatusCode)
                {
                    var responseContet = await response.Content.ReadAsStringAsync();
                    restResponse.Value = JsonConvert.DeserializeObject<UserInfo>(responseContet);
                }
                else
                {
                    restResponse.ErrorMessage = await response.Content.ReadAsStringAsync();
                }
            }
            catch (Exception ex)
            {
                restResponse.ErrorMessage = ex.Message;
            }

            return restResponse;
        }

        public async Task<RestResponse<TokenInfo>> AuthenticateUser(string login, string password)
        {
            var restResponse = new RestResponse<TokenInfo>();
            var request = new HttpRequestMessage()
            {
                RequestUri = new Uri(RestAuthUri),
                Method = HttpMethod.Post,
                Content = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("UserName", login),
                    new KeyValuePair<string, string>("Password", password),
                    new KeyValuePair<string, string>("grant_type", "password"),
                })
            };

            try
            {
                var response = await _client.SendAsync(request);
                restResponse.ResponseCode = response.StatusCode;
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    restResponse.Value = JsonConvert.DeserializeObject<TokenInfo>(content);
                }
                else
                {
                    restResponse.ErrorMessage = await response.Content.ReadAsStringAsync();
                }
            }
            catch (Exception ex)
            {
                restResponse.ErrorMessage = ex.Message;
            }

            return restResponse;
        }

        public async Task<RestResponse<TokenInfo>> CreateUser(string login, string password)
        {
            var restResponse = new RestResponse<TokenInfo>();
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
                restResponse.ResponseCode = response.StatusCode;
                if (response.IsSuccessStatusCode)
                {
                    var responseContet = await response.Content.ReadAsStringAsync();
                    restResponse.Value = JsonConvert.DeserializeObject<TokenInfo>(responseContet);
                }
                else
                {
                    restResponse.ErrorMessage = await response.Content.ReadAsStringAsync();
                }
            }
            catch (Exception ex)
            {
                restResponse.ErrorMessage = ex.Message;
            }

            return restResponse;
        }

        public async Task<RestResponse<bool>> EnrollUser(int workshopId)
        {
            var restResponse = new RestResponse<Boolean>();
            var request = new HttpRequestMessage()
            {
                RequestUri = new Uri(RestWorkshops + workshopId),
                Method = HttpMethod.Post,
            };
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", TokenManager.GetToken());
            try
            {
                var response = await _client.SendAsync(request);
                restResponse.ResponseCode = response.StatusCode;
                if (response.IsSuccessStatusCode)
                {
                    restResponse.Value = true;
                }
                else
                {
                    var content = await response.Content.ReadAsStringAsync();
                    restResponse.ErrorMessage = content;
                }
            }
            catch (Exception ex)
            {
                restResponse.ErrorMessage = ex.Message;
            }

            return restResponse;
        }
    }
}