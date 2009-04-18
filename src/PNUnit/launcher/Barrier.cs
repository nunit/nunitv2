// ****************************************************************
// This is free software licensed under the NUnit license. You may
// obtain a copy of the license at http://nunit.org.
// ****************************************************************

using System;
using System.Threading;

namespace PNUnit.Launcher
{
    public class Barrier
    {
        private int mCount;
        private int mMaxCount;
        private Object mLock = new Object();

        public Barrier(int maxCount)
        {
            mCount = 0;
            mMaxCount = maxCount;

        }

        public void Enter()
        {
            lock( mLock )
            {
                ++mCount;
                if( mCount >= mMaxCount )
                {                    
                    mCount = 0;
                    Monitor.PulseAll(mLock);
                }
                else
                    Monitor.Wait(mLock);
            }
        }

        public void Abandon()
        {
            lock( mLock )
            {
                --mMaxCount;
                if( mCount >= mMaxCount )
                {                    
                    mCount = 0;
                    Monitor.PulseAll(mLock);
                }
                
            }
        }

    }

}
