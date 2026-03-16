using Tests.Common.Factories;
using Xunit;

namespace ServiceUnit.IntegrationTests;

[CollectionDefinition(nameof(IntegrationTestCollection))]
public class IntegrationTestCollection : ICollectionFixture<MenuSnapWebAppFactory>;
