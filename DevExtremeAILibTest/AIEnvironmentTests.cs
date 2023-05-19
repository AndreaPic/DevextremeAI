using DevExtremeAI.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DevExtremeAILibTest
{
    public class AIEnvironmentTests : IClassFixture<TestApplication>
    {
        private readonly TestApplication _factory;

        public AIEnvironmentTests(TestApplication factory)
        {
            _factory = factory;
        }

        [Fact]
        public void EnvironmentValueTest()
        {
            using (var scope = _factory.Services.CreateScope())
            {
                var env = scope.ServiceProvider.GetService<IAIEnvironment>();
                var key = env.GetApiKey();
                Assert.NotNull(key);
                var org = env.GetOrganization();
                Assert.NotNull(org);
            }
        }
    }
}