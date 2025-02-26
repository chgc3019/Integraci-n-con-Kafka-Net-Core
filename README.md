# Integración de Apache Kafka con .NET Core y Docker

## Descripción
Este proyecto implementa Apache Kafka en un entorno Dockerizado y lo integra con una aplicación en .NET Core. Se proporciona una guía paso a paso para la configuración, pruebas y ejecución de Kafka en conjunto con .NET Core.

## Tecnologías Utilizadas
- **Apache Kafka**: Plataforma de mensajería distribuida para transmisión de eventos.
- **.NET Core**: Framework para la implementación del productor y consumidor.
- **Docker**: Contenedores para Kafka y Zookeeper.
- **Docker Compose**: Orquestación de contenedores.

## Requisitos Previos
Antes de comenzar, asegúrate de tener instalados:
- **Docker** y **Docker Compose**
- **.NET SDK (versión 6 o superior)**
- **Git** (opcional, para clonar este repositorio)

## Instalación y Configuración
### 1. Clonar el Repositorio
```sh
 git clone https://github.com/chgc3019/Integraci-n-con-Kafka-Net-Core.git
 cd Integraci-n-con-Kafka-Net-Core
```

### 2. Levantar Kafka con Docker
Ejecutar el siguiente comando para iniciar los contenedores de Kafka y Zookeeper:
```sh
docker-compose up -d
```

### 3. Crear un Tópico en Kafka
Ejecutar:
```sh
docker exec -it kafka /usr/bin/kafka-topics --create --topic test-topic --bootstrap-server kafka:9093 --partitions 1 --replication-factor 1
```
Para listar los tópicos:
```sh
docker exec -it kafka /usr/bin/kafka-topics --list --bootstrap-server kafka:9093
```

## Implementación en .NET Core
### 1. Crear una Aplicación de Consola en .NET Core
```sh
dotnet new console -n KafkaProducer
cd KafkaProducer
```

### 2. Instalar Paquetes de Kafka
```sh
dotnet add package Confluent.Kafka
```

### 3. Configurar el Productor
En `Program.cs`, agregar:
```csharp
using Confluent.Kafka;
using System;
using System.Threading.Tasks;

class Program
{
    static async Task Main()
    {
        var config = new ProducerConfig { BootstrapServers = "localhost:9093" };
        using var producer = new ProducerBuilder<Null, string>(config).Build();
        
        for (int i = 0; i < 10; i++)
        {
            var result = await producer.ProduceAsync("test-topic", new Message<Null, string> { Value = $"Mensaje {i}" });
            Console.WriteLine($"Enviado: {result.TopicPartitionOffset}");
        }
    }
}
```
Ejecutar:
```sh
dotnet run
```

### 4. Crear el Consumidor
```sh
dotnet new console -n KafkaConsumer
cd KafkaConsumer
dotnet add package Confluent.Kafka
```
Agregar en `Program.cs`:
```csharp
using Confluent.Kafka;
using System;

class Program
{
    static void Main()
    {
        var config = new ConsumerConfig
        {
            BootstrapServers = "localhost:9093",
            GroupId = "consumer-group-1",
            AutoOffsetReset = AutoOffsetReset.Earliest
        };
        
        using var consumer = new ConsumerBuilder<Ignore, string>(config).Build();
        consumer.Subscribe("test-topic");
        
        while (true)
        {
            var message = consumer.Consume();
            Console.WriteLine($"Mensaje recibido: {message.Value}");
        }
    }
}
```
Ejecutar:
```sh
dotnet run
```

## Verificación
1. Ejecutar el **productor** y confirmar que los mensajes son enviados a Kafka.
2. Ejecutar el **consumidor** y verificar que los mensajes se reciben correctamente.

## Conclusión
Este proyecto demuestra cómo usar Apache Kafka con .NET Core en un entorno Dockerizado, permitiendo la transmisión de mensajes en tiempo real. Es escalable y puede adaptarse a diferentes escenarios empresariales.

## Referencias
- [Apache Kafka](https://kafka.apache.org/)
- [Confluent Kafka .NET](https://github.com/confluentinc/confluent-kafka-dotnet)
- [Docker Kafka](https://hub.docker.com/r/confluentinc/cp-kafka/)

