namespace Monicais.Property
{
    using Monicais.ThrowHelper;
    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.Serialization;

    [Serializable]
    public class DefaultEffect : IEffect
    {
        private EffectProcessor processor;
        private EffectUpdater updater;

        protected DefaultEffect(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            this.processor = (EffectProcessor) info.GetValue("PROCESSOR", typeof(EffectProcessor));
            this.updater = (EffectUpdater) info.GetValue("UPDATER", typeof(EffectUpdater));
        }

        public DefaultEffect(EffectID id, PropertyID affectTo, EffectProcessor processor) : this(id, affectTo, null, processor)
        {
        }

        public DefaultEffect(EffectID id, PropertyID affectTo, Attributes defaultAttrs, EffectProcessor processor) : this(id, affectTo, defaultAttrs, processor, null)
        {
        }

        public DefaultEffect(EffectID id, PropertyID affectTo, Attributes defaultAttrs, EffectProcessor processor, EffectUpdater updater) : base(id, affectTo, defaultAttrs)
        {
            if (processor == null)
            {
                ArgumentNull.Throw("processor");
            }
            this.processor = processor;
            this.updater = updater ?? (<>c.<>9__2_0 ?? (<>c.<>9__2_0 = new EffectUpdater(<>c.<>9.<.ctor>b__2_0)));
        }

        public override void Affect(Attributes propertyAttrs, Attributes effectAttrs, ref int val)
        {
            this.processor(propertyAttrs, effectAttrs, ref val);
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue("PROCESSOR", this.processor);
            info.AddValue("UPDATER", this.updater);
        }

        public override bool Update(Attributes attrs)
        {
            return this.updater(attrs);
        }

        [Serializable, CompilerGenerated]
        private sealed class <>c
        {
            public static readonly DefaultEffect.<>c <>9 = new DefaultEffect.<>c();
            public static EffectUpdater <>9__2_0;

            internal bool <.ctor>b__2_0(Attributes attrs)
            {
                return false;
            }
        }
    }
}

