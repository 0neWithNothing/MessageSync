using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Executor.Models
{
    public class GeneratorDb
    {
        [Key]
        public Guid Id { get; set; }
        public DateTime Date { get; set; }
        public List<TaskDb> Tasks { get; } = new List<TaskDb>();
    }
}
