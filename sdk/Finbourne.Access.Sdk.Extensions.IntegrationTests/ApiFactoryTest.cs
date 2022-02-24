using Finbourne.Access.Sdk.Api;
using Finbourne.Access.Sdk.Client;
using Finbourne.Access.Sdk.Model;
using NUnit.Framework;
using System;

namespace Finbourne.Access.Sdk.Extensions.IntegrationTests
{
    public class ApiFactoryTest
    {
        private IApiFactory _factory;

        [OneTimeSetUp]
        public void SetUp()
        {
            _factory = IntegrationTestApiFactoryBuilder.CreateApiFactory("secrets.json");
        }

        /* Add this test for each Api within Finbourne.Access.Sdk.Api
        [Test]
        public void Create_XXXApi()
        {
            var api = _factory.Api<XXXApi>();

            Assert.That(api, Is.Not.Null);
            Assert.That(api, Is.InstanceOf<XXXApi>());
        }
        */

        /* Add this test for and interface of an Api within Finbourne.Access.Sdk.Api
        [Test]
        public void Api_From_Interface()
        {
            var api = _factory.Api<IXXXApi>();

            Assert.That(api, Is.Not.Null);
            Assert.That(api, Is.InstanceOf<IXXXApi>());
        }
        */

        /* Add this test for and interface of an Api within Finbourne.Access.Sdk.Api
        [Test]
        public void NetworkConnectivityErrors_ThrowsException()
        {
            var apiConfig = ApiConfigurationBuilder.Build("secrets.json");
            // nothing should be listening on this, so we should get a "No connection could be made" error...
            apiConfig.ApiUrl = "https://localhost:56789/insights"; 

            var factory = new ApiFactory(apiConfig);
            var api = factory.Api<IXXXApi>();

            // Can't be more specific as we get different exceptions locally vs in the build pipeline
            var expectedMsg = "Internal SDK error occurred when calling GetVendorResponse";

            Assert.That(
                () => api.GetxxxApiMethodxxxWithHttpInfo("$@!-"),
                Throws.InstanceOf<ApiException>()
                    .With.Message.Contains(expectedMsg));

            // Note: these non-"WithHttpInfo" methods just unwrap the `Data` property from the call above.
            // But these were the problematic ones, as they would previously just return a null value in this scenario.
            Assert.That(
                () => api.GetxxxApiMethodxxx("$@!-"),
                Throws.InstanceOf<ApiException>()
                    .With.Message.Contains(expectedMsg));
        }
        */

        [Test]
        public void Invalid_Requested_Api_Throws()
        {
            Assert.That(() => _factory.Api<InvalidApi>(), Throws.TypeOf<InvalidOperationException>());
        }

        class InvalidApi : IApiAccessor
        {
            public IReadableConfiguration Configuration { get; set; }
            public string GetBasePath()
            {
                throw new NotImplementedException();
            }

            public ExceptionFactory ExceptionFactory { get; set; }
        }
    }
}
