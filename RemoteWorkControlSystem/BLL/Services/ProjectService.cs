using AutoMapper;
using BLL.Interfaces;
using BLL.Models;
using BLL.Validations;
using DAL.Entities;
using DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services
{
    public class ProjectService: IProjectService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;


        public ProjectService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }


        public async Task AddAsync(ProjectModel model)
        {
            ModelsValidation.ProjectModelValidation(model);
            var mappedProject = _mapper.Map<Project>(model);

            await _unitOfWork.ProjectRepository.AddAsync(mappedProject);
            await _unitOfWork.SaveAsync();
        }

        public async Task DeleteAsync(int modelId)
        {
            await _unitOfWork.ProjectRepository.DeleteByIdAsync(modelId);
            await _unitOfWork.SaveAsync();
        }

        public async Task<IEnumerable<ProjectModel>> GetAllAsync()
        {
            IEnumerable<Project> unmappedProjects = await _unitOfWork.ProjectRepository.GetAllWithDetailsAsync();
            return _mapper.Map<IEnumerable<ProjectModel>>(unmappedProjects);
        }

        public Task<ProjectModel> GetByDomainAndProjectNameAsync(string domain_name, string project_key)
        {
            throw new NotImplementedException();
        }

        public async Task<ProjectModel> GetByIdAsync(int id)
        {
            var unmappedProject = await _unitOfWork.ProjectRepository.GetByIdWithDetailsAsync(id);
            return _mapper.Map<ProjectModel>(unmappedProject);
        }

        public async Task UpdateAsync(ProjectModel model)
        {
            ModelsValidation.ProjectModelValidation(model);
            var mapped = _mapper.Map<Project>(model);
            _unitOfWork.ProjectRepository.Update(mapped);
            await _unitOfWork.SaveAsync();
        }
    }
}
