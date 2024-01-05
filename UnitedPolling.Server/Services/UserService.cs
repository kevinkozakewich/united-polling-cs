using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using UnitedPolling.Enums;
using UnitedPolling.DataContext.Models;
using UnitedPolling.DataContext;
using System.Security.Claims;

namespace UnitedPolling.Services
{
    public interface IUserService
    {
        public Task<int> GetIdByAspUser(ClaimsPrincipal UserId);
        public Task<UserDto> GetUserData(int UserId);
    }

    public class UserService : IUserService
    {
        private readonly UnitedPollingDbContext _context;
        public UserService(UnitedPollingDbContext context) 
        {
            _context = context;
        }

        public async Task<int> GetIdByAspUser(ClaimsPrincipal user)
        {
            var aspUserId = user.FindFirstValue(ClaimTypes.NameIdentifier);
            var name = user.FindFirstValue("name");
            var email = user.FindFirstValue("preferred_username");

            var userFound = _context.User.Where(u => u.AspId == aspUserId).FirstOrDefault();

            if (userFound == null)
            {
                if (name == null)
                {
                    throw new Exception("Unable to retrieve name from user");
                }
                if (email == null)
                {
                    throw new Exception("Unable to retrieve email from user");
                }
                if (!email.Contains("@"))
                {
                    throw new Exception($"Invalid email from user: {email}");
                }

                var userToAdd = new User
                { 
                    AspId = aspUserId,
                    Name = name,
                    Email = email,
                    LastLoggedIn = DateTime.UtcNow
                };

                await _context.User.AddAsync(userToAdd);
                await _context.SaveChangesAsync();

                return userToAdd.Id;
            }
            else 
            { 
                userFound.LastLoggedIn = DateTime.UtcNow;
                return userFound.Id; 
            }
        }
        public async Task<UserDto> GetUserData(int userId)
        {
            var user = _context.User.Where(u => u.Id == userId).FirstOrDefault();

            if (user == null)
            {
                throw new Exception($"Asp User was not found for Id: {userId}");
            }

            var userDto = new UserDto()
            {
                UserName = user.Name
            };

            return userDto;
        }
    }

    public class UserDto
    {
        public string UserName { get; set; }
    }
}
