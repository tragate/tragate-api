namespace Tragate.Common.Library.Enum
{
    public enum LoginType
    {
        StandartLogin = 1,
        AutoLogin = 2
    }

    public enum PlatformType
    {
        Web = 1,
        Admin = 2
    }

    public enum StatusType : byte
    {
        All = 0,
        New = 1,
        WaitingApprove = 2,
        Active = 3,
        Deleted = 4,
        Passive = 5,
        Transferred = 6,
        Completed = 7
    }

    public enum DocumentType : byte
    {
        Company,
        Product
    }

    public enum UserType : byte
    {
        Person = 1,
        Company = 2
    }

    public enum RegisterType : byte
    {
        Tragate = 1,
        Facebook = 2,
        Google = 3,
        Linkedin = 4,
        Twitter = 5,
        Anonymous = 6
    }


    public enum CompanyAdminRoleType : byte
    {
        Owner = 1,
        ProductManager = 2,
        CommunicationAdvisor = 3
    }


    public enum CompanyTaskType : byte
    {
        UpdateCompanyInfo = 1,
        UpdateCompanyProducts = 2
    }

    public enum QuoteStatusType : byte
    {
        All = 0,
        Lead = 1,
        Deal = 2,
        Order = 3,
        Shipping = 4,
        Completed = 5,
        Cancelled = 6,
        Spam = 7,
        Deleted = 8
    }

    public enum OrderStatusType : byte
    {
        All = 0,
        Waiting_Price = 1,
        Waiting_Buyer_Confirm = 2,
        Waiting_Seller_Confirm = 3,
        Waiting_Payment = 4,
        Order_Processing = 5,
        Waiting_Shipment = 6,
        Waiting_Delivery = 7,
        Completed = 8,
        Cancelled = 9
    }

    public enum QuoteContactStatusType : byte
    {
        All = 0,
        Waiting_Buyer_Response = 1,
        Buyer_Read = 2,
        Waiting_Seller_Response = 3,
        Seller_Read = 4
    }

    public enum TradeTermType : byte
    {
        EXW = 1,
        FCA = 2,
        CFR = 3,
        CIF = 4,
        CPT = 5,
        CIP = 6,
        DAT = 7,
        DAP = 8,
        DDP = 9
    }

    public enum ShippingMethodType : byte
    {
        Express = 1,
        Sea_Freight_To_Port = 2,
        Sea_Freight_To_Door = 3,
        Air_Freight_To_Port = 4,
        Air_Freight_To_Door = 5,
        Land_Freight = 6,
        Postal = 7
    }

    public enum MembershipPackageType : byte
    {
        Free,
        Gold,
        GoldPlus
    }
}