using Autofac;
using BootStrapper;
using Transformalize.Contracts;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Attributes.Jobs;
using BenchmarkDotNet.Running;
using Transformalize.Logging;

namespace Benchmark {

    [LegacyJitX64Job]
    [SimpleJob(launchCount: 1, warmupCount: 1, targetCount: 1, invocationCount: 1, id: "QuickJob")]
    public class Benchmarks {

        [Benchmark(Baseline = true, Description = "1000 rows bogus=>lucene, ssd")]
        public void TestRows() {
            using (var outer = new ConfigurationContainer().CreateScope(@"files\bogus.xml?DriverLetter=c&Size=1000")) {
                using (var inner = new TestContainer().CreateScope(outer, new NullLogger())) {
                    var controller = inner.Resolve<IProcessController>();
                    controller.Execute();
                }
            }
        }

        [Benchmark(Baseline = false, Description = "1000 rows bogus=>lucene, hd 7200rpm")]

        public void CSharpRows() {
            using (var outer = new ConfigurationContainer().CreateScope(@"files\bogus.xml?DriverLetter=d&Size=1000")) {
                using (var inner = new TestContainer().CreateScope(outer, new NullLogger())) {
                    var controller = inner.Resolve<IProcessController>();
                    controller.Execute();
                }
            }
        }

    }

    public class Program {
        private static void Main(string[] args) {
            var summary = BenchmarkRunner.Run<Benchmarks>();
        }
    }
}
