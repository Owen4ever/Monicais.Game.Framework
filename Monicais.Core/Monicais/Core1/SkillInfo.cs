namespace Monicais.Core
{
    using System;

    [Serializable]
    public sealed class SkillInfo : NonNullDisplayable
    {
        public SkillInfo(string name) : base(name, null)
        {
        }

        public SkillInfo(string name, string description) : base(name, description)
        {
        }
    }
}

