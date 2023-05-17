using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using BLL.Interfaces;
using BLL.Models;
using BLL.Validations;
using DAL.Entities;
using DAL.Interfaces;

namespace BLL.Services
{
    public class WorkSessionService: IWorkSessionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;


        public WorkSessionService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }


        public async Task AddAsync(WorkSessionModel model)
        {
            ModelsValidation.WorkSessionModelValidation(model);
            var mappedWorkSession = _mapper.Map<WorkSession>(model);

            try
            {
                await _unitOfWork.WorkSessionRepository.AddAsync(mappedWorkSession);
                await _unitOfWork.SaveAsync();
            }catch(Exception e)
            {

            }
            /*await _unitOfWork.WorkSessionRepository.AddAsync(mappedWorkSession);
            await _unitOfWork.SaveAsync();*/
        }

        public async Task DeleteAsync(int modelId)
        {
            await _unitOfWork.WorkSessionRepository.DeleteByIdAsync(modelId);
            await _unitOfWork.SaveAsync();
        }

        public async Task<IEnumerable<WorkSessionModel>> GetAllAsync()
        {
            IEnumerable<WorkSession> unmappedWorkSessions = await _unitOfWork.WorkSessionRepository.GetAllWithDetailsAsync();
            return _mapper.Map<IEnumerable<WorkSessionModel>>(unmappedWorkSessions);
        }

        public async Task<WorkSessionModel> GetByIdAsync(int id)
        {
            var unmappedWorkSession = await _unitOfWork.WorkSessionRepository.GetByIdWithDetailsAsync(id);
            return _mapper.Map<WorkSessionModel>(unmappedWorkSession);
        }

        public async Task UpdateAsync(WorkSessionModel model)
        {
            ModelsValidation.WorkSessionModelValidation(model);
            var mapped = _mapper.Map<WorkSession>(model);
            _unitOfWork.WorkSessionRepository.Update(mapped);
            await _unitOfWork.SaveAsync();
        }
    }
}
