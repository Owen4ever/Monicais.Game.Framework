
using Monicais.ThrowHelper;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Monicais.Property
{

    [Serializable]
    public sealed class PropertyID : IComparable<PropertyID>
    {
        private string description;
        private readonly string id;
        private int index;
        private string name;

        internal PropertyID(string id, string name, string description)
        {
            if (string.IsNullOrWhiteSpace(id))
                ArgumentNull.Throw("id");
            this.id = id;
            this.Name = name;
            this.Description = description;
        }

        public int CompareTo(PropertyID pid)
        {
            if (pid == null)
                ArgumentNull.Throw("pid");
            return ID.CompareTo(pid.ID);
        }

        public override bool Equals(object obj)
        {
            PropertyID yid = obj as PropertyID;
            if (yid == null)
                return false;
            return (yid.Index == this.Index);
        }

        public override int GetHashCode()
        {
            return Index;
        }

        public static bool operator >(PropertyID p1, PropertyID p2)
        {
            if (p1 == null || p2 == null)
                ArgumentNull.Throw("PropertyID");
            return p1.Index > p2.Index;
        }

        public static bool operator <(PropertyID p1, PropertyID p2)
        {
            if (p1 == null || p2 == null)
                ArgumentNull.Throw("PropertyID");
            return p1.Index < p2.Index;
        }

        public override string ToString()
        {
            return string.Format("[PropertyID: Index={0}, Name=\"{1}\", Description=\"{2}\"]", Index, Name, Description);
        }

        public string Description
        {
            get { return description; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    description = string.Empty;
                else
                    description = value;
            }
        }

        public string ID
        {
            get { return id; }
        }

        public int Index
        {
            get { return index; }
            internal set { index = value; }
        }

        public string Name
        {
            get { return name; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    ArgumentNull.Throw("name");
                name = value;
            }
        }
    }

    [Serializable]
    public sealed class PropertyComparer : IComparer<IProperty>
    {
        public static readonly IComparer<IProperty> Instance = new PropertyComparer();

        private PropertyComparer() { }

        public int Compare(IProperty p1, IProperty p2)
        {
            return p1.ID.CompareTo(p2.ID);
        }
    }

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

        Attributes Attributes { get; }

        int FinalValue { get; }

        PropertyID ID { get; }

        bool IsValid { get; }

        int OriginalValue { get; set; }
    }

    [Serializable]
    public abstract class AbstractProperty : Restrictable, IProperty, IRestrictable, ISerializable
    {

        protected EventListener EffectAddListener;
        protected EventListener EffectRemoveListener;
        protected EventListener InvalidationListener;
        private readonly PropertyID propertyID;
        [NonSerialized]
        private readonly ValidValue validValue;

        protected AbstractProperty(PropertyID pid) : this(pid, 0) { }

        protected AbstractProperty(PropertyID pid, int val)
        {
            validValue = new ValidValue();
            InvalidationListener = new EventListener(EmptyEventListener);
            EffectAddListener = new EventListener(EmptyEventListener);
            EffectRemoveListener = new EventListener(EmptyEventListener);
            propertyID = pid;
            Attributes = new Attributes();
            OriginalValue = val;
            validValue.Validate(val);
        }

        protected AbstractProperty(SerializationInfo info, StreamingContext context)
        {
            validValue = new ValidValue();
            InvalidationListener = new EventListener(EmptyEventListener);
            EffectAddListener = new EventListener(EmptyEventListener);
            EffectRemoveListener = new EventListener(EmptyEventListener);
            propertyID = (PropertyID) info.GetValue("ID", typeof(PropertyID));
            Attributes = (Attributes) info.GetValue("ATTRIBUTES", typeof(Attributes));
            OriginalValue = info.GetInt32("ORIGINAL_VALUE");
            bool boolean = info.GetBoolean("IS_VALID");
            int val = info.GetInt32("VALID_VALUE");
            validValue.Validate(val);
            if (!boolean)
                validValue.Invalidate();
            InvalidationListener = (EventListener) info.GetValue("INVALIDATION_LISTENERS", typeof(EventListener));
            EffectAddListener = (EventListener) info.GetValue("EFFECT_ADD_LISTENERS", typeof(EventListener));
            EffectRemoveListener = (EventListener) info.GetValue("EFFECT_REMOVE_LISTENERS", typeof(EventListener));
        }

        public abstract bool AddEffect(IEffect effect);

        public abstract int RemoveEffect(EffectID effectID);

        public void AddEffectAddListener(EventListener listener)
        {
            if (listener == null)
                ArgumentNull.Throw("listener");
            EffectAddListener += listener;
        }

        public void RemoveEffectAddListener(EventListener listener)
        {
            if (listener == null)
                ArgumentNull.Throw("listener");
            EffectAddListener += listener;
        }

        public void AddEffectRemoveListener(EventListener listener)
        {
            if (listener == null)
                ArgumentNull.Throw("listener");
            EffectRemoveListener += listener;
        }

        public void RemoveEffectRemoveListener(EventListener listener)
        {
            if (listener == null)
                ArgumentNull.Throw("listener");
            EffectRemoveListener += listener;
        }

        public void AddInvalidationListener(EventListener listener)
        {
            if (listener == null)
                ArgumentNull.Throw("listener");
            InvalidationListener += listener;
        }

        public void RemoveInvalidationListener(EventListener listener)
        {
            if (listener == null)
                ArgumentNull.Throw("listener");
            InvalidationListener += listener;
        }

        public abstract void AddEffectSupport(EffectSupport support);

        public abstract bool RemoveEffectSupport(EffectSupport support);

        protected abstract int CalculateFinalValue();

        public static void EmptyEventListener() { }

        protected int getLastFinalValue() { return validValue.Value; }

        public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("ID", ID);
            info.AddValue("ATTRIBUTES", Attributes);
            info.AddValue("IS_VALID", IsValid);
            info.AddValue("ORIGINAL_VALUE", OriginalValue);
            info.AddValue("VALID_VALUE", validValue.Value);
            info.AddValue("INVALIDATION_LISTENERS", InvalidationListener);
            info.AddValue("EFFECT_ADD_LISTENERS", EffectAddListener);
            info.AddValue("EFFECT_REMOVE_LISTENERS", EffectRemoveListener);
        }

        public void Validate()
        {
            if (!IsValid)
                validValue.Validate(Restrict(CalculateFinalValue()));
        }

        public void Invalidate()
        {
            validValue.Invalidate();
            InvalidationListener();
        }

        public override string ToString()
        {
            return string.Format("[{0}: ID=\"{1}\", OriginalValue={2}, FinalValue={3}, Restricted={4}]",
                GetType().Name, ID.Name, OriginalValue, FinalValue, IsRestricted());
        }

        public abstract void Update();

        public Attributes Attributes { get; private set; }

        public int FinalValue
        {
            get
            {
                Validate();
                return validValue.Value;
            }
            protected set { validValue.Validate(value); }
        }

        public PropertyID ID
        {
            get { return propertyID; }
        }

        public bool IsValid
        {
            get { return validValue.IsValid; }
        }

        public abstract int OriginalValue { get; set; }
    }

    internal sealed class ValidValue
    {

        public ValidValue() : this(false, 0)
        {
        }

        public ValidValue(bool isValid, int val)
        {
            IsValid = isValid;
            Value = val;
        }

        public void Invalidate()
        {
            IsValid = false;
        }

        public void Validate(int val)
        {
            Value = val;
            IsValid = true;
        }

        public bool IsValid { get; private set; }

        public int Value { get; private set; }
    }
}
