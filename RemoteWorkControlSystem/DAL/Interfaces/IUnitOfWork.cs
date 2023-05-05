using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Interfaces
{
    public interface IUnitOfWork
    {
        IEmployeeScreenActivityRepository EmployeeScreenActivityRepository { get; }
        IProjectMemberRepository ProjectMemberRepository { get; }
        ITaskDurationRepository TaskDurationRepository { get; }
        IUserRepository UserRepository { get; }
        IWorkSessionRepository WorkSessionRepository { get; }
       
        Task SaveAsync();
    }
}
