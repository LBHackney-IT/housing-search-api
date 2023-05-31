using Hackney.Shared.HousingSearch.Domain.Asset;
using System.Collections.Generic;
using System.Linq;

namespace HousingSearchApi.V1.Helper
{
    public class AddressComparer : IComparer<AssetAddress>
    {
        public int Compare(AssetAddress address1, AssetAddress address2)
        {
            //If the street address is the same, sort by the preamble, otherwise sort by addressline1
            if (address1.AddressLine1 == address2.AddressLine1)
            {
                var flatNumber1 = TryParseFlatNumber(address1);
                var flatNumber2 = TryParseFlatNumber(address2);

                if (flatNumber1 != null && flatNumber2 != null)
                {
                    return (int) flatNumber1 - (int) flatNumber2;
                }
            }

            var address1Parts = ExtractAddressParts(address1);
            var address2Parts = ExtractAddressParts(address2);

            // prevent IndexOutOfRange exception (addressLine1 contains no spaces)
            if (address1Parts.Count >= 2 && address2Parts.Count >= 2)
            {
                // if the street is the same, try compare house number
                if (address1Parts[1] == address2Parts[1]
                    && int.TryParse(address1Parts[0], out var house1)
                    && int.TryParse(address2Parts[0], out var house2))
                {
                    return house1 - house2;
                }

                return address1Parts[1].CompareTo(address2Parts[1]);
            }

            // default sorting - compare addresssLine1 alphabetically
            return address1.AddressLine1.CompareTo(address2.AddressLine1);
        }

        private static List<string> ExtractAddressParts(AssetAddress address)
        {
            var addressParts = new List<string> { };

            // the relevant parts of the address will be in lines 1 and 2

            if (!string.IsNullOrWhiteSpace(address.AddressLine1))
            {
                addressParts.AddRange(address.AddressLine1.Split(' '));
            }

            if (!string.IsNullOrWhiteSpace(address.AddressLine2))
            {
                addressParts.AddRange(address.AddressLine2.Split(' '));
            }

            return addressParts;
        }

        private static int? TryParseFlatNumber(AssetAddress address)
        {
            if (address.PostPreamble == null) return null;

            var success = int.TryParse(address.PostPreamble.Where(char.IsDigit).ToArray(), out var flatNumber);

            if (!success) return null;
            return flatNumber;
        }
    }
}
