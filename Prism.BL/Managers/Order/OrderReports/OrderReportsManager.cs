using AutoMapper;
using Microsoft.Extensions.Configuration;
using Prism.BL.Managers.Business;
using Prism.BL.Managers.Common;
using Prism.BL.Managers.Notification;
using Prism.BL.Managers.Order.OrderDetails;
using Prism.BL.Managers.Order.OrderSamples;
using Prism.BL.Managers.Utilities;
using Prism.DAL;
using Prism.Repository;
using QRCodeResults.BL.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prism.BL.Managers.Order.OrderReports
{
    public class OrderReportsManager : IOrderReportsManager
    {
        public readonly IMapper _mapper;
        public readonly IConfiguration _configuration;
        public readonly IUnitOfWork _unitOfWork;

        public OrderReportsManager(IMapper mapper, IConfiguration configuration, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _configuration = configuration;
            _unitOfWork = unitOfWork;
        }

        public int GetCustomersServed(int durationType)
        {
            Func<TblOrders, bool> predicates = x => !x.IsDeleted && !x.IsCanceled &&
            x.OrderDetails.OrderByDescending(o => o.Id).FirstOrDefault().OrderStatus.Name.Equals(OrderStatus.ReleaseOfCOA) && x.OrderDetails.OrderByDescending(o => o.Id).FirstOrDefault().DateTime >= DateTime.UtcNow.AddDays(durationType == 1 ? -7 : -30);
            var ordersDB = _unitOfWork.Order.FindList(predicates);
            int customers = ordersDB.Select(x => x.BusinessId).Distinct().Count();
            return customers;
        }

        public double GetAverageCompleteTesting()
        {
            Func<TblOrderSamples, bool> predicates = x => !x.IsDeleted && !x.Order.IsCanceled && x.OrderSampleTests.Any(c => c.SampleTestStatus.Name.Equals(SampleTestStatus.Completed) && c.DateTime >= DateTime.UtcNow.AddDays(-30));
            var ordersSamplesDB = _unitOfWork.OrderSamples.FindList(predicates);
            var diffLst = ordersSamplesDB.Select(x =>
            x.OrderSampleTests.LastOrDefault(c => c.SampleTestStatus.Name.Equals(SampleTestStatus.Completed)).DateTime -
            x.OrderSampleTests.LastOrDefault(c => c.SampleTestStatus.Name.Equals(SampleTestStatus.Started)).DateTime);
            List<int> diffInHours = new List<int>();
            foreach (var diff in diffLst)
            {
                diffInHours.Add(diff.Hours);
            }
            double avg = 0;
            if (diffInHours.Count() > 0)
            {
                avg = Queryable.Average(diffInHours.AsQueryable());
            }
            return avg;
        }
    }
}
