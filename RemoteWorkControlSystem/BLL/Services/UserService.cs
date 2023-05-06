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
    public class UserService: IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;


        public UserService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }


        public async Task AddAsync(UserModel model)
        {
            ModelsValidation.UserModelValidation(model);
            var mappedUser = _mapper.Map<User>(model);

            await _unitOfWork.UserRepository.AddAsync(mappedUser);
            await _unitOfWork.SaveAsync();
        }

        public async Task DeleteAsync(int modelId)
        {
            await _unitOfWork.UserRepository.DeleteByIdAsync(modelId);
            await _unitOfWork.SaveAsync();
        }

        public async Task<IEnumerable<UserModel>> GetAllAsync()
        {
            IEnumerable<User> unmappedUsers = await _unitOfWork.UserRepository.GetAllWithDetailsAsync();
            return _mapper.Map<IEnumerable<UserModel>>(unmappedUsers);
        }

        public async Task<UserModel> GetByIdAsync(int id)
        {
            var unmappedUser = await _unitOfWork.UserRepository.GetByIdWithDetailsAsync(id);
            return _mapper.Map<UserModel>(unmappedUser);
        }

        public async Task UpdateAsync(UserModel model)
        {
            ModelsValidation.UserModelValidation(model);
            var mapped = _mapper.Map<User>(model);
            _unitOfWork.UserRepository.Update(mapped);
            await _unitOfWork.SaveAsync();
        }
    }
}
