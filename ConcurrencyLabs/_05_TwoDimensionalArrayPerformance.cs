using System;
using System.Diagnostics;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ConcurrencyLabs
{
    [TestClass]
    public class _05_TwoDimensionalArrayPerformance
    {
        static int dimension = 10000;
        static int[,] matrix = new int[dimension, dimension];

        [TestMethod]
        public void __05__Traversing_a_TwoDimensionalArray_ShouldBeFast()
        {
            var sw = Stopwatch.StartNew();

            // CODE UNDER TEST -->

            for (int column = 0; column < dimension; column++)
            {
                for (int row = 0; row < dimension; row++)
                {
                    matrix[row, column] = row * column;
                }
            }

            // <-- CODE UNDER TEST

            TimeSpan elapsed = sw.Elapsed;

            // When this test is run for the first time, it will write a benchmark to disk
            // Subsequently, the test will only succeed if you can improve the code under test above
            // by a factor of at least 5

            Console.WriteLine(elapsed);
            string benchmark = "benchmark";
            //File.Delete(benchmark);
            if (!File.Exists(benchmark))
            {
                File.WriteAllText(benchmark, elapsed.Ticks.ToString());
                Assert.Fail("benchmark has been written; modify the code to improve performance x5");
            }
            else
            {
                var ticks = Convert.ToInt64(File.ReadAllText(benchmark));
                Console.WriteLine("benchmark: {0}", TimeSpan.FromTicks(ticks));
                Assert.IsTrue(elapsed.Ticks < ticks / 5);
            }
        }

    }
}
