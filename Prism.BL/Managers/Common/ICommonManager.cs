using AutoMapper;
using Prism.BL.Dtos;
using Prism.DAL;
using Prism.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prism.BL.Managers.Common
{
    public interface ICommonManager
    {

        public List<AboutUsDto> GetAboutUsList();

        public List<TestTypesDto> GetTestTypesList();

        public List<TestSubTypesDto> GetTestSubTypesList();

        public List<SampleTypesDto> GetSampleTypesList();

        public List<SamplerDocumentTypesDto> GetSamplerDocumentTypesList();

        public List<OrderStatusDto> GetOrderStatusList();

        public List<SampleTestStatusDto> GetSampleTestStatusList();

        public OrderSamplesDto OrderSamplesMapping(TblOrderSamples orderSampleDB);
    }
}
