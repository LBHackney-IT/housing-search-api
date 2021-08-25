using HousingSearchApi.V1.Domain;
using System;
using System.Collections.Generic;

namespace HousingSearchApi.V1.Infrastructure
{
    public static class TypeMappingExtensions
    {
        public static List<string> GetPersonTypes(this PersonType personType)
        {
            if (personType == PersonType.Tenant)
            {
                return new List<string>()
                {
                    "Asylum Seeker",
                    "Commercial Let",
                    "Introductory",
                    "License Temp Ac",
                    "Lse 100% Stair",
                    "Mesne Profit Ac",
                    "Non-Secure",
                    "Private Garage",
                    "Private Sale LH",
                    "RenttoMortgage",
                    "Secure",
                    "Shared Equity",
                    "Shared Owners",
                    "Short Life Lse",
                    "Temp Annex",
                    "Temp B&B",
                    "Temp Decant",
                    "Temp Hostel",
                    "Temp Hostel Lse",
                    "Temp Private Lt",
                    "Temp Traveller",
                    "Tenant Acc Flat",
                    "Tenant Garage"
                };
            }
            else if (personType == PersonType.Leaseholder)
            {
                return new List<string>()
                {
                    "Freehold",
                    "Freehold (Serv)",
                    "Leasehold (RTB)",
                    "Registered Social Landlord"
                };
            }

            throw new ArgumentException("Invalid type of person.");
        }
    }
}
