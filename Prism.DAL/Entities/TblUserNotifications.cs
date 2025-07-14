using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Prism.DAL
{
    [Table("TblUserNotifications", Schema = "Notification")]
    public partial class TblUserNotifications
    {
        [System.ComponentModel.DataAnnotations.Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public int NotificationId { get; set; }
        [Required]
        public string UserId { get; set; }
        public bool IsRead { get; set; }
        public bool IsDeleted { get; set; }

        [ForeignKey("NotificationId")]
        public virtual TblNotifications Notification { get; set; }
        [ForeignKey("UserId")]
        public virtual TblAccounts Account { get; set; }
    }
}
