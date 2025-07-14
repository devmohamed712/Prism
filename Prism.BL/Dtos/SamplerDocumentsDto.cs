using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Prism.BL.Dtos
{
    public class SamplerDocumentsDto
    {
        public int Id { get; set; }
        public string AccountId { get; set; }
        public int SamplerDocumentTypeId { get; set; }
        public DateTime? ExpireDate { get; set; }
        public string FileName { get; set; }
        public string FileRealName { get; set; }
        public bool IsDeleted { get; set; }

        public string SamplerDocumentType { get; set; }
    }
}
