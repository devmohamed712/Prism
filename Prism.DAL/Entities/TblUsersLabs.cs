using System;
using System.Collections.Generic;

namespace Prism.DAL
{
    public partial class TblUsersLabs
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int LabId { get; set; }
        public int IsDeleted { get; set; }

        public virtual TblLabs Lab { get; set; }
        public virtual AspNetUsers User { get; set; }
    }
}
