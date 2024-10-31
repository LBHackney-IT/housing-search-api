from datetime import date, timedelta
from random import choice

from faker import Faker


def generate_assets(count: int = 2000) -> list[dict]:
    all_assets = []
    fake = Faker("en_GB")

    addresses_per_street = 10

    addresses = []
    for _ in range(count // addresses_per_street):
        street_name = fake.street_name()
        for i in range(addresses_per_street):
            flat_number = f"{choice([f'Flat {choice([fake.random_digit(), fake.random_letter().upper()])} ', f'Room {fake.random_digit()} ', 'Gge ', ' '])}{i+1}{choice([fake.random_letter().upper(), ''])}"
            address = f"{flat_number} {street_name}".replace("  ", " ").strip().title()
            addresses.append(address)

    for address in addresses:
        asset = {}

        asset["id"] = fake.uuid4()
        asset["assetId"] = str(fake.random_int(min=int(1e8), max=int(2e8)))
        asset["assetType"] = choice(["Property", "Block", "Estate", "Dwelling"])

        # Tenure
        start_of_tenure_date = fake.date_this_decade()
        end_of_tenure_date = choice(
            [start_of_tenure_date + timedelta(days=365),
            None]
        )
        type_ = choice(["Freehold", "Leasehold (RTB)", "Introductory", "Secure", "Temp Hostel Lse"])

        asset["tenure"] = {
            "id":  fake.uuid4(),
            "paymentReference": str(fake.random_int(min=int(1e8), max=int(2e8))),
            "startOfTenureDate": start_of_tenure_date.strftime("%Y-%m-%dT%H:%M:%SZ"),
            "endOfTenureDate": end_of_tenure_date.strftime("%Y-%m-%dT%H:%M:%SZ") if end_of_tenure_date else None,
            "type": type_,
            "isActive": True if end_of_tenure_date is None else False
        }

        asset["assetAddress"] = {
            "uprn": str(fake.random_int(min=int(1e11), max=int(11e10))),
            "addressLine1": address,
            "addressLine2": "Hackney",
            "addressLine3": "",
            "addressLine4": "",
            "postCode": fake.postcode(),
            "postPreamble": ""
        }

        asset["assetCharacteristics"] = {
            "numberOfBedrooms": fake.random_int(min=1, max=5),
            "numberOfLifts": fake.random_int(min=0, max=2),
            "numberOfLivingRooms": fake.random_int(min=1, max=2),
            "yearConstructed": str(fake.random_int(min=1900, max=date.today().year)),
            "optionToTax": fake.boolean(),
            "hasStairs": fake.boolean(),
            "numberOfStairs": fake.random_int(min=1, max=3),
            "hasRampAccess": fake.boolean(),
            "hasCommunalAreas": fake.boolean(),
            "hasPrivateBathroom": fake.boolean(),
            "numberOfBathrooms": fake.random_int(min=1, max=3),
            "hasPrivateKitchen": fake.boolean(),
            "numberOfKitchens": fake.random_int(min=1, max=3),
            "numberOfFloors": fake.random_int(min=1, max=3),
            "numberOfBedSpaces": fake.random_int(min=1, max=3),
            "numberOfCots": fake.random_int(min=1, max=3),
            "numberOfShowers": fake.random_int(min=1, max=3),
            "isStepFree": fake.boolean()
        }

        asset["rootAsset"] = fake.uuid4()
        asset["isActive"] = True if end_of_tenure_date is None else False
        asset["parentAssetIds"] = [fake.uuid4() for _ in range(fake.random_int(min=0, max=3))]

        asset["assetManagement"] = {
            
        }

        all_assets.append(asset)
    
    return all_assets