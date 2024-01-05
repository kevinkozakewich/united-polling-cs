using MessagePack;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace UnitedPolling.DataContext.Models
{
    public class PollParticipant
    {
        public int Id { get; set; }
        public int PollId { get; set; }
        public int UserId { get; set; }
        public DateTime? CompletedDateTime { get; set; }

        public virtual Poll Poll { get; set; }
        public virtual User User { get; set; }

    }
}
