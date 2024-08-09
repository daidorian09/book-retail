using Application.Constants;
using Shouldly;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;

namespace Tests.Integration.Statistics
{
    public class StatisticsControllerTests : IDisposable
    {
        private readonly HttpClient _client;
        private readonly WireMockServer _server;

        public StatisticsControllerTests()
        {
            _server = WireMockServer.Start();

            _client = new HttpClient
            {
                BaseAddress = new Uri(_server.Url)
            };

            SetupWireMockStubs();
        }

        [Fact]
        public async Task GetMonthlyStatistics_ReturnsExpectedData()
        {
            var year = 2024;
            var expectedUrl = $"/monthly-statistics/{year}";

            var request = new HttpRequestMessage(HttpMethod.Get, expectedUrl);
            request.Headers.Add(AppConstants.RequestOwnerId, "1");
            request.Headers.Add(AppConstants.Role, "Admin");

          
            var response = await _client.SendAsync(request);

            response.EnsureSuccessStatusCode();
            var responseBody = await response.Content.ReadAsStringAsync();

            var expectedJson = @"
                    [
                        {
                            ""month"": ""May"",
                            ""totalOrderCount"": 1,
                            ""totalPurchasedBooks"": 1,
                            ""monthlySum"": 105.78
                        },
                        {
                            ""month"": ""August"",
                            ""totalOrderCount"": 2,
                            ""totalPurchasedBooks"": 3,
                            ""monthlySum"": 249.56
                        }
                    ]";

            responseBody.Trim().ShouldBe(expectedJson.Trim());
        }

        [Fact]
        public async Task GetMonthlyStatistics_ReturnsValidationError()
        {
            var year = -1;
            var expectedUrl = $"/monthly-statistics/{year}";

            var request = new HttpRequestMessage(HttpMethod.Get, expectedUrl);
            request.Headers.Add(AppConstants.RequestOwnerId, "1");
            request.Headers.Add(AppConstants.Role, "Admin");


            var response = await _client.SendAsync(request);

            var responseBody = await response.Content.ReadAsStringAsync();

            var expectedJson = @"
                    {
                        ""title"": ""Validation Error"",
                        ""detail"": ""One or more validation failures have occurred."",
                        ""invalidParams"": {
                            ""Year"": [
                                ""'Year' must be greater than '0'.""
                            ]
                        }
                    }";

            responseBody.Trim().ShouldBe(expectedJson.Trim());
        }

        [Fact]
        public async Task GetMonthlyStatistics_ReturnsForbidden()
        {
            var year = 2022;
            var expectedUrl = $"/monthly-statistics/{year}";

            var request = new HttpRequestMessage(HttpMethod.Get, expectedUrl);
            request.Headers.Add(AppConstants.RequestOwnerId, "1");
            request.Headers.Add(AppConstants.Role, "Customer");


            var response = await _client.SendAsync(request);
            var responseBody = await response.Content.ReadAsStringAsync();

            var expectedJson = @"
                                 {
                                     ""detail"": ""Access denied. You do not have permission to access this resource."",
                                     ""title"": ""Authentication Error""
                                 }";

            responseBody.Trim().ShouldBe(expectedJson.Trim());
        }

        public void Dispose()
        {
            _server?.Stop();
        }
        private void SetupWireMockStubs()
        {
            #region GetMonthlyStatistics
            //Returns 200 Stub
            _server.Given(
                Request.Create()
                    .WithPath("/monthly-statistics/" + 2024)
                    .WithHeader(AppConstants.RequestOwnerId, "1")
                    .WithHeader(AppConstants.Role, "Admin")
                    .UsingGet()
            )
            .RespondWith(
                Response.Create()
                    .WithStatusCode(200)
                    .WithBody(@"
                    [
                        {
                            ""month"": ""May"",
                            ""totalOrderCount"": 1,
                            ""totalPurchasedBooks"": 1,
                            ""monthlySum"": 105.78
                        },
                        {
                            ""month"": ""August"",
                            ""totalOrderCount"": 2,
                            ""totalPurchasedBooks"": 3,
                            ""monthlySum"": 249.56
                        }
                    ]")
            );

            //Return 422 Stub
            _server.Given(
                Request.Create()
                    .WithPath("/monthly-statistics/" + -1)
                    .WithHeader(AppConstants.RequestOwnerId, "1")
                    .WithHeader(AppConstants.Role, "Admin")
                    .UsingGet()
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
                            ""Year"": [
                                ""'Year' must be greater than '0'.""
                            ]
                        }
                    }")
                );

            //Return 403 Stub
            _server.Given(
                 Request.Create()
                     .WithPath("/monthly-statistics/" + 2022)
                     .WithHeader(AppConstants.RequestOwnerId, "1")
                     .WithHeader(AppConstants.Role, "Customer")
                     .UsingGet()
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
            #endregion
        }
    }
}
