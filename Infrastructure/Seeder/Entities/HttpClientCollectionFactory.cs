using System.Net;
using System.Net.Http.Json;
using Microsoft.Extensions.Configuration;
using System.Text.Json;

namespace Infrastructure.Seeder.Entities;

public class HttpClientCollectionFactory
{
    static readonly HttpClient client = new HttpClient();
    private string Uri;
    private string UserName;
    private string Password;
    

    public HttpClientCollectionFactory(IConfiguration config)
    {
        Uri = config.GetValue<string>("AuthApi:Host");
        UserName = config.GetValue<string>("AuthApi:UserName");
        Password = config.GetValue<string>("AuthApi:Password");
    }

    public async Task<string> LoginInApi()
    {
        try	
        {
            HttpResponseMessage response = await client.PostAsJsonAsync(Uri + "/Login", new {UserName, Password});
            response.EnsureSuccessStatusCode();
            var responseBody = JsonSerializer.Deserialize<Dictionary<string, string>>(response.Content.ReadAsStringAsync().Result);
            return responseBody?["token"] ?? throw new InvalidOperationException();
        }
        catch(HttpRequestException e)
        {
            throw new InvalidOperationException(e.Message);
        }
    }
    
    public async Task<List<CollectionAmount>?> GetAmountVehicle(string token, DateTime date)
    {
        try	
        {
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
            var h = Uri + $"/ConteoVehiculos/{date.Year}-{date.Month}-{date.Day}";
            var response = await client.GetAsync(Uri + $"/ConteoVehiculos/{date.Year}-{date.Month}-{date.Day}");
            if (response.IsSuccessStatusCode && response.StatusCode == HttpStatusCode.OK)
            {
                var responseString = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<List<CollectionAmount>>(responseString, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true});
            }
            return new List<CollectionAmount>();
        }
        catch(HttpRequestException e)
        {
            throw new InvalidOperationException(e.Message);
        }
    }
    
    public async Task<List<CollectionVehicle>?> GetCollectionVehicle(string token, DateTime date)
    {
        try	
        {
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
            var response = await client.GetAsync(Uri + $"/RecaudoVehiculos/{date.Year}-{date.Month}-{date.Day}");
            if (response.IsSuccessStatusCode && response.StatusCode == HttpStatusCode.OK)
            { 
                return JsonSerializer.Deserialize<List<CollectionVehicle>>(await response.Content.ReadAsStreamAsync() ,
                    new JsonSerializerOptions() { PropertyNameCaseInsensitive = true});
            }
            return new List<CollectionVehicle>();
        }
        catch(HttpRequestException e)
        {
            throw new InvalidOperationException(e.Message);
        }
    }
    
}

public class CollectionBase
{
    public string Estacion { get; set; }
    public string Sentido { get; set; }
    public int Hora { get; set; }
    public string Categoria { get; set; }
}

public class CollectionVehicle : CollectionBase
{
    public double ValorTabulado { get; set; }
}

public class CollectionAmount : CollectionBase
{
    public int Cantidad { get; set; }
}
