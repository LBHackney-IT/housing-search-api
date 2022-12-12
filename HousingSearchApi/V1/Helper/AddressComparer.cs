using Hackney.Shared.HousingSearch.Domain.Asset;
using System;
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

            var addressline1_1 = address1.AddressLine1.Split(' ');
            var addressline1_2 = address2.AddressLine1.Split(' ');

            if (addressline1_1[1] == addressline1_2[1]
                && int.TryParse(addressline1_1[0], out var house1)
                && int.TryParse(addressline1_2[0], out var house2))
            {
                return house1 - house2;
            }
            return addressline1_1[1].CompareTo(addressline1_2[1]);
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
