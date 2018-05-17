namespace ESPSimModule
{
    using System;
    using System.IO;
    using System.Runtime.InteropServices;
    using System.Runtime.Loader;
    using System.Security.Cryptography.X509Certificates;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Azure.Devices.Client;
    using Microsoft.Azure.Devices.Client.Transport.Mqtt;
    using MQTTnet;
    using MQTTnet.Client;

    class Program
    {

        static async Task Main(string[] args)
        {

            // Create a new MQTT client.
            var factory = new MqttFactory();
            var mqttClient = factory.CreateMqttClient();
           // Use TCP connection
           //provide address of mqttServerModule
            var options = new MqttClientOptionsBuilder()
                .WithTcpServer("192.168.59.24", 1889) // Port is optional
                .Build();
            

            Random r = new Random();
            try{
                await mqttClient.ConnectAsync(options);

                //Send message with temperature info every two seconds
                while(true)
                {
                     var message = new MqttApplicationMessageBuilder()
                    .WithTopic("MyTopic")
                    .WithPayload("{\"temp\":"+r.Next(35,42).ToString()+", \"timecreated\":\""+DateTime.Now.ToString()+"\"}")
                    .WithExactlyOnceQoS()
                    .WithRetainFlag()
                    .Build();
                    await mqttClient.PublishAsync(message);
                    await Task.Delay(2000);
                
                }
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            };

        }
    }
}
