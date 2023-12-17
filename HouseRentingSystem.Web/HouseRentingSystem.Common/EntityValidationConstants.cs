namespace HouseRentingSystem.Common
{
    public static class EntityValidationConstants
    {
        public static class CategoryValidation
        {
            public const int NameMinLength = 2;
            public const int NameMaxLength = 50;
        }

        public static class HouseValidation 
        {
            public const int TitleMinLength = 10;
            public const int TitleMaxLength = 50;

            public const int AddressMinLength = 30;
            public const int AddressMaxLength = 150;

            public const int DescriptionMinLength = 50;
            public const int DescriptionMaxLength = 500;

            public const int ImageUrlMaxlLength = 2048;

            public const string PricePerMonthMinValue = "0";
            public const string PricePerMonthMaxValue = "2000";
        }

        public static class AgentValidation
        {
            public const int PhoneMinLength = 7;
            public const int PhoneMaxLength = 15;
        }
    }
}
