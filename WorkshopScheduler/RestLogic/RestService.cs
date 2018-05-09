using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PCLStorage;
using Plugin.DownloadManager;
using Plugin.DownloadManager.Abstractions;
using WorkshopScheduler.Logic;
using WorkshopScheduler.Models;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;

namespace WorkshopScheduler.RestLogic
{
    public class RestService : IRestService
    {
#if DEBUG
        private const string RestUri = "http://10.0.2.2:58165/";

#else
        private const string RestUri = "https://restnet.conveyor.cloud/";
#endif

        private const string RestUserInfo = "api/user/me";
        private const string RestAuthUri = "token";
        private const string RestWorkshopAllUri = "api/workshops/all";
        private const string RestWorkshops = "api/workshops/";
        private const string RestWorkshopMeUri = "api/workshops/me";
        private const string RestAddUser = "api/user/add";
        private const string RestEnrollUser = "api/workshops/enroll/";
        private const string RestDisenrollUser = "api/workshops/disenroll/";
        private const string RestEvalute = "api/workshops/evaluate/";
        private const string RestDaysWorkshop = "api/workshops/days";

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
                RequestUri = new Uri(RestUri + RestWorkshopAllUri),
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

        public async Task<RestResponse<List<WorkshopDTO>>> GetUserWorkshopAsync()
        {
            var restResponse = new RestResponse<List<WorkshopDTO>>();
            var request = new HttpRequestMessage()
            {
                RequestUri = new Uri(RestUri + RestWorkshopMeUri),
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
                RequestUri = new Uri(RestUri + RestWorkshops + id),
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
                RequestUri = new Uri(RestUri + RestUserInfo),
                Method = HttpMethod.Post,
                Content = new StringContent(JsonConvert.SerializeObject(userInfo), Encoding.UTF8, "application/json")
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
                RequestUri = new Uri(RestUri + RestUserInfo),
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
                RequestUri = new Uri(RestUri + RestAuthUri),
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
                RequestUri = new Uri(RestUri + RestAddUser),
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
                RequestUri = new Uri(RestUri + RestEnrollUser + workshopId),
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

        public async Task<RestResponse<bool>> DisenrollUser(int workshopId)
        {
            var restResponse = new RestResponse<Boolean>();
            var request = new HttpRequestMessage()
            {
                RequestUri = new Uri(RestUri + RestDisenrollUser + workshopId),
                Method = HttpMethod.Delete,
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

        public async Task<RestResponse<bool>> EvaluateWorkshop(int workshopId)
        {
            var restResponse = new RestResponse<Boolean>();
            var request = new HttpRequestMessage()
            {
                RequestUri = new Uri(RestUri + RestEvalute + workshopId),
                Method = HttpMethod.Put,
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

        public async Task<RestResponse<List<int>>> GetDaysWithWorkshop(int month, int year)
        {
            var restResponse = new RestResponse<List<int>>();
            var postString = String.Format("month={0}&year={1}", WebUtility.UrlEncode(month.ToString()),
                WebUtility.UrlEncode(year.ToString()));
            
            var request = new HttpRequestMessage()
            {
                RequestUri = new Uri(RestUri + RestDaysWorkshop + "?" + postString),
                Method = HttpMethod.Get,
            };
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", TokenManager.GetToken());
            try
            {
                var response = await _client.SendAsync(request);
                restResponse.ResponseCode = response.StatusCode;
                if (response.IsSuccessStatusCode)
                {
                    var responseContet = await response.Content.ReadAsStringAsync();
                    restResponse.Value = JsonConvert.DeserializeObject<List<int>>(responseContet);
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

        public async Task<RestResponse<bool>> SaveFile(string fileUri, string filename)
        {
            var restResponse = new RestResponse<bool>();
            //var fileDownloader = DependencyService.Get<IFileDownloader>();
            //fileDownloader.DownloadFile(fileUri);
            var downloadManager = CrossDownloadManager.Current;
            var file = downloadManager.CreateDownloadFile(fileUri);
            downloadManager.Start(file);

            restResponse.Value = true;
            return restResponse;
          
        }

        
    }
}