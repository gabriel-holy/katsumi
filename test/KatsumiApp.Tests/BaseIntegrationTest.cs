using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;

namespace KatsumiApp.Tests
{
    public abstract class BaseIntegrationTest
    {
        protected readonly IServiceCollection _services;
        public BaseIntegrationTest()
        {
            _services = new ServiceCollection();

            new Startup().ConfigureServices(_services);
        }

        public static Task DisposeAsync() => Task.CompletedTask;
    }
}
