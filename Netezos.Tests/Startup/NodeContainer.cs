using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using TestContainers.Container.Abstractions;
using TestContainers.Container.Abstractions.Hosting;

namespace Netezos.Tests.Startup
{
    public class NodeContainer
    {
        public IContainer Container { get; }

        public NodeContainer(string imageName, string tag, int portBinding)
        {
            //is default pulling quay.io/testcontainers/ryuk:0.2.3 and starting container
            Environment.SetEnvironmentVariable("REAPER_DISABLED", "true");

            Container = new ContainerBuilder<GenericContainer>()
                .ConfigureHostConfiguration(builder => builder.AddInMemoryCollection())
                .ConfigureAppConfiguration((context, builder) => builder.AddInMemoryCollection())
                .ConfigureDockerImageName($"{imageName}:{tag}")
                .ConfigureLogging(builder =>
                {
                    builder.AddConsole();
                    builder.SetMinimumLevel(LogLevel.Debug);
                })
                .ConfigureContainer((context, container) =>
                {
                    container.AutoRemove = true;
                    container.ExposedPorts.Add(portBinding);
                    container.PortBindings.Add(portBinding, portBinding);
                })
                .Build();
        }
    }
}