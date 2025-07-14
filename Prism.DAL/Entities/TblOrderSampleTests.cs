using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Prism.DAL
{
    [Table("TblOrderSampleTests", Schema = "Order")]
    public partial class TblOrderSampleTests
    {
        [System.ComponentModel.DataAnnotations.Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public int OrderSampleId { get; set; }
        [Required]
        public int TestId { get; set; }
        [Required]
        public DateTime DateTime { get; set; }
        public int? StatusId { get; set; }
        public bool IsDeleted { get; set; }
        public string? LabTechId { get; set; }

        [ForeignKey("OrderSampleId")]
        public virtual TblOrderSamples OrderSample { get; set; }
        [ForeignKey("TestId")]
        public virtual LkpGroupTests Test { get; set; }
        [ForeignKey("StatusId")]
        public virtual LkpSampleTestStatus SampleTestStatus { get; set; }
        [ForeignKey("LabTechId")]
        public virtual TblAccounts LabTech { get; set; }
    }
}
