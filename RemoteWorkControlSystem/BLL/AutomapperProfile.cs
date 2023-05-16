using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL.Models;
using DAL.Entities;

namespace BLL
{
    public class AutomapperProfile : Profile
    {
        public AutomapperProfile()
        {
            CreateMap<EmployeeScreenActivity, EmployeeScreenActivityModel>();

            CreateMap<EmployeeScreenActivityModel, EmployeeScreenActivity>()
                .ForMember(esa => esa.ProjectMember, esam => esam.UseDestinationValue());

            CreateMap<ProjectMember, ProjectMemberModel>()
                .ForMember(pmm => pmm.WorkSessionIds, pm => pm.MapFrom(x => x.WorkSessions.Select(x => x.Id)))
                .ForMember(pmm => pmm.EmployeeScreenActivityIds, pm => pm.MapFrom(x => x.EmployeeScreenActivities.Select(x => x.Id)));
                
            

            CreateMap<ProjectMemberModel, ProjectMember>()
                .ForMember(pm => pm.User, pmm => pmm.UseDestinationValue())
                .ForMember(pm => pm.Project, pmm => pmm.UseDestinationValue())
                .ForMember(pm => pm.EmployeeScreenActivities, pmm => new List<EmployeeScreenActivity>())
                .ForMember(pm => pm.WorkSessions, pmm => new List<WorkSession>());


            CreateMap<User, UserModel>()
                .ForMember(um => um.ProjectMemberIds, u => u.MapFrom(x => x.ProjectMembers.Select(x => x.Id)));

            CreateMap<UserModel, User>();


            CreateMap<WorkSession, WorkSessionModel>();

            CreateMap<WorkSessionModel, WorkSession>()
                .ForMember(wsm => wsm.ProjectMember, ws => ws.UseDestinationValue());

            CreateMap<Project, ProjectModel>()
                .ForMember(pm => pm.ProjectMemberIds, p => p.MapFrom(x => x.ProjectMembers.Select(x => x.Id)))
                .ReverseMap();
        }
    }
}
