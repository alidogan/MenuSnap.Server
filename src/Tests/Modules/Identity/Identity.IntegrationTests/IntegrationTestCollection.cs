using Tests.Common.Factories;
using Xunit;

namespace Identity.IntegrationTests;

[CollectionDefinition(nameof(IntegrationTestCollection))]
public class IntegrationTestCollection : ICollectionFixture<MenuSnapWebAppFactory>;
