using System;
using System.Collections.Generic;
using System.Text;

namespace Prism.BL.Dtos
{
    public class LabsDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Logo { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public bool IsDeleted { get; set; }

    }
}
