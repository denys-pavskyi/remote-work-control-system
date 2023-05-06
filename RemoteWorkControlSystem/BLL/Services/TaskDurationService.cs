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
    public class TaskDurationService: ITaskDurationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;


        public TaskDurationService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }


        public async Task AddAsync(TaskDurationModel model)
        {
            ModelsValidation.TaskDurationModelValidation(model);
            var mappedTaskDuration = _mapper.Map<TaskDuration>(model);

            await _unitOfWork.TaskDurationRepository.AddAsync(mappedTaskDuration);
            await _unitOfWork.SaveAsync();
        }

        public async Task DeleteAsync(int modelId)
        {
            await _unitOfWork.TaskDurationRepository.DeleteByIdAsync(modelId);
            await _unitOfWork.SaveAsync();
        }

        public async Task<IEnumerable<TaskDurationModel>> GetAllAsync()
        {
            IEnumerable<TaskDuration> unmappedTaskDurations = await _unitOfWork.TaskDurationRepository.GetAllWithDetailsAsync();
            return _mapper.Map<IEnumerable<TaskDurationModel>>(unmappedTaskDurations);
        }

        public async Task<TaskDurationModel> GetByIdAsync(int id)
        {
            var unmappedTaskDuration = await _unitOfWork.TaskDurationRepository.GetByIdWithDetailsAsync(id);
            return _mapper.Map<TaskDurationModel>(unmappedTaskDuration);
        }

        public async Task UpdateAsync(TaskDurationModel model)
        {
            ModelsValidation.TaskDurationModelValidation(model);
            var mapped = _mapper.Map<TaskDuration>(model);
            _unitOfWork.TaskDurationRepository.Update(mapped);
            await _unitOfWork.SaveAsync();
        }

    }
}
