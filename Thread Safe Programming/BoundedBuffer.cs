using System;
using System.Collections.Generic;
using System.Threading;

namespace Homework3
{
	public class BoundedBuffer<T>
	{
		private Queue<T> queue = new Queue<T>();
		private SpinLock _spinLock = new SpinLock ();
		private Semaphore _full;
		private Semaphore _empty;

		public BoundedBuffer(int bufferSize){
			_full = new Semaphore(0,bufferSize);
			_empty = new Semaphore(bufferSize, bufferSize);
		}


		public void Produce(T value) {
			bool lockTaken = false;
			_empty.WaitOne ();
			try{
				_spinLock.Enter (ref lockTaken);
				queue.Enqueue (value);
			}
			finally{
				if (lockTaken)
					_spinLock.Exit ();
			}
			_full.Release ();
		}

		public T Consume() {
			T value;
			bool lockTaken = false;
			_full.WaitOne ();
			try{
				_spinLock.Enter(ref lockTaken);
				value = queue.Dequeue ();
			}
			finally{
				if (lockTaken)
					_spinLock.Exit ();
			}
			_empty.Release ();
			return value;
		}

		public int Count(){
			return queue.Count;
		}
	}
}

