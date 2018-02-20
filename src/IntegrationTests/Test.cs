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

using System.Linq;
using Autofac;
using BootStrapper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Transformalize.Configuration;
using Transformalize.Contracts;
using Transformalize.Providers.Console;

namespace IntegrationTests {

    [TestClass]
    public class Test {

        [TestMethod]
        public void Write() {
            const string xml = @"<add name='TestProcess' mode='init'>
  <parameters>
    <add name='Size' type='int' value='1000' />
    <add name='DriveLetter' type='char' value='c' />
  </parameters>
  <connections>
    <add name='input' provider='bogus' seed='1' />
    <add name='output' provider='lucene' folder='@[DriveLetter]:\temp\bogus-lucene-index' />
  </connections>
  <entities>
    <add name='Contact' size='@[Size]'>
      <fields>
        <add name='FirstName' />
        <add name='LastName' />
        <add name='Stars' type='byte' min='1' max='5' />
        <add name='Reviewers' type='int' min='0' max='500' />
      </fields>
    </add>
  </entities>
</add>";
            using (var outer = new ConfigurationContainer().CreateScope(xml)) {
                using (var inner = new TestContainer().CreateScope(outer, new ConsoleLogger(LogLevel.Debug))) {

                    var process = inner.Resolve<Process>();

                    var controller = inner.Resolve<IProcessController>();
                    controller.Execute();

                    Assert.AreEqual(process.Entities.First().Inserts, (uint)1000);


                }
            }
        }

        [TestMethod]
        public void Read() {
            const string xml = @"<add name='TestProcess'>
  <parameters>
    <add name='DriveLetter' type='char' value='c' />
  </parameters>
  <connections>
    <add name='input' provider='lucene' folder='@[DriveLetter]:\temp\bogus-lucene-index' />
    <add name='output' provider='internal' />
  </connections>
  <entities>
    <add name='Contact'>
      <fields>
        <add name='FirstName' />
        <add name='LastName' />
        <add name='Stars' type='byte' />
        <add name='Reviewers' type='int' />
      </fields>
    </add>
  </entities>
</add>";
            using (var outer = new ConfigurationContainer().CreateScope(xml)) {
                using (var inner = new TestContainer().CreateScope(outer, new ConsoleLogger(LogLevel.Debug))) {

                    var process = inner.Resolve<Process>();

                    var controller = inner.Resolve<IProcessController>();
                    controller.Execute();
                    var rows = process.Entities.First().Rows;

                    Assert.AreEqual(1000, rows.Count);


                }
            }
        }
    }
}
