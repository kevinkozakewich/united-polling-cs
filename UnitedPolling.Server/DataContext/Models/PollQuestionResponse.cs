using MessagePack;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace UnitedPolling.DataContext.Models
{
    public class PollQuestionResponse
    {
        public int Id { get; set; }
        public int PollQuestionOptionId { get; set; }
        public int UserId { get; set; }

        [Column(TypeName = "VARCHAR(500)")]
        public string? WrittenResponse { get; set; }

        public virtual User User { get; set; }
        public virtual PollQuestionOption PollQuestionOption { get; set; }
    }
}
