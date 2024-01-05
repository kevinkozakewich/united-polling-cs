using MessagePack;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections.Generic;

namespace UnitedPolling.DataContext.Models
{
    public class User
    {
        public int Id { get; set; }
        public string AspId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public DateTime LastLoggedIn { get; set; }
        public virtual ICollection<Poll> CreatedUsers { get; set; }
        public virtual ICollection<Poll> UpdatedUsers { get; set; }
        public virtual ICollection<PollAdministrator> PollAdministrators { get; set; }
        public virtual ICollection<PollParticipant> PollParticipants { get; set; }
        public virtual ICollection<PollQuestionResponse> PollQuestionReponse { get; set; }
    }
}
