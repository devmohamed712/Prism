using Prism.BL.Dtos;
using Prism.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prism.BL.Managers.Order
{
    public interface IOrderManager
    {
        public OrderDtoList GetOrders(int pageNumber, int pageSize, List<int>? statusIds = null, string? userId = null, string? role = null);

        public OrderDto GetOrder(int id, string? role = null, string? userId = null);

        public OrderDto? GetOrderBySamplerId(string samplerId, bool isConfirmationPage = false);

        public OrderDto CreateOrEditOrder(OrderDto model);

        public bool Delete(int id);

        public bool Cancel(int id);

        public OrderDto GetUnAcceptedOrder(int orderId);

        public bool AcceptOrder(int orderId, string samplerId, decimal latitude, decimal longitude);

        public bool PickUpOrDropOffOrder(int orderId, string samplerId, bool isPickedUp);

        public void CompleteOrder(int orderId, bool isOrderEmpty);

        public OrderDto ApproveOrReportOrReleaseOrder(int orderId, string status, string userId);

        public OrderDto Mapping(TblOrders orderDB, string? role = null, string? userId = null);
    }
}
