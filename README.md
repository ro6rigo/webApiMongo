# webApiMongo

# Comandos Docker
docker run -d -e MONGO_INITDB_ROOT_USERNAME=ADM -e MONGO_INITDB_ROOT_PASSWORD=123 -p 27017:27017 --name meu-mongo mongo

docker ps

docker exec -it meu-mongo mongosh -u adm -p 123

show dbs
use ApiMongo
db.Produtos.insertOne({nome:"Mouse", marca: "Logitech"})
db.Produtos.find({nome:"Mouse"})
