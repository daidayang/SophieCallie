using System;

namespace CRM.BusinessEntities
{
    public enum InputFieldTypeEnum
    {
        Salutation = 1,
        Firstname = 2,
        Lastname = 3,
        Middlename = 4,
        Addresses1 = 5,
        Addresses2 = 6,
        City = 7,
        Region = 8,
        Zip = 9,
        Country = 10,
        Email = 11,
        EmailRetype = 12,
        EmailOptIn = 13,
        Phone = 14,
        PhoneOptIn = 15,
        PhoneMobile = 16,
        PhoneMobileOptIn = 17,

        Birthday = 18,
        Gender = 19,
        Company = 20,
        FrequentTravelerProgram = 21,
        FrequentTravelerId = 22,
        MarketingOptOut = 23,
        Password = 24,
        PasswordRetype = 25,
        UserDefined = 100,
        BlockAddreess = 200,
        BlockBillingAddreess = 201,
        BlockPhones = 202,
        BlockEmails = 203,
        BlockMultiAddress = 204,

 
        IsVip = 26,
        Udf1 = 27,
        Udf2 = 28,
        Udf3 = 29,
        Udf4 = 30,
        Udf5 = 31,
        Udf6 = 32,
        Udf7 = 33,
        Udf8 = 34,
        Udf9 = 35,
        Udf10 = 36,
        Language=37
    }

    public enum GuestRequestUITypeEnum
    {
        CheckBox = 1,
        DropdownList = 2,
        DropdownListMulti = 3,
        RadioBtnList = 4,
    }

    [Flags]
    public enum InputFieldOptionEnum
    {
        None = 0,
        Required = 1,

        AsNumeric = 1 << 12,
        AsAlphaNumeric = 1 << 13,
        AsEmail = 1 << 14,
    }

    public enum PhoneEnum : byte
    {
        Unknown = 0,
        Phone = 1,
        Fax = 2,
        Mobile = 3
    }

    public enum APEUseEnum : byte
    {
        ALL = 255,
        Unknown = 0,
        Home = 1,
        Business = 2,
        Billing = 3,
        Contact = 4,
        HotelReservation = 5,
    }

    [Flags]
    public enum APESettingEnum
    {
        None = 0,
        Active = 1,
        OptIn = 2,
        Primary = 4,
    }

    public enum HotelGroupTypeEnum
    {
        None = 0,
        HotelChain = 10,
    }

    [Flags]
    public enum HotelGroupConfigEnum
    {
        None = 0,
        Active = 1,
        ExclusiveGrouping = 1 << 1,
    }

    [Flags]
    public enum ExtInterfaceConfigEnum
    {
        None = 0,
        OutBound = 1,
        ComponentRoom = 2 << 21,
        RestrictOverSell = 2 << 22
    }


    #region Reservation Level

    [Flags]
    public enum ItineraryDetailEnum
    {
        None = 0,
        Confirmed = 1,
        MultiProperty = 2
    }

    public enum ReservationModifyMethodEnum
    {
        Complete = 1,
        Differencial = 2
    }

    public enum ReservationEmailTypeEnum
    {
        Regular = 1,
        Group = 2,
        Package = 3,
        WaitList = 4,
        Channel = 5,
        PreStay = 10,
        PostStay = 11,
        Hotel = 20,
        Fax = 21,
        WSFax = 24
    }

    public enum ReservationTypeEnum
    {
        NA = 0,
        Room = 1,
        Package = 2,
        Group = 3,
        ServiceItemOnly = 4
    }

    public enum ReservationNoteTypeEnum
    {
        Guest = 1,
        CRO = 2,
        Hotel = 3,
        Group = 4,
        TA = 5,
        HA = 6
    }

    [Flags]
    public enum ResLogMoreDetailEnum
    {
        None = 0,
        DisableGuestEmail = 1 << 0,                 //  1
        DisablePmsEvent = 1 << 1,                   //  2
        DiaableHotelResEmailFax = 1 << 2,           //  4
        PmsUpdatedWhileInSession = 1 << 3,          //  8
        NonCreditedCancellation = 1 << 4,           //  16
        //PromoCodeRedemption = 1 << 5,               //  32
        Reinstated = 1 << 6,                        //  64
    }

    #endregion

    #region User Access

    public enum UserAccessCheckPointEnum
    {
        ANY = -1,
        NONE = 0,
        SYSTEMAREA = 1,
        CLIENTAREA = 2,

        CHAINSETUP = 10000,
        ADMINAUDITLOG = 10001,

        CHAINHOTELGROUPING = 10006,
        CHAINPROPMGMT = 10010,
        CHAINOBJMGMT = 10020,
        CHAINUSERMGMT = 10030,
        CHAINCALLASSIST = 10050,
        CHAINBILLING = 10070,
        //CHAINREPORT=10060, 

        PROPERTY = 10,
        MULTIMEDIA = 11,
        BBEBRANDING = 12,
        RATEMGMT = 20,
        INVMGMT = 30,
        AVAILMGMT = 40,
        GPSMGMT = 50,
        ADDONS = 51,
        _3RDPTYMGMT = 60,
        RESVMGMT = 90,
        PROFILEMGMT = 100,
        REPORT = 110,
        MARKETING = 120,
        SECURITY = 130,
        CROACCESS = 140,
        BILLING = 150,
        DASHBOARD = 160,
        GRANTACCESSCREDITCARD = 200,
        ACCESSCREDITCRAD = 201,

        REPORTSUMMARY = 111,
        REPORTRESERVATION = 112,
        REPORTDISTRIBUTION = 113,
        REPORTPROFILE = 114,
        REPORTSCHEDULE = 115,

        PROP_Language = 20010,
        PROP_Address = 20020,
        PROP_Currency = 20030,
        PROP_Tax = 20040,
        PROP_Descriptions = 20050,
        PROP_Multimedia = 20060,
        PROP_Location = 20070,
        PROP_Meeting = 20080,
        PROP_Dining = 20090,
        PROP_Children = 20100,
        PROP_Roomtype = 20110,
        PROP_GuestPref = 20120,
        PROP_Branding = 20130,
        PROP_Amenities = 20140,
        PROP_Policies = 20500,
        RATE_RateCode = 21000,
        RATE_RateCodeGroup = 21010,
        RATE_MembershipTier = 21020,
        RATE_TierRate = 21030,
        RATE_SeasonRate = 21040,
        RATE_Overwrite = 21050,
        RATE_Discount = 21060,
        RATE_MktSegment = 21070,
        RATE_AccessCdoe = 21080,
        RATE_RateDayType = 21090,
        INV_Base = 21400,
        INV_Allot = 21410,
        INV_SellLimit = 21420,
        INV_Rollover = 21430,
        DASH_ALL = 21500,
        AVAIL_Hotel = 21510,
        AVAIL_Roomtype = 21520,
        AVAIL_RateCate = 21530,
        AVAIL_RateCodeGroup = 21540,
        AVAIL_RateCode = 21550,
        AVAIL_RoomRate = 21560,
        AVAIL_Channel = 21570,
        AVAIL_SubCh = 21580,
        AVAIL_ViewAll = 21590,
        GRPPKG_Group = 22000,
        GRPPKG_Package = 22010,
        GRPPKG_AddOn = 22020,
        RESV_ResDetail = 23000,
        RESV_AddOnDetail = 23010,
        RESV_InterResv = 23020,
        RPT_General = 24010,
        RPT_TopGuests =24020,
        RPT_Denail = 24050,
        RPT_RewardOverview = 24060,
        RPT_LoyaltyMemberProduction = 24070,
        RPT_LoyaltyCreditExpiration = 24080,
        RPT_Arrival = 24090,
        
        MKT_EmailSetup = 25000,
        //MKT_PrePostEmailDays = 25010,
        MKT_WebTrack = 25020,
        MKT_3rdParty = 25030,
        MKT_WebAnalytics = 25040,
        SEC_User = 26000,
        SEC_Role = 26010,
        PROF_Guest = 27000,
        PROF_MergeCandidates = 27001,
        PROF_Corp = 27010,
        PROF_Group = 27020,
        PROF_TA = 27030,
        PROF_HotelGroup = 27040,
        GDS_Hotel = 28100,
        ERS_Hotel = 28200,
        PMS_Hotel = 28300,

        PROP_CustomField = 20600,
        PROP_RewardFormula = 20700,
        PROP_EmailDaySetup = 20800,

        PROP_SystemLog=20900,
        PROP_LoyaltyEarningRule= 20710,
        PROP_GuestPortalNavigation=20720,
        PROP_Intergrations = 20730,
        PROP_WebContent = 20740,
        PROP_DedupeRule = 20750,
        PROP_PreprocessStandard=20760,
        PROP_TransactionEmail=20770,
        PROP_GuestLog= 20780,
        PROP_IdentityServerAccountSetUp = 20791
    }

    public enum UserAccessRightEnum
    {
        None = 0,
        ReadOnly = 1,
        Full = 2
    }

    public enum UserLevelEnum
    {
        ProgramLevel = 1,
        SystemLevel = 4
    }

    [Flags]
    public enum UserSettingEnum
    {
        None = 0,
        PciDisclaimer = 1 << 0,                 //  1
        PciDisclaimer1 = 1 << 1,                //  2
        PciDisclaimer2 = 1 << 2,                //  4
        PciDisclaimer3 = 1 << 3,                //  8
        PciDisclaimer4 = 1 << 4,                //  16
        SystemUse = 1 << 6,                     //  64  Only visible by systel level user
    }
    public enum _3rdPartyType
    {
        None = 0,
        AddOnsCategory = 1,
    }
    #endregion

    #region Hotel Level

    [Flags]
    public enum HotelConfigEnum
    {
        None = 0,
        AlternatDescription = 1,
        ResReviewer = 1 << 1,

        HotelLevelInventory = 1 << 2,               //          4
        //AutoRateYielding = 1 << 3,                //          8

        GDSRateCodeMappingByHotel = 1 << 4,         //         16
        AllowHotelManageChannel = 1 << 5,           //         32

        SimpleTax = 1 << 6,                         //         64
        ShowLOS_ABR_BES = 1 << 7,                   //        128
        SimpleTAComm = 1 << 8,                      //        256

        AllowBranding = 1 << 9,                     //        512
        ODDREnabled = 1 << 10,                      //       1024
        SimpleChannelDesc = 1 << 11,                //       2048
        HotelInvAvailBlockAlloted = 1 << 12,        //       4096
        //  True:  HotelInventory.MaxAllowed >= HotelInventory.Reserved + HotelInventory.BlockAlloted + NewRooms
        //  False: HotelInventory.MaxAllowed >= HotelInventory.Reserved + HotelInventory.BlockReserved + NewRooms
        //GroupAllotSeperate = 1 << 13,               //       8192
        //RubiconEnabled = 1 << 14,                   //      16384
        EnableResReinstate = 1 << 15,                 //      32768
        BBEShowRateCateList = 1 << 16,              //      65536
        BBEShowRateCheckBox = 1 << 17,              //     131072
        NotInUse1 = 1 << 18,                     //     262144        
        EnableHotelResEmail = 1 << 19,              //     524288
        DisableGuestResEmail = 1 << 20,             //    1048576
        DisableHotelDelivery = 1 << 21,             //    2097152       //  Will be removed after 10/1
        EnableCVV2 = 1 << 22,                       //    4194304
        BaseInvUpdateAvail = 1 << 23,               //  0:  Base Inventory screen update the RoomInventory.Allocation.
        //  1:  Base Inventory screen update the Available Rooms.
        MultiRoomAutoSplit = 1 << 24,               //   16777216
        //  Pms interface enabled hotel
        //  False:  Send Email, Fax for every reservation event
        //  True:   Do not send email, Fax to hotel
        //          Send Email, Fax only when there is a delivery faliure
        //  
        //  Non pms interface enabled hotel
        //  False:  Send Email, Fax for every reservation evernt
        //  True:   Do not send email, Fax to hotel
        //  If the sum of all available rooms is less than 0.  Does not allow booking.

        AllowCCInHotelEmailFax = 1 << 25,           //   33554432
        CallAssist = 1 << 26,                       //   67108864    
        EPEFR = 1 << 27,                            //  134217728   EnablePmsEventForReporting
        EnablePrePostEmail = 1 << 28,               //  268435456
        EnableMultiByteCharacter = 1 << 29,         //  536870912
        EnableHotelCRO = 1 << 30,
    }

    [Flags]
    public enum HotelDetailOptionEnum
    {
        DBTable = 0,
        Language = 1,
        Address = 2,
        ODDInfo = 4,
        AdminMiscGroup1 = 1 << 3,                   //          8
        //AdminMiscGroup2 = 1 << 4,                   //          16
    }

    [Flags]
    public enum ChainConfigEnum
    {
        None = 0,
        ChainLevelMediaLibrary = 1,
        DHLMMM = 1 << 1,                //  2           DisableHotelLevelMultiMediaManagement
        ForDemoUse = 1 << 2,            //  4           Data will not be used in any billing
        HasLogoForAdmin = 1 << 3,       //  8           
        ExclusiveGrouping = 1 << 4      //  16          Only for hotel grouping.  Hotel can only belongs to 1 hotel group within this hotel grouping
    }

    [Flags]
    public enum ClientConfigEnum
    {
        None = 0,
        AuditLog = 1,
        VDNSetup = 1 << 1,              //  2    
        ForDemoUse = 1 << 2,            //  4
    }

    public enum RateModeEnum
    {
        NA = 0,
        RateTierSimple = 1,
        RateTierAdvanced = 2,
        RateSeasonSimple = 3,
        RateSeasonAdvanced = 4,
        MixedAdvaced = 10
    }

    [Flags]
    public enum SupportedRateDetailEnum : short
    {
        Basic = 0,
        Rate3P = 1,
        Rate4P = 2,
        RateChild2 = 4,
        RateChild3 = 8,
        RateChild4 = 16,
        Crib = 32,
        RollawayAdult = 64,
        RollawayChild = 128
    }

    [Flags]
    public enum SelectHotelDetailEnum
    {
        ChDefault = 0,
        Full = 1,
        RefPoints = 2,
        HOD = 4,
        AddOn = 8,
        BBERtFltr = 16,
        Amenity = 32,
        TUBE = 64,
        Mobile = 128
    }

    public enum ResNotifBarCodeTypeEnum
    {
        None = 0,
        Code39 = 1,
        QRCode = 2,
    }

    #endregion

    [Flags]
    public enum ProfileDetailOptionEnum
    {
        DBTable = 0,
        Full = 1
    }

    [Flags]
    public enum UIBrandingSettingsEnum
    {
        Active = 1,
        Default = 2,
        Exclusive = 4,
        Deleted = 8,
    }

    public enum BBETemplateElementTypeEnum
    {
        DivBlock = 1,
        CssFile = 2,
        InlineJsBlock = 3,
        InlineStyleBlock = 4,

        HotelLevelColorVariable = 5,
        HotelLevelStringVariable = 6,
        HotelLevelImageVariable = 7
    }

    public enum BBETemplateElementConfigEnum
    {
        PerBranding = 1,
        PerTemplate = 2,
        //PerHotel = 3
    }

    public enum EntityTypeEnum
    {
        System = 0,
        Hotel = 1,
        Chain = 2,
        Client = 3,
        CallCenter = 4,
        Guest = 5,
        TravelAgent = 10,
        Corporation = 12,

        RateCategory = 100,
        Ratecode = 101,
        Rateplan = 102,
        Roomtype = 103,
        Policy = 104,
        GuestRequest = 105,
        Group = 106,
        Package = 107,
        Service = 109,
        ServiceItem = 110,
        Ratetier = 111,
        RatecodeGroup = 112,
        Tax = 113,
        TaxItem = 114,
        RoomRate = 115,
        RoomChannel = 116,
        RoomSubChannel = 117,
        Reservation = 118,
        RateChannel = 116,
        GroupMaster = 120,

        ProfileType = 30,
        CreditCard = 31,
        GuaranteeType = 32,
        ContactInfoType = 33,
        TelephoneTechType = 34,
        LanguageCode = 36,
        CurrencyCode = 37,
        Channel = 38,
        SubChannel = 39,

        Cluster = 99,
        EmailTemplate = 121,
        CorpUser = 122,
    }

    //public enum StandardMappingEnum
    //{
    //    Roomview = 1,
    //    RoomCategory = 2,
    //    BedType = 3,
    //    RoomClass = 4,
    //    SomkingPref = 5,
    //}

    public enum CachableObjectTypeEnum
    {
        None = 0,
        Hotel = 1,
        HotelLanguage = 2,
        HotelChannel = 3,
        HotelAmenity = 4,
        HotelCreditCard = 5,
        HotelCancelReason = 6,
        HotelCurrency = 7,
        HotelPaymentType = 8,

        PmsSetup = 9,

        Rateplan = 10,
        RateplanChannel = 11,
        RateplanText = 12,
        RateplanRaster = 13,
        Ratecode = 15,
        RatecodeChannel = 16,
        //        RatecodeText = 17,
        RatecodeRateplan = 18,
        RatecodeGroup = 19,

        Roomtype = 20,
        RoomtypeAmenity = 21,
        RoomtypeChannel = 22,
        //        RoomtypeText = 23,

        Service = 25,
        ServiceChannel = 26,

        ServiceItem = 30,
        ServiceItemChannel = 31,

        EQCSetup = 40,
        GDSPropertyMapping = 41,
        GDSRatecodeMapping = 42,
        GDSRoomtypeMapping = 43,
        ExtResSvcSetup = 44,

        Ratetier = 45,
        RatetierCalendar = 46,

        Group = 50,
        GroupChannel = 51,

        Package = 55,
        PackageText = 56,

        Fee = 60,

        Tax = 65,
        TaxByEntity = 66,

        Policy = 70,
        PolicyByEntity = 71,
        DiscountRule = 73,
        DiscountRuleByEntity = 74,

        Restriction = 75,

        GuestRequest = 78,

        TACommission = 80,
        TACommissionByEntity = 81,
        BBEBranding = 83,
        UiSetup = 84,
        RateYieldingRule = 85,


        HOD_Enable = 88,
        HOD_Disable = 89,
        HOD_Media = 90,
        HOD_GDS = 91,
        HOD_Facility = 92,
        HOD_Contact = 93,
        HOD_Basic = 94,
        HOD_Area = 95,
        HOD_Affiliation = 96,
        HOD_Policy = 97,


        ImgVideo = 100,
        GroupText = 101,
        EmailDesign = 102,
        EmailBody = 103,
        Guest = 104,
        Address = 105,
        PhoneNumber = 106,
        GroupMaster = 107,
        Corporation = 108,
        PackageChannel = 109

    }



    [Flags]
    public enum RatecodeOptionEnum
    {
        None = 0,
        HideRateAmount = 1,
        NoGuestEmail = 2,
        GoogleROH = 4,
        //YieldingAuto = 8,
        IgnoreHurdle = 16,
        IgnoreDayType = 32,     //YieldingDayType = 32,
        QualifiedRate = 64,
        Sorting = 2 << 9,
        RateDerivation = 2 << 10,
        BeginEndSellDate = 2 << 13,
        BeginEndDate = 2 << 15,
        LengthOfStay = 2 << 17,
        AdvancedBooking = 2 << 19,
        Category = 2 << 20,
        RestrictedRate = 2 << 22,
        BBEFilter = 2 << 23,
        TaComm = 2 << 24,
        SingleTier = 2 << 28,
        RateCodeGroup = 2 << 29,
        //2 << 14,
        //2 << 16,
        //2 << 18,

        ChannelControl = 2 << 11,
        MultiMedia = 2 << 12,
        MultiChannelDescription = 2 << 7,
        MultiLanguageDescription = 2 << 8,
        Description = 2 << 21,
        GDSDescription = 2 << 25,
        GDSMapping = 2 << 26,

        ExtPromoCodeRedemption = 2 << 27,
    }

    [Flags]
    public enum RatecodeUpdateEnum
    {
        None = 0,
        UpdateAccessControl = 2 << 23,
        UpdateRateDerivation = 2 << 24,
        UpdateRemoveOverWriteRates = 2 << 25,
        UpdateBase = 2 << 26,
        UpdateChannelDesc = 2 << 27,
        UpdateLanguageDesc = 2 << 28,
        UpdateChannelControl = 2 << 29,
        UpdateMultiMedia = 2 << 30
    }
    [Flags]
    public enum DiscountRuleOptionEnum
    {
        None = 0,
        UpdateBase = 2 << 0,
        UpdateDiscountCondition = 2 << 1,
        UpdateDiscountAction = 2 << 2,
        UpdateLanguageDesc = 2 << 3,
        DiscountCode = 2 << 5,
        DiscountName = 2 << 6,
        DiscountDescription = 2 << 7,
        DiscountCondition = 2 << 8,
        DiscountAction = 2 << 9,
        Priority = 2 << 10
    }
    [Flags]
    public enum DayOfWeekEnum
    {
        None = 0,
        Sun = 1,
        Mon = 2,
        Tue = 4,
        Wed = 8,
        Thu = 16,
        Fri = 32,
        Sat = 64,
        NA = 128
    }

    public enum RoomStayListGroupByEnum
    {
        None = 0,                   //  Need to remove this and compile the entire system by 12/31/2011
        Ratecode = 1,
        Roomtype = 2,
        Amount = 3
    }

    public enum RoomStayListOrderByEnum
    {
        None = 0,                   //  Need to remove this and compile the entire system by 12/31/2011
        Amount = 1,
        HotelPrefered = 2
    }

    public enum ContactMethodUseTypeEnum
    {
        All = -1,
        Unknown = 0,
        Business = 1,
        Home = 2,
        GuestContact = 3,
        Billing = 4,
        HotelReservation = 10
    }



    public enum HotelMediaFormatEnum
    {
        Image_JPG = 1,
        Image_GIF = 2,
        Image_PNG = 3,
        Video_FLV = 4,
        Video_UTB = 5,      //  YouTube Video
        Video_FLK = 6,      //  Flicker Phone Albumn
    }

    public enum HotelMediaTypeEnum
    {
        PropertyLogo = 1,
        PropertyImage_Stardard = 2,
        PropertyVideo_GMGreeting = 3,
        Group_Small = 11,
        Group_Stardard = 12,
        RoomType_Small = 21,
        RoomType_Stardard = 22,
        RoomTypeVideo_Stardard = 23,
        Package_Small = 31,
        Package_Stardard = 32,
        PackageVideo_Stardard = 33,
        Service_Small = 41,
        Service_Stardard = 42,
        ServiceVideo_Stardard = 43,
        ServiceItem_Small = 51,
        ServiceItem_Stardard = 52,
        ServiceItemVideo_Stardard = 53,
        SimplePackage_Standard = 61,
    }

    public enum TextTypeEnum
    {
        WebPageLabels = 1,
        PropertyLevelText = 2,

        RoomtypeName = 5,
        RoomtypeDescription = 6,

        RatecodeName = 9,
        RatecodeDescription = 10,

        PolicyName = 16,
        PolicyDescription = 17,

        GroupName = 57,
        GroupDescription = 58,
        GroupAdditionalPolicy = 52,

        PackageName = 55,
        PackageDescription = 56,
        PackageAdditionalPolicy = 53,

        TaxItem = 60,

        BooleanYN = 30,
        GuaranteeCode = 31,
        MonthName = 32,
        WeekdayName = 33,

        GuestRequestDetailDescription = 42,
        GuestRequestName = 40,
        GuestRequestDetailName = 41,

        CancelReason = 45,
        Language = 34,
        Country = 35,
        Region = 36,

        Amenity = 37,
        RateCategory = 38,
    }

    public enum TravelAgentTypeEnum
    {
        IATA_ARC_TIDS = 1,
        EMBRATUR = 2,
        JATA = 3,
        IHA = 4,
        PATA = 5,
        ASTA = 6,
        ABTA = 7,
        PSRA = 8,
        BookingAgent = 10
    }

    //public enum AmenityScopeEnum
    //{
    //    Property = 1,
    //    Roomtype = 2,
    //    BothPropertyRoomtype = 3,
    //    Nearby = 4,
    //    Exist = 5
    //}

    //public enum AmenityFlagEnum
    //{
    //    ConfirmableAndComplimentary = 1,
    //    ConfirmableWithCost = 2,
    //    OnRequestAndConplimentary = 3,
    //    OnRequestWithCode = 4,
    //    Exist = 5
    //}

    [Flags]
    public enum UpdateTypeEnum
    {
        None = 0,
        Insert = 1,
        Update = 2,
        Delete = 4
    }

    public enum HtmlElementTypeEnum
    {
        CheckBox = 1,
        DropDownList = 2,
        RadioButton = 3,
        TextBox = 4,
        USCityCombo = 5
    }

    public enum ExtEventTypeEnum
    {
        ResvNotif = 1,        //  With pre and post
        ProfileUpdate = 2,        //  W/O pre and post
        SignUp = 3,        //  W/O pre and post
        SignIn = 4,

        ResvStayInfo = 10,

        InvalidLogin = 20,
    }

    public enum ResExportFormatEnum
    {
        Res_WindSurferPms = 1,
        Res_OTA_ver1 = 10,
        Res_CSV_AmericInn = 20
    }

    [Flags]
    public enum ResExportConfigEnum
    {
        None = 0,
        IncludeCC = 1,
        IncludePrePost = 2,
    }

    public enum UISetupTypeEnum
    {
        TUBEVer1 = 1,
        MobileVer1 = 100
    }

    [Flags]
    public enum GuestSettingEnum
    {
        None = 0,
        SingleUse = 1,
        UnSubscribe = 2,
        Primary = 4
    }


    [Flags]
    public enum AppServiceControlSettingEnum
    {
        None = 0,
        AlertWhenTimeout = 1,
        AlertWhenRepeat = 2,
    }

    [Flags]
    public enum AppServiceAlertSettingEnum
    {
        None = 0,
        Disabled = 1,
    }

    [Flags]
    public enum AppServiceOutageTypeEnum
    {
        None = 0,
    }

    [Flags]
    public enum AppServiceInstanceSettingEnum
    {
        None = 0,
        Disabled = 1,
    }

    public enum LanguageEnum
    {
        EN = 1,
        CN = 2,
        HK = 3,
        FR = 4,
        RU = 5,
        ES = 6,
        VN = 7,
        DE = 8,
        JP = 9,
        KR = 10
    }

    public enum CorpUserTypeEnum
    {
        ADMINISTRATOR = 1,
        USER = 2,
        BOOKER = 3
    }

    public enum GuestPointActivityTypeEnum
    {
        CheckOutAdd = 1,
        Buy = 2,//extra Service
        AuthorizedUserAdd = 3,
        LinkReservation = 4,
        Expired = 5,
        Redeem = 6,
        RedeemCancel = 7,
        ManualCancelledReservationPoints = 8,
        ReservationCancelled = 10,
        TangoCardReward = 9

    }

    public enum EmailTypeEnum
    {
        Guest_SignUp = 31,
        GuestPasswordRecovery = 32,
        CorpSignUp = 35,
        CorpPasswordRecovery = 36,
        Sys_ResetPasswordNotification = 37,
        Sys_CreateUserSuccess = 38,
        Resv_New = 39,
        Resv_Mod = 40,
        Resv_Cxl = 41,
        Resv_Pre = 42,
        Resv_Post = 43,
        Sys_ImportGuestNotice = 44,
        Sys_ImportReservationNotice = 45,
        ReservationIssuedReward = 46,
        Reward_Resv_Cxl = 47,
        Guest_UpDownGrading = 48,
        Reward_Room_Redeem=49,
        Reward_Room_RedeemReverse = 50
    }

    public enum EmailRateCodeEnum
    {
        Sys = 41,
        Resv = 42,
        Guest = 43
    }

    public enum ReservationSearchFilter
    {

        /// <remarks/>
        All = 1,

        /// <remarks/>
        Past = 2,

        /// <remarks/>
        Cancellations = 3,
    }

    public enum GuestMenuEnum
    {
        Overview = 1,
        Reservations=2,
        Reward = 3,
        Profile = 4,
        Offers=5,
        Point=6,
        Purchases=7,
        GiftCard=8,
        HotelStay = 9,     
        Preferences = 10
        
    }

    public enum GuestSourceEnum
    {
        AdminInsert=1,
        GuestPortalRegistor=2,
        GuestImport=3,
        ReservationImport=4
    }

    public enum GuestSubSourceEnum
    {
        AdminPortal = 1,
        GuestPortal = 2
    }

    public enum LoyaltyEarningRuleTypeEnum
    {
        Base = 1,
        Promotion = 2
    }

    public enum LoyaltyEarningStayDateTypeEnum
    {
        Arrival = 1,
        Departure = 2,
        Both=3
    }

    public enum SpendCalculationType
    {
        RoomSpend=1,
        NonRoomSpend=2,
        Both=3
    }

    public enum RewardType
    {
        ReservationReward = 1,
        RoomReward=2,
        CreditRedemption=3,
        PartnerReward=4
    }

    public enum ProfielUpdateEvent
    {
        Insert = 1,
        Update = 2,
        Delete = 3,
        AutoMerge = 4,
        ManualMerge=5,
        UpGranding=6,
        DownGrading=7,
        Import=8,
        ModifyPassword=9,
        SendEmail = 10,
        ReservationAssignGuest=11
    }


    public enum GrantTypes
    {
        Authorization_code=1,
        Client_credentials=2,
        Hybrid=3,
        Implicit=4,
        Password=5
    }

    public enum RewardReason
    {
        Loyalty = 1,
        Promotion = 2,
        Gesture = 3
    }

}


