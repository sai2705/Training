using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace PostFunction
{
    public class ReceiveShipments
    {

        [FunctionName("ReceiveShipments")]
        public  async Task Run([HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequestMessage req,
            ILogger log)
        {
            
            try
            {
                //Reading Content From Request Body
                string jsonContent = await req.Content.ReadAsStringAsync();

                log.LogInformation("Incoming json : "+jsonContent);
                Shipment shipments = JsonConvert.DeserializeObject<Shipment>(jsonContent);
                
                //Getting the SessionId
                string sessionId = Guid.NewGuid().ToString();
                log.LogInformation("Sessionid:" + sessionId);

                // Mapping to integration message
                IntegrationMessage integrationMessage = new IntegrationMessage();
                integrationMessage.SessionId = sessionId;
               

                integrationMessage.FunctionName =GetType().Name;
                log.LogInformation("FunctionName:" + integrationMessage.FunctionName);
                Content content = new Content();
                content.BatchId = shipments.BatchId;
                content.Company = shipments.Company;
                content.ReceivedDate = shipments.ReceivedDate;
              
                List<MessageItem> messageItems = new List<MessageItem>();
                foreach (var item in shipments.Items)
                {
                    MessageItem messageItem = new MessageItem();
                    messageItem.Address = item.Address;
                    messageItem.CountryCode = item.CountryCode;
                    messageItem.Description = item.Description;
                    messageItem.ItemNumber = item.ItemNumber;
                    messageItem.ItemType = item.ItemType;
                    messageItem.Quantity = item.Quantity;
                    messageItem.State = item.State;
                    messageItem.UnitOfMeasure = item.UnitOfMeasure;
                    messageItems.Add(messageItem);
                }
                integrationMessage.Content = content;
                content.MessageItems = messageItems;
                
                //Calling function to Insert data into blob 
                await Sendmessage(integrationMessage, sessionId, log);
                log.LogInformation("IntegrationMessage :" + JsonConvert.SerializeObject(integrationMessage));
               
            }
            catch (Exception ex)
            {

                log.LogError("Excepion: "+ex.Message);
                throw;
            }


        }
        public async static Task Sendmessage(IntegrationMessage message, String sessionId, ILogger log)
        {
            //Declaring variables
            string connectionString;
            CloudStorageAccount storageAccount;
            CloudBlobClient client;
            CloudBlobContainer container;
            CloudBlockBlob blob;

            //Assigning values to variables
            string connection = Connections.cstring;
            string containername = Connections.blobcontainer;
            string type = Connections.contenttype;
            // Connecting to blob storage
            connectionString = connection;
            storageAccount = CloudStorageAccount.Parse(connectionString);

            client = storageAccount.CreateCloudBlobClient();

            //Assigning the Container Name
            container = client.GetContainerReference(containername);
            //Creating the Container IfNotExist
            await container.CreateIfNotExistsAsync();

            string fileName = sessionId + ".json";
            log.LogInformation("File name: "+fileName);
            blob = container.GetBlockBlobReference(fileName);

            //Assignning the ContentType
            blob.Properties.ContentType = type;
          
            //Wrapper class to transfer bytes 
            using (Stream stream = new MemoryStream(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message))))
            {
                await blob.UploadFromStreamAsync(stream);
            }

        }


    }



}
