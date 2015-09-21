using System;
using System.Collections.Generic;
using System.Threading;

namespace Homework3 {
    internal class IsNumberPrimeCalculator {
        private readonly ICollection<long> _primeNumbers;
		private readonly BoundedBuffer<long> _numbersToCheck;
		private readonly Semaphore _resultMutex;
	

		public IsNumberPrimeCalculator(ICollection<long> primeNumbers, BoundedBuffer<long> numbersToCheck,Semaphore resultMutex) {
            _primeNumbers = primeNumbers;
            _numbersToCheck = numbersToCheck;
			_resultMutex = resultMutex;
        }

        public void CheckIfNumbersArePrime() {
            while (true) {
				var numberToCheck = _numbersToCheck.Consume ();
					
				if (IsNumberPrime (numberToCheck)) {
					_resultMutex.WaitOne ();
					_primeNumbers.Add (numberToCheck);
					_resultMutex.Release ();
				}
			
            }
        }

        private bool IsNumberPrime(long numberWeAreChecking) {
            const long firstNumberToCheck = 3;

			if (numberWeAreChecking == 1) return false;
			if (numberWeAreChecking == 2) return true;

            if (numberWeAreChecking % 2 == 0) {
                return false;
            }
            var lastNumberToCheck = Math.Sqrt(numberWeAreChecking);
            for (var currentDivisor = firstNumberToCheck; currentDivisor < lastNumberToCheck; currentDivisor += 2) {
                if (numberWeAreChecking % currentDivisor == 0) {
                    return false;
                }
            }
            return true;
        }
    }
}