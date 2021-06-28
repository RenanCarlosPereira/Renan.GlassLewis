# GlassLewis
## Glass Lewis Code Challenge

Design and code a Web API solution in .NET Core/5+ and C# for a middle tier “Company API.”
 
Using this WebAPI, an end user should be able to:
 
1.	Create a Company record specifying the Name, Stock Ticker, Exchange, ISIN, and optionally a website URL. You are not allowed create two Companies with the same ISIN. The first two characters of an ISIN must be letters / non numeric.
2.	Retrieve an existing Company by Id
3.	Retrieve a Company by ISIN
4.	Retrieve a collection of all Companies
5.	Update an existing Company
 
Sample company records:

| Name               | Exchange             | Ticker | ISIN         | website                    |
|--------------------|----------------------|--------|--------------|----------------------------|
| Apple Inc.         | NASDAQ               | AAPL   | US0378331005 | http://www.apple.com       |
| tish Airways Plc   | Pink Sheets          | BAIRY  | US1104193065 |                            |
| Heineken NV        | Euronext Amsterdam   | HEIA   | NL0000009165 |                            |
| Panasonic Corp     | Tokyo Stock Exchange | 6752   | JP3866800000 | http://www.panasonic.co.jp |
| Porsche Automobil  | Deutsche Börse       | PAH3   | DE000PAH0038 | https://www.porsche.com/   |



# requirements
1. Docker must be installed and runing, you can find how to install here: [docker](https://docs.docker.com/engine/install/)
2. Docker compose must be installed you can find how to install here: [docker-compose](https://docs.docker.com/engine/install/)
3. Make sure port 800, 801 and 443 are not begin used by another process
4. Optionally, for debugging proposes in visual studio you must have .NET Core/5

# Installation

Execute the command line bellow in the root directory of the solution.

```sh
docker-compose -f .\src\Renan.GlassLewis.Docker\docker-compose.yml -p glass up
```

By default, the Docker will expose port 801 and 800, so change this within the
_docker-compose_ file if necessary.

This project has 2 applications, front-end and back-endÇ 

## Front-end
The front-end is exposed on http port 801, you can check it up going to http://localhost:801/.

## Back-end
The back-end is exposed on http port 800, you can check it up going to http://localhost:800/swagger/.
To authenticate with swagger you must go to path URL 

## swagger Authentication

> /api/Token/login

* Username: GlassLewis 
* Password: 123

this route will generate a **Token** this token you must use to authorize swagger calls


# Getting started

1. Open the front end application going to http://localhost:801/.

2. Navegate to the menu company and create a new company. 

3. Fill the company information with the requeriments of this challange and save it. 

4. Go back to menu company and edit the information you just add with the requeriments of this challange and save it

