using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Data.SqlClient;
using System.Data;
using System.Net.Http;
using System.Net;
using System.Configuration;

namespace DeleteOrderDetails
{
    public static class Function1
    {
        [FunctionName("Function1")]
        public static async Task<HttpResponseMessage> Run(
            [HttpTrigger(AuthorizationLevel.Function, "delete", "post", Route = null)] HttpRequestMessage req,
            ILogger log)
        {
            string content = await req.Content.ReadAsStringAsync();
            var id = JsonConvert.SerializeObject(content);
            log.LogInformation("C# HTTP trigger function processed a request.");

            SqlConnection conn = new SqlConnection("Data Source=SMANKALA02;Initial Catalog=EventGrid;User ID = harsha;Password = Msai2705*;Connection Timeout=30;Integrated Security=True");
            //var conn = ConfigurationManager.ConnectionStrings["MyConnectionString"].ConnectionString;

            try
            {
                SqlCommand command = new SqlCommand();
                command.CommandText = "exec SP_Delete_Order";
                command.CommandType = CommandType.StoredProcedure;
                SqlParameter parameter = command.CreateParameter();
                command.Parameters.Add("@ID", SqlDbType.VarChar).Value = id;
                command.Connection = conn;
                conn.Open();

                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {

                log.LogError("Cannot open connection");
            }
            return null;

        }
    }
}
