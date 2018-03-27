using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkshopScheduler.Models;

namespace WorkshopScheduler.RestLogic
{
    public interface IRestService
    {
        Task<List<WorkshopDTO>> GetAllWorkshopsAsync();

        Task<List<WorkshopDTO>> GetUserWorkshopAsynch();

        Task<Workshop> GetSingleWorkshop(int id);

        Task<UserInfo> GetUserDetail();

        Task<bool> UpdateUserInfo(UserInfo userInfo);

        Task<TokenInfo> AuthenticateUser(String login, String password);

        Task<TokenInfo> CreateUser(String login, String password);
    }
}
