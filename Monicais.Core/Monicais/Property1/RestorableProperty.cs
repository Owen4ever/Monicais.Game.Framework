namespace Monicais.Property
{
    using System;
    using System.Runtime.Serialization;

    [Serializable]
    public class RestorableProperty : HasEffectProperty
    {
        private int originalValue;

        public RestorableProperty(PropertyID pid) : base(pid, new RestorabeEffectList())
        {
            this.originalValue = 0;
        }

        public RestorableProperty(PropertyID pid, int val) : base(pid, val, new RestorabeEffectList())
        {
            this.originalValue = 0;
        }

        protected RestorableProperty(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            this.originalValue = 0;
        }

        protected override int CalculateFinalValue()
        {
            return base.CalculateViaEffect(this.OriginalValue);
        }

        public override int OriginalValue
        {
            get
            {
                return this.originalValue;
            }
            set
            {
                this.originalValue = value;
                base.Invalidate();
            }
        }
    }
}

