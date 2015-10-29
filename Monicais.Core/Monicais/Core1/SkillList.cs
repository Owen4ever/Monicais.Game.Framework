namespace Monicais.Core
{
    using Monicais.ThrowHelper;
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using System.Runtime.Serialization;

    [Serializable]
    public class SkillList : ISerializable
    {
        [NonSerialized]
        private List<Skill> activeSkills;
        private List<Skill> allSkills;
        [NonSerialized]
        private List<Skill> continuingSkills;
        [NonSerialized]
        private List<Skill> passiveSkills;

        public SkillList()
        {
            this.allSkills = new List<Skill>();
            this.activeSkills = new List<Skill>();
            this.continuingSkills = new List<Skill>();
            this.passiveSkills = new List<Skill>();
        }

        protected SkillList(SerializationInfo info, StreamingContext context)
        {
            this.allSkills = (List<Skill>) info.GetValue("SKILLS", typeof(List<Skill>));
            this.activeSkills = this.allSkills.FindAll(<>c.<>9__1_0 ?? (<>c.<>9__1_0 = new Predicate<Skill>(<>c.<>9.<.ctor>b__1_0)));
            this.continuingSkills = this.allSkills.FindAll(<>c.<>9__1_1 ?? (<>c.<>9__1_1 = new Predicate<Skill>(<>c.<>9.<.ctor>b__1_1)));
            this.passiveSkills = this.allSkills.FindAll(<>c.<>9__1_2 ?? (<>c.<>9__1_2 = new Predicate<Skill>(<>c.<>9.<.ctor>b__1_2)));
        }

        public void AddSkill(Skill skill)
        {
            switch (skill.UsingType)
            {
                case SkillUsingType.ACTIVE:
                    this.activeSkills.Add(skill);
                    return;

                case SkillUsingType.CONTINUING:
                    this.continuingSkills.Add(skill);
                    return;

                case SkillUsingType.PASSIVE:
                    this.passiveSkills.Add(skill);
                    return;

                case SkillUsingType.ALL:
                    this.allSkills.Add(skill);
                    this.activeSkills.Add(skill);
                    this.continuingSkills.Add(skill);
                    this.passiveSkills.Add(skill);
                    return;
            }
            ArgumentOutOfRange.Throw("UsingType");
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("SKILLS", this.allSkills);
        }

        public List<Skill> this[SkillUsingType usingType]
        {
            get
            {
                switch (usingType)
                {
                    case SkillUsingType.ACTIVE:
                        return this.activeSkills;

                    case SkillUsingType.CONTINUING:
                        return this.continuingSkills;

                    case SkillUsingType.PASSIVE:
                        return this.passiveSkills;

                    case SkillUsingType.ALL:
                        return this.allSkills;
                }
                ArgumentOutOfRange.Throw("UsingType");
                return null;
            }
        }

        public List<Skill> Skills
        {
            get
            {
                return this.allSkills;
            }
        }

        [Serializable, CompilerGenerated]
        private sealed class <>c
        {
            public static readonly SkillList.<>c <>9 = new SkillList.<>c();
            public static Predicate<Skill> <>9__1_0;
            public static Predicate<Skill> <>9__1_1;
            public static Predicate<Skill> <>9__1_2;

            internal bool <.ctor>b__1_0(Skill skill)
            {
                return ((skill.UsingType & SkillUsingType.ACTIVE) == SkillUsingType.ACTIVE);
            }

            internal bool <.ctor>b__1_1(Skill skill)
            {
                return ((skill.UsingType & SkillUsingType.CONTINUING) == SkillUsingType.CONTINUING);
            }

            internal bool <.ctor>b__1_2(Skill skill)
            {
                return ((skill.UsingType & SkillUsingType.PASSIVE) == SkillUsingType.PASSIVE);
            }
        }
    }
}

