

namespace Hz.Libraries.Messaging.data
{
    public class DataBody<T> : Body
    {
        public string entityName { get; set; }
        public T entityBody { get; set; }

    }
}
