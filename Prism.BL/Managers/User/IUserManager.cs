using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Prism.BL.Dtos;
using Prism.DAL;
using QRCodeResults.BL.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prism.BL.Managers.User
{
    public interface IUserManager
    {
        public Task<RegisterDto?> Register(RegisterDto model, bool isAddingByAdmin = false);

        public Task<IdentityUser?> RegisterByPhoneNumber(string phoneNumber, string roleId = "User");

        public Task<LoginDto?> Login(LoginDto model);

        public Task<bool> ForgetPassword(string email);

        public Task<IdentityResult?> ResetPassword(ResetPasswordDto model);

        public List<RoleDto> GetRoles();

        public AccountDtoList GetUsers(int pageNumber, int pageSize, string? role = null, bool hasOrders = false);

        public AccountDtoList GetSamplers(int pageNumber, int pageSize);

        public AccountDto? GetUser(string id);

        public AccountDto? GetUserByEmail(string email);

        public AccountDto? GetUserByPhoneNumber(string phoneNumber);

        public AccountDto CreateOrEdit(AccountDto model);

        public bool ChangeUserActivation(string id, bool status);

        public bool DeleteUser(string id);

        public SamplerTracksDto AddSamplerTrack(SamplerTracksDto model, string samplerId);

        public SamplerTracksDtoList GetLastSamplerTrack(string samplerId);

        public AccountDto Mapping(TblAccounts account);

        public AccountDto Mapping(AspNetUsers user);
    }
}
