
using Monicais.Property;
using Monicais.ThrowHelper;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

namespace Monicais.Core
{

    [Serializable]
    public enum SkillActionUsingType : uint
    {
        ACTIVE = 1,
        CONTINUING = 2,
        PASSIVE = 4,
        ALL = ACTIVE | CONTINUING | PASSIVE
    }

    public interface SkillUpgrader : ISerializable
    {

        bool Used(bool success);

        int CurrentExp { get; }

        int CurrentLevel { get; }

        int MaxExp { get; }

        int MaxLevel { get; }
    }

    public class Skill : INameable
    {

        public Skill(string name, SkillActionUsingType usingType, SkillUpgrader upgrader)
        {
        }

        public string Name
        {
            get { return name; }
            set { name = value ?? ""; }
        }
        private string name;

        public List<MonoAction> Actions { get; private set; }

        public SkillUpgrader Upgrader { get; private set; }

        public SkillActionUsingType UsingType { get; private set; }

        public Attributes Attributes { get; private set; }

        public void AddAction(MonoAction action) { }
    }
}
