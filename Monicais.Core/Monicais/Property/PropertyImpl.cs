
using Monicais.ThrowHelper;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Monicais.Property
{

    [Serializable]
    public abstract class HasEffectProperty : AbstractProperty
    {
        private readonly IEffectList effectList;
        private List<EffectSupport> effectSupports;

        protected HasEffectProperty(PropertyID pid, IEffectList effectList) : base(pid)
        {
            this.effectSupports = new List<EffectSupport>();
            this.effectList = effectList;
        }

        protected HasEffectProperty(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            effectSupports = new List<EffectSupport>();
            effectList = (IEffectList) info.GetValue("EFFECT_LIST", typeof(IEffectList));
            effectSupports = (List<EffectSupport>) info.GetValue("EFFECT_SUPPORTS", typeof(List<EffectSupport>));
        }

        protected HasEffectProperty(PropertyID pid, int val, IEffectList effectList) : base(pid, val)
        {
            this.effectSupports = new List<EffectSupport>();
            this.effectList = effectList;
        }

        public override bool AddEffect(IEffect effect)
        {
            if (IsEffectPrevented(effect))
                return false;
            effectList.AddEffect(effect);
            Invalidate();
            if (effect.ID.Priority == EffectPriority.IMMEDIATE)
                Validate();
            return true;
        }

        public override void AddEffectSupport(EffectSupport support)
        {
            effectSupports.Add(support);
        }

        protected int CalculateViaEffect(int val)
        {
            return effectList.Calculate(Attributes, val);
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue("EFFECT_LIST", effectList);
            info.AddValue("EFFECT_SUPPORTS", effectSupports);
        }

        private bool IsEffectPrevented(IEffect effect)
        {
            foreach (EffectSupport support in effectSupports)
                if (support(effect))
                    return true;
            return false;
        }

        public override int RemoveEffect(EffectID eid)
        {
            int num = effectList.RemoveEffect(eid);
            Invalidate();
            return num;
        }

        public override bool RemoveEffectSupport(EffectSupport support)
        {
            return effectSupports.Remove(support);
        }

        public override string ToString()
        {
            return base.ToString() + string.Format("[Effect Count={0}]", effectList.Count);
        }

        public override void Update()
        {
            if (effectList.Update())
                Invalidate();
        }
    }

    [Serializable]
    public class RestorableProperty : HasEffectProperty
    {
        private int originalValue;

        public RestorableProperty(PropertyID pid) : base(pid, new RestorableEffectList()) { }

        public RestorableProperty(PropertyID pid, int val) : base(pid, val, new RestorableEffectList()) { }

        protected RestorableProperty(SerializationInfo info, StreamingContext context) : base(info, context) { }

        protected override int CalculateFinalValue()
        {
            return CalculateViaEffect(OriginalValue);
        }

        public override int OriginalValue
        {
            get { return originalValue; }
            set
            {
                originalValue = value;
                Invalidate();
            }
        }
    }

    [Serializable]
    public class UnrecoverableProperty : HasEffectProperty
    {
        private int originalValue;

        public UnrecoverableProperty(PropertyID pid) : base(pid, new UnrecoverableEffectList()) { }

        public UnrecoverableProperty(PropertyID pid, int val) : base(pid, val, new UnrecoverableEffectList()) { }

        protected UnrecoverableProperty(SerializationInfo info, StreamingContext context) : base(info, context) { }

        protected override int CalculateFinalValue()
        {
            return CalculateViaEffect(getLastFinalValue());
        }

        public override int OriginalValue
        {
            get { return originalValue; }
            set { FinalValue = originalValue = Restrict(value); }
        }
    }

    [Serializable]
    public abstract class NonEffectProperty : AbstractProperty
    {
        protected NonEffectProperty(PropertyID pid) : base(pid) { }

        protected NonEffectProperty(PropertyID pid, int val) : base(pid, val) { }

        protected NonEffectProperty(SerializationInfo info, StreamingContext context) : base(info, context) { }

        public override bool AddEffect(IEffect effect)
        {
            EffectAddListener();
#if DEBUG
            NotSupported.Throw("Non-Effect Property");
#endif
            return false;
        }

        public override void AddEffectSupport(EffectSupport support)
        {
#if DEBUG
            NotSupported.Throw("Non-Effect Property");
#endif
        }

        public override int RemoveEffect(EffectID eid)
        {
            EffectRemoveListener();
#if DEBUG
            NotSupported.Throw("Non-Effect Property");
#endif
            return 0;
        }

        public override bool RemoveEffectSupport(EffectSupport support)
        {
#if DEBUG
            NotSupported.Throw("Non-Effect Property");
#endif
            return false;
        }

        public override int OriginalValue
        {
            get { return FinalValue; }
            set { FinalValue = value; }
        }
    }

    [Serializable]
    public class SimpleProperty : NonEffectProperty
    {
        public SimpleProperty(PropertyID pid) : base(pid) { }

        public SimpleProperty(PropertyID pid, int val) : base(pid, val) { }

        protected SimpleProperty(SerializationInfo info, StreamingContext context) : base(info, context) { }

        protected override int CalculateFinalValue()
        {
            return OriginalValue;
        }

        public override void Update() { }
    }
}
