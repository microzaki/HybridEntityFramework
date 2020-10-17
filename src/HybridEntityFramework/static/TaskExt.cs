using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HybridEntityFramework
{
     public static class TaskExt
    {
        public static void WaitThrowInnerException(this Task t)
        {
            try
            {
                t.Wait();
            }catch (AggregateException ex)
            {
                throw ex.InnerException;
            }
        }
        public static void WaitThrowInnerException(this Task t,Task t2)
        {
            Exception exception = null;
           Task w1=  Task.Factory.StartNew(() => {
                try
                {
                    t.Wait();
                }
                catch (AggregateException ex)
                {
                   exception= ex.InnerException;
                }
            });
            Task w2 = Task.Factory.StartNew(() => {
                try
                {
                    t2.Wait();
                }
                catch (AggregateException ex)
                {
                    exception = ex.InnerException;
                }
            });
            while (true)
            {
                if (exception != null)
                    throw exception;
                if (w1.IsCompleted && w2.IsCompleted)
                    break;
                Thread.Sleep(1);
            }
        }
    }
}
