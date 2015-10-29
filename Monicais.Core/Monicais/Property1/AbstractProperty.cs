namespace Monicais.Property
{
    using Monicais.ThrowHelper;
    using System;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using System.Runtime.Serialization;

    [Serializable]
    public abstract class AbstractProperty : Restrictable, IProperty, IRestrictable, ISerializable
    {
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Monicais.Property.Attributes <Attributes>k__BackingField;
        protected EventListener EffectAddListener;
        protected EventListener EffectRemoveListener;
        protected EventListener InvalidationListener;
        private readonly PropertyID propertyID;
        [NonSerialized]
        private readonly ValidValue validValue;

        protected AbstractProperty(PropertyID pid) : this(pid, 0)
        {
        }

        protected AbstractProperty(PropertyID pid, int val)
        {
            this.validValue = new ValidValue();
            this.InvalidationListener = new EventListener(AbstractProperty.EmptyEventListener);
            this.EffectAddListener = new EventListener(AbstractProperty.EmptyEventListener);
            this.EffectRemoveListener = new EventListener(AbstractProperty.EmptyEventListener);
            this.propertyID = pid;
            this.Attributes = new Monicais.Property.Attributes();
            this.OriginalValue = val;
            this.validValue.Validate(val);
        }

        protected AbstractProperty(SerializationInfo info, StreamingContext context)
        {
            this.validValue = new ValidValue();
            this.InvalidationListener = new EventListener(AbstractProperty.EmptyEventListener);
            this.EffectAddListener = new EventListener(AbstractProperty.EmptyEventListener);
            this.EffectRemoveListener = new EventListener(AbstractProperty.EmptyEventListener);
            this.propertyID = (PropertyID) info.GetValue("ID", typeof(PropertyID));
            this.Attributes = (Monicais.Property.Attributes) info.GetValue("ATTRIBUTES", typeof(Monicais.Property.Attributes));
            this.OriginalValue = info.GetInt32("ORIGINAL_VALUE");
            bool boolean = info.GetBoolean("IS_VALID");
            int val = info.GetInt32("VALID_VALUE");
            this.validValue.Validate(val);
            if (!boolean)
            {
                this.validValue.Invalidate();
            }
            this.InvalidationListener = (EventListener) info.GetValue("INVALIDATION_LISTENERS", typeof(EventListener));
            this.EffectAddListener = (EventListener) info.GetValue("EFFECT_ADD_LISTENERS", typeof(EventListener));
            this.EffectRemoveListener = (EventListener) info.GetValue("EFFECT_REMOVE_LISTENERS", typeof(EventListener));
        }

        public abstract bool AddEffect(IEffect effect);
        public void AddEffectAddListener(EventListener listener)
        {
            if (listener == null)
            {
                ArgumentNull.Throw("listener");
            }
            this.EffectAddListener = (EventListener) Delegate.Combine(this.EffectAddListener, listener);
        }

        public void AddEffectRemoveListener(EventListener listener)
        {
            if (listener == null)
            {
                ArgumentNull.Throw("listener");
            }
            this.EffectRemoveListener = (EventListener) Delegate.Combine(this.EffectRemoveListener, listener);
        }

        public abstract void AddEffectSupport(EffectSupport support);
        public void AddInvalidationListener(EventListener listener)
        {
            if (listener == null)
            {
                ArgumentNull.Throw("listener");
            }
            this.InvalidationListener = (EventListener) Delegate.Combine(this.InvalidationListener, listener);
        }

        protected abstract int CalculateFinalValue();
        public static void EmptyEventListener()
        {
        }

        protected int getLastFinalValue()
        {
            return this.validValue.Value;
        }

        public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("ID", this.ID);
            info.AddValue("ATTRIBUTES", this.Attributes);
            info.AddValue("IS_VALID", this.IsValid);
            info.AddValue("ORIGINAL_VALUE", this.OriginalValue);
            info.AddValue("VALID_VALUE", this.validValue.Value);
            info.AddValue("INVALIDATION_LISTENERS", this.InvalidationListener);
            info.AddValue("EFFECT_ADD_LISTENERS", this.EffectAddListener);
            info.AddValue("EFFECT_REMOVE_LISTENERS", this.EffectRemoveListener);
        }

        public void Invalidate()
        {
            this.validValue.Invalidate();
            this.InvalidationListener();
        }

        public abstract int RemoveEffect(EffectID effectID);
        public void RemoveEffectAddListener(EventListener listener)
        {
            if (listener == null)
            {
                ArgumentNull.Throw("listener");
            }
            this.EffectAddListener = (EventListener) Delegate.Remove(this.EffectAddListener, listener);
        }

        public void RemoveEffectRemoveListener(EventListener listener)
        {
            if (listener == null)
            {
                ArgumentNull.Throw("listener");
            }
            this.EffectRemoveListener = (EventListener) Delegate.Remove(this.EffectRemoveListener, listener);
        }

        public abstract bool RemoveEffectSupport(EffectSupport support);
        public void RemoveInvalidationListener(EventListener listener)
        {
            if (listener == null)
            {
                ArgumentNull.Throw("listener");
            }
            this.InvalidationListener = (EventListener) Delegate.Remove(this.InvalidationListener, listener);
        }

        public override string ToString()
        {
            object[] args = new object[] { base.GetType().Name, this.ID.Name, this.OriginalValue, this.FinalValue, base.IsRestricted() };
            return string.Format("[{0}: ID=\"{1}\", OriginalValue={2}, FinalValue={3}, Restricted={4}]", args);
        }

        public abstract void Update();
        public void Validate()
        {
            if (!this.IsValid)
            {
                this.validValue.Validate(base.Restrict(this.CalculateFinalValue()));
            }
        }

        public Monicais.Property.Attributes Attributes { get; private set; }

        public int FinalValue
        {
            get
            {
                this.Validate();
                return this.validValue.Value;
            }
            protected set
            {
                this.validValue.Validate(value);
            }
        }

        public PropertyID ID
        {
            get
            {
                return this.propertyID;
            }
        }

        public bool IsValid
        {
            get
            {
                return this.validValue.IsValid;
            }
        }

        public abstract int OriginalValue { get; set; }
    }
}

