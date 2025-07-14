using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Prism.DAL
{
    [Table("TblOrders", Schema = "Order")]
    public partial class TblOrders
    {
        public TblOrders()
        {
            SamplerTrackers = new HashSet<TblSamplerTracks>();
            OrderSamples = new HashSet<TblOrderSamples>();
            OrderDetails = new HashSet<TblOrderDetails>();
            Notifications = new HashSet<TblNotifications>();
        }

        [System.ComponentModel.DataAnnotations.Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [StringLength(128)]
        public string Name { get; set; }
        [Required]
        public int BusinessId { get; set; }
        public string? SamplerId { get; set; }
        [Required]
        public int EstimateNumber { get; set; }
        public int? CompanyOrIndividual { get; set; }
        [StringLength(512)]
        public string AdultUse { get; set; }
        [StringLength(512)]
        public string Med { get; set; }
        public bool IsCanceled { get; set; }
        public bool IsDeleted { get; set; }
        public int? LabTechFilledFormsCount { get; set; }
        public string? QAId { get; set; }

        [ForeignKey("BusinessId")]
        public virtual TblBusinesses Business { get; set; }
        [ForeignKey("SamplerId")]
        [InverseProperty("Orders")]
        public virtual TblAccounts Sampler { get; set; }
        [ForeignKey("QAId")]
        [InverseProperty("SecOrders")]
        public virtual TblAccounts QA { get; set; }
        public virtual ICollection<TblSamplerTracks> SamplerTrackers { get; set; }
        public virtual ICollection<TblOrderSamples> OrderSamples { get; set; }
        public virtual ICollection<TblOrderDetails> OrderDetails { get; set; }
        public virtual ICollection<TblNotifications> Notifications { get; set; }
    }
}
