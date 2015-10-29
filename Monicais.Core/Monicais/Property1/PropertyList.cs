namespace Monicais.Property
{
    using Monicais.ThrowHelper;
    using System;
    using System.Collections;
    using System.Reflection;

    [Serializable]
    public sealed class PropertyList
    {
        private IProperty[] properties;

        public PropertyList(IProperty[] pros)
        {
            this.properties = pros;
        }

        public void AddEffect(IEffect effect)
        {
            if (effect == null)
            {
                ArgumentNull.Throw("effect");
            }
            this[effect.AffectTo].AddEffect(effect);
        }

        public IEnumerator GetEnumerator()
        {
            return this.properties.GetEnumerator();
        }

        public void RemoveEffect(IEffect effect)
        {
            if (effect == null)
            {
                ArgumentNull.Throw("effect");
            }
            this[effect.AffectTo].RemoveEffect(effect.ID);
        }

        public void Update()
        {
            foreach (IProperty property in this.properties)
            {
                property.Update();
            }
        }

        public int Count
        {
            get
            {
                return this.properties.Length;
            }
        }

        public IProperty this[PropertyID key]
        {
            get
            {
                return this.properties[key.Index];
            }
        }
    }
}

