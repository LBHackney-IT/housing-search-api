using Nest;

namespace HousingSearchApi.V1.Domain.ElasticSearch
{
    public class Tenures
    {
        [Text(Name = "id")]
        public string Id { get; set; }

        [Text(Name = "type")]
        public string Type { get; set; }

        public string StartDate { get; set; }

        public string EndDate { get; set; }

        public string AssetFullAddress { get; set; }

    }
}
