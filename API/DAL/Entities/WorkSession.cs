using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entities
{
    public class WorkSession: BaseEntity
    {
        [Required]
        public int ProjectMemberId { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        [Required]
        public float WorkTime { get; set; }

        public ProjectMember ProjectMember { get; set; }



    }
}
