using UnitedPolling.Services;

namespace UnitedPolling.Tests.Utilities
{
    public class PollServiceTests : InitializeServices
    {
        [Fact]
        public async Task GetUserPolls()
        {
            Init();

            var result = await _pollService.GetUserPolls(users.First().AspId);

            Assert.True(result != null && result.PollList != null);
            Assert.True(result.PollList.Count > 0);
        }
    }
}