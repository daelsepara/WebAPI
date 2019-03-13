# WebAPI

This is a sample project for demonstrating Web APIs. The REST service implemented in C# and responds to **GET**/**POST**/**PUT**/**DELETE** request calls on a single endpoint (_~/prescriptions_). An MySQL server with a database and table named **Prescriptions** is required. To create the table structure, please refer to the .sql file provided: ![migrations.sql](/migrations/migrations.sql)

The calls operate on the records inside the _Prescriptions_ table.

**GET ~/prescriptions/**

Returns a list of all prescriptions, available in the database.

Input: &lt;empty&gt;

Output: Array&lt;Prescription&gt;

Returns an HTTP 404 status code if no records are available

