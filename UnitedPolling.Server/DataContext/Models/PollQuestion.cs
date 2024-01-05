using MessagePack;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using UnitedPolling.Enums;

namespace UnitedPolling.DataContext.Models
{
    public class PollQuestion
    {
        public int Id { get; set; }
        public int PollId { get; set; }
        public QuestionType QuestionType { get; set; }
        public bool IsRequired { get; set; }

        [Column(TypeName = "VARCHAR(250)")]
        public string Question { get; set; }

        public virtual ICollection<PollQuestionOption> PollQuestionOptions { get; set; }
    }
}
