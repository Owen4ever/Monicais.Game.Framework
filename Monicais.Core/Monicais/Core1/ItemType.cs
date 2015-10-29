namespace Monicais.Core
{
    using System;

    [Serializable]
    public class ItemType : NonNullDisplayable
    {
        public static ItemType BaseItemType = new ItemType();

        private ItemType() : base("")
        {
        }

        public ItemType(string name, string description) : this(name, description, BaseItemType)
        {
        }

        public ItemType(string name, string description, ItemType parent) : base(name, description)
        {
        }
    }
}

