using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using PrescriptionsApp.Filters;
using PrescriptionsApp.Models;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;

namespace PrescriptionsApp.Controllers
{
    public class prescriptionsController : ApiController
    {
        MySqlConnection conn;
        readonly string connectionString = "server=127.0.0.1;uid={username};pwd={password};database=Prescriptions";

        private HttpResponseMessage GenerateResponse(object obj, HttpStatusCode code)
        {
            var content = JsonConvert.SerializeObject(obj);

            var response = new HttpResponseMessage(code);
            response.Content = new StringContent(content);
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            return response;
        }

        private void OpenConnection()
        {
            try
            {
                conn = new MySqlConnection(connectionString);
                conn.Open();
            }
            finally
            {

            }
        }

        private MySqlDataReader QueryDatabase(string query)
        {
            MySqlDataReader dataReader = null;

            OpenConnection();

            var cmd = new MySqlCommand(query, conn);

            dataReader = cmd.ExecuteReader();

            return dataReader;
        }

        private int ExecuteQuery(MySqlCommand cmd, bool IsInsert = false)
        {
            int affectedRows = 0;

            try
            {
                affectedRows = cmd.ExecuteNonQuery();

                if (IsInsert)
                    affectedRows = Convert.ToInt32(cmd.LastInsertedId);
            }
            finally
            {

            }

            return affectedRows;
        }

        private int ExecuteQuery(string query, bool IsInsert = false)
        {
            int affectedRows = 0;

            try
            {
                OpenConnection();

                var cmd = new MySqlCommand(query, conn);

                affectedRows = cmd.ExecuteNonQuery();

                if (IsInsert)
                    affectedRows = Convert.ToInt32(cmd.LastInsertedId);
            }
            finally
            {

            }

            return affectedRows;
        }

        // GET: prescriptionrecords
        public HttpResponseMessage Get()
        {
            var prescriptions = new List<Prescription>();

            var dataReader = QueryDatabase("SELECT * FROM Prescriptions");

            if (dataReader.HasRows)
            {
                while (dataReader.Read())
                {
                    prescriptions.Add(new Prescription(
                        Convert.ToInt32(dataReader["Id"]),
                        Convert.ToDateTime(dataReader["ExpirationDate"]),
                        Convert.ToString(dataReader["ProductName"]),
                        Convert.ToInt32(dataReader["UsesLeft"]),
                        Convert.ToString(dataReader["Description"]),
                        Convert.ToBoolean(dataReader["IsActive"]),
                        Convert.ToString(dataReader["PatientId"])
                    ));
                }
            }

            dataReader.Close();
            conn.Close();

            return prescriptions.Count > 0 ? GenerateResponse(prescriptions, HttpStatusCode.OK) : new HttpResponseMessage(HttpStatusCode.NotFound);
        }

        // GET: prescriptions/{id}
        // Get prescription record
        public HttpResponseMessage Get(int id)
        {
            Prescription prescription = null;

            if (id > 0)
            {
                var dataReader = QueryDatabase(string.Format("SELECT * FROM Prescriptions WHERE Id = {0}", id));

                dataReader.Read();

                if (dataReader.HasRows)
                {
                    prescription = new Prescription(
                            Convert.ToInt32(dataReader["Id"]),
                            Convert.ToDateTime(dataReader["ExpirationDate"]),
                            Convert.ToString(dataReader["ProductName"]),
                            Convert.ToInt32(dataReader["UsesLeft"]),
                            Convert.ToString(dataReader["Description"]),
                            Convert.ToBoolean(dataReader["IsActive"]),
                            Convert.ToString(dataReader["PatientId"])
                    );
                }

                dataReader.Close();
                conn.Close();
            }

            return prescription != null ? GenerateResponse(prescription, HttpStatusCode.OK) : new HttpResponseMessage(HttpStatusCode.NotFound);
        }

        // POST: prescriptions
        // Create prescription record
        [BasicAuthentication]
        public HttpResponseMessage Post([FromBody]Prescription value)
        {
            if (value != null)
            {
                if (value.ExpirationDate != null && value.ProductName != null && value.UsesLeft > 0 && value.Description != null)
                {
                    OpenConnection();

                    var cmd = new MySqlCommand("INSERT INTO Prescriptions(`ExpirationDate`, `ProductName`, `UsesLeft`, `Description`, `PatientId`) VALUES(@expDate, @productName, @usesLeft, @description, @patient)", conn);

                    cmd.Parameters.AddWithValue("@expDate", ((DateTime)value.ExpirationDate).ToString("yyyy-MM-dd HH:mm:ss"));
                    cmd.Parameters.AddWithValue("@productName", value.ProductName);
                    cmd.Parameters.AddWithValue("@usesLeft", value.UsesLeft);
                    cmd.Parameters.AddWithValue("@description", value.Description);
                    cmd.Parameters.AddWithValue("@patient", string.IsNullOrEmpty(value.PatientId) ? "" : value.PatientId);
                    cmd.Prepare();

                    var insertId = ExecuteQuery(cmd, true);

                    conn.Close();

                    if (insertId > 0)
                    {
                        value.Id = insertId;

                        var response = GenerateResponse(value, HttpStatusCode.OK);

                        return response;
                    }
                }
            }

            return new HttpResponseMessage(HttpStatusCode.BadRequest);
        }

        private void AddParameter(ref int parameters, ref string query, string value)
        {
            if (parameters > 0)
                query += ", ";

            query += value;

            parameters++;
        }

        // PUT: prescriptions/{id}
        // Update prescription record
        [BasicAuthentication]
        public HttpResponseMessage Put(int id, [FromBody]Prescription value)
        {
            if (id > 0 && value != null)
            {
                var query = "UPDATE Prescriptions SET ";

                int parameters = 0;

                if (value.ExpirationDate != null)
                {
                    AddParameter(ref parameters, ref query, string.Format("ExpirationDate = '{0}'", ((DateTime)value.ExpirationDate).ToString("yyyy-MM-dd HH:mm:ss")));
                }

                if (!string.IsNullOrEmpty(value.ProductName))
                {
                    AddParameter(ref parameters, ref query, string.Format("ProductName = '{0}'", value.ProductName));
                }

                if (value.UsesLeft != null)
                {
                    AddParameter(ref parameters, ref query, string.Format("UsesLeft = {0}", value.UsesLeft));
                }

                if (!string.IsNullOrEmpty(value.Description))
                {
                    AddParameter(ref parameters, ref query, string.Format("Description = '{0}'", value.Description));
                }

                if (value.IsActive != null)
                {
                    AddParameter(ref parameters, ref query, string.Format("IsActive = {0}", (bool)value.IsActive ? 1 : 0));
                }
                
                if (!string.IsNullOrEmpty(value.PatientId))
                {
                    AddParameter(ref parameters, ref query, string.Format("PatientId = '{0}'", value.PatientId));
                }

                if (parameters > 0)
                {
                    query += string.Format(" WHERE ID = {0}", id);

                    var affectedRows = ExecuteQuery(query);

                    Console.WriteLine(query);
                    Console.WriteLine(JsonConvert.SerializeObject(value));

                    conn.Close();

                    if (affectedRows > 0)
                    {
                        return new HttpResponseMessage(HttpStatusCode.OK);
                    }
                    else
                    {
                        return new HttpResponseMessage(HttpStatusCode.NotFound);
                    }
                }
            }

            return new HttpResponseMessage(HttpStatusCode.BadRequest);
        }

        // DELETE: prescriptions/{id}
        // Delete prescription record
        [BasicAuthentication]
        public HttpResponseMessage Delete(int id)
        {
            if (id > 0)
            {
                var query = string.Format("DELETE FROM Prescriptions WHERE Id = {0}", id);

                var affectedRows = ExecuteQuery(query);

                conn.Close();

                if (affectedRows > 0)
                {
                    return new HttpResponseMessage(HttpStatusCode.OK);
                }
            }

            return new HttpResponseMessage(HttpStatusCode.NotFound);
        }
    }
}
