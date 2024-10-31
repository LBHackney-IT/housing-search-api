"""
Fixture generator for elasticsearch
Depends on faker library (pip install faker)
"""

import json
import os
from assets import generate_assets
from tenures import generate_tenures
from persons import generate_persons

OUT_DIR = os.path.join(os.path.dirname(os.path.realpath(__file__)), "..", "Files")

def to_elasticsearch_bulk(item: dict, index_name: str):
    quoted_id = f'{item["id"]}'
    return f'{{"index": {{"_index": "{index_name}", "_id": "{quoted_id}"}}}}\n{json.dumps(item)}'

def to_file(data: list[dict], filename: str):
    bulk_items = [
        to_elasticsearch_bulk(item, filename.split(".")[0])
        for item in data
    ]
    bulk_str = "\n".join(bulk_items) + "\n"
    with open(os.path.join(OUT_DIR, filename), "w") as f:
        f.write(bulk_str)


def main():
    assets = generate_assets()
    to_file(assets, "assets.json")
    
    tenures = generate_tenures(assets)
    to_file(tenures, "tenures.json")

    persons = generate_persons(tenures)
    to_file(persons, "persons.json")


if __name__ == "__main__":
    main()
