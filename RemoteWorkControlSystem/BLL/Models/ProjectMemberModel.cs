﻿using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Models
{
    public class ProjectMemberModel
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public int ProjectId { get; set; }

        public UserRole Role { get; set; }

        public List<int> WorkSessionIds { get; set; }
        public List<int> EmployeeScreenActivityIds { get; set; }
    }

}
