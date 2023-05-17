﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RWCS_Desktop.Entities
{
    public class WorkSession
    {
        public int Id { get; set; }

        public int ProjectMemberId { get; set; }

        public string SprintKey { get; set; }

        public string TaskKey { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public float WorkTime { get; set; }
    }
}