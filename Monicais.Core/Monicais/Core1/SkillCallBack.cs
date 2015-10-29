namespace Monicais.Core
{
    using System;
    using System.Runtime.CompilerServices;

    [Serializable]
    public delegate void SkillCallBack(IEntity entity, int originalDamage, int finalDamage);
}

