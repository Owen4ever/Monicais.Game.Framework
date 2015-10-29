namespace Monicais.Property
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;

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
            this.effectSupports = new List<EffectSupport>();
            this.effectList = (IEffectList) info.GetValue("EFFECT_LIST", typeof(IEffectList));
            this.effectSupports = (List<EffectSupport>) info.GetValue("EFFECT_SUPPORTS", typeof(List<EffectSupport>));
        }

        protected HasEffectProperty(PropertyID pid, int val, IEffectList effectList) : base(pid, val)
        {
            this.effectSupports = new List<EffectSupport>();
            this.effectList = effectList;
        }

        public override bool AddEffect(IEffect effect)
        {
            if (this.IsEffectPrevented(effect))
            {
                return false;
            }
            this.effectList.AddEffect(effect);
            base.Invalidate();
            return true;
        }

        public override void AddEffectSupport(EffectSupport support)
        {
            this.effectSupports.Add(support);
        }

        protected int CalculateViaEffect(int val)
        {
            return this.effectList.Calculate(base.Attributes, val);
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue("EFFECT_LIST", this.effectList);
            info.AddValue("EFFECT_SUPPORTS", this.effectSupports);
        }

        private bool IsEffectPrevented(IEffect effect)
        {
            foreach (EffectSupport support in this.effectSupports)
            {
                if (support(effect))
                {
                    return true;
                }
            }
            return false;
        }

        public override int RemoveEffect(EffectID eid)
        {
            int num;
            try
            {
                num = this.effectList.RemoveEffect(eid);
            }
            finally
            {
                base.Invalidate();
            }
            return num;
        }

        public override bool RemoveEffectSupport(EffectSupport support)
        {
            return this.effectSupports.Remove(support);
        }

        public override string ToString()
        {
            return (base.ToString() + string.Format("[Effect Count={0}]", this.effectList.Count));
        }

        public override void Update()
        {
            if (this.effectList.Update())
            {
                base.Invalidate();
            }
        }
    }
}

