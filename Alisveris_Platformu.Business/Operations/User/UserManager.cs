using Alisveris_Platformu.Business.DataProtection;
using Alisveris_Platformu.Business.Operations.User.Dtos;
using Alisveris_Platformu.Business.Types;
using Alisveris_Platformu.Data.Entities;
using Alisveris_Platformu.Data.Enums;
using Alisveris_Platformu.Data.Repositories;
using Alisveris_Platformu.Data.UnitOfWork;
using Microsoft.AspNetCore.DataProtection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alisveris_Platformu.Business.Operations.User
{
    public class UserManager : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<UserEntity> _userRepository;
        private readonly IDataProtection _protector;


        public UserManager(IUnitOfWork unitOfWork, IRepository<UserEntity> userRepository, 
            IDataProtection protector)
        {
            _unitOfWork = unitOfWork;
            _userRepository = userRepository;
            _protector = protector;
        }


        public async Task<ServiceMessage> AddUser(AddUserDto user)
        {
            var hasMail = _userRepository.GetAll(x =>  x.Email.ToLower() == user.Email.ToLower());

            if(hasMail.Any())
            {
                return new ServiceMessage
                {
                    IsSucceed = false,
                    Message = "Bu e-mail zaten kayıtlı."
                };
            }

            var userEntity = new UserEntity
            {
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Password = _protector.Protect(user.Password),
                PhoneNumber = user.PhoneNumber,
                UserType = UserType.Customer            
            };
            _userRepository.Add(userEntity);

            try
            {
                await _unitOfWork.SaveChangesAsync();

            }
            catch (Exception)
            {
                throw new Exception("Kullanıcı kaydı sırasında hata oluştu.");
            }
            return new ServiceMessage
                { IsSucceed = true };
        }

        public ServiceMessage<UserInfoDto> LoginUser(LoginUserDto user)
        {
            var userEntity = _userRepository.Get(x => x.Email.ToLower() == user.Email.ToLower());

            if(userEntity is null)
            {
                return new ServiceMessage<UserInfoDto>
                {
                    IsSucceed = false,
                    Message = "Kullanıcı adı veya şifre hatalı."
                };  
            }

            var unprotectedPassword = _protector.UnProtect(userEntity.Password);

            if(unprotectedPassword == user.Password)
            {
                return new ServiceMessage<UserInfoDto>
                {
                    IsSucceed = true,
                    Data = new UserInfoDto
                    {
                        Email =userEntity.Email,
                        FirstName = userEntity.FirstName,
                        LastName = userEntity.LastName,
                        UserType = userEntity.UserType,
                    }
                };
            }

            else
            {
                return new ServiceMessage<UserInfoDto>
                {
                    IsSucceed = false,
                    Message = "Kullanıcı veya şifre hatalı."
                };
            }
        }
    }
}
