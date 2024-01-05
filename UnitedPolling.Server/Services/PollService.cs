using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using UnitedPolling.Enums;
using UnitedPolling.DataContext.Models;
using UnitedPolling.DataContext;
using System.Linq;

namespace UnitedPolling.Services
{
    public interface IPollService
    {
        public Task<PollListDto> GetUserPolls(int userId);
        public Task<PollQuestionListDto> GetPollQuestionList(int pollId, int userId);
        public Task<PollDto> CreatePoll(CreatePollDto createPollDto, int userId);
    }

    public class PollService : IPollService
    {
        private readonly UnitedPollingDbContext _context;
        private readonly IUserService _userService;

        public PollService(UnitedPollingDbContext context, IUserService userService) 
        {
            _context = context;
            _userService = userService;
        }

        public PollDto GetPoll(int pollId, int userId)
        {
            var pollUserQuery = RetrieveUserPollsQuery(userId).Where(o => o.Id == pollId);
            var pollDto = SelectPollDto(pollUserQuery, userId).FirstOrDefault();

            return pollDto;
        }

        public async Task<PollListDto> GetUserPolls(int userId)
        {
            var pollListDto = new PollListDto();

            var pollUserQuery = RetrieveUserPollsQuery(userId);
            pollListDto.PollList = await SelectPollDto(pollUserQuery, userId).ToListAsync();

            return pollListDto;
        }

        public async Task<PollQuestionListDto> GetPollQuestionList(int pollId, int userId)
        {
            var poll = RetrieveUserPollsQuery(userId)
                .Where(p => p.Id == pollId).FirstOrDefault();

            if(poll == null)
            {
                throw new Exception($"Poll {pollId} was not found for user {userId}");
            }

            var pollQuestionListDto = new PollQuestionListDto() { PollQuestions = new List<PollQuestionDto>() };

            var pollQuestions = await _context.PollQuestion
                .Where(pq => pq.PollId == pollId)
                .Select(pq =>
                    new PollQuestionDto
                    {
                        Id = pq.Id,
                        QuestionType = pq.QuestionType,
                        IsRequired = pq.IsRequired,
                        Question = pq.Question,
                        OptionList = pq.PollQuestionOptions.Select(pqo =>
                            new PollQuestionOptionDto
                            {
                                Id = pqo.Id,
                                Text = pqo.Text,
                                ParticipantPercentage = pqo.PollQuestionResponse.Count() / pq.PollQuestionOptions.Sum(pqos => pqos.PollQuestionResponse.Count()),
                                UserResponse = pqo.PollQuestionResponse
                                    .Where(pqr => pqr.UserId == userId)
                                    .Select(pqr => new PollQuestionResponseDto
                                    {
                                        Id = pqr.Id,
                                        WrittenResponse = pqr.WrittenResponse
                                    })
                                    .FirstOrDefault()
                            }
                        ).ToList(),
                    }
                )
                .ToListAsync();

            if(pollQuestions != null)
            {
                pollQuestionListDto.PollQuestions = pollQuestions;
            }

            return pollQuestionListDto;
        }

        public async Task<PollDto> CreatePoll(CreatePollDto createPollDto, int userId)
        {
            if(createPollDto.Title.Length < 2 || createPollDto.Title.Length > 50)
            { 
                throw new Exception("Title must be between 2-50 characters!");    
            }

            var poll = new Poll
            {
                Title = createPollDto.Title,
                UrlShareable = createPollDto.UrlShareable,
                CreatedUserId = userId,
                CreatedDateTime = DateTime.UtcNow,
                UpdatedUserId = userId,
                UpdatedDateTime = DateTime.UtcNow
            };

            await _context.AddAsync(poll);
            await _context.SaveChangesAsync();

            return GetPoll(poll.Id, userId);
        }


        // Ensures users are only accessing 
        public IQueryable<Poll> RetrieveUserPollsQuery(int userId)
        {
            // Return if the user owns the poll, or is a participant of the poll.
            return _context.Poll.Where(p =>
                p.CreatedUserId == userId ||
                p.PollAdministrators.Where(pa => pa.UserId == userId).Count() > 0 ||
                p.PollParticipants.Where(pp => pp.UserId == userId).Count() > 0
            );
        }
        // Ensures users are only accessing 
        public IQueryable<PollDto> SelectPollDto(IQueryable<Poll> iQueryable, int userId)
        {
            // Return if the user owns the poll, or is a participant of the poll.
            return iQueryable
                .OrderByDescending(o => o.UpdatedDateTime)
                .Select(p => new PollDto
                {
                    Id = p.Id,
                    AdministratorNames = p.PollAdministrators.Select(o => o.User.Name).ToList(),
                    IsAdministrator = p.PollAdministrators.Where(pa => pa.UserId == userId).Count() > 0,
                    Title = p.Title,
                    NumberOfQuestions = p.PollQuestions.Count(),
                    UrlShareable = p.UrlShareable,
                    ParticipantsInvited = p.PollParticipants.Count(),
                    ParticipantsCompleted = p.PollParticipants.Where(pp => pp.CompletedDateTime != null).Count(),
                    CreatedDateTime = p.CreatedDateTime,
                    CreatedUserName = p.CreatedUser.Name,
                    UpdatedDateTime = p.UpdatedDateTime,
                    UpdatedUserName = p.UpdatedUser.Name,
                    UserCompletedDateTime = p.PollParticipants.Where(pp => pp.UserId == userId).Select(pp => pp.CompletedDateTime).FirstOrDefault(),
                }
            );
        }
    }

    public class PollListDto
    {
        public List<PollDto> PollList { get; set; }
    }

    public class PollDto
    {
        public int Id { get; set; }
        public List<string> AdministratorNames { get; set; }
        public bool IsAdministrator { get; set; }
        public string Title { get; set; }
        public int NumberOfQuestions { get; set; }
        public bool UrlShareable { get; set; }
        public int ParticipantsInvited { get; set; }
        public int ParticipantsCompleted { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public string CreatedUserName { get; set; }
        public DateTime UpdatedDateTime { get; set; }
        public string UpdatedUserName { get; set; }
        public DateTime? UserCompletedDateTime { get; set; }
    }

    public class PollQuestionListDto
    {
        public List<PollQuestionDto> PollQuestions { get; set; }
    }

    public class PollQuestionDto
    {
        public int Id { get; set; }
        public QuestionType QuestionType { get; set; }
        public bool IsRequired { get; set; }
        public string Question { get; set; }

        public List<PollQuestionOptionDto> OptionList { get; set; }
    }

    public class PollQuestionOptionDto
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public double ParticipantPercentage { get; set; }
        public PollQuestionResponseDto? UserResponse { get; set; }
    }

    public class PollQuestionResponseDto
    {
        public int Id { get; set; }
        public string? WrittenResponse { get; set; }
    }

    public class CreatePollDto
    {
        public string Title { get; set; }
        public bool UrlShareable { get; set; }
    }
}
