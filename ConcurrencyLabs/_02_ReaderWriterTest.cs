using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ConcurrencyLabs
{
    [TestClass]
    public class _02_ReaderWriterTest
    {
        private static long s_x;
        private static readonly long _max = long.MaxValue;

        [TestMethod]
        public void __02__Value_WhenWrittenFromOneThread_And_ReadFromAnotherThread_ShouldAlwaysBeTheSame()
        {
            s_x = 0;

            var cts = new CancellationTokenSource();

            // CODE UNDER TEST -->

            var taskA = Task.Run(() => ThreadA(), cts.Token);
            var taskB = Task.Run(() => ThreadB(), cts.Token);

            // <-- CODE UNDER TEST

            // if the test doesn't fail unmodified, try increasing wait time
            Task.Delay(TimeSpan.FromSeconds(1)).Wait();

            if (taskA.IsFaulted)
                Assert.Fail(taskA.Exception.Flatten().InnerExceptions.First().Message);
            if (taskB.IsFaulted)
                Assert.Fail(taskB.Exception.Flatten().InnerExceptions.First().Message);

            // cancel any tasks still running
            cts.Cancel();
        }

        static void ThreadA()
        {
            int i = 0;
            while (true)
            {
                s_x = (i & 1) == 0 ? 0x0L : _max;

                i++;
            }
        }
        static void ThreadB()
        {
            while (true)
            {
                long x = s_x;
                if (x != 0x0L && x != _max)
                {
                    throw new Exception(string.Format("!! x = {0} ", Convert.ToString(x, 2).PadLeft(64, '0')));
                }
            }
        }


    }
}
