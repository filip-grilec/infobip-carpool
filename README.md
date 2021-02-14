# Carpool Management

Run backend:
`dotnet run`

Run front end :

```cd .\ClientApp\```

```npm i```

```npm start```

Deployed on the web at [Azure](https://infobip-carpool.azurewebsites.net/)


Technologies used:

* `.Net Core 3.1`  backend
    * Database: `EntityFrameworkCore SQLite provider`
    * Authentication: `Firebase`
    * Authorization: `None`
* `React.js` front end (CRA)
  * `MaterialUI`
  

Developed using TDD with `80%` total test coverage with
`Service` and `Cotroller` layers having `100%` test coverage

![Coverage](https://github.com/filip-grilec/blob/blob/master/TestCoverage.jpg)

Some features are missing because of time constraints :

* Edit Travel Plan

Apply migration to database
`python3 .\UpdateDatabase.py`


Postman collection and enviroment included
