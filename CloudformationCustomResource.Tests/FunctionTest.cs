using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Amazon.Lambda;
using Amazon.Lambda.Core;
using Amazon.Lambda.TestUtilities;
using Amazon.Lambda.S3Events;
using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Util;
using CloudformationCustomResource;
using System.IO;
using Newtonsoft.Json;
using CloudformationCustomResource.Model.Cloudformation;
using CloudformationCustomResource.HelperClasses;

namespace CloudformationCustomResource.Tests
{
    public class FunctionTest
    {
        [Fact]
        public void TestCloudformationCustomEventLambdaFunction()
        {
                var json = File.ReadAllText(@"SampleTestData\CloudformationCustomResourceEvent1.json");
                var cloudformationCustomResourceEvent = JsonConvert.DeserializeObject<LoadMasterDataCustomCloudformationEvent>(json);
                var context = new TestLambdaContext();

                // Invoke the lambda function and confirm the content type was returned.
                StartupProgram lambdaObject = new StartupProgram(true);
                CloudFormationResponse response = lambdaObject.LoadMasterData(cloudformationCustomResourceEvent, context);

                Assert.Equal(Constants.CloudformationSuccessCode, response.Status);
        }
    }
}
