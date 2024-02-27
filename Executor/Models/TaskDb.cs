using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Executor.Models
{
    public class TaskDb
    {
        [Key]
        public int Id { get; set; }
        public int Number { get; set; }
        public int Data { get; set; }
        public Guid GeneratorDbId { get; set; }
        public GeneratorDb? Generator { get; set; }
        public bool IsCopmleted { get; set; }
    }
}
