using MessagePack;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace UnitedPolling.DataContext.Models
{
    public class PollQuestionOption
    {
        public int Id { get; set; }
        public int PollQuestionId { get; set; }

        [Column(TypeName = "VARCHAR(100)")]
        public string Text { get; set; }

        public virtual ICollection<PollQuestionResponse> PollQuestionResponse { get; set; }
    }
}
