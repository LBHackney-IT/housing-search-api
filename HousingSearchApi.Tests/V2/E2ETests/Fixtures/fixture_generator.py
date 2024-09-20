"""
Fixture generator for elasticsearch (currently assets only)
Depends on faker library (pip install faker)
"""


import json
from datetime import timedelta
from random import choice

from faker import Faker

CWD = "HousingSearchApi.Tests/V2/E2ETests/Fixtures"

all_assets = []

fake = Faker("en_GB")

addresses = []
for _ in range(100):
    street_name = fake.street_name()
    for i in range(10):
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

    all_assets.append(asset)


def to_elasticsearch_bulk(asset: dict, index_name = "assets"):
    quoted_id = f'{asset["id"]}'

    return f'{{"index": {{"_index": "{index_name}", "_id": "{quoted_id}"}}}}\n{json.dumps(asset)}'


ASSET_ES_BULK = "\n".join([to_elasticsearch_bulk(asset) for asset in all_assets])
with open(f"{CWD}/assets.json", "w") as f:
    f.write(ASSET_ES_BULK + "\n")
