using DeviceFleet.Application.Abstractions;
using DeviceFleet.Application.Models;
using DeviceFleet.Application.Services;
using DeviceFleet.Domain;
using Moq;

namespace DeviceFleet.Tests;

public class DeviceServiceTests
{
    private readonly Mock<IDeviceRepository> _repo = new();
    private readonly DeviceService _sut; // system under test

    public DeviceServiceTests() => _sut = new DeviceService(_repo.Object);

    private static Device SampleDevice(string serial = "SN-1") =>
        new() { Name = "Radio", SerialNumber = serial, Status = DeviceStatus.Online };

    [Fact]
    public async Task GetAllAsync_maps_entities_to_dtos()
    {
        _repo.Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
             .ReturnsAsync([SampleDevice("SN-1"), SampleDevice("SN-2")]);

        var result = await _sut.GetAllAsync();

        Assert.Equal(2, result.Count);
        Assert.Contains(result, d => d.SerialNumber == "SN-1");
    }

    [Fact]
    public async Task GetByIdAsync_returns_null_when_not_found()
    {
        _repo.Setup(r => r.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
             .ReturnsAsync((Device?)null);

        var result = await _sut.GetByIdAsync(Guid.NewGuid());

        Assert.Null(result);
    }

    [Fact]
    public async Task CreateAsync_returns_DuplicateSerialNumber_when_serial_exists()
    {
        _repo.Setup(r => r.SerialNumberExistsAsync("SN-DUP", It.IsAny<CancellationToken>()))
             .ReturnsAsync(true);

        var (result, device) = await _sut.CreateAsync(new CreateDeviceRequest("Radio", "SN-DUP"));

        Assert.Equal(CreateResult.DuplicateSerialNumber, result);
        Assert.Null(device);
        _repo.Verify(r => r.AddAsync(It.IsAny<Device>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task CreateAsync_adds_device_and_returns_dto_on_success()
    {
        _repo.Setup(r => r.SerialNumberExistsAsync("SN-NEW", It.IsAny<CancellationToken>()))
             .ReturnsAsync(false);

        var (result, device) = await _sut.CreateAsync(
            new CreateDeviceRequest("Radio", "SN-NEW", DeviceStatus.Provisioning));

        Assert.Equal(CreateResult.Success, result);
        Assert.NotNull(device);
        Assert.Equal("SN-NEW", device!.SerialNumber);
        Assert.Equal(DeviceStatus.Provisioning, device.Status);
        _repo.Verify(r => r.AddAsync(It.IsAny<Device>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_returns_false_when_device_missing()
    {
        _repo.Setup(r => r.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
             .ReturnsAsync((Device?)null);

        var ok = await _sut.UpdateAsync(Guid.NewGuid(), new UpdateDeviceRequest("X", "SN-X", DeviceStatus.Error));

        Assert.False(ok);
        _repo.Verify(r => r.UpdateAsync(It.IsAny<Device>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task UpdateAsync_updates_fields_and_returns_true()
    {
        var existing = SampleDevice();
        _repo.Setup(r => r.GetByIdAsync(existing.Id, It.IsAny<CancellationToken>()))
             .ReturnsAsync(existing);

        var ok = await _sut.UpdateAsync(existing.Id,
            new UpdateDeviceRequest("Nowa nazwa", "SN-CHANGED", DeviceStatus.Offline));

        Assert.True(ok);
        Assert.Equal("Nowa nazwa", existing.Name);
        Assert.Equal("SN-CHANGED", existing.SerialNumber);
        Assert.Equal(DeviceStatus.Offline, existing.Status);
        _repo.Verify(r => r.UpdateAsync(existing, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_returns_false_when_device_missing()
    {
        _repo.Setup(r => r.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
             .ReturnsAsync((Device?)null);

        var ok = await _sut.DeleteAsync(Guid.NewGuid());

        Assert.False(ok);
        _repo.Verify(r => r.DeleteAsync(It.IsAny<Device>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task DeleteAsync_removes_device_and_returns_true()
    {
        var existing = SampleDevice();
        _repo.Setup(r => r.GetByIdAsync(existing.Id, It.IsAny<CancellationToken>()))
             .ReturnsAsync(existing);

        var ok = await _sut.DeleteAsync(existing.Id);

        Assert.True(ok);
        _repo.Verify(r => r.DeleteAsync(existing, It.IsAny<CancellationToken>()), Times.Once);
    }
}
