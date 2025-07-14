using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Prism.DAL
{
    [Table("TblBusinessTests", Schema = "Business")]
    public partial class TblBusinessTests
    {
        [System.ComponentModel.DataAnnotations.Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public int BusinessId { get; set; }
        [Required]
        public int TestTypeId { get; set; }
        public int? TestSubTypeId { get; set; }
        public bool IsDeleted { get; set; }

        [ForeignKey("BusinessId")]
        public virtual TblBusinesses Business { get; set; }
        [ForeignKey("TestTypeId")]
        public virtual LkpTestTypes TestType { get; set; }
        [ForeignKey("TestSubTypeId")]
        public virtual LkpTestSubTypes TestSubType { get; set; }
    }
}