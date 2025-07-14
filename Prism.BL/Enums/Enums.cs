using System;
using System.Collections.Generic;
using System.Text;

namespace QRCodeResults.BL.Enums
{
    public static class Roles
    {
        public const string Admin = "Admin";
        public const string LabAssistant = "LabAssistant";
        public const string Sampler = "Sampler";
        public const string LabTechnician = "LabTechnician";
        public const string QA = "QA";
        public const string User = "User";
    }

    public static class OrderStatus
    {
        public const string Placed = "Placed";
        public const string Accepted = "Accepted";
        public const string PickedUp = "Picked Up";
        public const string DroppedOff = "Dropped Off";
        public const string SampleAccepted = "Sample Accepted";
        public const string TestingStarted = "Testing Started";
        public const string TestingCompleted = "Testing Completed";
        public const string ResultsApproved = "Results Approved";
        public const string ResultsReported = "Results Reported";
        public const string ReleaseOfCOA = "Release Of COA";
    }

    public enum LicenseTypes
    {
        MedicalLicense = 1,
        RecreationalLicense = 2,
        BothLicenses = 3
    }

    public enum FacilityInformation
    {
        Company = 1,
        Individual = 2
    }

    public static class SampleGroupTests
    {
        public const string ForeignMatter = "Foreign Matter";
        public const string WaterActivity = "Water Activity";
        public const string TotalYeastMoldCount = "Total Yeast & Mold Count";
        public const string TotalColiform = "Total Coliform";
        public const string EColi = "E- Coli";
        public const string Salmonella = "Salmonella";
        public const string Aspergillus = "Aspergillus";
        public const string PestisideTesting = "Pestiside Testing";
        public const string MetalTesting = "Metal Testing";
        public const string PotencyTesting = "Potency Testing";
        public const string TerpensTesting = "Terpens Testing";
    }

    public static class SampleTestStatus
    {
        public const string Started = "Started";
        public const string Failed = "Failed";
        public const string Completed = "Completed";
    }

    public static class NotificationTypes
    {
        public const string ANewOrderIsCreated = "A New Order Is Created";
        public const string DriverAcceptsAnOrder = "Driver Accepts An Order";
        public const string OrderIsNottAcceptedByAnySamplerWithinTwoHours = "Order Is Not Accepted By Any Sampler Within Two Hours";
        public const string OrderDroppedOffByASampler = "Order Dropped Off By A Sampler";
        public const string TestingSplitIntoTwoGroups = "Testing Split Into Two Groups";
        public const string ASpecificTestIsStarted = "A Specific Test Is Started";
        public const string ASpecificTestIsCompleted = "A Specific Test Is Completed";
        public const string ASpecificTestIsFailed = "A Specific Test Is Failed";
        public const string TestingIsCompletedForSamplesOfASpecificOrder = "Testing Is Completed For Samples Of A Specific Order";
        public const string ApproveOrder = "Approve Order";
        public const string ResultsReportedToState = "Results Reported To State";
        public const string ReleaseOfCOA = "Release Of COA";
    }
    public enum StatisticsTypes
    {
        SamplesCollected = 1,
        SamplesHasBeenTested = 2,
        CustomersServed = 3,
        AverageTimeItTakesToCompleteTesting = 4,
        SamplesHasBeenReceived = 5,
        SamplesInProcess = 6,
        SamplesHasBeenFinished = 7
    }
}
