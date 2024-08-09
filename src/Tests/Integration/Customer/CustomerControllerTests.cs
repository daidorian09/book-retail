using Application.Constants;
using Shouldly;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;

namespace Tests.Integration.Customer
{
    public class CustomerControllerTests : IDisposable
    {
        private readonly HttpClient _client;
        private readonly WireMockServer _server;

        public CustomerControllerTests()
        {
            _server = WireMockServer.Start();

            _client = new HttpClient
            {
                BaseAddress = new Uri(_server.Url)
            };

            SetupWireMockStubs();
        }

        [Fact]
        public async Task CreateCustomer_ReturnsExpectedData()
        {
            var expectedUrl = "/customer";

            var request = new HttpRequestMessage(HttpMethod.Post, expectedUrl)
            {
                Content = new StringContent(@"
                    {
                        ""firstName"": ""test"",
                        ""lastName"": ""test"",
                        ""address"": ""test"",
                        ""email"": ""test@test.com""
                    }", System.Text.Encoding.UTF8, "application/json")
            };
            request.Headers.Add(AppConstants.RequestOwnerId, "1");
            request.Headers.Add(AppConstants.Role, "Customer");

            var response = await _client.SendAsync(request);

            var responseBody = await response.Content.ReadAsStringAsync();

            var expectedJson = @"
                    {
                        ""id"": ""3cfc7623-773c-4728-a46a-aa51dd1d9706""
                    }";

            responseBody.Trim().ShouldBe(expectedJson.Trim());
        }

        [Fact]
        public async Task CreateCustomer_ReturnsValidationError()
        {
            var expectedUrl = "/customer";

            var request = new HttpRequestMessage(HttpMethod.Post, expectedUrl)
            {
                Content = new StringContent(@"
                    {
                        ""firstName"": null,
                        ""lastName"": ""test"",
                        ""address"": ""test"",
                        ""email"": ""test@test.com""
                    }", System.Text.Encoding.UTF8, "application/json")
            };
            request.Headers.Add(AppConstants.RequestOwnerId, "1");
            request.Headers.Add(AppConstants.Role, "Customer");

            var response = await _client.SendAsync(request);

            var responseBody = await response.Content.ReadAsStringAsync();

            var expectedJson = @"
                    {
                        ""title"": ""Validation Error"",
                        ""detail"": ""One or more validation failures have occurred."",
                        ""invalidParams"": {
                            ""FirstName"": [
                                ""'First Name' must not be empty.""
                            ]
                        }
                    }";

            responseBody.Trim().ShouldBe(expectedJson.Trim());
        }

        [Fact]
        public async Task CreateCustomer_ReturnsForbidden()
        {
            var expectedUrl = "/customer";

            var request = new HttpRequestMessage(HttpMethod.Post, expectedUrl)
            {
                Content = new StringContent(@"
                    {
                        ""firstName"": ""test"",
                        ""lastName"": ""test"",
                        ""address"": ""test"",
                        ""email"": ""test@test.com""
                    }", System.Text.Encoding.UTF8, "application/json")
            };
            request.Headers.Add(AppConstants.RequestOwnerId, "1");
            request.Headers.Add(AppConstants.Role, "Admin");

            var response = await _client.SendAsync(request);

            var responseBody = await response.Content.ReadAsStringAsync();

            var expectedJson = @"
                     {
                         ""detail"": ""Access denied. You do not have permission to access this resource."",
                         ""title"": ""Authentication Error""
                     }";

            responseBody.Trim().ShouldBe(expectedJson.Trim());
        }

        [Fact]
        public async Task CreateCustomer_ReturnsConflict()
        {
            var expectedUrl = "/customer";

            var request = new HttpRequestMessage(HttpMethod.Post, expectedUrl)
            {
                Content = new StringContent(@"
                            {
                                ""firstName"": ""test"",
                                ""lastName"": ""test"",
                                ""address"": ""test"",
                                ""email"": ""test@test1.com""
                            }", System.Text.Encoding.UTF8, "application/json")
            };
            request.Headers.Add(AppConstants.RequestOwnerId, "1");
            request.Headers.Add(AppConstants.Role, "Customer");

            var response = await _client.SendAsync(request);

            var responseBody = await response.Content.ReadAsStringAsync();

            var expectedJson = @"
                            {
                                ""title"": ""Conflict"",
                                ""status"": 409,
                                ""detail"": ""Customer is registered""
                            }";

            responseBody.Trim().ShouldBe(expectedJson.Trim());
        }

        public void Dispose()
        {
            _server?.Stop();
        }
        private void SetupWireMockStubs()
        {
            #region CreateCustomer
            //Returns 201 Stub
            _server.Given(
                Request.Create()
                    .WithPath("/customer")
                    .WithHeader(AppConstants.RequestOwnerId, "1")
                    .WithHeader(AppConstants.Role, "Customer")
                    .WithBody(@"
                    {
                        ""firstName"": ""test"",
                        ""lastName"": ""test"",
                        ""address"": ""test"",
                        ""email"": ""test@test.com""
                    }")
                    .UsingPost()
            )
            .RespondWith(
                Response.Create()
                    .WithStatusCode(201)
                   .WithBody(@"
                    {
                        ""id"": ""3cfc7623-773c-4728-a46a-aa51dd1d9706""
                    }")
            );

            //Return 422 Stub
            _server.Given(
                Request.Create()
                     .WithPath("/customer")
                    .WithHeader(AppConstants.RequestOwnerId, "1")
                    .WithHeader(AppConstants.Role, "Customer")
                    .WithBody(@"
                    {
                        ""firstName"": null,
                        ""lastName"": ""test"",
                        ""address"": ""test"",
                        ""email"": ""test@test.com""
                    }")
                    .UsingPost()
                )
                .RespondWith(
                Response.Create()
                    .WithStatusCode(422)
                    .WithHeader("Content-Type", "application/json")
                    .WithBody(@"
                    {
                        ""title"": ""Validation Error"",
                        ""detail"": ""One or more validation failures have occurred."",
                        ""invalidParams"": {
                            ""FirstName"": [
                                ""'First Name' must not be empty.""
                            ]
                        }
                    }")
                );

            //Return 403 Stub
            _server.Given(
                 Request.Create()
                     .WithPath("/customer")
                     .WithHeader(AppConstants.RequestOwnerId, "1")
                     .WithHeader(AppConstants.Role, "Admin")
                     .WithBody(@"
                    {
                        ""firstName"": ""test"",
                        ""lastName"": ""test"",
                        ""address"": ""test"",
                        ""email"": ""test@test.com""
                    }")
                    .UsingPost()
                 )
                 .RespondWith(
                 Response.Create()
                     .WithStatusCode(403)
                     .WithHeader("Content-Type", "application/json")
                     .WithBody(@"
                     {
                         ""detail"": ""Access denied. You do not have permission to access this resource."",
                         ""title"": ""Authentication Error""
                     }")
                 );

            //Return 409 Stub
            _server.Given(
             Request.Create()
                 .WithPath("/customer")
                 .WithHeader(AppConstants.RequestOwnerId, "1")
                 .WithHeader(AppConstants.Role, "Customer")
                 .WithBody(@"
                            {
                                ""firstName"": ""test"",
                                ""lastName"": ""test"",
                                ""address"": ""test"",
                                ""email"": ""test@test1.com""
                            }")
                .UsingPost()
             )
             .RespondWith(
             Response.Create()
                 .WithStatusCode(409)
                 .WithHeader("Content-Type", "application/json")
                 .WithBody(@"
                            {
                                ""title"": ""Conflict"",
                                ""status"": 409,
                                ""detail"": ""Customer is registered""
                            }")
                  );     
                }
            }
    #endregion
}
