namespace Monicais.Property
{
    using System;
    using System.Runtime.Serialization;

    [Serializable]
    public class UnrecoverableProperty : HasEffectProperty
    {
        private int originalValue;

        public UnrecoverableProperty(PropertyID pid) : base(pid, new UnrecoverableEffectList())
        {
            this.originalValue = 0;
        }

        public UnrecoverableProperty(PropertyID pid, int val) : base(pid, val, new UnrecoverableEffectList())
        {
            this.originalValue = 0;
        }

        protected UnrecoverableProperty(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            this.originalValue = 0;
        }

        protected override int CalculateFinalValue()
        {
            return base.CalculateViaEffect(base.getLastFinalValue());
        }

        public override int OriginalValue
        {
            get
            {
                return this.originalValue;
            }
            set
            {
                base.FinalValue = this.originalValue = base.Restrict(value);
            }
        }
    }
}

