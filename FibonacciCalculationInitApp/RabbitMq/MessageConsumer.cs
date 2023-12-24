using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System.Text;

namespace FibonacciCalculationInitApp.RabbitMq
{
    public class MessageConsumer : IDisposable
    {
        public event EventHandler<string> MessageReceived;
        public string QueueName;
        private IModel _channel;
        public MessageConsumer(IModel channel, string queueName)
        {
            //Start consuming
            QueueName = queueName;
            _channel = channel;

            var consumer = new EventingBasicConsumer(channel);

            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);

                onMessageReceived(message);
            };

            channel.BasicConsume(queue: queueName, autoAck: true, consumer: consumer);
        }

        private void onMessageReceived(string message)
        {
            MessageReceived?.Invoke(this, message);
        }

        public void Dispose()
        {
            _channel.Dispose();
        }
    }
}
