using TweetWebAPI.Dto;
using TweetWebAPI.Models;

namespace TweetWebAPI.Services
{
    public interface IUserService
    {
        Task<ServiceResponse<List<UserDto>>> GetAllUsers();

        Task<ServiceResponse<List<UserDto>>> SearchUsers(string loginId);

        Task<ServiceResponse<int>> Register(UserDto user, string password);

        Task<ServiceResponse<string>> Login(string loginId, string password);

        Task<ServiceResponse<int>> ForgotPassword(string loginId, string password, string confirmPassword);

        Task<bool> IsLoginIdExists(string loginId);

        Task<bool> IsEmailExists(string email);

        Task<bool> IsConfirmPasswordValid(string password, string confirmPassword);
    }
}
