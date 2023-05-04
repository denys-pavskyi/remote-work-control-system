using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entities
{
    public class ProjectMember: BaseEntity
    {
        [Required]
        public int UserId { get; set; }

        [Required, StringLength(40)]
        public string ProjectKey { get; set; }

        [Required, StringLength(40)]
        public string ProjectTitle { get; set; }

        [Required]
        public UserRole Role { get; set; }

        public User User { get; set; }
        public List<TaskDuration> TaskDurations { get; set; }
        public List<WorkSession> WorkSessions { get; set; }
        public List<EmployeeScreenActivity> EmployeeScreenActivities { get; set; }
    }

    public enum UserRole
    {
        Developer,
        AgileManager
    }
}
