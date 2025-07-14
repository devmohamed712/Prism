using Prism.BL.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace Prism.BL.Dtos
{
    public class CustomerResultsDto
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string DisplayName { get; set; }
        public string FileName { get; set; }
        public string Qr { get; set; }
        public string Passcode { get; set; }
        public DateTime Time { get; set; }
        public bool IsDeleted { get; set; }
        public string ModifiedFileName { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public DateTime? CreateAt { get; set; }
        public DateTime? UpdateAt { get; set; }
        public AccountDto Customer { get; set; }
    }

    public class CustomerResultsDtoList
    {
        public int PageNumber { set; get; }
        public int PageSize { set; get; }
        public List<CustomerResultsDto> CustomerResults { set; get; }
        public int Count { set; get; }
        public CustomerResultsDtoList()
        {
            CustomerResults = new List<CustomerResultsDto>();
        }

    }
}
