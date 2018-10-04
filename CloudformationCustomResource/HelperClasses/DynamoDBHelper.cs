using System;
using System.Collections.Generic;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;
using Amazon.Runtime;
using Amazon.Lambda.Core;
using CloudformationCustomResource.Model.DynamoDB;
using Amazon.Runtime.CredentialManagement;

namespace CloudformationCustomResource.HelperClasses
{
    class DynamoDBHelper
    {
        private ILambdaContext context;
        private bool isLocalDebug;

        private static AmazonDynamoDBClient client;

        public DynamoDBHelper(ILambdaContext mContext, bool isLocalDebug)
        {
            try
            {
                this.context = mContext;
                this.isLocalDebug = isLocalDebug;
                if (isLocalDebug)
                {
                    var chain = new CredentialProfileStoreChain();
                    AWSCredentials awsCredentials;
                    if (chain.TryGetAWSCredentials(Constants.AWSProfileName, out awsCredentials))
                    {
                        // use awsCredentials
                        client = new AmazonDynamoDBClient(awsCredentials, Amazon.RegionEndpoint.USEast1);
                    }
                }
                else
                {
                    client = new AmazonDynamoDBClient();
                }
            }
            catch (Exception ex)
            {
                context.Logger.LogLine("DynamoDBHelper  => " + ex.StackTrace);
            }
        }
        public void putItemTable1(DynamoDBMasterItem1 masterItem, string TableName)
        {
            try
            {
                context.Logger.LogLine("DynamoDBHelper::putItemTable1()=> TableName = " + TableName);
                context.Logger.LogLine("DynamoDBHelper::putItemTable1()=> PKID =  " + masterItem.EmployeeID);

                if (getItem(masterItem.UniqueID,TableName)== null) // this item does not exist
                {                    
                    Table table = Table.LoadTable(client, TableName);

                    var clientItem = new Document();
                    clientItem["UniqueID"] = masterItem.UniqueID;
                    clientItem["EmployeeID"] = masterItem.EmployeeID;
                    clientItem["Name"] = masterItem.Name;
                    clientItem["Employee"] = masterItem.Designation;
                    clientItem["Age"] = masterItem.Age;
                    clientItem["Department"] = masterItem.Department;

                    table.PutItemAsync(clientItem).GetAwaiter().GetResult();
                    context.Logger.LogLine("DynamoDBHelper::PutItem() -- PutOperation succeeded");
                }
                else
                {
                    context.Logger.LogLine("DynamoDBHelper::putItemTable1()=> UniqueID = " + TableName);
                }
            }
            catch (Exception ex)
            {
                context.Logger.LogLine("DynamoDBHelper::PutItem() -- " + ex.Message);
                context.Logger.LogLine("DynamoDBHelper::PutItem() -- " + ex.StackTrace);
            }
        }

        public Document getItem(string PrimaryKeyID, string TableName)
        {
            context.Logger.LogLine("DynamoDBHelper::GetItem()=> TableName = " + TableName);
            context.Logger.LogLine("DynamoDBHelper::GetItem()=> PKID =  " + PrimaryKeyID);
            Document clientItem = null;
            try
            {
                Table table = Table.LoadTable(client, TableName);

                // Configuration object that specifies optional parameters.
                GetItemOperationConfig config = new GetItemOperationConfig()
                {
                    AttributesToGet = new List<string>() { "Name" }
                };
                // Pass in the configuration to the GetItem method.
                // 1. Table that has only a partition key as primary key.
                clientItem = table.GetItemAsync(PrimaryKeyID, config).GetAwaiter().GetResult();
                context.Logger.LogLine("DynamoDBHelper::GetItem() -- GetOperation succeeded");
            }
            catch (Exception ex)
            {
                context.Logger.LogLine("DynamoDBHelper::GetItem() -- " + ex.StackTrace);
            }
            return clientItem;
        }

        public void DeleteItem(string PrimaryKeyID,string TableName)
        {
            try
            {
                context.Logger.LogLine("DynamoDBHelper::DeleteItem()=> TableName = " + TableName);
                context.Logger.LogLine("DynamoDBHelper::DeleteItem()=> PKID =  " + PrimaryKeyID);

                Table table = Table.LoadTable(client, TableName);

                Dictionary<string, AttributeValue> key = 
                                new Dictionary<string, AttributeValue>
                                {
                                    { "PrimaryKeyID1", new AttributeValue { S = PrimaryKeyID } }
                                };

                // Create DeleteItem request
                DeleteItemRequest request = new DeleteItemRequest
                {
                    TableName = TableName,
                    Key = key
                };

                // Issue request
                client.DeleteItemAsync(request).GetAwaiter().GetResult();
                context.Logger.LogLine("DynamoDBHelper::DeleteItem() -- Delete Operation succeeded");
            }
            catch (Exception ex)
            {
                context.Logger.LogLine("DynamoDBHelper::DeleteItem() -- " + ex.Message);
                context.Logger.LogLine("DynamoDBHelper::DeleteItem() -- " + ex.StackTrace);
            }
        }
    }
}


