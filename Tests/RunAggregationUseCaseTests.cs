using DataAggregator.Application.Models;
using DataAggregator.Application.Services.Abstractions;
using DataAggregator.Application.Services;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using FluentAssertions;

namespace Tests
{
    public class RunAggregationUseCaseTests
    {
        [Fact]
        public async Task RunAsync_ProcessesTenants_AndReturnsCorrectCounters()
        {
            var tenants = new List<(int TenantId, string TenantName)>
            {
                (101, "Jack Sparrow Warship"),
                (145, "Some Org Without Datasource")
            };

            var tenantsReader = new Mock<ITenantsReader>();
            tenantsReader.Setup(x => x.GetTenantsAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(tenants);

            var ds101 = new Mock<ITenantDataSource>();
            ds101.SetupGet(x => x.TenantId).Returns(101);

            var quiet101 = new List<QuietCustomerCandidate>
            {
                new QuietCustomerCandidate
                {
                    TenantId = 101,
                    TenantName = "Jack Sparrow Warship",
                    CustomerId = "1",
                    FirstName = "Jack",
                    LastName = "Sparrow",
                    Email = "jack@sparrow.com",
                    ActivityCount = 0
                },
                new QuietCustomerCandidate
                {
                    TenantId = 101,
                    TenantName = "Jack Sparrow Warship",
                    CustomerId = "2",
                    FirstName = "No",
                    LastName = "Email",
                    Email = "  ",
                    ActivityCount = 2
                }
            };

            var from = new DateTime(2024, 4, 1, 0, 0, 0, DateTimeKind.Utc);
            var to = from.AddMonths(1);

            ds101.Setup(x => x.GetQuietCustomersAsync("Jack Sparrow Warship", from, to, It.IsAny<CancellationToken>()))
                .ReturnsAsync(quiet101);

            var codeGen = new Mock<IAuxiliaryClientCodeGenerator>();
            codeGen.Setup(x => x.Generate("Jack", "Sparrow", "Jack Sparrow Warship"))
                .Returns("KCA-RAP-JSW");

            var broker = new Mock<INotificationsBrokerWriter>();
            broker.Setup(x => x.EnqueueAsync(It.IsAny<IEnumerable<Notification>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(1);

            var sut = new RunAggregationUseCase(
                tenantsReader.Object,
                new[] { ds101.Object },
                codeGen.Object,
                broker.Object,
                NullLogger<RunAggregationUseCase>.Instance);

            var result = await sut.RunAsync(from, to, CancellationToken.None);

            result.FromUtc.Should().Be(from);
            result.ToUtcExclusive.Should().Be(to);
            result.TenantsProcessed.Should().Be(2);
            result.QuietCustomersFound.Should().Be(2);
            result.NotificationsEnqueued.Should().Be(1);

            result.QuietCustomersByTenant.Should().ContainKey(101);
            result.QuietCustomersByTenant[101].Should().Be(2);

            result.QuietCustomersByTenant.Should().ContainKey(145);
            result.QuietCustomersByTenant[145].Should().Be(0);

            broker.Verify(x => x.EnqueueAsync(
                    It.Is<IEnumerable<Notification>>(items =>
                        items.Count() == 1 &&
                        items.First().Email == "jack@sparrow.com" &&
                        items.First().FirstName == "Jack" &&
                        items.First().LastName == "Sparrow" &&
                        items.First().FinHash == "KCA-RAP-JSW"),
                    It.IsAny<CancellationToken>()),
                Times.Once);

            codeGen.Verify(x => x.Generate("Jack", "Sparrow", "Jack Sparrow Warship"), Times.Once);
        }

        [Fact]
        public async Task RunAsync_WhenDatasourceMissing_SetsCountZero_AndDoesNotThrow()
        {
            var tenants = new List<(int TenantId, string TenantName)>
            {
                (101, "Org101")
            };

            var tenantsReader = new Mock<ITenantsReader>();
            tenantsReader.Setup(x => x.GetTenantsAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(tenants);

            var codeGen = new Mock<IAuxiliaryClientCodeGenerator>();
            var broker = new Mock<INotificationsBrokerWriter>();

            var sut = new RunAggregationUseCase(
                tenantsReader.Object,
                Array.Empty<ITenantDataSource>(),
                codeGen.Object,
                broker.Object,
                NullLogger<RunAggregationUseCase>.Instance);

            var from = new DateTime(2024, 4, 1, 0, 0, 0, DateTimeKind.Utc);
            var to = from.AddMonths(1);

            var result = await sut.RunAsync(from, to, CancellationToken.None);

            result.TenantsProcessed.Should().Be(1);
            result.QuietCustomersFound.Should().Be(0);
            result.NotificationsEnqueued.Should().Be(0);

            result.QuietCustomersByTenant.Should().ContainKey(101);
            result.QuietCustomersByTenant[101].Should().Be(0);

            broker.Verify(x => x.EnqueueAsync(
                It.Is<IEnumerable<Notification>>(n => !n.Any()),
                It.IsAny<CancellationToken>()),
            Times.Once);
            codeGen.Verify(x => x.Generate(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public async Task RunAsync_PassesDateRangeToDatasource()
        {
            var tenants = new List<(int TenantId, string TenantName)>
            {
                (2, "Org2")
            };

            var tenantsReader = new Mock<ITenantsReader>();
            tenantsReader.Setup(x => x.GetTenantsAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(tenants);

            var ds2 = new Mock<ITenantDataSource>();
            ds2.SetupGet(x => x.TenantId).Returns(2);
            ds2.Setup(x => x.GetQuietCustomersAsync(It.IsAny<string>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<QuietCustomerCandidate>());

            var codeGen = new Mock<IAuxiliaryClientCodeGenerator>();
            var broker = new Mock<INotificationsBrokerWriter>();
            broker.Setup(x => x.EnqueueAsync(It.IsAny<IEnumerable<Notification>>(), It.IsAny<CancellationToken>()))
                  .ReturnsAsync(0);

            var sut = new RunAggregationUseCase(
                tenantsReader.Object,
                new[] { ds2.Object },
                codeGen.Object,
                broker.Object,
                NullLogger<RunAggregationUseCase>.Instance);

            var from = new DateTime(2024, 4, 1, 0, 0, 0, DateTimeKind.Utc);
            var to = from.AddMonths(1);

            await sut.RunAsync(from, to, CancellationToken.None);

            ds2.Verify(x => x.GetQuietCustomersAsync("Org2", from, to, It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
