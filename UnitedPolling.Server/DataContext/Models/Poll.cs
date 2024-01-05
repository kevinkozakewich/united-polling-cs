using MessagePack;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace UnitedPolling.DataContext.Models
{
    public class Poll
    {
        public int Id { get; set; }

        [Column(TypeName = "VARCHAR(100)")]
        public string Title { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public DateTime UpdatedDateTime { get; set; }
        public bool UrlShareable { get; set; }

        public int CreatedUserId { get; set; }
        public User CreatedUser { get; set; }
        public int UpdatedUserId { get; set; }
        public User UpdatedUser { get; set; }
        public virtual ICollection<PollAdministrator> PollAdministrators { get; set; }
        public virtual ICollection<PollParticipant> PollParticipants { get; set; }
        public virtual ICollection<PollQuestion> PollQuestions { get; set; }

    }
}
