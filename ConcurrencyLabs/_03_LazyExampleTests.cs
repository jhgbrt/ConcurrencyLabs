using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace ConcurrencyLabs
{
    class LazyExample
    {
        private int _value;
        private bool _initialized;
        public int GetInt()
        {
            // extra reads to get _value
            if (_value < 0) throw new Exception(); 
            // pre-loaded into a register
            if (!_initialized)      // Read 1
            {
                _value = 42;
                _initialized = true;
                return _value;
            }
            return _value;          // Read 2
        }
    }


    public class _03_LazyExampleTests
    {
        [Fact]
        public void __03__GetInt_WhenCalledFromMultipleThreads_ShouldAlwaysReturnSameValue()
        {
            const int max = 100000; // if this test does not systematically fail, increase this value with an order of magnitude
            for (int i = 0; i < max; i++)
                DoTest(i);
        }

        private static void DoTest(int n)
        {
            var lazy = new LazyExample();

            var results = new ConcurrentBag<int>();
            
            // start as many threads as there are CPU cores
            // each thread adds the result of the lazily initialized integer 
            // in the results bag
            var tasks = new Task[Environment.ProcessorCount];
            for (int i = 0; i < tasks.Length; i++)
            {
                tasks[i] = Task.Run(() => results.Add(lazy.GetInt()));
            }
            Task.WaitAll(tasks);

            // The expectation is that the results collection can only contain the number
            // 42. Why is it not the case?
            // How can you fix the LazyExample class?
            // How SHOULD you fix it?
            if (results.Any(i => i != 42))
            {
                var message = string.Format($"FAILED after {n} times: {0}", string.Join(",", results));
                throw new Exception(message);
            }
        }

    }
}
