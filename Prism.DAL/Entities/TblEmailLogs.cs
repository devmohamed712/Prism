using System;
using System.Collections.Generic;

namespace Prism.DAL
{
    public partial class TblEmailLogs
    {
        public int Id { get; set; }
        public int ResultId { get; set; }
        public bool IsSent { get; set; }
        public DateTime Date { get; set; }
        public bool IsDeleted { get; set; }
        public virtual TblCustomerResults CustomerResults { get; set; }

    }
}
