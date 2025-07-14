using AutoMapper;
using Microsoft.Extensions.Configuration;
using Prism.BL.Managers.Business;
using Prism.BL.Managers.Common;
using Prism.BL.Managers.Order.OrderSamples;
using Prism.DAL;
using Prism.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prism.BL.Managers.Order.OrderDetails
{
    public class OrderDetailsManager : IOrderDetailsManager
    {
        public readonly IMapper _mapper;
        public readonly IConfiguration _configuration;
        public readonly IUnitOfWork _unitOfWork;

        public OrderDetailsManager(IMapper mapper, IConfiguration configuration, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _configuration = configuration;
            _unitOfWork = unitOfWork;
        }

        public void CreateOrderDetails(int orderId, int statusId, DateTime currentTime)
        {
            TblOrderDetails detail = new TblOrderDetails
            {
                OrderId = orderId,
                StatusId = statusId,
                DateTime = currentTime
            };
            _unitOfWork.OrderDetails.Add(detail);
            _unitOfWork.Complete();
        }
    }
}
