using System;
using System.Collections.Generic;
using System.Text;

namespace CloudformationCustomResource.HelperClasses
{
    public class Constants
    {
        public const string AWSProfileName = "Hackathon";
        public const string CloudformationSuccessCode = "SUCCESS";
        public const string CloudformationErrorCode = "FAILED";

        public const string CloudFormationDeleteRequestType = "Delete";
        public const string CloudFormationCreateRequestType = "Create";
        public const string CloudFormationUpdateRequestType = "Update";
    }
}
