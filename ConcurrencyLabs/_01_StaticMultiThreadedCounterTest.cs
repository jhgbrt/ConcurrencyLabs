using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Xunit;

namespace ConcurrencyLabs
{
    public class _01_StaticMultiThreadedCounterTest
    {
        private const int max = 10000000;
        private static int counter;

        [Fact]
        public void __01__Counter_WhenIncrementedSameAmountFromTwoThreads_ShouldResultInTwiceTheCount()
        {
            counter = 0;
            var sw = Stopwatch.StartNew();

            // start 2 threads, each incrementing the counter "max" times
            var t1 = Task.Run((Action) count);
            var t2 = Task.Run((Action) count);
            Task.WaitAll(t1, t2);
            
            // the expectation is that the counter ends up at a value of 2* max
            // why does this test fail? how can you correct it?
            Assert.Equal(2*max, counter);
        }

        static void count()
        {
            for (int i = 0; i < max; i++)
                counter++;
        }

    }
}
