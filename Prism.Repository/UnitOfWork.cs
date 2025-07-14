using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Prism.DAL;
using Prism.Repository.Repository;

namespace Prism.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private PrismContext Context;
        public UnitOfWork(PrismContext context)
        {
            Context = context;
            AspNetUsers = new AspNetUserRepository(Context);
            AspNetRoles = new AspNetRolesRepository(Context);
            Labs = new LabsRepository(Context);
            UsersLabs = new UsersLabsRepository(Context);
            CustomerResults = new CustomerResultsRepository(Context);
            EmailLogs = new EmailLogsRepository(Context);
            Accounts = new AccountsRepository(Context);
            Business = new BusinessRepository(Context);
            BusinessTests = new BusinessTestsRepository(Context);
            BusinessSamples = new BusinessSamplesRepository(Context);
            BusinessContacts = new BusinessContactsRepository(Context);
            Order = new OrderRepository(Context);
            OrderDetails = new OrderDetailsRepository(Context);
            OrderSamples = new OrderSamplesRepository(Context);
            AboutUs = new AboutUsRepository(Context);
            TestSubTypes = new TestSubTypesRepository(Context);
            TestTypes = new TestTypesRepository(Context);
            SampleTypes = new SampleTypesRepository(Context);
            Notifications = new NotificationsRepository(Context);
            UserNotifications = new UserNotificationsRepository(Context);
            MobileNotificationTokens = new MobileNotificationTokensRepository(Context);
            SamplerDocumentTypes = new SamplerDocumentTypesRepository(Context);
            SamplerDocuments = new SamplerDocumentsRepository(Context);
            OrderStatus = new OrderStatusRepository(Context);
            BusinessAddresses = new BusinessAddressesRepository(Context);
            States = new StatesRepository(Context);
            Areas = new AreasRepository(Context);
            SamplerTracks = new SamplerTracksRepository(Context);
            GroupTests = new GroupTestsRepository(Context);
            OrderSampleTests = new OrderSampleTestsRepository(Context);
            SampleTestStatus = new SampleTestStatusRepository(Context);
            NotificationTypes = new NotificationTypesRepository(Context);
        }
        public IAspNetUserRepository AspNetUsers { get; }
        public IAspNetRolesRepository AspNetRoles { get; }
        public ILabsRepository Labs { get; }
        public IUsersLabsRepository UsersLabs { get; }
        public ICustomerResultsRepository CustomerResults { get; }
        public IEmailLogsRepository EmailLogs { get; }
        public IAccountsRepository Accounts { get; }
        public IBusinessRepository Business { get; }
        public IBusinessTestsRepository BusinessTests { get; }
        public IBusinessSamplesRepository BusinessSamples { get; }
        public IBusinessContactsRepository BusinessContacts { get; }
        public IOrderRepository Order { get; }
        public IOrderDetailsRepository OrderDetails { get; }
        public IOrderSamplesRepository OrderSamples { get; }
        public IAboutUsRepository AboutUs { get; }
        public ITestSubTypesRepository TestSubTypes { get; }
        public ITestTypesRepository TestTypes { get; }
        public ISampleTypesRepository SampleTypes { get; }
        public INotificationsRepository Notifications { get; }
        public IUserNotificationsRepository UserNotifications { get; }
        public IMobileNotificationTokensRepository MobileNotificationTokens { get; }
        public ISamplerDocumentTypesRepository SamplerDocumentTypes { get; }
        public ISamplerDocumentsRepository SamplerDocuments { get; }
        public IOrderStatusRepository OrderStatus { get; }
        public IBusinessAddressesRepository BusinessAddresses { get; }
        public IStatesRepository States { get; }
        public IAreasRepository Areas { get; }
        public ISamplerTracksRepository SamplerTracks { get; }
        public IGroupTestsRepository GroupTests { get; }
        public IOrderSampleTestsRepository OrderSampleTests { get; }
        public ISampleTestStatusRepository SampleTestStatus { get; }
        public INotificationTypesRepository NotificationTypes { get; }

        public int Complete()
        {
            return Context.SaveChanges();
        }

        public void Dispose()
        {
            Context.Dispose();
        }

        public int ExecuteStoredProcedure(string sql, params object[] parameters)
        {
            return Context.Database.ExecuteSqlRaw(sql, parameters);
        }
    }
}
