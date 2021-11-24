# DigitalShop

A microservice-based RESTful API for a simple eCommerce application, created to meet the Distinction requirements of COS30041.

## Prerequisities

In order to run the containers you'll need `docker` and `docker-compose` installed.
* [docker](https://docs.docker.com/get-docker/)
* [docker-compose](https://docs.docker.com/compose/install/)

## Setup
Copy the example env file and make the required configuration changes in the .env file. Note JWT_SECRET has to have a minimum of 16 character
```bash
cp .env.example .env
```
Then build and start the containers
```bash
docker-compose build
docker-compose up -d
```
The API will be accessible through `http://localhost:5000`
