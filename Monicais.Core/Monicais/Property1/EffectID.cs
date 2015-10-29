namespace Monicais.Property
{
    using System;
    using System.Runtime.Serialization;

    [Serializable]
    public sealed class EffectID : ISerializable
    {
        private readonly string id;
        private readonly Monicais.Property.Priority priority;

        private EffectID(SerializationInfo info, StreamingContext context)
        {
            this.id = info.GetString("ID");
            this.priority = info.GetInt32("PRIORITY");
        }

        public EffectID(string id, Monicais.Property.Priority priority)
        {
            this.id = id;
            this.priority = priority;
        }

        public override bool Equals(object obj)
        {
            EffectID tid = obj as EffectID;
            if (tid == null)
            {
                return false;
            }
            return (this.ID == tid.ID);
        }

        public override int GetHashCode()
        {
            return this.ID.GetHashCode();
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("ID", this.id);
            info.AddValue("PRIORITY", (int) this.priority);
        }

        public static bool operator ==(EffectID eid1, EffectID eid2)
        {
            return object.Equals(eid1, eid2);
        }

        public static bool operator >(EffectID eid1, EffectID eid2)
        {
            return (eid1.Priority > eid2.Priority);
        }

        public static bool operator !=(EffectID eid1, EffectID eid2)
        {
            return !object.Equals(eid1, eid2);
        }

        public static bool operator <(EffectID eid1, EffectID eid2)
        {
            return (eid1.Priority < eid2.Priority);
        }

        public override string ToString()
        {
            return string.Format("[EffectID: ID={0}, Priority={1}]", this.ID, this.Priority);
        }

        public string ID
        {
            get
            {
                return this.id;
            }
        }

        public Monicais.Property.Priority Priority
        {
            get
            {
                return this.priority;
            }
        }
    }
}

