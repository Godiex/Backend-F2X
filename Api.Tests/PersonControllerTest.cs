using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Application.Person.Queries;
using Xunit;

namespace Api.Tests;


public class PersonControllerTest
{

    IntegrationTestBuilder _builder;
    HttpClient _client = default!;

    public PersonControllerTest()
    {
         _builder = new IntegrationTestBuilder();
        _client = _builder.CreateClient();
    }
    [Fact]
    public async Task GetPersonSuccess()
    {       
        var PersonId = _builder.Id;       
        var c = await _client.GetAsync($"api/Person/{PersonId}");
        c.EnsureSuccessStatusCode();
        var response = await c.Content.ReadAsStringAsync();
        var personData = System.Text.Json.JsonSerializer.Deserialize<PersonDto>(response, new System.Text.Json.JsonSerializerOptions
        {
            PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase
        });
        Assert.True(personData != null);
    }

    [Fact]
    public async Task PostPersonSuccess()
    {
        var postContent = new Application.Person.Commands.PersonCreateCommand
        (          
            "john", "doe", "john@doe.com", System.DateTime.Now.AddYears(-20)
        );
        var content = new StringContent(System.Text.Json.JsonSerializer.Serialize(postContent), System.Text.Encoding.UTF8 , "text/json" );
        var c = await _client.PostAsync("api/Person", content);
        c.EnsureSuccessStatusCode();
        Assert.Equal(HttpStatusCode.OK, c.StatusCode);
    }
    
    [Fact]
    public async Task GetPersonFailure()
    {
        var c = await _client.GetAsync($"api/Person/{Guid.NewGuid()}");
        c.EnsureSuccessStatusCode();
        var response = await c.Content.ReadAsStringAsync();
        Assert.Throws<System.Text.Json.JsonException>(() => 
            System.Text.Json.JsonSerializer.Deserialize<PersonDto>(response));
    }

    [Fact]
    public async Task GetPersonBadRequestFailure()
    {
        var c = await _client.GetAsync($"api/Person/foobar");
        Assert.Throws<System.Net.Http.HttpRequestException>(() => c.EnsureSuccessStatusCode());
    }

}