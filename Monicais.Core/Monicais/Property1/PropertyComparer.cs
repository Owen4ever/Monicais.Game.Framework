namespace Monicais.Property
{
    using System;
    using System.Collections.Generic;

    [Serializable]
    public sealed class PropertyComparer : IComparer<IProperty>
    {
        public static readonly IComparer<IProperty> Instance = new PropertyComparer();

        private PropertyComparer()
        {
        }

        public int Compare(IProperty p1, IProperty p2)
        {
            return p1.ID.CompareTo(p2.ID);
        }
    }
}

