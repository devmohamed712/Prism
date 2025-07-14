using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Prism.DAL
{
    [Table("TblNotifications", Schema = "Notification")]
    public partial class TblNotifications
    {
        public TblNotifications()
        {
            UserNotifications = new HashSet<TblUserNotifications>();
        }

        [System.ComponentModel.DataAnnotations.Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public string NotificationText { get; set; }
        [Required]
        public int NotificationTypeId { get; set; }
        [Required]
        public DateTime DateTime { get; set; }
        public bool IsDeleted { get; set; }
        public int? OrderId { get; set; }

        [ForeignKey("NotificationTypeId")]
        public virtual LkpNotificationTypes LkpNotificationType { get; set; }
        [ForeignKey("OrderId")]
        public virtual TblOrders Order { get; set; }
        public virtual ICollection<TblUserNotifications> UserNotifications { get; set; }
    }
}
