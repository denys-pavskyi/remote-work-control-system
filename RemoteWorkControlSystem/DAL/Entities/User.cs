using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entities
{
    public class User: BaseEntity
    {

        [Required, StringLength(60)]
        public string FirstName { get; set; }

        [Required, StringLength(60)]
        public string LastName { get; set; }

        [Required, StringLength(20)]
        public string UserName { get; set; }

        [Required, StringLength(20)]
        public string Password { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        public string JiraBaseUrl { get; set; }

        public string JiraApiKey { get; set; }

        public List<ProjectMember> ProjectMembers { get; set; }


    }

    

}
