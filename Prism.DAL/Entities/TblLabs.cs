using System;
using System.Collections.Generic;

namespace Prism.DAL
{
    public partial class TblLabs
    {
        public TblLabs()
        {
            Accounts = new HashSet<TblAccounts>();
            TblUsersLabs = new HashSet<TblUsersLabs>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Logo { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public bool IsDeleted { get; set; }

        public virtual ICollection<TblAccounts> Accounts { get; set; }
        public virtual ICollection<TblUsersLabs> TblUsersLabs { get; set; }
    }
}
