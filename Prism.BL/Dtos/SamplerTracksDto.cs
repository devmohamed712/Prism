using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Prism.BL.Dtos
{
    public class SamplerTracksDto
    {
        public int Id { get; set; }
        [Required]
        public int OrderId { get; set; }
        [Required]
        public decimal Latitude { get; set; }
        [Required]
        public decimal Longitude { get; set; }
        [Required]
        public int StatusId { get; set; }
        [Required]
        public DateTime DateTime { get; set; }
        public bool IsDeleted { get; set; }

        public OrderDto Order { get; set; }
    }

    public class SamplerTracksDtoList
    {
        public OrderDto Order { get; set; }
        public BusinessAddressesDto MainAddress { get; set; }
        public SamplerTracksDto SamplerTrack { get; set; }
        [Column(TypeName = "decimal(18, 12)")]
        public decimal OrderAcceptLatitudePoint { get; set; }
        [Column(TypeName = "decimal(18, 12)")]
        public decimal OrderAcceptLongitudePoint { get; set; }
    }
}
