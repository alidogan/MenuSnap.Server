using Tests.Common.Factories;
using Xunit;

namespace Catalog.IntegrationTests;

[CollectionDefinition(nameof(IntegrationTestCollection))]
public class IntegrationTestCollection : ICollectionFixture<MenuSnapWebAppFactory>;
