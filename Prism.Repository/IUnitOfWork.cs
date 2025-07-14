using System;
using System.Collections.Generic;
using System.Text;

namespace Prism.Repository
{
    public interface IUnitOfWork : IDisposable
    {
        IAspNetUserRepository AspNetUsers { get; }
        IAspNetRolesRepository AspNetRoles { get; }
        ILabsRepository Labs { get; }
        IUsersLabsRepository UsersLabs { get; }
        ICustomerResultsRepository CustomerResults { get; }
        IEmailLogsRepository EmailLogs { get; }
        IAccountsRepository Accounts { get; }
        IBusinessRepository Business { get; }
        IBusinessContactsRepository BusinessContacts { get; }
        IBusinessTestsRepository BusinessTests { get; }
        IBusinessSamplesRepository BusinessSamples { get; }
        IOrderRepository Order { get; }
        IOrderDetailsRepository OrderDetails { get; }
        IOrderSamplesRepository OrderSamples { get; }
        IAboutUsRepository AboutUs { get; }
        ITestSubTypesRepository TestSubTypes { get; }
        ITestTypesRepository TestTypes { get; }
        ISampleTypesRepository SampleTypes { get; }
        INotificationsRepository Notifications { get; }
        IUserNotificationsRepository UserNotifications { get; }
        IMobileNotificationTokensRepository MobileNotificationTokens { get; }
        ISamplerDocumentTypesRepository SamplerDocumentTypes { get; }
        ISamplerDocumentsRepository SamplerDocuments { get; }
        IOrderStatusRepository OrderStatus { get; }
        IBusinessAddressesRepository BusinessAddresses { get; }
        IStatesRepository States { get; }
        IAreasRepository Areas { get; }
        ISamplerTracksRepository SamplerTracks { get; }
        IGroupTestsRepository GroupTests { get; }
        IOrderSampleTestsRepository OrderSampleTests { get; }
        ISampleTestStatusRepository SampleTestStatus { get; }
        INotificationTypesRepository NotificationTypes { get; }

        int Complete();
        int ExecuteStoredProcedure(string sql, params object[] parameters);
    }
}
