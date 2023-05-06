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
    public class EmployeeScreenActivityService: IEmployeeScreenActivityService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;


        public EmployeeScreenActivityService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }


        public async Task AddAsync(EmployeeScreenActivityModel model)
        {
            ModelsValidation.EmployeeScreenActivityModelValidation(model);
            var mappedEmployeeScreenActivity = _mapper.Map<EmployeeScreenActivity>(model);

            await _unitOfWork.EmployeeScreenActivityRepository.AddAsync(mappedEmployeeScreenActivity);
            await _unitOfWork.SaveAsync();
        }

        public async Task DeleteAsync(int modelId)
        {
            await _unitOfWork.EmployeeScreenActivityRepository.DeleteByIdAsync(modelId);
            await _unitOfWork.SaveAsync();
        }

        public async Task<IEnumerable<EmployeeScreenActivityModel>> GetAllAsync()
        {
            IEnumerable<EmployeeScreenActivity> unmappedEmployeeScreenActivitys = await _unitOfWork.EmployeeScreenActivityRepository.GetAllWithDetailsAsync();
            return _mapper.Map<IEnumerable<EmployeeScreenActivityModel>>(unmappedEmployeeScreenActivitys);
        }

        public async Task<EmployeeScreenActivityModel> GetByIdAsync(int id)
        {
            var unmappedEmployeeScreenActivity = await _unitOfWork.EmployeeScreenActivityRepository.GetByIdWithDetailsAsync(id);
            return _mapper.Map<EmployeeScreenActivityModel>(unmappedEmployeeScreenActivity);
        }

        public async Task UpdateAsync(EmployeeScreenActivityModel model)
        {
            ModelsValidation.EmployeeScreenActivityModelValidation(model);
            var mapped = _mapper.Map<EmployeeScreenActivity>(model);
            _unitOfWork.EmployeeScreenActivityRepository.Update(mapped);
            await _unitOfWork.SaveAsync();
        }
    }
}
