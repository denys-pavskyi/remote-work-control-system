using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entities
{
    public class TaskDuration: BaseEntity
    {
        [Required]
        public string SprintId { get; set; }

        [Required]
        public string TaskId { get; set; }

        [Required]
        public float TimeSpent { get; set; }

        [Required]
        public int ProjectMemberId { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        public ProjectMember ProjectMember { get; set; }


    }
}
