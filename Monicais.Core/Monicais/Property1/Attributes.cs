namespace Monicais.Property
{
    using System;
    using System.Collections;
    using System.Runtime.Serialization;

    [Serializable]
    public class Attributes : Hashtable
    {
        public Attributes()
        {
        }

        public Attributes(Hashtable attrs) : base(attrs)
        {
        }

        protected Attributes(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}

