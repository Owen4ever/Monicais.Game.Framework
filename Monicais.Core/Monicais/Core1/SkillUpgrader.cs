namespace Monicais.Core
{
    using System;
    using System.Runtime.Serialization;

    public interface SkillUpgrader : ISerializable
    {
        bool Used(bool success);

        int CurrentExp { get; }

        int CurrentLevel { get; }

        int MaxExp { get; }

        int MaxLevel { get; }
    }
}

