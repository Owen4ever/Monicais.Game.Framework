namespace Monicais.Property
{
    using Monicais.ThrowHelper;
    using System;
    using System.Runtime.Serialization;

    [Serializable]
    public abstract class NonEffectProperty : AbstractProperty
    {
        protected NonEffectProperty(PropertyID pid) : base(pid)
        {
        }

        protected NonEffectProperty(PropertyID pid, int val) : base(pid, val)
        {
        }

        protected NonEffectProperty(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public override bool AddEffect(IEffect effect)
        {
            base.EffectAddListener();
            NotSupported.Throw("Non-Effect Property");
            return false;
        }

        public override void AddEffectSupport(EffectSupport support)
        {
            NotSupported.Throw("Non-Effect Property");
        }

        public override int RemoveEffect(EffectID eid)
        {
            base.EffectRemoveListener();
            NotSupported.Throw("Non-Effect Property");
            return -1;
        }

        public override bool RemoveEffectSupport(EffectSupport support)
        {
            NotSupported.Throw("Non-Effect Property");
            return false;
        }

        public override int OriginalValue
        {
            get
            {
                return base.FinalValue;
            }
            set
            {
                base.FinalValue = value;
            }
        }
    }
}

