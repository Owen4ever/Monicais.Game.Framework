
using Monicais.Property;
using Monicais.ThrowHelper;
using System;
using System.Collections.Generic;

namespace Monicais.Core
{

    [Serializable]
    public abstract class AbstractCharacter : AbstractEntity
    {

        private List<MonoAction> actions;
        private EntityStatus status;

        protected AbstractCharacter(int id, string name, string desc, PropertyList properties)
            : base(id, name, desc, properties)
        { }

        public SkillList Skills { get; private set; }
    }
}
