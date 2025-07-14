using System;
using System.Collections.Generic;
using System.Text;

namespace Prism.BL.Dtos
{
    public class EmailLogsDto
    {
        public int Id { get; set; }
        public int ResultId { get; set; }
        public bool IsSent { get; set; }
        public DateTime Date { get; set; }
        public bool IsDeleted { get; set; }
        public CustomerResultsDto CustomerResults { get; set; }
    }

    public class EmailLogsDtoList
    {
        public EmailLogsDtoList()
        {
            EmailLogs = new List<EmailLogsDto>();
        }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public bool IsLastPage { get; set; }
        public List<EmailLogsDto> EmailLogs { get; set; }
    }
}
