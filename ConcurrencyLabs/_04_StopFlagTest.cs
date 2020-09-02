using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace ConcurrencyLabs
{

    public class _04_StopFlagTest
    {
        private static bool _stopping;

        [Fact]
        public void __04__WhenStopFlagIsSet_OtherThreadShouldStop()
        {
            _stopping = false;

            var work = Task.Run(() => DoWork());
            
            Task.Delay(TimeSpan.FromMilliseconds(1000)).Wait();

            _stopping = true;

            // give the DoWork method plenty of time to stop
            Task.Delay(TimeSpan.FromMilliseconds(500)).Wait();

            // why does this test fail? How to fix it?
            Assert.True(work.IsCompleted, "DoWork did not stop");
        }


        static void DoWork()
        {
            Console.WriteLine("DoWork enter; _stopping = {0}", _stopping);

            int i = 0;

            while (!_stopping)
            {
                i++;
            }

            Console.WriteLine("DoWork exit " + i);
        }

    }
}
