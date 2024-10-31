from faker import Faker


def generate_tenures(assets: list[dict]) -> list[dict]:    
    all_tenures = []

    fake = Faker("en_GB")

    for asset in assets:
        tenure = {}

        tenure["id"] = fake.uuid4()
        tenure["paymentReference"] = asset["tenure"]["paymentReference"]
        tenure["startOfTenureDate"] = asset["tenure"]["startOfTenureDate"]
        tenure["endOfTenureDate"] = asset["tenure"]["endOfTenureDate"]
        tenure["isActive"] = asset["tenure"]["isActive"]
        tenure["tenureType"] = {
            "description": asset["tenure"]["type"],
            "code": asset["tenure"]["type"][:3].strip().upper() # Good enough for test data
        }

        tenure["tenuredAsset"] = {
            "id": asset["id"],
            "fullAddress": asset["assetAddress"]["addressLine1"] + " " + asset["assetAddress"]["postCode"],
            "uprn": asset["assetAddress"]["uprn"],
            "type": asset["assetType"]
        }

        tenure["householdMembers"] = []
        member_count = fake.random_int(min=1, max=4)
        for i in range(member_count):
            member = {}
            member["id"] = fake.uuid4()
            member["fullName"] = f'{fake.prefix().replace(".","")} {fake.first_name()} {fake.first_name()} {fake.last_name()}'
            member["isResponsible"] = i == 0 # First member is responsible
            member["dateOfBirth"] = fake.date_of_birth().strftime("%Y-%m-%dT%H:%M:%SZ")
            member["personTenureType"] = "Freeholder" if member["isResponsible"] else "HouseholdMember"
            member["type"] = "Person"
            tenure["householdMembers"].append(member)
        
        all_tenures.append(tenure)
    
    return all_tenures