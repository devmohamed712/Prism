using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Prism.DAL
{
    [Table("TblOrderSamples", Schema = "Order")]
    public partial class TblOrderSamples
    {
        public TblOrderSamples()
        {
            OrderSampleTests = new HashSet<TblOrderSampleTests>();
        }
        [System.ComponentModel.DataAnnotations.Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public int OrderId { get; set; }
        [Required]
        public int TestTypeId { get; set; }
        [StringLength(512)]
        public string FacilityRepPrintedName { get; set; }
        [StringLength(512)]
        public string FacilityRepAttachedPhoto { get; set; }
        [StringLength(512)]
        public string FacilityRepSignature { get; set; }
        [StringLength(512)]
        public string CheckWeightMeasurement { get; set; }
        [StringLength(512)]
        public string MetrcSourcePackageId { get; set; }
        [StringLength(512)]
        public string WeightOfSourcePackage { get; set; }
        [StringLength(512)]
        public string NameOfProduct { get; set; }
        public int? TypeOfProduct { get; set; }
        public int? NumberOfContainers { get; set; }
        public decimal? WeightOfSampleCollected { get; set; }
        public int? NumberOfIncruments { get; set; }
        [StringLength(512)]
        public string NoteAnyEquipentUsed { get; set; }
        [StringLength(512)]
        public string MetrcManifest { get; set; }
        [StringLength(512)]
        public string SamplerPrintedName { get; set; }
        public string SamplerSignature { get; set; }
        [StringLength(512)]
        public string PrismAccession { get; set; }
        public int? TestSubTypeId { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsSplit { get; set; }
        public int? StatusId { get; set; }
        [Column(TypeName = "decimal(18, 12)")]
        public decimal? Progress { get; set; }
        // LabTech
        public string? LabTechId { get; set; }
        [StringLength(512)]
        public string ForeignMaterial { get; set; }
        [StringLength(512)]
        public string Microbiology { get; set; }
        [StringLength(512)]
        public string Potency { get; set; }
        [StringLength(512)]
        public string Pesticides { get; set; }
        [StringLength(512)]
        public string HeavyMetals { get; set; }
        [StringLength(512)]
        public string WaterActivity { get; set; }
        [StringLength(512)]
        public string Terpenes { get; set; }
        [StringLength(512)]
        public string Other { get; set; }
        public string LabTechSignature { get; set; }
        public decimal? WeightBeforeDestruction { get; set; }
        public DateTime? DateOfDistruction { get; set; }
        [StringLength(512)]
        public string ResposiblePersonnel { get; set; }


        [ForeignKey("OrderId")]
        public virtual TblOrders Order { get; set; }
        [ForeignKey("TestTypeId")]
        public virtual LkpTestTypes TestType { get; set; }
        [ForeignKey("TypeOfProduct")]
        public virtual LkpSampleTypes SampleType { get; set; }
        [ForeignKey("TestSubTypeId")]
        public virtual LkpTestSubTypes TestSubType { get; set; }
        [ForeignKey("StatusId")]
        public virtual LkpSampleTestStatus SampleTestStatus { get; set; }
        [ForeignKey("LabTechId")]
        public virtual TblAccounts LabTech { get; set; }
        public virtual ICollection<TblOrderSampleTests> OrderSampleTests { get; set; }
    }
}
