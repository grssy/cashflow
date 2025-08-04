using CashFlow.Communication.Requests;
using CashFlow.Exception;
using CommonTestUtilities.Requests;
using Microsoft.AspNetCore.Server.Kestrel.Core.Features;
using Shouldly;
using System.Globalization;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using WebApi.Test.InlineData;

namespace WebApi.Test.Login.DoLogin;
public class DoLoginTest : IClassFixture<CustomWebApplicationFactory>
{
    private const string METHOD = "api/Login";

    private readonly HttpClient _httpClient;
    private readonly string _email;
    private readonly string _name;
    private readonly string _password;

    public DoLoginTest(CustomWebApplicationFactory webApplicationFactory)
    {
        _httpClient = webApplicationFactory.CreateClient();
        _email = webApplicationFactory.GetEmail();
        _name = webApplicationFactory.GetName();
        _password = webApplicationFactory.GetPassword();
    }

    [Fact]
    public async Task Success()
    {
        var request = new RequestLoginJson
        {
            Email = _email,
            Password = _password
        };

        var result = await _httpClient.PostAsJsonAsync(METHOD, request);

        result.StatusCode.ShouldBe(HttpStatusCode.OK);

        var body = await result.Content.ReadAsStreamAsync();

        var response = await JsonDocument.ParseAsync(body);

        response.RootElement.GetProperty("name").GetString().ShouldBe(_name);
        response.RootElement.GetProperty("token").GetString().ShouldNotBeNullOrWhiteSpace();
    }


    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task Error_Login_Invalid(string culture)
    {
        var request = RequestLoginJsonBuilder.Build();

        _httpClient.DefaultRequestHeaders.AcceptLanguage.Add(new StringWithQualityHeaderValue(culture));
        var result = await _httpClient.PostAsJsonAsync(METHOD, request);

        result.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);

        var body = await result.Content.ReadAsStreamAsync();

        var response = await JsonDocument.ParseAsync(body);

        var errors = response.RootElement.GetProperty("errorMessages").EnumerateArray();

        var expectedMessage = ResourceErrorMessages.ResourceManager.GetString("EMAIL_OR_PASSWORD_INVALID", new CultureInfo(culture));

        errors.Count().ShouldBe(1);
        errors.ShouldContain(e => e.GetString()!.Equals(expectedMessage));
    }
}
