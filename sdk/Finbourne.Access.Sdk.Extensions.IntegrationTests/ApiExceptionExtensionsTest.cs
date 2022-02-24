using Finbourne.Access.Sdk.Api;
using Finbourne.Access.Sdk.Client;
using Finbourne.Access.Sdk.Model;
using Newtonsoft.Json;
using NUnit.Framework;
using System.Linq;
using System.Net;

namespace Finbourne.Access.Sdk.Extensions.IntegrationTests
{
    [TestFixture]
    public class ApiExceptionExtensionsTest
    {
        private IApiFactory _factory;

        [OneTimeSetUp]
        public void SetUp()
        {
            _factory = IntegrationTestApiFactoryBuilder.CreateApiFactory("secrets.json");
        }
        
        [Test]
        public void Generate_HttpStatusCode_BadRequest()
        {
            try
            {
                //TODO: Test with a valid API Method
                //_factory.Api<Api.xxxApi>().Methodxxx("$@!-");
            }
            catch (ApiException e)
            {
                Assert.AreEqual((int)HttpStatusCode.BadRequest, e.ErrorCode);
            }
        }

        [Test]
        public void GetRequestId_MalformedInsightsUrl_ReturnsNull()
        {
            try
            {
                //TODO: Test with a valid API Method
                //_factory.Api<Api.xxxApi>().Methodxxx("$@!-");
            }
            catch (ApiException e)
            {
                var problemDetails = e.ProblemDetails();

                // Remove the InsightsURL which contains the requestId
                problemDetails.Instance = "";

                var apiExceptionMalformed = new ApiException(
                    errorCode: e.ErrorCode,
                    message: e.Message,
                    errorContent: JsonConvert.SerializeObject(problemDetails));

                var requestId = apiExceptionMalformed.GetRequestId();
                Assert.That(requestId, Is.Null);
            }
        }

        [Test]
        public void ProblemDetails_Converts_To_ProblemDetails_Detail()
        {
            try
            {
                //TODO: Test with a valid API Method
                //_factory.Api<Api.xxxApi>().Methodxxx("$@!-");
            }
            catch (ApiException e)
            {
                //    ApiException.ErrorContent contains a JSON serialized ErrorResponse
                LusidProblemDetails errorResponse = e.ProblemDetails();
                Assert.That(errorResponse.Detail, Does.Match("One or more elements of the request were invalid. Please check that all supplied identifiers are valid and of the correct format, and that all provided data is correctly structured."));
            }
        }

        [Test]
        public void ProblemDetails_Converts_To_ProblemDetails_Name()
        {
            try
            {
                //TODO: Test with a valid API Method
                //_factory.Api<Api.xxxApi>().Methodxxx("no-scope", "no-code", someObject);
            }
            catch (ApiException e)
            {
                //    ApiException.ErrorContent contains a JSON serialized ErrorResponse
                LusidProblemDetails errorResponse = e.ProblemDetails();
                Assert.That(errorResponse.Name, Is.EqualTo("SubscriptionNotFound"));
            }
        }

        [Test]
        public void ApiException_Converts_To_ValidationProblemDetails_AllowedRegex()
        {
            try
            {
                //TODO: Test with a valid API Method
                //_factory.Api<Api.xxxApi>().Methodxxx("@£$@£%", "#####", someObject);
            }
            catch (ApiException e)
            {
                //Returns a 404 Not Found error
                Assert.That(e.ErrorCode, Is.EqualTo((int)HttpStatusCode.BadRequest), "Expect BadRequest error code");
                Assert.That(e.IsValidationProblem, Is.True, "Response should indicate that there was a validation error with the request. ");

                //    An ApiException.ErrorContent thrown because of a request validation contains a JSON serialized LusidValidationProblemDetails
                if (e.TryGetValidationProblemDetails(out var errorResponse))
                {
                    //Should identify that there was a validation error with the code
                    Assert.That(errorResponse.Errors, Contains.Key("code"));
                    Assert.That(errorResponse.Errors["code"].Single(), Is.EqualTo("Values for the field code must be comprised of either alphanumeric characters, hyphens or underscores. For more information please consult the documentation."));

                    //Should identify that there was a validation error with the scope
                    Assert.That(errorResponse.Errors, Contains.Key("scope"));
                    Assert.That(errorResponse.Errors["scope"].Single(), Is.EqualTo("Values for the field scope must be comprised of either alphanumeric characters, hyphens or underscores. For more information please consult the documentation."));

                    Assert.That(errorResponse.Detail, Does.Match("One or more elements of the request were invalid.*"));
                    Assert.That(errorResponse.Name, Is.EqualTo("InvalidRequestFailure"));
                }
                else
                {
                    Assert.Fail("The request should have failed due to a validation error, and the validation details should be returned");
                }
            }
        }

        [Test]
        public void ApiException_Converts_To_ValidationProblemDetails_MaxLength()
        {
            try
            {
                var testScope = new string('a', 100);
                var testCode = new string('b', 100);
                //TODO: Test with a valid API Method
                //_factory.Api<Api.xxxApi>().Methodxxx(testScope, testCode, someObject);
            }
            catch (ApiException e)
            {
                Assert.That(e.IsValidationProblem, Is.True, "Response should indicate that there was a validation error with the request");

                //    An ApiException.ErrorContent thrown because of a request validation contains a JSON serialized LusidValidationProblemDetails
                if (e.TryGetValidationProblemDetails(out var errorResponse))
                {
                    //Should identify that there was a validation error with the code
                    Assert.That(errorResponse.Errors, Contains.Key("code"));
                    Assert.That(errorResponse.Errors["code"].Single(), Is.EqualTo("Values for the field code must be non-zero in length and have no more than 64 characters. For more information please consult the documentation."));

                    //Should identify that there was a validation error with the scope
                    Assert.That(errorResponse.Errors, Contains.Key("scope"));
                    Assert.That(errorResponse.Errors["scope"].Single(), Is.EqualTo("Values for the field scope must be non-zero in length and have no more than 64 characters. For more information please consult the documentation."));

                    Assert.That(errorResponse.Detail, Does.Match("One or more elements of the request were invalid.*"));
                    Assert.That(errorResponse.Name, Is.EqualTo("InvalidRequestFailure"));
                }
                else
                {
                    Assert.Fail("The request should have failed due to a validation error, and the validation details should be returned");
                }
            }
        }        
    }
}
