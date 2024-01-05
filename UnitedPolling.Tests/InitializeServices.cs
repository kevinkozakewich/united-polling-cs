using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnitedPolling.DataContext;
using UnitedPolling.DataContext.Models;
using UnitedPolling.Services;

namespace UnitedPolling.Tests
{
    public abstract class InitializeServices
    {
        protected UnitedPollingDbContext _context;
        protected PollService _pollService;
        protected UserService _userService;


        public void Init()
        {
            SetupInMemoryDb();

            _context.Poll.AddRange(polls);
            _context.PollAdministrators.AddRange(pollAdministrators);
            _context.User.AddRange(users);

            _context.SaveChanges();

            _userService = new UserService(_context);

            _pollService = new PollService(_context, _userService);
        }

        public void SetupInMemoryDb()
        {
            var tempProductionDatabaseName = "test_" + DateTime.Now.ToFileTimeUtc();

            var productionOptions = new DbContextOptionsBuilder<UnitedPollingDbContext>()
                            .UseInMemoryDatabase(databaseName: tempProductionDatabaseName)
                            .Options;

            _context = new UnitedPollingDbContext(productionOptions);
        }

        public void CleanUp()
        {
            _context.Dispose();
        }

        public List<Poll> polls = new List<Poll>
        {
            new Poll
            {
                Id = 1,
                Title = "Poll1"
            }
        };

        public List<PollAdministrator> pollAdministrators = new List<PollAdministrator>
        {
            new PollAdministrator
            {
                Id = 1,
                PollId = 1,
                UserId = 1
            }
        };

        public List<User> users = new List<User>
        {
            new User
            {
                Id = 1,
                Name = "User1",
                AspId = "123",
                Email = "email@noreply.com",
            }
        };
    }
}
