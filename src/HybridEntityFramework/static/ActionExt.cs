using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HybridEntityFramework
{
    public static class ActionExt
    {
        public static Task InfinityTask(this Action ac, Action<Exception> exceptionhandler = null, CancellationTokenSource token = null, uint interval = 1, bool start = false, PauseTokenSource ptoken = null)
        {
            Task t= new Task(ac.InfinityAction(exceptionhandler,token, interval, ptoken));
            if (start)
            {
                t.Start();
            }
            return t;
        }
        public static Action InfinityAction(this Action ac, Action<Exception> exceptionhandler = null, CancellationTokenSource token = null, uint interval = 1, PauseTokenSource ptoken=null)
        {
            return () =>
            {
                while (true)
                {
                    if (token!=null && token.IsCancellationRequested)
                        return;
                    try
                    {
                        if (ptoken != null && ptoken.pause)
                            continue;
                        ac.Invoke();
                    }
                    catch (Exception ex)
                    {
                        exceptionhandler?.Invoke(ex);
                    }
                    finally
                    {
                        Thread.Sleep((int)interval);
                    }
                }
            };
        }
        public static Task FinityTask(this Func<bool> ac, Action<Exception> exceptionhandler = null, uint interval = 1,bool start=false)
        {
            Task t= new Task(ac.FinityAction(exceptionhandler, interval)); 
            if (start)
            {
                t.Start();
            }
            return t;
        }
        public static Action FinityAction(this Func<bool> ac, Action<Exception> exceptionhandler = null, uint interval = 1)
        {
            
            return () =>
            {
                bool Token = true;
                while (Token)
                {
                    try
                    {
                        Token=ac.Invoke();
                    }
                    catch (Exception ex)
                    {
                        exceptionhandler?.Invoke(ex);
                    }
                    finally
                    {
                        Thread.Sleep((int)interval);
                    }
                }
            };
        }
        public static void TryCatch(this Action ac, Action fi=null, Action<Exception> exceptionhandler = null)
        {
            try
            {
                ac.Invoke();
            }
            catch (Exception ex)
            {
                exceptionhandler?.Invoke(ex);
            }
            finally
            {
                fi?.Invoke();
            }
        }
    }
    public class PauseTokenSource
    {
        public  bool pause;
    }
}
