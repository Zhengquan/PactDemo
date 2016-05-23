using System;
using System.Collections.Generic;
using System.Net.Http;
using PactNet.Mocks.MockHttpService;
using PactNet.Mocks.MockHttpService.Models;
using Xunit;

namespace PactDemo
{
    public class Facts : IClassFixture<ProviderApiPact>
    {
        private readonly IMockProviderService _mockProviderService;
        private readonly string _mockProviderServiceBaseUri;

        public Facts(ProviderApiPact pact)
        {
            _mockProviderService = pact.MockProviderService;
            _mockProviderService.ClearInteractions();
            _mockProviderServiceBaseUri = pact.MockProviderServiceBaseUri;
        }

        [Fact]
        public void failed_test_case_send_request_without_extra_header()
        {
            _mockProviderService
                .Given("There is a something with id 'tester'")
                .UponReceiving("A GET request to retrieve the something")
                .With(new ProviderServiceRequest
                {
                    Method = HttpVerb.Get,
                    Path = "/somethings/tester",
                    Headers = new Dictionary<string, string>
                    {
                        {"Accept", "application/json"},
                        {"Origin", "http://localhost.com"},
                        {"Referrer", "http://baidu.com/index.html"},
                        {"Agent", "1111"},
                        {"Service", "Product"},
                    }
                })
                .WillRespondWith(new ProviderServiceResponse
                {
                    Status = 200,
                    Headers = new Dictionary<string, string>
                    {
                        {"Content-Type", "application/json; charset=utf-8"}
                    },
                    Body = new
                    {
                        id = "tester",
                        firstName = "Totally",
                        lastName = "Awesome"
                    }
                });

            using (var client = new HttpClient {BaseAddress = new Uri(_mockProviderServiceBaseUri)})
            {
                var request = new HttpRequestMessage(HttpMethod.Get, "/somethings/tester");
                request.Headers.Add("Accept", "application/json");

                client.SendAsync(request).Wait();
            }

            _mockProviderService.VerifyInteractions();
        }
        [Fact]
        public void success_case_send_request_with_extra_header()
        {
            _mockProviderService
                .Given("There is a something with id 'tester'")
                .UponReceiving("A GET request to retrieve the something")
                .With(new ProviderServiceRequest
                {
                    Method = HttpVerb.Get,
                    Path = "/somethings/tester",
                    Headers = new Dictionary<string, string>
                    {
                        {"Accept", "application/json"},
                        {"Origin", "http://localhost.com"},
                        {"Referrer", "http://baidu.com/index.html"},
                        {"Agent", "1111"},
                        {"Service", "Product"},
                    }
                })
                .WillRespondWith(new ProviderServiceResponse
                {
                    Status = 200,
                    Headers = new Dictionary<string, string>
                    {
                        {"Content-Type", "application/json; charset=utf-8"}
                    },
                    Body = new
                    {
                        id = "tester",
                        firstName = "Totally",
                        lastName = "Awesome"
                    }
                });

            using (var client = new HttpClient {BaseAddress = new Uri(_mockProviderServiceBaseUri)})
            {
                var request = new HttpRequestMessage(HttpMethod.Get, "/somethings/tester");
                request.Headers.Add("Accept", "application/json");
                request.Headers.Add("Origin", "http://localhost.com");
                request.Headers.Add("Referrer", "http://baidu.com/index.html");
                request.Headers.Add("Agent", "1111");
                request.Headers.Add("Service", "Product");

                client.SendAsync(request).Wait();
            }

            _mockProviderService.VerifyInteractions();
        }
    }
}