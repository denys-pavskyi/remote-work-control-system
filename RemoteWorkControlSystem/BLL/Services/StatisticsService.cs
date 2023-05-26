using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL.Models;
using BLL.Interfaces;
using DAL.Interfaces;
using AutoMapper;
using DAL.Entities;

namespace BLL.Services
{
    public class StatisticsService: IStatisticsService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;


        public StatisticsService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }


        public async Task<StatisticsData> GetStatisticsByDataFrame(DateTime st, DateTime end, int projectMemberId)
        {
            IEnumerable<WorkSession> unmappedWorkSessions = await _unitOfWork.WorkSessionRepository.GetAllWithDetailsAsync();
            var sessions = unmappedWorkSessions.Where(x => x.ProjectMemberId == projectMemberId).Where(x => x.StartDate>=st && x.EndDate<=end);

            int tasks = 0;
            float minutes = 0;
            HashSet<(string sprint, string task)> unique_tasks = new HashSet<(string sprint, string task)>();

            foreach (var session in sessions)
            {
                minutes += session.WorkTime;
                unique_tasks.Add((session.SprintKey, session.TaskKey));
            }

            
            return new StatisticsData { HoursWorking = minutes/60f, NumberOfTasks = unique_tasks.Count() };
        }
    
    
    }
}
