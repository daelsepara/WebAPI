# WebAPI

This is a sample project for demonstrating Web APIs. The REST service implemented in C# and responds to **GET**/**POST**/**PUT**/**DELETE** request calls on a single endpoint (_~/prescriptions_). An MySQL server with a database and table named **Prescriptions** is required. To create the table structure, please refer to the .sql file provided: ![migrations.sql](/migrations/migrations.sql)

The calls operate on the records inside the _Prescriptions_ table. REST calls to the endpoint responds with an HTTP 200 status code upon success. Different status codes are returned when an error is encountered. The input, output, and return codes are listed below.

On rest calls that require authentication, use the following credentials: username: **_test_**, password: _**test123**_

## GET ~/prescriptions/ ##
Returns a list of all prescriptions, available in the database.

Input: &lt;empty&gt;

Output: Array&lt;Prescription&gt;

### Status Codes ###
**HTTP 404 Not found**: if there are no records in the database.

## GET ~/prescriptions/{id} ##
Returns data for a particular prescription (identified by **_id_**)

Input: &lt;empty&gt;

Output: Prescription

### Status Codes ###
**HTTP 404 Not found**: if the record does not exist in the database.


## POST ~/prescriptions/ ##
Creates a new prescription. As an input accepts a prescription object in body. The Id field shall not be specified. Output shall be the created object

Input: Prescription
Output: Prescription

_**Additional Note:**_ requires basic authentication (refer to the credentials above)
**HTTP 400 Bad request**: if fields **ExpirationDate**, **ProductName**, **UsesLeft**, or **Description** are empty.

**HTTP 401 Unauthorized**: if username/password is incorrect or if the call is not authenticated.

## DELETE ~/prescriptions/{id} ##
Deletes a prescription with the specified identifier.

Input: &lt;empty&gt;

Output: &lt;empty&gt;

_**Additional Note:**_ requires basic authentication (refer to the credentials above)

### Status Codes ###
**HTTP 401 Unauthorized**: if username/password is incorrect or if the call is not authenticated.

**HTTP 404 Not found**: if the record does not exist in the database.

## PUT ~/prescriptions/{id} ##
Update aprescription with the specified identifier. Accepts a prescription object in body. New values specified in the prescription object will be used to update the record in the database.

Input: Prescription

Output: &lt;empty&gt;

_**Additional Note:**_ requires basic authentication (refer to the credentials above)

### Status Codes ### 
**HTTP 400 Bad request**: if no prescription object is passed as input.

**HTTP 401 Unauthorized**: if username/password is incorrect or if the call is not authenticated.

**HTTP 404 Not found**: if the record does not exist in the database.
