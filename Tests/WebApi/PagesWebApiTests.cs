using Domain.Aggregates.Pages;
using Microsoft.AspNetCore.Http.HttpResults;
using WebApi.EndpointDefinitions;

namespace Tests.WebApi;
public class PagesWebApiTests {
    [Fact]
    public async Task Get_NonExistingPage_ReturnsBadRequest() {
        // Arrange
        var pageId = PageId.New();

        // Act
        var response = await PagesEndpoint.PullAsync(pageId, ReplicationId.Empty);

        // Assert
        var result = response.Result;
        Assert.IsType<BadRequest<string>>(result);
    }
}
