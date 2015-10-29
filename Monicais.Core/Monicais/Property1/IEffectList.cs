﻿namespace Monicais.Property
{
    using System;

    [Serializable]
    public abstract class IEffectList
    {
        protected IEffectList()
        {
        }

        public abstract void AddEffect(IEffect effect);
        public abstract int Calculate(Attributes propertyAttrs, int val);
        public abstract int RemoveEffect(EffectID eid);
        public abstract bool Update();

        public abstract int Count { get; }

        public bool Empty
        {
            get
            {
                return (this.Count == 0);
            }
        }

        public bool NotEmpty
        {
            get
            {
                return (this.Count > 0);
            }
        }
    }
}

