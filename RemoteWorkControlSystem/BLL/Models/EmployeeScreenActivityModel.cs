using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Models
{
    public class EmployeeScreenActivityModel
    {
        public int Id { get; set; }

        public DateTime Date { get; set; }

        public string ScreenshotURL { get; set; }

        public int ProjectMemberId { get; set; }

        public int WorkSessionId { get; set; }
    }
}
