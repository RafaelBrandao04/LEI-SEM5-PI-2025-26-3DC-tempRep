using System.Text;
using System.Text.Json;
using Application.DTO;
using DataModel.Repository;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using WebApi.IntegrationTests.Helpers;
using Xunit;

namespace WebApi.IntegrationTests;


// based on https://learn.microsoft.com/en-us/aspnet/core/test/integration-tests?view=aspnetcore-8.0

public class SomeIntegrationTests 
    : IClassFixture<IntegrationTestsWebApplicationFactory<Program>>
{
    private readonly IntegrationTestsWebApplicationFactory<Program> _factory;

    private readonly HttpClient _client;

    public SomeIntegrationTests(IntegrationTestsWebApplicationFactory<Program> factory)
    {
        _factory = factory;

        _client = _factory.CreateClient(new WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = false
        });
    }

    [Theory]
    [InlineData("/WeatherForecast")]
    [InlineData("/api/colaborator")]
    public async Task Get_EndpointsReturnSuccessAndCorrectContentType(string url)
    {
        // Arrange

        // Act
        var response = await _client.GetAsync(url);

        // Assert
        response.EnsureSuccessStatusCode(); // Status Code 200-299
        Assert.Equal("application/json; charset=utf-8", 
            response.Content.Headers.ContentType.ToString());
    }

    [Fact]
    public async Task Get_ReturnData()
    {
        // Arrange
        using (var scope = _factory.Services.CreateScope())
        {
            var scopedServices = scope.ServiceProvider;
            var db = scopedServices.GetRequiredService<AbsanteeContext>();

            Utilities.ReinitializeDbForTests(db);
        }

        // Act
        var response = await _client.GetAsync("/api/colaborator");

        // Assert
        var responseBody = await response.Content.ReadAsStringAsync();
        Assert.NotNull(responseBody);


        var jsonDocument = JsonDocument.Parse(responseBody);
        var jsonArray = jsonDocument.RootElement;

        Assert.True(jsonArray.ValueKind == JsonValueKind.Array, "Response body is not a JSON array");
        Assert.Equal(3, jsonArray.GetArrayLength());
    }

    [Fact]
    public async Task Post_AddsColaborator()
    {
        // Arrange
        var collaborator = new ColaboratorDTO
        {
            Name = "John Doe",
            Email = "john.doe@example.com",
            Street = "adlkfjasdlkfs",
            PostalCode = "4000-000"
        };

        var jsonContent = JsonConvert.SerializeObject(collaborator);
        var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

        // Act
        HttpResponseMessage response = await _client.PostAsync("/api/colaborator", content);

        // Assert
        var responseBody = await response.Content.ReadAsStringAsync();
        Assert.NotNull(responseBody);


        var jsonDocument = JsonDocument.Parse(responseBody);
    }
}