using System;

namespace HousingSearchApi.V1.Boundary.Responses.Transactions
{
    public class Person
    {
        public Guid Id { get; }

        public string FullName { get; }

        private Person(Guid id, string fullName)
        {
            Id = id;
            FullName = fullName;
        }

        public static Person Create(Guid id, string fullName)
        {
            return new Person(id, fullName);
        }
    }
}
