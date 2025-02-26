using System;
using System.Threading;
using Confluent.Kafka;

class Program
{
    static void Main(string[] args)
    {
        string bootstrapServers = "localhost:9092"; // Dirección del broker Kafka
        string topic = "test-topic"; // Topic a consumir
        string groupId = "test-group"; // Grupo de consumidores

        var config = new ConsumerConfig
        {
            BootstrapServers = bootstrapServers,
            GroupId = groupId,
            AutoOffsetReset = AutoOffsetReset.Earliest // Leer desde el inicio si no hay offset guardado
        };

        using var consumer = new ConsumerBuilder<string, string>(config).Build();
        consumer.Subscribe(topic);

        Console.WriteLine($"Escuchando mensajes de {topic}...");

        try
        {
            while (true)
            {
                try
                {
                    var consumeResult = consumer.Consume(CancellationToken.None);
                    Console.WriteLine($"Mensaje recibido: {consumeResult.Message.Value}");
                }
                catch (ConsumeException e)
                {
                    Console.WriteLine($"Error al consumir: {e.Error.Reason}");
                }
            }
        }
        catch (OperationCanceledException)
        {
            Console.WriteLine("Consumo cancelado.");
        }
        finally
        {
            consumer.Close();
        }
    }
}
