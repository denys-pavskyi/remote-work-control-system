using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Models
{
    public class ProjectModel
    {
        public string ProjectKey { get; set; }

        public string ProjectTitle { get; set; }

        public bool IsScreenActivityControlEnabled { get; set; }

        public float ScreenshotInterval { get; set; }

        public List<int> ProjectMemberIds { get; set; }
    }
}
