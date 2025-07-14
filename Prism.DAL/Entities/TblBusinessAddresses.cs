using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Prism.DAL
{
    [Table("TblBusinessAddresses", Schema = "Business")]
    public partial class TblBusinessAddresses
    {
        [System.ComponentModel.DataAnnotations.Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        [StringLength(512)]
        public string Address { get; set; }
        [Required]
        [Column(TypeName = "decimal(18, 12)")]
        public decimal Latitude { get; set; }
        [Required]
        [Column(TypeName = "decimal(18, 12)")]
        public decimal Longitude { get; set; }
        [StringLength(512)]
        public string Country { get; set; }
        public int? AreaId { get; set; }
        [StringLength(128)]
        public string Zipcode { get; set; }
        [Required]
        public int BusinessId { get; set; }
        [Required]
        public bool IsMain { get; set; }
        public bool IsDeleted { get; set; }

        [ForeignKey("BusinessId")]
        public virtual TblBusinesses Business { get; set; }
        [ForeignKey("AreaId")]
        public virtual LkpAreas Area { get; set; }
    }
}
