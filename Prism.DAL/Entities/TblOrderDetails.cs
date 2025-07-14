using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Prism.DAL
{
    [Table("TblOrderDetails", Schema = "Order")]
    public partial class TblOrderDetails
    {
        [System.ComponentModel.DataAnnotations.Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public int OrderId { get; set; }
        [Required]
        public int StatusId { get; set; }
        [Required]
        public DateTime DateTime { get; set; }

        [ForeignKey("OrderId")]
        public virtual TblOrders Order { get; set; }
        [ForeignKey("StatusId")]
        public virtual LkpOrderStatus OrderStatus { get; set; }
    }
}
