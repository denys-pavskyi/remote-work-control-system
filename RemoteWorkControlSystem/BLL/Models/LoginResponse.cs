using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Models
{
    public class LoginResponse
    {
        public string Username { get; set; } = string.Empty;

        public int? Id { get; set; }

        public string Email { get; set; }

        public string JiraBaseUrl { get; set; }

        public string JiraApiKey { get; set; }
    }
}
