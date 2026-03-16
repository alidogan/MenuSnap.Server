using Tests.Common.Factories;
using Xunit;

namespace Location.IntegrationTests;

[CollectionDefinition(nameof(IntegrationTestCollection))]
public class IntegrationTestCollection : ICollectionFixture<MenuSnapWebAppFactory>;
