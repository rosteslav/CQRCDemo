docker --version
docker ps //show images
docker network create --attachable -d bridge [name] // create network
docker network ls // show all networks 
docker-compose --version

with file "docker-compose.yml"
docker-compose up -d
docker ps

docker run -it -d --name mongo-container -p 27017:27017 --network mydockernetwork --restart always -v mongodb_data_container:/data/db mongo:latest // install mongoDB
docker ps

docker run -d -p 1433:1433 --restart always --name sql-container --network mydockernetwork -e 'ACCEPT_EULA=Y' -e 'SA_PASSWORD=$tr0ngS@P@ssw0rd02' -e 'MSSQL_PID=Express'  mcr.microsoft.com/mssql/server:2017-latest-ubuntu
docker ps

//use
https://robomongo.org/download

yourStrong(!)Password