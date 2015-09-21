using System;
using System.Collections.Generic;
using System.Threading;

namespace Homework3 {
    internal class ProgressMonitor {
        private readonly List<long> _results;
        public long TotalCount = 0;
		private readonly Semaphore _mutex;

		public ProgressMonitor(List<long> results,Semaphore mutex) {
            _results = results;
			_mutex = mutex;
        }

        public void Run() {
            while (true) {
                Thread.Sleep(500); // wait for 1/10th of a second
				long currentcount;
				//lock
				_mutex.WaitOne ();
					currentcount = _results.Count;
					TotalCount += currentcount;
					_results.Clear (); // clear out the current primes to save some memory
				_mutex.Release();

                if (currentcount > 0) {
                    Console.WriteLine("{0} primes found so far", TotalCount);
                }
            }
        }
    }
}