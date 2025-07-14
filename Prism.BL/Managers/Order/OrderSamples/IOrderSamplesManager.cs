using Microsoft.AspNetCore.Http;
using Prism.BL.Dtos;
using Prism.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prism.BL.Managers.Order.OrderSamples
{
    public interface IOrderSamplesManager
    {
        public OrderSamplesDtoList GetOrderSamples(int pageNumber, int pageSize, int orderId);

        public List<OrderSamplesDto> GetOrderSamples(int orderId = 0, string? samplerId = null);

        public OrderSamplesDto CreateOrEditOrderSample(OrderSamplesDto model, IFormFileCollection files, string orderSampleFilesPath);

        public OrderSamplesDtoList SamplesSearch(int pageNumber, int pageSize, string orderName, int testId, int sampleId, bool pendingSplit, bool pendingForeignMatterTesting, bool pendingWaterActivity, bool totalYeastAndMoldCount, bool totalColiform, bool eColi, bool salmonella, bool aspergillus, bool pendingPesticidesTesting, bool pendingMetalTesting, bool pendingPotencyTesting, bool pendingTerpensTesting);

        public OrderDto AcceptOrderSamples(int orderId);

        public OrderSamplesDto SplitSample(int id, bool isSplit, string role, string userId);

        public SamplerDocumentsDto CreateOrEditSamplerDocument(SamplerDocumentsDto model);

        public List<SamplerDocumentsDto> CreateOrEditSamplerDocFiles(IFormFileCollection files, AccountDto model, string samplerDocsPath);

        public int GetSamplesWithStatus(int type, int durationType = 0);
    }
}
