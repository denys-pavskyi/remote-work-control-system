using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Interfaces
{
    public interface IUnitOfWork
    {
        IProjectMemberRepository ProjectMemberRepository { get; }
        IUserRepository UserRepository { get; }
        IWorkSessionRepository WorkSessionRepository { get; }
        IProjectRepository ProjectRepository { get; }
       
        Task SaveAsync();
    }
}
