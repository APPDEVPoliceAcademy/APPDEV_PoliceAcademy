using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WorkshopScheduler.Models;

namespace WorkshopScheduler.RestLogic
{
    public interface IRestService
    {
        Task<RestResponse<List<WorkshopDTO>>> GetAllWorkshopsAsync();

        Task<RestResponse<List<WorkshopDTO>>> GetUserWorkshopAsync();

        Task<RestResponse<Workshop>> GetSingleWorkshop(int id);

        Task<RestResponse<UserInfo>> GetUserDetail();

        Task<RestResponse<Boolean>> UpdateUserInfo(UserInfo userInfo);

        Task<RestResponse<TokenInfo>> AuthenticateUser(string login, string password);

        Task<RestResponse<TokenInfo>> CreateUser(string login, string password);

        Task<RestResponse<Boolean>> EnrollUser(int workshopId);

        Task<RestResponse<Boolean>> DisenrollUser(int workshopId);

        Task<RestResponse<Boolean>> EvaluateWorkshop(int workshopId);

        Task<RestResponse<Boolean>> SaveFile(string fileUri, string filename);

        Task<RestResponse<List<int>>> GetDaysWithWorkshop(int month, int year);

        Task<RestResponse<List<WorkshopDTO>>> GetWorkshopsForDay(int year, int month, int day);

    }
}
