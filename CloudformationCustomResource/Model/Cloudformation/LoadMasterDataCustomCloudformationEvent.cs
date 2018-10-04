using System;
using System.Collections.Generic;
using System.Text;

namespace CloudformationCustomResource.Model.Cloudformation
{
    public class LoadMasterDataCustomCloudformationEvent : CloudFormationRequest
    {
        public ResourcePropertiesModel ResourceProperties { get; set; }

        public class ResourcePropertiesModel
        {
            public string TableName { get; set; }
        }
    }
}
