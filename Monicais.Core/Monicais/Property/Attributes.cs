
using Monicais.ThrowHelper;
using System;
using System.Collections;
using System.Runtime.Serialization;

namespace Monicais.Property
{

    [Serializable]
    public class Attributes : Hashtable
    {
        public Attributes() { }

        public Attributes(Hashtable attrs) : base(attrs) { }

        protected Attributes(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }

    [Serializable]
    public class ReadOnlyAttributes : Monicais.Property.Attributes
    {
        public static readonly ReadOnlyAttributes EMPTY = ReadOnly(new Hashtable(0));

        private ReadOnlyAttributes(Hashtable hashtable) : base(hashtable)
        { }

        protected ReadOnlyAttributes(SerializationInfo info, StreamingContext context) : base(info, context)
        { }

        public override void Add(object key, object value)
        {
            throwNotSupportedException();
        }

        public override void Clear()
        {
            throwNotSupportedException();
        }

        public override object Clone()
        {
            return new ReadOnlyAttributes((Hashtable) base.Clone());
        }

        public static ReadOnlyAttributes ReadOnly(Hashtable hashtable)
        {
            if (hashtable == null)
                return null;
            return new ReadOnlyAttributes(hashtable);
        }

        public override void Remove(object key)
        {
            throwNotSupportedException();
        }

        private static void throwNotSupportedException()
        {
            NotSupported.Throw("Read Only");
        }

        public override bool IsFixedSize
        {
            get { return true; }
        }

        public override bool IsReadOnly
        {
            get { return true; }
        }

        public override object this[object key]
        {
            get { return base[key]; }
            set { throwNotSupportedException(); }
        }
    }
}
