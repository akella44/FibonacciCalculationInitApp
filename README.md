### Description
Simple C# application, which initiates calculation of Fibonacci seq for a data set. For this, app creates RabbitMq for each series of calculations, sends to [API](https://github.com/akella44/FibonacciCalculationApi) request with data and queue name, and then waits for updates from rabbit.
### Requirements
Required user secrets file to run ([offical doc](https://learn.microsoft.com/en-us/aspnet/core/security/app-secrets?view=aspnetcore-8.0&tabs=windows#secret-manager)). It should have following structure:
```json
{
  "RabbitMQ": {
    "NodeIp": "",
    "Port": ,
    "UserName": "",
    "Password": ""
  },
  "API": {
    "FibonacciNumsEndpoint": "https://..../api/v1/FibonacciNumbers"
  }
}
```
After that you need to specify path to .json file with secrets in 'program.cs' file:
```c#
IConfiguration config = new ConfigurationBuilder().AddJsonFile("PATH HERE").Build();
```
### How it works
<video width="1300" height="800" src="https://github.com/akella44/FibonacciCalculationInitApp/assets/61851015/657c406b-bd6a-4e18-aa9b-4926f33ebff6"></video>

