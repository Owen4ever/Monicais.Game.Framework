namespace Monicais.Property
{
    using System;
    using System.Collections.Generic;

    public interface IPropertyCreater
    {
        void AddAfterCreate(AfterPropertyListCreate afterCreate);
        PropertyList Create(List<PropertyID> ids);
        void RemoveAfterCreate(AfterPropertyListCreate afterCreate);
    }
}

