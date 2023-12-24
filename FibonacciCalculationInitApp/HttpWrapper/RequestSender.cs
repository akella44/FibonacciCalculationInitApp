using System.Net.Http.Json;

namespace FibonacciCalculationInitApp.HttpWrapper
{
    public class RequestSender
    {
        private readonly HttpClient _httpClient;
        public RequestSender()
        {
            _httpClient = new HttpClient();
        }

        public async Task SendPostRequestToFibApi(object data, string endpoint)
        {
            JsonContent jsonContent = JsonContent.Create(data);
            HttpResponseMessage responseMessage = await _httpClient.PostAsync(endpoint, jsonContent);
            if (responseMessage.IsSuccessStatusCode)
            {
                return;
            }
            else
            {
                throw new HttpRequestException();
            }
        }
    }
}
