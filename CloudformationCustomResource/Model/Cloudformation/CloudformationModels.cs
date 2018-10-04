using System;
using System.Collections.Generic;
using System.Text;
using Amazon.Lambda.Core;
using Newtonsoft.Json;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using CloudformationCustomResource.HelperClasses;

namespace CloudformationCustomResource.Model.Cloudformation
{
    public class CloudFormationRequest
    {
        public string StackId { get; set; }
        public string ResponseURL { get; set; }
        public string RequestType { get; set; }
        public string ResourceType { get; set; }
        public string RequestId { get; set; }
        public string LogicalResourceId { get; set; }
    }

    public class CloudFormationResponse
    {
        public string Status { get; set; }
        public string Reason { get; set; }
        public string PhysicalResourceId { get; set; }
        public string StackId { get; set; }
        public string RequestId { get; set; }
        public string LogicalResourceId { get; set; }
        public object Data { get; set; }

        public CloudFormationResponse(string Status , string Reason , string PhysicalResourceId , string StackId , string RequestId,
                                      string LogicalResourceId , object Data)
        {
            this.Status = Status;
            this.Reason = Reason;
            this.PhysicalResourceId = PhysicalResourceId;
            this.StackId = StackId;
            this.RequestId = RequestId;
            this.LogicalResourceId = LogicalResourceId;
            this.Data = Data;
        }

        public async Task<CloudFormationResponse> CompleteCloudFormationResponse
                                                        (
                                                         CloudFormationRequest request,
                                                         ILambdaContext context
                                                         )
        {
            try
            {
                HttpClient client = new HttpClient();
                var jsonContent = new StringContent(JsonConvert.SerializeObject(this));
                jsonContent.Headers.Remove("Content-Type");

                var postResponse = await client.PutAsync(request.ResponseURL, jsonContent);

                postResponse.EnsureSuccessStatusCode();
            }
            catch (Exception ex)
            {
                context.Logger.LogLine($"CloudformationModels::CompleteCloudFormationResponse Exception:{ex.Message}");

                this.Status = Constants.CloudformationErrorCode;
                this.Data = ex;
            }

            return this;
        }
    }
}