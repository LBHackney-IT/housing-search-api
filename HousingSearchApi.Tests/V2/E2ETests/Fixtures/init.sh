#!/bin/bash

# install curl and jq
apk add --no-cache curl jq;

# Wait until Elasticsearch is ready by checking the cluster health
until curl -s test-elasticsearch:9200/_cluster/health?wait_for_status=green | grep -q '"status":"green"'; do
    echo "Waiting for Elasticsearch to be ready..."
    sleep 1
done

echo "Elasticsearch is up and running - loading data one by one"

# Read the JSON file and insert documents one by one
jq -c '.[]' /usr/share/elasticsearch/data/assets.json | while read doc; do
    curl -XPOST 'test-elasticsearch:9200/assets/_doc/' -H "Content-Type: application/json" -d "$doc"
    echo "Inserted: $doc"
done

echo "Data insertion complete"

