using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using Prism.BL;
using Prism.BL.Dtos;
using Prism.DAL;


namespace Prism.BL.AutoMapper
{
    public class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<RoleDto, AspNetRoles>();
            CreateMap<AspNetRoles, RoleDto>();

            CreateMap<LabsDto, TblLabs>();
            CreateMap<TblLabs, LabsDto>();

            CreateMap<UsersLabsDto, TblUsersLabs>();
            CreateMap<TblUsersLabs, UsersLabsDto>();

            CreateMap<CustomerResultsDto, TblCustomerResults>();
            CreateMap<TblCustomerResults, CustomerResultsDto>();

            CreateMap<EmailLogsDto, TblEmailLogs>();
            CreateMap<TblEmailLogs, EmailLogsDto>();

            CreateMap<AccountDto, TblAccounts>().ForMember(c => c.SamplerDocuments, v => v.Ignore());
            CreateMap<TblAccounts, AccountDto>();

            CreateMap<TblBusinesses, BusinessDto>();
            CreateMap<BusinessDto, TblBusinesses>();

            CreateMap<TblBusinessTests, BusinessTestsDto>();
            CreateMap<BusinessTestsDto, TblBusinessTests>().ForMember(c => c.TestSubType, v => v.Ignore());

            CreateMap<TblBusinessSamples, BusinessSamplesDto>();
            CreateMap<BusinessSamplesDto, TblBusinessSamples>().ForMember(c => c.SampleType, v => v.Ignore());

            CreateMap<TblBusinessContacts, BusinessContactsDto>();
            CreateMap<BusinessContactsDto, TblBusinessContacts>();

            CreateMap<TblOrders, OrderDto>();
            CreateMap<OrderDto, TblOrders>();

            CreateMap<TblOrderSamples, OrderSamplesDto>();
            CreateMap<OrderSamplesDto, TblOrderSamples>();

            CreateMap<LkpAboutUs, AboutUsDto>();
            CreateMap<AboutUsDto, LkpAboutUs>();

            CreateMap<LkpTestTypes, TestTypesDto>();
            CreateMap<TestTypesDto, LkpTestTypes>();

            CreateMap<LkpTestSubTypes, TestSubTypesDto>();
            CreateMap<TestSubTypesDto, LkpTestSubTypes>();

            CreateMap<LkpSampleTypes, SampleTypesDto>();
            CreateMap<SampleTypesDto, LkpSampleTypes>();

            CreateMap<TblNotifications, NotificationDto>();
            CreateMap<NotificationDto, TblNotifications>();

            CreateMap<TblUserNotifications, UserNotificationsDto>();
            CreateMap<UserNotificationsDto, TblUserNotifications>();

            CreateMap<TblMobileNotificationTokens, MobileNotificationTokensDto>();
            CreateMap<MobileNotificationTokensDto, TblMobileNotificationTokens>();

            CreateMap<LkbSamplerDocumentTypes, SamplerDocumentTypesDto>();
            CreateMap<SamplerDocumentTypesDto, LkbSamplerDocumentTypes>();

            CreateMap<TblSamplerDocuments, SamplerDocumentsDto>();
            CreateMap<SamplerDocumentsDto, TblSamplerDocuments>().ForMember(c => c.SamplerDocumentType, v => v.Ignore());

            CreateMap<LkpOrderStatus, OrderStatusDto>();
            CreateMap<OrderStatusDto, LkpOrderStatus>();

            CreateMap<TblBusinessAddresses, BusinessAddressesDto>();
            CreateMap<BusinessAddressesDto, TblBusinessAddresses>();

            CreateMap<LkpStates, StateDto>();
            CreateMap<StateDto, LkpStates>();

            CreateMap<LkpAreas, AreaDto>();
            CreateMap<AreaDto, LkpAreas>();

            CreateMap<TblSamplerTracks, SamplerTracksDto>();
            CreateMap<SamplerTracksDto, TblSamplerTracks>();

            CreateMap<LkpGroupTests, GroupTestsDto>();
            CreateMap<GroupTestsDto, LkpGroupTests>();

            CreateMap<TblOrderSampleTests, OrderSampleTestsDto>();
            CreateMap<OrderSampleTestsDto, TblOrderSampleTests>();

            CreateMap<LkpSampleTestStatus, SampleTestStatusDto>();
            CreateMap<SampleTestStatusDto, LkpSampleTestStatus>();

            CreateMap<LkpNotificationTypes, NotificationTypesDto>();
            CreateMap<NotificationTypesDto, LkpNotificationTypes>();
        }
    }
}
