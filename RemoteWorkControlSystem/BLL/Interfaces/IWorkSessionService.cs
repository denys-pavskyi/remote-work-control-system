using BLL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interfaces
{
    public interface IWorkSessionService : ICrud<WorkSessionModel>
    {
        Task<IEnumerable<WorkSessionModel>> GetAllByProjectMemberId(int projectMemberId);
        Task<IEnumerable<WorkSessionModel>> GetAllByProjectId(int projectId);
    }
}
