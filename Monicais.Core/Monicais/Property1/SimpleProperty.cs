namespace Monicais.Property
{
    using System;
    using System.Runtime.Serialization;

    [Serializable]
    public class SimpleProperty : NonEffectProperty
    {
        public SimpleProperty(PropertyID pid) : base(pid)
        {
        }

        public SimpleProperty(PropertyID pid, int val) : base(pid, val)
        {
        }

        protected SimpleProperty(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        protected override int CalculateFinalValue()
        {
            return this.OriginalValue;
        }

        public override void Update()
        {
        }
    }
}

