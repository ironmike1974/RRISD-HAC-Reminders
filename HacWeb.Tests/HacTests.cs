using FakeItEasy;
using HacWeb.Lib;
using HacWeb.Models;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using Xunit;

namespace HacWeb.Tests
{
    public class HacTests
    {
        private IHAC _hacLib;

        public HacTests()
        {
            IOptions<HacInterfaceUris> uriOptions = A.Fake<IOptions<HacInterfaceUris>>();
            IOptions<Students> studentList = A.Fake<IOptions<Students>>();

            // Load the secret json to populate the tests
            var te = Environment.CurrentDirectory;
            JObject credJson = JObject.Parse(File.ReadAllText(@"..\..\..\..\HacWeb\appsettings.creds.json"));
            var students = JsonConvert.DeserializeObject<Students>(credJson["Students"].ToString());
            var urls = JsonConvert.DeserializeObject<HacInterfaceUris>(credJson["HacInterfaceUris"].ToString());

            A.CallTo(() => uriOptions.Value).Returns(urls);
            A.CallTo(() => studentList.Value).Returns(students);
            
            _hacLib = new HAC(uriOptions, studentList);
        }

        [Fact]
        public void GetStudentsTest()
        {
            var students = _hacLib.GetStudents();
            Assert.Equal(2, students.Count);
        }
    }
}
