using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prism.BL.Managers.Order.OrderReports
{
    public interface IOrderReportsManager
    {
        public int GetCustomersServed(int durationType);

        public double GetAverageCompleteTesting();
    }
}
