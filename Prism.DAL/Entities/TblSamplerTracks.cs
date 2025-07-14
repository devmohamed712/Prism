using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Prism.DAL
{
    [Table("TblSamplerTracks", Schema = "Sampler")]
    public partial class TblSamplerTracks
    {
        [System.ComponentModel.DataAnnotations.Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public int OrderId { get; set; }
        [Required]
        [Column(TypeName = "decimal(18, 16)")]
        public decimal Latitude { get; set; }
        [Required]
        [Column(TypeName = "decimal(18, 16)")]
        public decimal Longitude { get; set; }
        public int? StatusId { get; set; }
        [Required]
        public DateTime DateTime { get; set; }
        public bool IsDeleted { get; set; }

        [ForeignKey("OrderId")]
        public virtual TblOrders Order { get; set; }
        [ForeignKey("StatusId")]
        public virtual LkpOrderStatus OrderStatus { get; set; }
    }
}
