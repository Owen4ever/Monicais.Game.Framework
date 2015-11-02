
using Monicais.Property;
using Monicais.ThrowHelper;
using System;
using System.Collections.Generic;

namespace Monicais.Core
{

    [Serializable]
    public class ItemType : NonNullDisplayable
    {
        public static ItemType BaseItemType = new ItemType();

        private ItemType() : base("") { parentType = null; }

        public ItemType(string name, string description = "") : this(name, description, BaseItemType) { }

        public ItemType(string name, string description, ItemType parent) : base(name, description)
        {
            ParentType = parent;
        }

        public ItemType(string name, ItemType parent) : this(name, "", parent) { }

        public bool IsBaseType { get { return parentType == null; } }

        public ItemType ParentType
        {
            get { return ParentType; }
            set
            {
                if (parentType == null)
                    ArgumentNull.Throw("ParentItmeType");
                else
                    parentType = value;
            }
        }
        private ItemType parentType;

        public IEntity AttachedOn { get; set; }
    }

    [Serializable]
    public class Item : AbstractEntity
    {

        public Item(int id, string name, string desc, PropertyList properties)
            : base(id, name, desc, properties)
        { }

        public SkillList Skills { get; private set; }
    }
}
