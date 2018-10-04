using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace CloudformationCustomResource.Model.DynamoDB
{
    public class DynamoDBMasterItem1
    {
        public string Name { get; set; }
        public string Designation { get; set; }
        public string Age { get; set; }
        public string Department { get; set; }
        public string EmployeeID { get; set; }
        public string UniqueID { get; set; }

        public DynamoDBMasterItem1 (string UniqueID ,string EmployeeID, string Name , string Employee ,
                                    string Age , string Department)
        {
            this.UniqueID = UniqueID;
            this.EmployeeID = EmployeeID;
            this.Name = Name;
            this.Designation = Employee;
            this.Age = Age;
            this.Department = Department;
        }
    }
}
