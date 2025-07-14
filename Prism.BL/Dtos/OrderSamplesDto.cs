using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Prism.BL.Dtos
{
    public class OrderSamplesDto
    {
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

        public string GroupA { get; set; }
        public string GroupB { get; set; }

        public OrderDto SampleOrder { get; set; }
        public TestTypesDto SampleTest { get; set; }
        public SampleTestStatusDto TestStatus { get; set; }

    }

    public class OrderSamplesDtoList
    {
        public OrderSamplesDtoList()
        {
            OrderSamples = new List<OrderSamplesDto>();
        }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public bool IsLastPage { get; set; }
        public List<OrderSamplesDto> OrderSamples { get; set; }
    }
}
