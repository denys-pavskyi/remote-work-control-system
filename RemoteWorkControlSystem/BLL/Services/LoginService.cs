using AutoMapper;
using BLL.Interfaces;
using BLL.Models;
using BLL.Validations;
using DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services
{
    public class LoginService : ILoginService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public LoginService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<LoginResponse> LoginAsync(LoginRequest loginRequest)
        {
            var users = await _unitOfWork.UserRepository.GetAllWithNoTrackingAsync();

            var currentUserUnmapped = users.FirstOrDefault(x => x.UserName.ToLower() == loginRequest.UserName.ToLower());
            var currentUser = _mapper.Map<UserModel>(currentUserUnmapped);
            if (currentUser == null)
            {
                throw new RemoteWorkControlSystemException("User wasn't found");
            }

            if (currentUser.Password != loginRequest.Password)
            {
                throw new RemoteWorkControlSystemException("User wasn't found");
            }

            //TODO
            //var token = await Task.Run(() => TokenHelper.GenerateToken(currentUser));

            //return new LoginResponse { Username = currentUser.UserName, Id = currentUser.Id, Role = currentUser.Role.ToString(), Token = token };
            return new LoginResponse { };
        }



    }
}
