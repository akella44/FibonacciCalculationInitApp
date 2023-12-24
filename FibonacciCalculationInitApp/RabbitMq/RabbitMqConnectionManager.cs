using FibonacciCalculationInitApp.DTO;
using RabbitMQ.Client;

namespace FibonacciCalculationInitApp.RabbitMq
{
    public class RabbitMqConnectionManager : IDisposable
    {
        private static RabbitMqConnectionManager _instance = new RabbitMqConnectionManager();
        private RabbitMqConnectionManager() { }

        private IConnection _connection;
        public static RabbitMqConnectionManager Instance
        {
            get
            {
                return _instance;
            }
        }

        public void MakeConnection(RabbitMqConfig rabbitMqConfig)
        {
            //Create a connection to RabbitMq
            var connectionFactory = new ConnectionFactory()
            {
                HostName = rabbitMqConfig.HostName,
                Port = rabbitMqConfig.Port,
                UserName = rabbitMqConfig.UserName,
                Password = rabbitMqConfig.Password
            };

            _connection = connectionFactory.CreateConnection();
        }

        public MessageConsumer GetMessageConsumer()
        {
            //Creating new queue for consumer
            string queueName = Guid.NewGuid().ToString();
            IModel channel = _connection.CreateModel();

            channel.QueueDeclare(
                queue: queueName,
                durable: false,
                exclusive: false,
                autoDelete: true,
                arguments: null);

            return new MessageConsumer(channel, queueName);
        }

        public void Dispose()
        {
            _connection.Dispose();
        }
    }
}
