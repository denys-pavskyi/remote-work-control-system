﻿using DAL.Interfaces;
using DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Data
{
    public class UnitOfWork: IUnitOfWork
    {
        private readonly RemoteWorkControlSystemDbContext dbContext;

        private EmployeeScreenActivityRepository employeeScreenActivityRepository;
        private ProjectMemberRepository projectMemberRepository;
        private TaskDurationRepository taskDurationRepository;
        private UserRepository userRepository;
        private WorkSessionRepository workSessionRepository;


        public UnitOfWork(RemoteWorkControlSystemDbContext context)
        {
            dbContext = context;
        }


        public IEmployeeScreenActivityRepository EmployeeScreenActivityRepository
        {
            get
            {
                if (employeeScreenActivityRepository == null)
                {
                    employeeScreenActivityRepository = new EmployeeScreenActivityRepository(dbContext);
                }
                return employeeScreenActivityRepository;
            }
        }

        public IProjectMemberRepository ProjectMemberRepository
        {
            get
            {
                if (projectMemberRepository == null)
                {
                    projectMemberRepository = new ProjectMemberRepository(dbContext);
                }
                return projectMemberRepository;
            }
        }

        public ITaskDurationRepository TaskDurationRepository
        {
            get
            {
                if (taskDurationRepository == null)
                {
                    taskDurationRepository = new TaskDurationRepository(dbContext);
                }
                return taskDurationRepository;
            }
        }

        public IUserRepository UserRepository
        {
            get
            {
                if (userRepository == null)
                {
                    userRepository = new UserRepository(dbContext);
                }
                return userRepository;
            }
        }

        public IWorkSessionRepository WorkSessionRepository
        {
            get
            {
                if (WorkSessionRepository == null)
                {
                    workSessionRepository = new WorkSessionRepository(dbContext);
                }
                return workSessionRepository;
            }
        }

        public async Task SaveAsync()
        {
            await dbContext.SaveChangesAsync();
        }

    }
}
