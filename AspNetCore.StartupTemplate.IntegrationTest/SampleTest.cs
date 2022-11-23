using AspNetCore.StartUpTemplate.Contract;
using AspNetCore.StartupTemplate.IntegrationTest.Core;
using Flurl.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit.Abstractions;

namespace AspNetCore.StartupTemplate.IntegrationTest;

public class SampleTest:IClassFixture<CustomWebApplicationFactory<Program>>
{
    private readonly ITestOutputHelper _output;
    private readonly WebApplicationFactory<Program> _application;
    private readonly HttpClient _httpClient;
    private readonly FlurlClient _flurlClient;

    public SampleTest(ITestOutputHelper tempOutput,CustomWebApplicationFactory<Program> factory)
    {
        _output = tempOutput;
        _application = factory;
        _httpClient = _application.CreateClient();
        _flurlClient = new FlurlClient(_httpClient);
        // _flurlClient.WithHeader("Authorization", "Bearer " + "add token here");

    }
    [Fact]
    public async Task User_Get_OK()
    {
        var response=await "User/Get?id=1".WithClient(
            _flurlClient).GetStringAsync();
        // var httpResponseMessage=await _client.GetAsync("User/Get?id=1");
        Assert.Equal(response,"1");
    }
}