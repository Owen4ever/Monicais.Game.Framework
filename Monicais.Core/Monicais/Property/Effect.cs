
using Monicais.ThrowHelper;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Monicais.Property
{

    public sealed class EffectPriority
    {
        public static readonly EffectPriority IMMEDIATE = new EffectPriority(0);
        public static readonly EffectPriority FIRST = new EffectPriority(1);
        public static readonly EffectPriority SECOND = new EffectPriority(2);
        public static readonly EffectPriority THIRD = new EffectPriority(3);
        public static readonly EffectPriority FOURTH = new EffectPriority(4);
        public static readonly EffectPriority FIFTH = new EffectPriority(5);
        public static readonly EffectPriority SIXTH = new EffectPriority(6);
        public static readonly EffectPriority SEVENTH = new EffectPriority(7);
        public static readonly EffectPriority EIGHTH = new EffectPriority(8);
        public static readonly EffectPriority NINTH = new EffectPriority(9);
        public static readonly EffectPriority TENTH = new EffectPriority(10);
        public static readonly EffectPriority ELEVENTH = new EffectPriority(11);
        public static readonly EffectPriority TWELFTH = new EffectPriority(12);
        public static readonly EffectPriority THIRTEENTH = new EffectPriority(13);
        public static readonly EffectPriority FOURTEENTH = new EffectPriority(14);
        public static readonly EffectPriority FIFTEENTH = new EffectPriority(15);
        public const int PriorityCount = 16;
        private static readonly EffectPriority[] PRIORITIES =
             {
            IMMEDIATE, FIRST, SECOND, THIRD,
            FOURTH, FIFTH, SIXTH, SEVENTH,
            EIGHTH, NINTH, TENTH, ELEVENTH,
            TWELFTH, THIRTEENTH, FOURTEENTH, FIFTEENTH };

        internal EffectPriority(int priority)
        {
            this.priority = priority;
        }

        public override bool Equals(object obj)
        {
#if DEBUG
            EffectPriority priority = obj as EffectPriority;
            if (priority == null)
                return false;
            else
                return this.priority == priority.priority;
#else
            return this.priority == ((EffectPriority) obj).priority;
#endif
        }

        public override int GetHashCode()
        {
            return priority;
        }

        public static List<Node>[] NewEffectLists<Node>()
        {
            List<Node>[] listArray = new List<Node>[PriorityCount];
            for (int i = 0; i < PriorityCount; ++i)
                listArray[i] = new List<Node>();
            return listArray;
        }

        public static bool operator ==(EffectPriority p1, EffectPriority p2)
        {
            return Equals(p1, p2);
        }

        public static bool operator >(EffectPriority p1, EffectPriority p2)
        {
            return p1.priority > p2.priority;
        }

        public static implicit operator int (EffectPriority p)
        {
            return p.priority;
        }

        public static implicit operator EffectPriority(int p)
        {
            if (p < 0 || p >= PriorityCount)
                throwIndexOutOfRangeException();
            return PRIORITIES[p];
        }

        public static bool operator !=(EffectPriority p1, EffectPriority p2)
        {
            return !Equals(p1, p2);
        }

        public static bool operator <(EffectPriority p1, EffectPriority p2)
        {
            return p1.priority < p2.priority;
        }

        private static void throwIndexOutOfRangeException()
        {
            IndexOutOfRange.Throw("Priority should between 0 ~ 16");
        }

        public override string ToString()
        {
            return string.Format("[EffectID: Priority={0}]", priority);
        }
        private int priority;
    }

    [Serializable]
    public sealed class EffectID : ISerializable
    {

        private EffectID(SerializationInfo info, StreamingContext context)
            : this(info.GetString("ID"), info.GetInt32("PRIORITY"))
        { }

        public EffectID(string id, EffectPriority priority)
        {
            this.id = id;
            this.priority = priority;
        }

        public override bool Equals(object obj)
        {
#if DEBUG
            EffectID tid = obj as EffectID;
            if (tid != null)
                return ID == tid.ID;
            else
                return false;
#else
            return ID == ((EffectID) obj).ID;
#endif
        }

        public override int GetHashCode()
        {
            return ID.GetHashCode();
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("ID", id);
            info.AddValue("PRIORITY", priority);
        }

        public static bool operator ==(EffectID eid1, EffectID eid2)
        {
            return Equals(eid1, eid2);
        }

        public static bool operator >(EffectID eid1, EffectID eid2)
        {
            return eid1.Priority > eid2.Priority;
        }

        public static bool operator !=(EffectID eid1, EffectID eid2)
        {
            return !Equals(eid1, eid2);
        }

        public static bool operator <(EffectID eid1, EffectID eid2)
        {
            return eid1.Priority < eid2.Priority;
        }

        public override string ToString()
        {
            return string.Format("[EffectID: ID={0}, Priority={1}]", ID, Priority);
        }

        public string ID
        {
            get { return id; }
        }
        private readonly string id;

        public EffectPriority Priority
        {
            get { return priority; }
        }
        private readonly EffectPriority priority;
    }

    [Serializable]
    public abstract class IEffect
    {

        protected IEffect(EffectID id, PropertyID affectTo) : this(id, affectTo, null) { }

        protected IEffect(EffectID id, PropertyID affectTo, Attributes defaultAttrs)
        {
            if (id == null)
                ArgumentNull.Throw("id");
            if (affectTo == null)
                ArgumentNull.Throw("affectTo");
            ID = id;
            AffectTo = affectTo;
            DefaultAttributes = ReadOnlyAttributes.ReadOnly(defaultAttrs) ?? ReadOnlyAttributes.EMPTY;
        }

        public abstract void Affect(Attributes propertyAttrs, Attributes effectAttrs, ref int val);

        public abstract bool Update(Attributes attrs);

        public PropertyID AffectTo { get; private set; }

        public ReadOnlyAttributes DefaultAttributes { get; private set; }

        public EffectID ID { get; private set; }
    }

    [Serializable]
    public enum EffectType
    {
        NORMAL = 1,
        BUFF = 2,
        DEBUFF = 3,
        BOTH_BUFF = 4,
        ALL = 5
    }

    [Serializable]
    public class DefaultEffect : IEffect
    {
        private EffectProcessor processor;
        private EffectUpdater updater;

        public DefaultEffect(EffectID id, PropertyID affectTo, EffectProcessor processor)
            : this(id, affectTo, null, processor)
        { }

        public DefaultEffect(EffectID id, PropertyID affectTo, Attributes defaultAttrs, EffectProcessor processor)
            : this(id, affectTo, defaultAttrs, processor, null)
        { }

        public DefaultEffect(EffectID id, PropertyID affectTo, Attributes defaultAttrs,
            EffectProcessor processor, EffectUpdater updater) : base(id, affectTo, defaultAttrs)
        {
            if (processor == null)
                ArgumentNull.Throw("processor");
            this.processor = processor;
            this.updater = updater ?? (a => false);
        }

        public override void Affect(Attributes propertyAttrs, Attributes effectAttrs, ref int val)
        {
            processor(propertyAttrs, effectAttrs, ref val);
        }

        public override bool Update(Attributes attrs)
        {
            return updater(attrs);
        }
    }

    [Serializable]
    public delegate bool EffectUpdater(Attributes effectAttrs);

    [Serializable]
    public delegate void EffectProcessor(Attributes propertyAttrs, Attributes effectAttrs, ref int val);

    [Serializable]
    public delegate bool EffectSupport(IEffect effect);

    public static class EffectSupportUtil
    {
        public static EffectSupport PreventAll()
        {
            return e => true;
        }
    }
}
