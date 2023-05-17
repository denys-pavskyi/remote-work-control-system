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
    public class ProjectMemberService: IProjectMemberService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;


        public ProjectMemberService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }


        public async Task AddAsync(ProjectMemberModel model)
        {
            ModelsValidation.ProjectMemberModelValidation(model);
            var mappedProjectMember = _mapper.Map<ProjectMember>(model);
            await _unitOfWork.ProjectMemberRepository.AddAsync(mappedProjectMember);
            await _unitOfWork.SaveAsync();
        }

        public async Task DeleteAsync(int modelId)
        {
            await _unitOfWork.ProjectMemberRepository.DeleteByIdAsync(modelId);
            await _unitOfWork.SaveAsync();
        }

        public async Task<IEnumerable<ProjectMemberModel>> GetAllAsync()
        {
            IEnumerable<ProjectMember> unmappedProjectMembers = await _unitOfWork.ProjectMemberRepository.GetAllWithDetailsAsync();
            return _mapper.Map<IEnumerable<ProjectMemberModel>>(unmappedProjectMembers);
        }

        public async Task<ProjectMemberModel> GetByIdAsync(int id)
        {
            var unmappedProjectMember = await _unitOfWork.ProjectMemberRepository.GetByIdWithDetailsAsync(id);
            return _mapper.Map<ProjectMemberModel>(unmappedProjectMember);
        }

        public async Task UpdateAsync(ProjectMemberModel model)
        {
            ModelsValidation.ProjectMemberModelValidation(model);
            var mapped = _mapper.Map<ProjectMember>(model);
            _unitOfWork.ProjectMemberRepository.Update(mapped);
            await _unitOfWork.SaveAsync();
        }

        public async Task<ProjectMemberModel> GetByUserId_And_ProjectId_Async(int userId, int projectId)
        {
            var unmappedProjectMembers = await _unitOfWork.ProjectMemberRepository.GetAllAsync();
            var unmappedProjectMember = unmappedProjectMembers.FirstOrDefault(x => x.UserId == userId && x.ProjectId == projectId);
            return _mapper.Map<ProjectMemberModel>(unmappedProjectMember);
        }

        public async Task<IEnumerable<ProjectMemberModel>> GetByProjectId(int projectId)
        {
            IEnumerable<ProjectMember> unmappedProjectMembers = await _unitOfWork.ProjectMemberRepository.GetAllWithDetailsAsync();
            return _mapper.Map<IEnumerable<ProjectMemberModel>>(unmappedProjectMembers.Where(x => x.ProjectId == projectId));
        }
    }
}
