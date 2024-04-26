using Microsoft.AspNetCore.Mvc;

namespace HousingSearchApi.V1.Boundary.Requests
{
    public class GetTenureListRequest : HousingSearchRequest
    {
        [FromQuery(Name = "uprn")]
        public string Uprn { get; set; }

        [FromQuery(Name = "bookingStatus")]
        public string BookingStatus { get; set; }

        [FromQuery(Name = "isTemporaryAccommodation")]
        public string IsTemporaryAccommodation { get; set; }
    }
}
