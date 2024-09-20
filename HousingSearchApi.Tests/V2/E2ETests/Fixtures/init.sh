#!/bin/bash
# Bulk load fixture data into Elasticsearch

apk add --no-cache curl

# Wait until Elasticsearch is ready by checking the cluster health
until curl -s test-elasticsearch:9200/_cluster/health?wait_for_status=green | grep -q '"status":"green"'; do
    echo "Waiting for Elasticsearch to be ready..."
    sleep 1
done

echo "Elasticsearch is up and running - performing bulk data insertion"

# Perform bulk insert using the Bulk API
curl -s -XPOST 'test-elasticsearch:9200/_bulk' \
     -H "Content-Type: application/json" \
     --data-binary "@/usr/share/elasticsearch/data/assets.json"

echo "Bulk data insertion complete"


