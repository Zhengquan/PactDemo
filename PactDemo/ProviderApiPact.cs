using System;
using PactNet;
using PactNet.Mocks.MockHttpService;

namespace PactDemo
{
    public class ProviderApiPact : IDisposable
    {
        public IPactBuilder PactBuilder { get; }
        public IMockProviderService MockProviderService { get; private set; }

        public int MockServerPort => 1234;

        public string MockProviderServiceBaseUri => $"http://localhost:{MockServerPort}";

        public ProviderApiPact()
        {
            PactBuilder = new PactBuilder();
            PactBuilder
                .ServiceConsumer("Consumer")
                .HasPactWith("Something API");

            MockProviderService = PactBuilder.MockService(MockServerPort);
        }

        public void Dispose()
        {
            PactBuilder.Build();
        }
    }
}
