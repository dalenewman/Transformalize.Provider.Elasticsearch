﻿#region license
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

namespace Test.Integration.Core {

   [TestClass]
   public class TestTags {

      // note: these credentials are specific to the container running on dale's computer
      private static string Version = "8.3.2";
      private static string User = "elastic";
      private static string Password = "Zgf4+hQ7+dmCnIUpk6Wr";
      private static string Fingerprint = "37:84:43:71:D1:57:AE:34:9D:FE:FB:2A:A5:E2:DC:65:7B:62:16:9C:8E:E3:58:DB:30:EF:CD:9E:06:85:7D:03";

      [TestMethod]
      public void Write() {
         string xml = $@"<add name='TestProcess' mode='init'>
  <parameters>
    <add name='Size' type='int' value='1000' />
  </parameters>
  <connections>
    <add name='input' provider='bogus' seed='1' />
    <add name='output' provider='elasticsearch' server='localhost' index='bogus-tags' shards='3' replicas='0' port='9200'  version='{Version}' useSsl='true' user='{User}' password='{Password}' certificate-fingerprint='{Fingerprint}' />
  </connections>
  <entities>
    <add name='Contact' size='@[Size]'>
      <fields>
        <add name='Identity' type='int' />
        <add name='ProductName' />
        <add name='Categories' length='255' max='3' delimiter=',' />
      </fields>
      <calculated-fields>
         <add name='CategoryArray' t='copy(Categories).replace( ,).split(,)' />
      </calculated-fields>
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

   }
}
