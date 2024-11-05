#!/bin/bash

ES_BASE_URL=$ELASTICSEARCH_DOMAIN_URL
CWD=$(pwd)
INDEX_DIR="/setup/indices"
DATA_DIR="/setup/data"

apk add --no-cache curl

# delete indices if they exist
curl -X DELETE "$ES_BASE_URL/persons"
curl -X DELETE "$ES_BASE_URL/tenures"
curl -X DELETE "$ES_BASE_URL/assets"


echo "Creating indices..."

# Create indices
curl -X PUT "$ES_BASE_URL/persons" -H 'Content-Type: application/json' -d @"$INDEX_DIR/personIndex.json"
curl -X PUT "$ES_BASE_URL/tenures" -H 'Content-Type: application/json' -d @"$INDEX_DIR/tenureIndex.json"
curl -X PUT "$ES_BASE_URL/assets" -H 'Content-Type: application/json' -d @"$INDEX_DIR/assetIndex.json"

echo "Indices created."

echo "Inserting data..."

# Insert bulk data
for index in "persons" "tenures" "assets"; do
    ES_ENDPOINT="$ES_BASE_URL/$index/_bulk"

    # Loop through each file in the index directory
    for file in "$DATA_DIR"/*; do
        if [ -f "$file" ]; then
            echo "Processing $file for index $index"
            curl -s -H "Content-Type: application/json" -XPOST "$ES_ENDPOINT" --data-binary "@$file"
        else
            echo "No files found in $DATA_DIR"
        fi
    done
done

echo "Bulk insert completed."