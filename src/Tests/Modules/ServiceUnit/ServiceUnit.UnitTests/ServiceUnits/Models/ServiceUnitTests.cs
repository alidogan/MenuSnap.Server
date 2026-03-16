using ServiceUnit.ServiceUnits.Events;
using ServiceUnit.ServiceUnits.Models;
using ServiceUnitModel = ServiceUnit.ServiceUnits.Models.ServiceUnit;

namespace ServiceUnit.UnitTests.ServiceUnits.Models;

public class ServiceUnitTests : BaseUnitTest
{
    private static ServiceUnitModel CreateValid(
        string name = "Table 1",
        string code = "T1",
        ServiceUnitType type = ServiceUnitType.Table) =>
        ServiceUnitModel.Create(
            Guid.CreateVersion7(),
            Guid.CreateVersion7(),
            name, code, type,
            capacity: 4,
            groupName: "Terrace",
            externalReference: null,
            sortOrder: 1);

    [Fact]
    public void Create_WithValidData_ReturnsServiceUnitWithCorrectProperties()
    {
        var locationId = Guid.CreateVersion7();
        var serviceUnit = ServiceUnitModel.Create(
            Guid.CreateVersion7(), locationId,
            "Table 1", "T1", ServiceUnitType.Table,
            4, "Terrace", "ext-ref-1", 1,
            ServiceUnitStatus.Available, true);

        serviceUnit.LocationId.Should().Be(locationId);
        serviceUnit.Name.Should().Be("Table 1");
        serviceUnit.Code.Should().Be("T1");
        serviceUnit.Type.Should().Be(ServiceUnitType.Table);
        serviceUnit.Capacity.Should().Be(4);
        serviceUnit.GroupName.Should().Be("Terrace");
        serviceUnit.ExternalReference.Should().Be("ext-ref-1");
        serviceUnit.SortOrder.Should().Be(1);
        serviceUnit.Status.Should().Be(ServiceUnitStatus.Available);
        serviceUnit.IsActive.Should().BeTrue();
        serviceUnit.IsDeleted.Should().BeFalse();
        serviceUnit.LastUsedAt.Should().BeNull();
    }

    [Fact]
    public void Create_WithValidData_AddsServiceUnitCreatedDomainEvent()
    {
        var serviceUnit = CreateValid();

        serviceUnit.DomainEvents.Should().ContainSingle()
            .Which.Should().BeOfType<ServiceUnitCreatedEvent>();
    }

    [Fact]
    public void Create_WithEmptyName_ThrowsArgumentException()
    {
        var act = () => CreateValid(name: "");
        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void Create_WithEmptyCode_ThrowsArgumentException()
    {
        var act = () => CreateValid(code: "");
        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void Create_WithEmptyLocationId_ThrowsArgumentException()
    {
        var act = () => ServiceUnitModel.Create(
            Guid.CreateVersion7(), Guid.Empty,
            "Table 1", "T1", ServiceUnitType.Table,
            null, null, null, 0);

        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void Update_WithValidData_UpdatesAllProperties()
    {
        var serviceUnit = CreateValid();

        serviceUnit.Update(
            "Room 1", "R1", ServiceUnitType.Room,
            2, "Indoor", null,
            5, ServiceUnitStatus.Occupied, false);

        serviceUnit.Name.Should().Be("Room 1");
        serviceUnit.Code.Should().Be("R1");
        serviceUnit.Type.Should().Be(ServiceUnitType.Room);
        serviceUnit.Capacity.Should().Be(2);
        serviceUnit.GroupName.Should().Be("Indoor");
        serviceUnit.SortOrder.Should().Be(5);
        serviceUnit.Status.Should().Be(ServiceUnitStatus.Occupied);
        serviceUnit.IsActive.Should().BeFalse();
    }

    [Fact]
    public void MarkUsed_SetsLastUsedAt()
    {
        var serviceUnit = CreateValid();
        var before = DateTime.UtcNow;

        serviceUnit.MarkUsed();

        serviceUnit.LastUsedAt.Should().NotBeNull();
        serviceUnit.LastUsedAt!.Value.Should().BeOnOrAfter(before);
    }

    [Fact]
    public void Delete_SetsIsDeletedToTrue()
    {
        var serviceUnit = CreateValid();

        serviceUnit.Delete();

        serviceUnit.IsDeleted.Should().BeTrue();
    }

    [Fact]
    public void Delete_AddsServiceUnitDeletedDomainEvent()
    {
        var serviceUnit = CreateValid();
        serviceUnit.ClearDomainEvents(); // clear Create event

        serviceUnit.Delete();

        serviceUnit.DomainEvents.Should().ContainSingle()
            .Which.Should().BeOfType<ServiceUnitDeletedEvent>();
    }
}
