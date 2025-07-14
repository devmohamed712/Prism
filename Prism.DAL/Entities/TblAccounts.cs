using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Prism.DAL
{
    [Table("TblAccount", Schema = "User")]
    public partial class TblAccounts
    {
        public TblAccounts()
        {
            MobileNotificationTokens = new HashSet<TblMobileNotificationTokens>();
            SamplerDocuments = new HashSet<TblSamplerDocuments>();
            Orders = new HashSet<TblOrders>();
            OrderSamples = new HashSet<TblOrderSamples>();
            UserNotifications = new HashSet<TblUserNotifications>();
            CustomerResults = new HashSet<TblCustomerResults>();
        }

        [System.ComponentModel.DataAnnotations.Key]
        public string Id { get; set; }
        [Required]
        [MaxLength(512)]
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? CreateAt { get; set; }
        public DateTime? UpdateAt { get; set; }
        public int? LabId { get; set; }

        [ForeignKey("Id")]
        public virtual AspNetUsers AspNetUser { get; set; }
        public virtual ICollection<TblMobileNotificationTokens> MobileNotificationTokens { get; set; }
        public virtual ICollection<TblSamplerDocuments> SamplerDocuments { get; set; }
        [InverseProperty("Sampler")]
        public virtual ICollection<TblOrders> Orders { get; set; }
        [InverseProperty("QA")]
        public virtual ICollection<TblOrders> SecOrders { get; set; }
        public virtual ICollection<TblOrderSamples> OrderSamples { get; set; }
        public virtual ICollection<TblUserNotifications> UserNotifications { get; set; }

        [ForeignKey("LabId")]
        public virtual TblLabs Lab { get; set; }
        public virtual ICollection<TblCustomerResults> CustomerResults { get; set; }
    }
}
