using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RWCS_Desktop.Entities
{
    public class EmployeeScreenActivity
    {
        public int Id { get; set; }

        public DateTime Date { get; set; }

        public string ScreenshotURL { get; set; }

        public int ProjectMemberId { get; set; }
    }
}
