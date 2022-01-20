using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SOM.IO;
using SOM.Procedures;
using SOM.Extentions;
using Newtonsoft.Json;
using System.Text.RegularExpressions;
using SOM.Parsers;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;
namespace CoreTests
{ 
    [TestClass]
    public class YamlTests
    { 
        [TestMethod]
        public void Yaml_Serializes()
        { 
            var yml = @"
  - task:  
    name: unittest
    context: bomdriver
    taskSteps:   
    - cmd: OpenTab
      args:  ['http://automationpractice.com/index.php?controller=authentication&back=identity']  
    - cmd: Key
      args:  ['email_create', 'TestyMcTestFace@Domain.com']  
    - cmd: Click
      args:  ['SubmitCreate']  
    - cmd: Pause
      args: [  
            document.title = 'Docs';
            window.scrollTo(0, 1000);
      ] 
  - task:  
    name: foo
    context: bomdriver
            "; 
            var deserializer = new DeserializerBuilder()
                .WithNamingConvention(CamelCaseNamingConvention.Instance)   
                .Build(); 
            //yml contains a string containing your YAML
            var t = deserializer.Deserialize<List<Task>>(yml);
            var steps = t[0].taskSteps;
        }
        [TestMethod]
        public void Yaml_DeSerializes()
        {
        
            Task task = new Task();
            task.Context = "context";
            task.Name = "name";
            task.taskSteps.Add( new TaskStep("a", new string[] { "1", "2" })  );
            task.taskSteps.Add( new TaskStep("b", new string[] { "11", "22" })  );
            task.taskSteps.Add( new TaskStep("c", new string[] { "111", "222" })  );
            var serializer = new SerializerBuilder()
                .WithNamingConvention(CamelCaseNamingConvention.Instance)
                .Build();
            var yaml = serializer.Serialize(task);
            System.Console.WriteLine(yaml);
        }
    }
    [Serializable]
    public class Task 
    {
        public Task()
        {
            taskSteps = new List<TaskStep>();
        }
        public string Name { get; set; }
        public string Context { get; set; } 
        public List<TaskStep> taskSteps { get; set; }
    }
    [Serializable]
    public class TaskStep
    {
        public TaskStep()
        { 
        }
        public TaskStep(string Cmd, string[] Args)
        {
            this.Cmd = Cmd;
            this.Args = Args;
        }
        public string Cmd { get; set; }
        public string[] Args { get; set; }
    }
}
