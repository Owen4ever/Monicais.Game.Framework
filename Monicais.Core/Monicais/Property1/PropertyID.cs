namespace Monicais.Property
{
    using Monicais.ThrowHelper;
    using System;

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
            {
                ArgumentNull.Throw("id");
            }
            this.id = id;
            this.Name = name;
            this.Description = description;
        }

        public int CompareTo(PropertyID pid)
        {
            if (pid == null)
            {
                ArgumentNull.Throw("pid");
            }
            return this.ID.CompareTo(pid.ID);
        }

        public override bool Equals(object obj)
        {
            PropertyID yid = obj as PropertyID;
            if (yid == null)
            {
                return false;
            }
            return (yid.Index == this.Index);
        }

        public override int GetHashCode()
        {
            return this.Index;
        }

        public static bool operator >(PropertyID p1, PropertyID p2)
        {
            if ((p1 == null) || (p2 == null))
            {
                ArgumentNull.Throw("PropertyID");
            }
            return (p1.Index > p2.Index);
        }

        public static bool operator <(PropertyID p1, PropertyID p2)
        {
            if ((p1 == null) || (p2 == null))
            {
                ArgumentNull.Throw("PropertyID");
            }
            return (p1.Index < p2.Index);
        }

        public override string ToString()
        {
            return string.Format("[PropertyID: Index={0}, Name=\"{1}\", Description=\"{2}\"]", this.Index, this.Name, this.Description);
        }

        public string Description
        {
            get
            {
                return this.description;
            }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    this.description = string.Empty;
                }
                else
                {
                    this.description = value;
                }
            }
        }

        public string ID
        {
            get
            {
                return this.id;
            }
        }

        public int Index
        {
            get
            {
                return this.index;
            }
            internal set
            {
                this.index = value;
            }
        }

        public string Name
        {
            get
            {
                return this.name;
            }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    ArgumentNull.Throw("name");
                }
                this.name = value;
            }
        }
    }
}

