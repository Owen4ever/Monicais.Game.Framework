namespace Monicais.Property
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.Serialization;

    public interface IProperty : IRestrictable, ISerializable
    {
        event EventListener RestrictorChangedListener;

        bool AddEffect(IEffect effect);
        void AddEffectAddListener(EventListener listener);
        void AddEffectRemoveListener(EventListener listener);
        void AddEffectSupport(EffectSupport support);
        void AddInvalidationListener(EventListener listener);
        void Invalidate();
        int RemoveEffect(EffectID effectID);
        void RemoveEffectAddListener(EventListener listener);
        void RemoveEffectRemoveListener(EventListener listener);
        bool RemoveEffectSupport(EffectSupport support);
        void RemoveInvalidationListener(EventListener listener);
        void Update();
        void Validate();

        Monicais.Property.Attributes Attributes { get; }

        int FinalValue { get; }

        PropertyID ID { get; }

        bool IsValid { get; }

        int OriginalValue { get; set; }
    }
}

