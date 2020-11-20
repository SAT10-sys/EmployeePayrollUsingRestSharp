using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Net;

namespace EmployeePayrollUsingRestSharp
{
    [TestClass]
    public class UnitTest1
    {
        RestClient client;
        [TestInitialize]
        public void SetUp()
        {
            client = new RestClient(" http://localhost:3000");
        }
        private IRestResponse GetEmployeeList()
        {
            IRestRequest request = new RestRequest("/employees", Method.GET);
            IRestResponse response = client.Execute(request);
            return response;
        }
        [TestMethod]
        public void TestMethod1()
        {
            //on calling should retrieve all elements from json server
            IRestResponse response = GetEmployeeList();
            List<Employee> employeeList = JsonConvert.DeserializeObject<List<Employee>>(response.Content);
            Assert.AreEqual(response.StatusCode, System.Net.HttpStatusCode.OK);
            Assert.AreEqual(7, employeeList.Count);
            foreach(var i in employeeList)
                Console.WriteLine("ID: "+i.id+"\nNAME: "+i.Name+"\nSALARY: "+i.salary);
        }
        [TestMethod]
        public void TestMethod2()
        {
            //returns added employee
            RestRequest request = new RestRequest("/employees", Method.POST);
            JObject object= new JObject();
            object.Add("name", "Axl Rose");      
            object.Add("salary", 90000);
            request.AddParameter("application/json", object, ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            Employee employeeList = JsonConvert.DeserializeObject<Employee>(response.Content);
            Assert.AreEqual(response.StatusCode, System.Net.HttpStatusCode.Created);
            Assert.AreEqual("Axl Rose", employeeList.Name);
            Assert.AreEqual(90000, employeeList.salary);
        }
    }
}
