using Prism.BL.Dtos;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Prism.BL.Dtos
{
    public class RegisterDto
    {
        [Required]
        [StringLength(255)]
        public string Email { set; get; }
        [Required]
        [StringLength(255, MinimumLength = 8)]
        public string Password { set; get; }
        public string Phone { set; get; }
        public string FullName { set; get; }
        public string Token { set; get; }
        public string Role { set; get; }
        public string UserId { set; get; }
    }
    public class LoginDto
    {
        public string UserId { get; set; }
        public string DisplayName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Token { get; set; }
        public string Role { get; set; }

        public string PhoneNumber { get; set; }
        public string FirebaseUserId { get; set; }
        [NotMapped]
        public MobileNotificationTokensDto MobileNotificationToken { get; set; }

    }
    public class ResetPasswordDto
    {
        [Required]
        [StringLength(255)]
        public string Email { set; get; }
        [Required]
        [StringLength(255, MinimumLength = 8)]
        public string Password { set; get; }
        [Required]
        [Compare("Password")]
        public string ConfirmPassword { set; get; }

        public string Token { get; set; }
    }

    public class AccountDto
    {
        public string Id { get; set; }
        [Required]
        [MaxLength(512)]
        public string Name { get; set; }
        [Required]
        [MaxLength(128)]
        public string PhoneNumber { get; set; }
        public DateTime? RegistrationDateTime { get; set; }
        public bool? IsActive { get; set; }
        public bool IsDeleted { get; set; }

        public string Email { get; set; }
        [NotMapped]
        public string Role { get; set; }
        [NotMapped]
        public string RoleId { get; set; }
        [NotMapped]
        public int SamplerOrdersPickedUp { get; set; }
        [NotMapped]
        public int SamplerOrdersDroppedOff { get; set; }
        [NotMapped]
        public MobileNotificationTokensDto MobileNotificationToken { get; set; }
        [NotMapped]
        public List<SamplerDocumentsDto> SamplerDocuments { get; set; }
        [NotMapped]
        public bool IsSent { get; set; }
        [NotMapped]
        public CustomerResultsDto CustomerResult { set; get; }
    }

    public class AccountDtoList
    {
        public AccountDtoList()
        {
            Users = new List<AccountDto>();
        }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public bool IsLastPage { get; set; }
        public List<AccountDto> Users { get; set; }
    }

    public class RoleDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string NormalizedName { get; set; }
        public string ConcurrencyStamp { get; set; }
    }
}
