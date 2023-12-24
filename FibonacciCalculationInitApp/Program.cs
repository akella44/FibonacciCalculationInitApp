using Microsoft.Extensions.Configuration;
using FibonacciCalculationInitApp.RabbitMq;
using FibonacciCalculationInitApp.DTO;
using FibonacciCalculationInitApp.HttpWrapper;
class RabbitClient
{
    static async Task Main(string[] args)
    {
        IConfiguration config = new ConfigurationBuilder().AddJsonFile("").Build();

        RequestSender requestSender = new RequestSender();

        try
        {
            RabbitMqConnectionManager.Instance.MakeConnection(new RabbitMqConfig()
            {
                HostName = config["RabbitMQ:NodeIp"],
                Port = int.Parse(config["RabbitMQ:Port"]),
                UserName = config["RabbitMQ:UserName"],
                Password = config["RabbitMQ:Password"]
            });
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error during connection to RabbitMq:", ex.ToString());
        }

        Console.WriteLine("Enter the number of calculations to start:");

        try
        {
            int.TryParse(Console.ReadLine(), out int numberOfCalculations);

            Random random = new Random();

            for (int i = 0; i < numberOfCalculations; i++)
            {
                MessageConsumer messageConsumer = RabbitMqConnectionManager.Instance.GetMessageConsumer();
                messageConsumer.MessageReceived += ((o, e) =>
                {
                    Console.WriteLine($"Received message {e} from queue {messageConsumer.QueueName}");
                });

                var data = new
                {
                    argsList = Enumerable.Range(0, random.Next(1, 20))
                    .Select(i => random.Next(0, 80)),
                    queueName = messageConsumer.QueueName
                };

                Console.WriteLine($"Starting calculating with '{messageConsumer.QueueName}' queue name and data '{string.Join(", ", data.argsList)}'");

                try
                {
                    await requestSender.SendPostRequestToFibApi(data, config["API:FibonacciNumsEndpoint"]);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error during http request to API");
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Invalid input");
        }

        Console.WriteLine("To exit press any key");
        Console.ReadKey();
    }
}