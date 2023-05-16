using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RWCS_Desktop.Entities
{
    public class Project
    {
        public int Id { get; set; }

        public string ProjectKey { get; set; }

        public string ProjectTitle { get; set; }

        public bool IsScreenActivityControlEnabled { get; set; }

        public float ScreenshotInterval { get; set; }

        public string JiraDomain { get; set; }

        public List<int> ProjectMemberIds { get; set; }
    }
}
