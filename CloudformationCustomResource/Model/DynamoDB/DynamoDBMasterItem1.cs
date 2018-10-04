using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace CloudformationCustomResource.Model.DynamoDB
{
    public class DynamoDBMasterItem1
    {
        public string Name { get; set; }
        public string Employee { get; set; }
        public string Age { get; set; }
        public string Department { get; set; }
        public string EmployeeID { get; set; }

        public DynamoDBMasterItem1 (string EmployeeID, string Name , string Employee ,string Age , string Department)
        {
            this.EmployeeID = EmployeeID;
            this.Name = Name;
            this.Employee = Employee;
            this.Age = Age;
            this.Department = Department;
        }
    }
}
