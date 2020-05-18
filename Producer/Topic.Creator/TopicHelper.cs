using Confluent.Kafka;
using Confluent.Kafka.Admin;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Producer.Topic.Creator
{
    public static class TopicHelper
    {

        public static async Task CreateTopic(ClientConfig config, string name, int numPartitions, short replicationFactor)
        {
            using (var adminClient = new AdminClientBuilder(config).Build())
            {
                try
                {
                    await adminClient.CreateTopicsAsync(new List<TopicSpecification> {
                        new TopicSpecification { Name = name, NumPartitions = numPartitions, ReplicationFactor = replicationFactor } }, new CreateTopicsOptions {});
                }
                catch (CreateTopicsException e)
                {
                    if (e.Results[0].Error.Code != ErrorCode.TopicAlreadyExists)
                    {
                        Console.WriteLine($"An error occured creating topic {name}: {e.Results[0].Error.Reason}");
                    }
                    else
                    {
                        Console.WriteLine("Topic already exists");
                    }
                }
            }
        }
    }
}
