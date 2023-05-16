using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entities
{
    public class WorkSession: BaseEntity
    {
        [Required]
        [ForeignKey("ProjectMembers")]
        public int ProjectMemberId { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public string SprintKey { get; set; }

        [Required]
        public string TaskKey { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        [Required]
        public float WorkTime { get; set; }

        public ProjectMember ProjectMember { get; set; }



    }
}
