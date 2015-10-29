
using Monicais.ThrowHelper;
using System;
using System.Collections;

namespace Monicais.Property
{

    [Serializable]
    public sealed class PropertyList
    {
        private IProperty[] properties;

        public PropertyList(IProperty[] pros)
        {
            properties = pros;
        }

        public void AddEffect(IEffect effect)
        {
            if (effect == null)
                ArgumentNull.Throw("effect");
            this[effect.AffectTo].AddEffect(effect);
        }

        public IEnumerator GetEnumerator()
        {
            return properties.GetEnumerator();
        }

        public void RemoveEffect(IEffect effect)
        {
            if (effect == null)
                ArgumentNull.Throw("effect");
            this[effect.AffectTo].RemoveEffect(effect.ID);
        }

        public void Update()
        {
            foreach (IProperty property in properties)
                property.Update();
        }

        public int Count
        {
            get { return properties.Length; }
        }

        public IProperty this[PropertyID key]
        {
            get { return properties[key.Index]; }
        }
    }
}

