# WebAPI

This is a sample project for demonstrating Web APIs. The REST service implemented in C# and responds to GET/POST/PUT/DELETE request calls on a single endpoint (_~/prescriptions_). It requires MySQL server with a database and table named *Prescriptions* is assumed. To create the table structure, please refer to the .sql file provided: ![migration](/migrations/migrations.sql)

The calls operate on the records inside the _Prescriptions_ table.

**GET ~/prescriptions**
