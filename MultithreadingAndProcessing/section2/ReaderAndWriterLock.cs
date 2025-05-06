using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultithreadingAndProcessing.section2
{
    class ReaderAndWriterLock
    {
        private Dictionary<int, string> _cache = new Dictionary<int, string>();
        private ReaderWriterLockSlim _lock = new ReaderWriterLockSlim();

        //writers
        public void Add(int key, string value)
        {
            bool isLockAcquired = false;
            try
            {
                _lock.EnterWriteLock();
                isLockAcquired = true;
                _cache[key] = value;
            }
            finally
            {
                if (isLockAcquired)
                {
                    _lock.ExitWriteLock();
                }
            }
            
        }

        //readers
        public string? Get(int key)
        {
            bool isLockAcquired = false;
            try
            {
                _lock.EnterReadLock();
                isLockAcquired = true;
                return _cache.TryGetValue(key, out var value) ? value : null;
            }
            finally
            {
                if (!isLockAcquired)
                {
                    _lock.ExitReadLock();
                }
            }
            
        }
    }
}
