using System;
using System.Threading.Tasks;
using Confluent.Kafka;

class Program
{
    static async Task Main(string[] args)
    {
        string bootstrapServers = "localhost:9092"; // Dirección del broker Kafka
        string topic = "test-topic"; // Nombre del topic

        var config = new ProducerConfig
        {
            BootstrapServers = bootstrapServers
        };

        using var producer = new ProducerBuilder<string, string>(config).Build();

        Console.WriteLine("Escribe un mensaje para enviarlo a Kafka (o 'salir' para terminar):");

        while (true)
        {
            Console.Write("> ");
            string? message = Console.ReadLine();

            if (message?.ToLower() == "salir")
                break;

            try
            {
                var result = await producer.ProduceAsync(topic, new Message<string, string>
                {
                    Key = Guid.NewGuid().ToString(), // Clave única
                    Value = message
                });

                Console.WriteLine($"Mensaje enviado a {result.TopicPartitionOffset}");
            }
            catch (ProduceException<string, string> e)
            {
                Console.WriteLine($"Error enviando mensaje: {e.Error.Reason}");
            }
        }
    }
}
