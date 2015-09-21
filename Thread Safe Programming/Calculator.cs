using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Homework3 {
    internal class Calculator {
		public static int bufferSize = (int)(Constants.UpperBound - Constants.LowerBound);
		private static readonly Semaphore resultMutex = new Semaphore (1,1);

        public void Run(NumberReader reader) {
            var results = new List<long>();
			var numbersToCheck = new BoundedBuffer<long>(bufferSize);

            StartComputationThreads(results, numbersToCheck);

			var progressMonitor = new ProgressMonitor(results,resultMutex);

            new Thread(progressMonitor.Run) {IsBackground = true}.Start();
            
            foreach (var value in reader.ReadIntegers()) {
				numbersToCheck.Produce (value);
            }
            
			while (numbersToCheck.Count() > 0) {
                Thread.Sleep(100); // wait for the computation to complete.
            }
            Console.WriteLine("{0} of the numbers were prime", progressMonitor.TotalCount);
        }

		private static void StartComputationThreads(List<long> results, BoundedBuffer<long> numbersToCheck) {
            var threads = CreateThreads(results, numbersToCheck);
            threads.ForEach(thread => thread.Start());
        }
        
		private static List<Thread> CreateThreads(List<long> results, BoundedBuffer<long> numbersToCheck) {
			var threadCount = Environment.ProcessorCount*2;

            Console.WriteLine("Using {0} compute threads and 1 I/O thread", threadCount);

            var threads =
                (from threadNumber in Sequence.Create(0, threadCount)
                    let calculator = new IsNumberPrimeCalculator(results, numbersToCheck,resultMutex)
                    let newThread =
                        new Thread(calculator.CheckIfNumbersArePrime) {
                            IsBackground = true,
                            Priority = ThreadPriority.BelowNormal
                        }
                    select newThread).ToList();
            return threads;
        }
    }
}