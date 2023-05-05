using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entities
{
    public class EmployeeScreenActivity: BaseEntity
    {
        [Required]
        public DateTime Date { get; set; }

        [Required]
        public string ScreenshotURL { get; set; }

        [Required]
        public int ProjectMemberId { get; set; }

        public ProjectMember ProjectMember { get; set; }
    }
}
