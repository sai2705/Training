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
using Newtonsoft.Json.Linq;

namespace DeleteOrder
{
    public static class DeleteOrderDetails
    {
        [FunctionName("DeleteOrderDetails")]
        public static void Run([EventGridTrigger]EventGridEvent eventGridEvent, ILogger log)
        {
            //Logging the Event Grid Data
            log.LogInformation(eventGridEvent.Data.ToString());
            var id = eventGridEvent.Data.ToString();
            
            var parseid = JObject.Parse(id);
            var parsevalue = parseid["id"].Value<string>();
            var deserializevalue = JsonConvert.DeserializeObject(parsevalue);
            

        
            //Sql Connection String 
            using  (  SqlConnection connection = new SqlConnection("Data Source = SMANKALA02; Initial Catalog = EventGrid; User ID = harsha; Password = Msai2705*; Connection Timeout = 30;"))
              
            {
                

                try
                {
                    
                    SqlCommand command = new SqlCommand();
                    command.Connection = connection;

                    //Reading the stored procedure name
                    command.CommandText = "SP_Delete_Details";
                    
                    //Declaring the command type as stored procedure

                    command.CommandType = CommandType.StoredProcedure;
                    //Passing the parameter value 
                    SqlParameter parameter = command.CreateParameter();
                    command.Parameters.Add("@ID", SqlDbType.VarChar).Value = deserializevalue;
                  
                  
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
