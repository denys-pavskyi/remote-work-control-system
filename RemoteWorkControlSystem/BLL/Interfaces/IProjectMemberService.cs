using BLL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interfaces
{
    public interface IProjectMemberService : ICrud<ProjectMemberModel>
    {
        Task<ProjectMemberModel> GetByUserId_And_ProjectId_Async(int userId, int projectId);

        Task<IEnumerable<ProjectMemberModel>> GetByProjectId(int projectId);

        Task<string> GetFullName(int projectMemberId);
    }
}
