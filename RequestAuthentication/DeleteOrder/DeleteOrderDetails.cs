// Default URL for triggering event grid function in the local environment.
// http://localhost:7071/runtime/webhooks/EventGrid?functionName={functionname}
using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Azure.EventGrid.Models;
using Microsoft.Azure.WebJobs.Extensions.EventGrid;
using Microsoft.Extensions.Logging;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using Newtonsoft.Json;

namespace DeleteOrder
{
    public static class DeleteOrderDetails
    {
        [FunctionName("DeleteOrderDetails")]
        public static void Run([EventGridTrigger]EventGridEvent eventGridEvent, ILogger log)
        {
            log.LogInformation(eventGridEvent.Data.ToString());
            var id = eventGridEvent.Data.ToString();
            var identity = JsonConvert.SerializeObject(id);

        using  (  SqlConnection connection = new SqlConnection("Data Source = SMANKALA02; Initial Catalog = EventGrid; User ID = harsha; Password = Msai2705 *; Connection Timeout = 30; Integrated Security = True;"))
                {
                //var conn = ConfigurationManager.ConnectionStrings["MyConnectionString"].ConnectionString;

                try
                {
                    
                    SqlCommand command = new SqlCommand();
                    command.Connection = connection;
                    command.CommandText = "SP_Delete_Details";
                    command.CommandType = CommandType.StoredProcedure;
                    //SqlParameter parameter = command.CreateParameter();
                    //command.Parameters.Add(new SqlParameter("@ID",(eventGridEvent.Data.ToString()))).Value = id.Trim();
                    command.Parameters.Add("@ID", SqlDbType.VarChar).Value = identity;
                    connection.Open();

                    command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    throw ex;
                    log.LogError("Cannot open connection");
                }
            }
        }
    }
}
