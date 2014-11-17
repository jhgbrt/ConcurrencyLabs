using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

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


    [TestClass]
    public class _03_LazyExampleTests
    {
        [TestMethod]
        public void __03__GetInt_WhenCalledFromMultipleThreads_ShouldAlwaysReturnSameValue()
        {
            for (int i = 0; i < 1000000; i++)
                DoTest();
        }

        private static void DoTest()
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
                var message = string.Format("FAILED: {0}", string.Join(",", results));
                throw new Exception(message);
            }
        }

    }
}
