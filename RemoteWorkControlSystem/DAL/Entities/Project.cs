using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entities
{
    public class Project: BaseEntity
    {

        [Required, StringLength(40)]
        public string ProjectKey { get; set; }

        [Required, StringLength(40)]
        public string ProjectTitle { get; set; }

        [Required]
        public string JiraDomain { get; set; }

        [Required]
        public bool IsScreenActivityControlEnabled { get; set; }

        public int ScreenshotInterval { get; set; }
    
        public List<ProjectMember> ProjectMembers { get; set; }
    }
}
