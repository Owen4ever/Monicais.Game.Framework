namespace Monicais.Core
{
    using System;

    [Serializable]
    public enum SkillUsingType : byte
    {
        ACTIVE = 1,
        ALL = 7,
        CONTINUING = 2,
        PASSIVE = 4
    }
}

