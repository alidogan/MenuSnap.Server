using Tests.Common.Factories;
using Xunit;

namespace Tenant.IntegrationTests;

[CollectionDefinition(nameof(IntegrationTestCollection))]
public class IntegrationTestCollection : ICollectionFixture<MenuSnapWebAppFactory>;
