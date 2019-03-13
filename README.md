# WebAPI

This is a sample project for demonstrating Web APIs. The REST service implemented in C# and responds to **GET**/**POST**/**PUT**/**DELETE** request calls on a single endpoint (_~/prescriptions_). An MySQL server with a database and table named **Prescriptions** is required. To create the table structure, please refer to the .sql file provided: ![migrations.sql](/migrations/migrations.sql)

The calls operate on the records inside the _Prescriptions_ table. REST calls to the endpoint responds with an **HTTP 200 OK** status code upon success. Different status codes are returned when an error is encountered.

On rest calls that require authentication, use the following credentials: username: **_test_**, password: _**test123**_

The input requirements and output of the REST calls as well as the response codes (on error) are listed below.

## GET ~/prescriptions/ ##
Returns a list of all prescriptions, available in the database.

Input: &lt;empty&gt;

Output: Array&lt;Prescription&gt;

### Response codes ###
**HTTP 404 Not found**: if there are no records in the database.

## GET ~/prescriptions/{id} ##
Returns data for a particular prescription (identified by **_id_**)

Input: &lt;empty&gt;

Output: Prescription

### Response codes ###
**HTTP 404 Not found**: if the record does not exist in the database.


## POST ~/prescriptions/ ##
Creates a new prescription. As an input accepts a prescription object in body. The Id field shall not be specified. Output shall be the created object

Input: Prescription
Output: Prescription

_**Additional Note:**_ requires basic authentication (refer to the credentials above)
### Response codes ###
**HTTP 400 Bad request**: if fields **ExpirationDate**, **ProductName**, **UsesLeft**, or **Description** are empty.

**HTTP 401 Unauthorized**: if username/password is incorrect or if the call is not authenticated.

## DELETE ~/prescriptions/{id} ##
Deletes a prescription with the specified identifier.

Input: &lt;empty&gt;

Output: &lt;empty&gt;

_**Additional Note:**_ requires basic authentication (refer to the credentials above)

### Response codes ###
**HTTP 401 Unauthorized**: if username/password is incorrect or if the call is not authenticated.

**HTTP 404 Not found**: if the record does not exist in the database.

## PUT ~/prescriptions/{id} ##
Update aprescription with the specified identifier. Accepts a prescription object in body. New values specified in the prescription object will be used to update the record in the database.

Input: Prescription

Output: &lt;empty&gt;

_**Additional Note:**_ requires basic authentication (refer to the credentials above)

### Response codes ###
**HTTP 400 Bad request**: if no prescription object is passed as input.

**HTTP 401 Unauthorized**: if username/password is incorrect or if the call is not authenticated.

**HTTP 404 Not found**: if the record does not exist in the database.

# Testing

Open the solution file: ![PrescriptionsApp.sln](/PrescriptionsApp.sln) provided in Visual Studio 2015/2017 then modify the connection string in the ![prescriptionsController.cs](/PrescriptionsApp/Controllers/prescriptionsController.cs) and modify the **{username}** and **{password}** placeholders in the code:

```C#
readonly string connectionString = "server=127.0.0.1;uid={username};pwd={password};database=Prescriptions";
```

Run the Debug/Release version. Visual Studio runs IISExpress on a separate browser window. Take note of the URL, e.g. http://localhost:port. Use the URL provided and make request calls to the /prescriptions end point, e.g. http://localhost:port/prescriptions

To create GET/POST/DELETE/PUT requests you can use the free version of the Postman software downloadable at: [https://www.getpostman.com/](https://www.getpostman.com/) 

# Dependencies

Uses Newtonsoft Json library to serialize objects into a JSON string.

# Deployment
