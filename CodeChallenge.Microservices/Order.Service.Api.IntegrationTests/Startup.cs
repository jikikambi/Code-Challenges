/// <summary>
/// For more information see; https://xunit.net/docs/shared-context
/// </summary>
[CollectionDefinition("Startup")]
public class Startup : ICollectionFixture<StartupFixture>
{
    // This class has no code, and is never created. Its purpose is simply
    // to be the place to apply [CollectionDefinition] and all the
    // ICollectionFixture<> interfaces.
}
