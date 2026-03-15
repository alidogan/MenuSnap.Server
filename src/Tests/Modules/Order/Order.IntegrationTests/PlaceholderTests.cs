namespace Order.IntegrationTests;

public class PlaceholderTests(MenuSnapWebAppFactory factory) : BaseIntegrationTest(factory)
{
    [Fact]
    public void Placeholder_IntegrationTestInfrastructure_IsReady()
    {
        Client.Should().NotBeNull();
        Services.Should().NotBeNull();
    }
}
