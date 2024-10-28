import json
from faker import Faker


def generate_persons(tenures: list[dict]) -> list[dict]:    
    all_persons = []

    fake = Faker("en_GB")

    for tenure in tenures:
        for household_member in tenure["householdMembers"]:
            person = {}
            
            prefix, first, middle, last = household_member["fullName"].split()

            middle_weighted_choice = fake.random_element(
                elements=(
                    (middle, 0.1),
                    ("", 0.9)
                )
            )[0]

            person["id"] = fake.uuid4()
            person["title"] = prefix
            person["preferredTitle"] = ""
            person["preferredFirstname"] = ""
            person["preferredMiddleName"] = ""
            person["preferredSurname"] = ""
            person["firstname"] = (first + " " + middle_weighted_choice).strip()
            person["middleName"] = ""
            person["surname"] = last
            person["dateOfBirth"] = household_member["dateOfBirth"]
            person["placeOfBirth"] = ""
            person["tenures"] = [
                {
                    "id": tenure["id"],
                    "paymentReference": tenure["paymentReference"],
                    "type": tenure["tenureType"]["description"],
                    "startDate": tenure["startOfTenureDate"],
                    "endDate": tenure["endOfTenureDate"],
                    "assetFullAddress": tenure["tenuredAsset"]["fullAddress"],
                    "uprn": tenure["tenuredAsset"]["uprn"],
                    "propertyReference": tenure["tenuredAsset"]["uprn"],
                    "assetId": tenure["tenuredAsset"]["id"]
                }
            ]
            person["personTypes"] = ["Tenant"]
            all_persons.append(person)
    
    return all_persons