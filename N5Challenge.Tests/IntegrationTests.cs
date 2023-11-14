using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using N5Challenge.Domain.Entities.Permissions;
using System.Net;
using System.Text;
using System.Text.Json;

namespace N5Challenge.Tests;

[TestClass]
public class IntegrationTests
{
    private static WebApplicationFactory<Program> _factory = new();
    private static HttpClient _httpClient = _factory.CreateClient();

    [TestMethod]
    public async Task GetPermissions_ReturnsAllPermissions()
    {
        // Act
        var response = await _httpClient.GetAsync($"permissions");

        // Assert
        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        
        var content = await response.Content.ReadAsStringAsync();
        var permisssions = JsonSerializer.Deserialize<IList<Permission>>(content, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        Assert.IsNotNull(permisssions);
        Assert.IsTrue(permisssions.Count > 0);
        Assert.IsTrue(permisssions.Any(x => x.Id == 1));
    }

    [TestMethod]
    public async Task ModifyPermission_ReturnsNoContent()
    {
        // Act
        var httpMessage = new HttpRequestMessage()
        {
            Method = HttpMethod.Put,
            Content = new StringContent("{\"EmployeeForename\":\"Gonzalo\"}", Encoding.UTF8, "application/json"),
            RequestUri = new Uri($"{_httpClient.BaseAddress}permissions/modify/1")
        };

        var response = await _httpClient.SendAsync(httpMessage);

        // Assert
        Assert.AreEqual(HttpStatusCode.NoContent, response.StatusCode);
    }

    [TestMethod]
    public async Task CreatePermission_ReturnsCreated()
    {
        // Act
        var httpMessage = new HttpRequestMessage()
        {
            Method = HttpMethod.Post,
            Content = new StringContent("{\"EmployeeForename\":\"Gonzalo\",\"EmployeeSurname\":\"Cabo\",\"PermissionType\":1}", Encoding.UTF8, "application/json"),
            RequestUri = new Uri($"{_httpClient.BaseAddress}permissions/request")
        };

        var response = await _httpClient.SendAsync(httpMessage);

        // Assert
        Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);
    }
}
