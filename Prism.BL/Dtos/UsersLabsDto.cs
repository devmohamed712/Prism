using System;
using System.Collections.Generic;
using System.Text;

namespace Prism.BL.Dtos
{
    public class UsersLabsDto
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int LabId { get; set; }
        public bool IsDeleted { get; set; }

    }
}
