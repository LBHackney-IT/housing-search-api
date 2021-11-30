using System;

namespace HousingSearchApi.V1.Boundary.Responses.Transactions
{
    public class Sender
    {
        public Guid Id { get; }

        public string FullName { get; }

        private Sender(Guid id, string fullName)
        {
            Id = id;
            FullName = fullName;
        }

        public static Sender Create(Guid id, string fullName)
        {
            return new Sender(id, fullName);
        }
    }
}
