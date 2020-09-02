using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace ConcurrencyLabs
{
    public class _02_ReaderWriterTest
    {
        private static long s_x;
        private static readonly long _max = long.MaxValue;

        [Fact]
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
                throw taskA.Exception.Flatten().InnerExceptions.First();
            if (taskB.IsFaulted)
                throw taskB.Exception.Flatten().InnerExceptions.First();

            // cancel any tasks still running
            cts.Cancel();
        }

        static void ThreadA()
        {
            int i = 0;
            while (true)
            {
                var r = (i & 1) == 0 ? 0x0L : _max;
                Interlocked.Exchange(ref s_x, r);

                i++;
            }
        }
        static void ThreadB()
        {
            while (true)
            {
                long x = Interlocked.Read(ref s_x);
                if (x != 0x0L && x != _max)
                {
                    throw new Exception(string.Format("!! x = {0} ", Convert.ToString(x, 2).PadLeft(64, '0')));
                }
            }
        }


    }
}
