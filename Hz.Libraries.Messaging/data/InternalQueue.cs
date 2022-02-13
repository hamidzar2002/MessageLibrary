
using System.Collections.Concurrent;

namespace Hz.Libraries.Messaging.data
{
    public sealed class InternalQueue
    {
        private static object _lock = new object();

        private InternalQueue()
        {

        }

        public ConcurrentQueue<object> messageQueue = new ConcurrentQueue<object>();

        private static InternalQueue instance = null;

        
        public static InternalQueue Instance
        {
            get
            {
                lock (_lock)
                {
                    if (instance == null)
                        instance = new InternalQueue();
                    return instance;
                }
            }
        }

    }
}
