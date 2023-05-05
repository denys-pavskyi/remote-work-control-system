using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Models
{
    public class TaskDurationModel
    {
        public int Id { get; set; }

        public string SprintId { get; set; }

        public string TaskId { get; set; }

        public float TimeSpent { get; set; }

        public int ProjectMemberId { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

    }
}
