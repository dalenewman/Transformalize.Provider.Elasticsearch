#region license
// Transformalize
// Configurable Extract, Transform, and Load
// Copyright 2013-2017 Dale Newman
//  
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//   
//       http://www.apache.org/licenses/LICENSE-2.0
//   
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
#endregion

using Autofac;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using Transformalize.Configuration;
using Transformalize.Containers.Autofac;
using Transformalize.Contracts;
using Transformalize.Providers.Bogus.Autofac;
using Transformalize.Providers.Console;
using Transformalize.Providers.Elasticsearch.Autofac;

namespace IntegrationTests {

   [TestClass]
   public class Test {

      [TestMethod]
      public void Write() {
         const string xml = @"<add name='TestProcess' mode='init'>
  <parameters>
    <add name='Size' type='int' value='1000' />
  </parameters>
  <connections>
    <add name='input' provider='bogus' seed='1' />
    <add name='output' provider='elasticsearch' server='1514lt.scope-services.local' index='bogus' shards='3' replicas='0' port='9201' version='7.0.0' />
  </connections>
  <entities>
    <add name='Contact' size='@[Size]'>
      <fields>
        <add name='Identity' type='int' />
        <add name='FirstName' />
        <add name='LastName' />
        <add name='Stars' type='byte' min='1' max='5' />
        <add name='Reviewers' type='int' min='0' max='500' />
      </fields>
    </add>
  </entities>
</add>";
         var logger = new ConsoleLogger(LogLevel.Debug);

         using (var x = new ConfigurationContainer().CreateScope(xml, logger)) {
            var process = x.Resolve<Process>();
            using (var y = new Container(new BogusModule(), new ElasticsearchModule()).CreateScope(process, logger)) {
               y.Resolve<IProcessController>().Execute();
               Assert.AreEqual(process.Entities.First().Inserts, (uint)1000);
            }
         }
      }

      [TestMethod]
      public void Read() {
         const string xml = @"<add name='TestProcess'>
  <connections>
    <add name='input' provider='elasticsearch' server='1514lt.scope-services.local' index='bogus' port='9201' version='7.0.0' />
    <add name='output' provider='internal' />
  </connections>
  <entities>
    <add name='contact'>
      <fields>
        <add name='firstname' />
        <add name='lastname' />
        <add name='stars' type='byte' />
        <add name='reviewers' type='int' />
      </fields>
    </add>
  </entities>
</add>";
         var logger = new ConsoleLogger(LogLevel.Debug);
         using (var outer = new ConfigurationContainer().CreateScope(xml, logger)) {
            var process = outer.Resolve<Process>();
            using (var inner = new Container(new ElasticsearchModule()).CreateScope(process, logger)) {

               var controller = inner.Resolve<IProcessController>();
               controller.Execute();
               var rows = process.Entities.First().Rows;

               Assert.AreEqual(1000, rows.Count);
            }
         }
      }
   }
}
