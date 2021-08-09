To work with local ElasticSearch you need to do next steps:

1. Install docker and write next command 
docker run -p 9200:9200 -p 9300:9300 -e "discovery.type=single-node" docker.elastic.co/elasticsearch/elasticsearch:7.14.0
If all works fine you can go to page localhost:9200

2. Install Postman and create GET request with following path
http://localhost:9200/_cat/indices
You will get list of all current indices

3. If you want to add index (table) - create PUT request with following path
http://localhost:9200/index_name
In body you need to insert configuration for index (for example: asset_index.json in this folder)

4. For insert data in index you need to create POST request with following path
http://localhost:9200/index_name/_create/id
Where id - unique int number
In body insert a json model

5. To delete index you need to create DELETE reuqest http://localhost:9200/index_name
