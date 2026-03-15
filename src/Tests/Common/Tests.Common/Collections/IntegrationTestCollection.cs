using Tests.Common.Factories;
using Xunit;

namespace Tests.Common.Collections;

[CollectionDefinition(nameof(IntegrationTestCollection))]
public class IntegrationTestCollection : ICollectionFixture<MenuSnapWebAppFactory>;
