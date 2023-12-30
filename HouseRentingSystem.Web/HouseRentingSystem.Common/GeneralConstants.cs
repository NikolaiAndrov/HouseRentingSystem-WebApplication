namespace HouseRentingSystem.Common
{
    public static class GeneralConstants
    {
        public const int AppReleasedYear = 2023;

        public const int DefaultPage = 1;
        public const int DefaultEntitiesPerPage = 3;

        public const string AdminRoleName = "Administrator";
        public const string DevelopmentAdminEmail = "admin@abv.bg";
        public const string AdminAreaName = "Admin";

        public const string UsersCacheKey = "UsersCache";
        public const string RentsCacheKey = "RentsCache";
        public const int UsersCacheDurationMinutes = 5;
    }
}
