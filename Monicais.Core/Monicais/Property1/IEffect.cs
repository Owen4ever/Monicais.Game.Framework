namespace Monicais.Property
{
    using Monicais.ThrowHelper;
    using System;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using System.Runtime.Serialization;

    [Serializable]
    public abstract class IEffect : ISerializable
    {
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private PropertyID <AffectTo>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private ReadOnlyAttributes <DefaultAttributes>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private EffectID <ID>k__BackingField;

        protected IEffect(EffectID id, PropertyID affectTo) : this(id, affectTo, null)
        {
        }

        protected IEffect(SerializationInfo info, StreamingContext context)
        {
            this.ID = (EffectID) info.GetValue("ID", typeof(EffectID));
            this.AffectTo = (PropertyID) info.GetValue("AFFECT_TO", typeof(PropertyID));
            this.DefaultAttributes = (ReadOnlyAttributes) info.GetValue("DEFAULT_ATTRIBUTES", typeof(ReadOnlyAttributes));
        }

        protected IEffect(EffectID id, PropertyID affectTo, Attributes defaultAttrs)
        {
            if (id == null)
            {
                ArgumentNull.Throw("id");
            }
            if (affectTo == null)
            {
                ArgumentNull.Throw("affectTo");
            }
            this.ID = id;
            this.AffectTo = affectTo;
            this.DefaultAttributes = ReadOnlyAttributes.ReadOnly(defaultAttrs) ?? ReadOnlyAttributes.EMPTY;
        }

        public abstract void Affect(Attributes propertyAttrs, Attributes effectAttrs, ref int val);
        public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("ID", this.ID);
            info.AddValue("AFFECT_TO", this.AffectTo);
            info.AddValue("DEFAULT_ATTRIBUTES", this.DefaultAttributes);
        }

        public abstract bool Update(Attributes attrs);

        public PropertyID AffectTo { get; private set; }

        public ReadOnlyAttributes DefaultAttributes { get; private set; }

        public EffectID ID { get; private set; }
    }
}

