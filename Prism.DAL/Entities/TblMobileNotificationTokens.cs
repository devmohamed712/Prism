using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Prism.DAL
{
    [Table("TblMobileNotificationTokens", Schema = "Notification")]
    public class TblMobileNotificationTokens
    {
        [System.ComponentModel.DataAnnotations.Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public string UserId { get; set; }
        [Required]
        public string DeviceId { get; set; }
        [Required]
        public string DeviceToken { get; set; }
        [Required]
        public int Platform { get; set; }
        public bool IsStoped { get; set; }
        public bool IsDeleted { get; set; }

        [ForeignKey("UserId")]
        public virtual TblAccounts Account { get; set; }
    }
}
