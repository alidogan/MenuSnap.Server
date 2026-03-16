using Location.Locations.Events;
using Location.Locations.Models;
using LocationModel = Location.Locations.Models.Location;

namespace Location.UnitTests.Locations.Models;

public class LocationTests : BaseUnitTest
{
    [Fact]
    public void Create_WithValidData_ReturnsLocationWithCorrectProperties()
    {
        var tenantId = Guid.CreateVersion7();
        var location = LocationModel.Create(
            Guid.CreateVersion7(), tenantId, "Hotel Amsterdam", "hotel-amsterdam",
            LocationType.Hotel, "Dam 1", "+31201234567", "Great hotel", true);

        location.TenantId.Should().Be(tenantId);
        location.Name.Should().Be("Hotel Amsterdam");
        location.Slug.Should().Be("hotel-amsterdam");
        location.Type.Should().Be(LocationType.Hotel);
        location.Address.Should().Be("Dam 1");
        location.Phone.Should().Be("+31201234567");
        location.Description.Should().Be("Great hotel");
        location.IsActive.Should().BeTrue();
        location.LogoUrl.Should().BeNull();
    }

    [Fact]
    public void Create_WithValidData_AddsLocationCreatedDomainEvent()
    {
        var location = LocationModel.Create(
            Guid.CreateVersion7(), Guid.CreateVersion7(), "Restaurant",
            "restaurant", LocationType.Restaurant, null, null, null);

        location.DomainEvents.Should().ContainSingle()
            .Which.Should().BeOfType<LocationCreatedEvent>();
    }

    [Fact]
    public void Create_WithEmptyName_ThrowsArgumentException()
    {
        var act = () => LocationModel.Create(
            Guid.CreateVersion7(), Guid.CreateVersion7(), "",
            "slug", LocationType.Restaurant, null, null, null);

        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void Create_WithEmptySlug_ThrowsArgumentException()
    {
        var act = () => LocationModel.Create(
            Guid.CreateVersion7(), Guid.CreateVersion7(), "Name",
            "", LocationType.Restaurant, null, null, null);

        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void Create_WithEmptyTenantId_ThrowsArgumentException()
    {
        var act = () => LocationModel.Create(
            Guid.CreateVersion7(), Guid.Empty, "Name",
            "slug", LocationType.Restaurant, null, null, null);

        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void Update_WithValidData_UpdatesAllProperties()
    {
        var location = LocationModel.Create(
            Guid.CreateVersion7(), Guid.CreateVersion7(), "Old Name",
            "old-name", LocationType.Restaurant, null, null, null);

        location.Update("New Name", "new-name", LocationType.Hotel, "Dam 1", "+31", "Updated", false);

        location.Name.Should().Be("New Name");
        location.Slug.Should().Be("new-name");
        location.Type.Should().Be(LocationType.Hotel);
        location.Address.Should().Be("Dam 1");
        location.IsActive.Should().BeFalse();
    }

    [Fact]
    public void SetLogoUrl_WithValidUrl_SetsLogoUrl()
    {
        var location = LocationModel.Create(
            Guid.CreateVersion7(), Guid.CreateVersion7(), "Name",
            "name", LocationType.Restaurant, null, null, null);

        location.SetLogoUrl("http://minio/menusnap/location/logo.png");

        location.LogoUrl.Should().Be("http://minio/menusnap/location/logo.png");
    }

    [Fact]
    public void SetLogoUrl_WithEmptyUrl_ThrowsArgumentException()
    {
        var location = LocationModel.Create(
            Guid.CreateVersion7(), Guid.CreateVersion7(), "Name",
            "name", LocationType.Restaurant, null, null, null);

        var act = () => location.SetLogoUrl("");

        act.Should().Throw<ArgumentException>();
    }
}
