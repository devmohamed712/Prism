using Prism.BL.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prism.BL.Managers.Order.OrderSamplesTests
{
    public interface IOrderSamplesTestsManager
    {
        public List<GroupTestsDto> GetGroupTestsList(int sampleId = 0);

        public OrderSampleTestsDto AddTest(OrderSampleTestsDto model);
    }
}
