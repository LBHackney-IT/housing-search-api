namespace HousingSearchApi.V1.Infrastructure
{
    public static class Constants
    {
        public const string CorrelationId = "x-correlation-id";
        public const string CorsHeader = "Access-Control-Allow-Origin";
        public const string CorsHeaderMethods = "Access-Control-Allow-Methods";
        public const string CorsHeaderHeaders = "Access-Control-Allow-Headers";

        // Elastic Search
        public const string EsIndex = "hackney_persons";
    }
}
