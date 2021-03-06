using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Amazon.Lambda.Core;
using Amazon.Lambda.S3Events;
using Amazon.S3;
using Amazon.S3.Util;
using CloudformationCustomResource.HelperClasses;
using CloudformationCustomResource.Model.Cloudformation;
using CloudformationCustomResource.Model.DynamoDB;
using Newtonsoft.Json;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace CloudformationCustomResource
{
    public class StartupProgram
    {
        private bool isLocalDebug { get; set; }

        /// <summary>
        /// Default constructor. This constructor is used by Lambda to construct the instance. When invoked in a Lambda environment
        /// the AWS credentials will come from the IAM role associated with the function and the AWS region will be set to the
        /// region the Lambda function is executed in.
        /// </summary>
        public StartupProgram()
        {
            this.isLocalDebug = false;
        }

        public StartupProgram(bool isLocalDebug)
        {
            this.isLocalDebug = isLocalDebug;
        }

        public CloudFormationResponse LoadMasterData(LoadMasterDataCustomCloudformationEvent request, ILambdaContext context)
        {
            try
            {
                string UniqueIdGenerated = SecurityHelper.GetSha256Hash($"{request.StackId}:{request.LogicalResourceId}");
                DynamoDBMasterItem1 item1 = new DynamoDBMasterItem1(
                                                    UniqueIdGenerated,
                                                    Convert.ToString(Guid.NewGuid()),
                                                    "Rohit Srivastava",
                                                    "Senior Consultant",
                                                    "29",
                                                    "Advisory");

                context.Logger.LogLine($"Input event invoked: {JsonConvert.SerializeObject(request)}");
                DynamoDBHelper dynamoDBHelper = new DynamoDBHelper(context, this.isLocalDebug);

                context.Logger.LogLine($"Custom cloudformation event request type: {request.RequestType}");
                //Since this is not basically creation of any custom resource , rather initial data load , we donot need
                //to care about anything other than the CREATE request type
                if (string.Equals(request.RequestType, Constants.CloudFormationCreateRequestType))
                {
                    dynamoDBHelper.putItemTable1(item1, request.ResourceProperties.TableName);

                    //Success - data inserted properly in the dynamoDB
                    CloudFormationResponse objResponse =
                            new CloudFormationResponse(
                                                Constants.CloudformationSuccessCode,
                                                "Custom Resource Creation Successful",
                                                $"{request.StackId}-{request.LogicalResourceId}-DataLoad",
                                                request.StackId,
                                                request.RequestId,
                                                request.LogicalResourceId,
                                                item1
                                    );

                    return objResponse.CompleteCloudFormationResponse(request, context).GetAwaiter().GetResult();
                }
                else
                {
                    CloudFormationResponse objResponse =
                        new CloudFormationResponse(
                                            Constants.CloudformationSuccessCode,
                                            "Do nothing.Data will be pushed in only when stack event is Create",
                                            context.LogStreamName,
                                            request.StackId,
                                            request.RequestId,
                                            request.LogicalResourceId,
                                            null
                                );
                    return objResponse.CompleteCloudFormationResponse(request, context).GetAwaiter().GetResult();
                }
            }
            catch (Exception ex)
            {
                context.Logger.LogLine($"StartupProgram::LoadMasterData => {ex.Message}");
                context.Logger.LogLine($"StartupProgram::LoadMasterData => {ex.StackTrace}");

                //Error - log it into the cloudformation console
                CloudFormationResponse objResponse =
                        new CloudFormationResponse(
                                            Constants.CloudformationErrorCode,
                                            ex.Message,
                                            context.LogStreamName,
                                            request.StackId,
                                            request.RequestId,
                                            request.LogicalResourceId,
                                            null
                                );

                return objResponse.CompleteCloudFormationResponse(request, context).GetAwaiter().GetResult();
            }
        }
    }
}
