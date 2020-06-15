using Confluent.Kafka;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SignalR.Kafka.Broker.Consumer
{
    public class KafkaConsumer : IConsumer
    {
        public void ConsumerMessage()
        {
            var topic = "Random";
            var cts = new CancellationTokenSource();

            var config = new ConsumerConfig
            {
                BootstrapServers = "localhost:9092",
                GroupId = "test0123",
                AutoOffsetReset = AutoOffsetReset.Earliest,
                EnableAutoCommit = false
            };

            using (var consumer = new ConsumerBuilder<string, string>(config).Build())
            {
                Console.WriteLine($"Consumer start with groupId: {config.GroupId} and topic: {topic}");
                consumer.Subscribe(topic);

                try
                {
                    while (true)
                    {
                        var consumeResult = consumer.Consume(cts.Token);

                        var blob = JsonConvert.SerializeObject(consumeResult);

                        Console.WriteLine($"Message from kafka: {consumeResult.Message.Value}");

                        //consumer.Commit(consumeResult);
                    }
                }
                catch(Exception ex)
                {
                    //catch bug :P
                }
                finally
                {
                    consumer.Close();
                }
            }
        }
    }
}
