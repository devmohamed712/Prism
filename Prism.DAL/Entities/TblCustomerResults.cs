using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Prism.DAL
{
    public partial class TblCustomerResults
    {
        public int Id { get; set; }
        public string DisplayName { get; set; }
        public string FileName { get; set; }
        public string Qr { get; set; }
        public string Passcode { get; set; }
        public DateTime Time { get; set; }
        public string ModifiedFileName { get; set; }
        public bool IsDeleted { get; set; }
        public string UserId { get; set; }
        public string FullName { get; set; }
        public string Phone { get; set; }
        public DateTime? CreateAt { get; set; }
        public DateTime? UpdateAt { get; set; }

        [ForeignKey("UserId")]
        public virtual TblAccounts Account { get; set; }
        public virtual ICollection<TblEmailLogs> TblEmailLogs { get; set; }


    }
}
