using Confluent.Kafka;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading;

namespace Consumer
{
    class Program
    {
        static void Main(string[] args)
        {
            List<string> topics = new List<string>();
            topics.Add("test");
            topics.Add("test2");
            var cts = new CancellationTokenSource();

            var config = new ConsumerConfig
            {
                BootstrapServers = "localhost:9092",
                GroupId = $"kiran",
                AutoOffsetReset = AutoOffsetReset.Earliest,
                EnableAutoCommit = false,
                Acks = Acks.Leader,
            };

            using (var consumer = new ConsumerBuilder<string, string>(config)
                .SetOffsetsCommittedHandler((c, p) =>
                {
                    Console.WriteLine($"Offsets Commited Handler: {c.ConsumerGroupMetadata}");

                })
                .SetPartitionsAssignedHandler((c, p) =>
                {
                    var partitionCommittedOffsets = c.Committed(p, TimeSpan.FromSeconds(2));

                    partitionCommittedOffsets.ForEach(x =>
                    {
                        Console.WriteLine($"Committed Offsets for: {x.Topic}-{x.Partition}-{x.Offset}");
                    });
                })
                .Build())
            {
                consumer.Subscribe(topics);

                try
                {
                    while (true)
                    {
                        var consumeResult = consumer.Consume(cts.Token);

                        var blob = JsonConvert.SerializeObject(consumeResult);

                        Console.WriteLine($"Message from kafka: {consumeResult.Message.Value} from data stream: {consumeResult.Topic}-{consumeResult.TopicPartition}-{consumeResult.TopicPartitionOffset}-{consumeResult.Offset}");

                        //consumer.Commit(consumeResult);
                    }
                }
                catch
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
