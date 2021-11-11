using System;

namespace HousingSearchApi.V1.Boundary.Responses.Transactions
{
    public class SuspenseResolutionInfo
    {
        public DateTime? ResolutionDate { get; }

        public bool IsResolve
        {
            get
            {
                if ((IsConfirmed && IsApproved))
                    return true;
                return false;
            }
        }

        public bool IsConfirmed { get; }
        public bool IsApproved { get; }
        public string Note { get; }

        private SuspenseResolutionInfo(DateTime? resolutionDate, bool isConfirmed, bool isApproved, string note)
        {
            ResolutionDate = resolutionDate;
            IsConfirmed = isConfirmed;
            IsApproved = isApproved;
            Note = note;
        }

        public static SuspenseResolutionInfo Create(DateTime? resolutionDate, bool isConfirmed, bool isApproved, string note)
        {
            return new SuspenseResolutionInfo(resolutionDate, isConfirmed, isApproved, note);
        }
    }
}
