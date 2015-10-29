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
        private List<Skill> allSkills;
        [NonSerialized]
        private List<Skill> activeSkills;
        [NonSerialized]
        private List<Skill> continuingSkills;
        [NonSerialized]
        private List<Skill> passiveSkills;

        public SkillList()
        {
            allSkills = new List<Skill>();
            activeSkills = new List<Skill>();
            continuingSkills = new List<Skill>();
            passiveSkills = new List<Skill>();
        }

        protected SkillList(SerializationInfo info, StreamingContext context)
        {
            allSkills = (List<Skill>) info.GetValue("SKILLS", typeof(List<Skill>));
            activeSkills = allSkills.FindAll(s => s.UsingType == SkillUsingType.ACTIVE);
            continuingSkills = allSkills.FindAll(s => s.UsingType == SkillUsingType.CONTINUING);
            passiveSkills = allSkills.FindAll(s => s.UsingType == SkillUsingType.PASSIVE);
        }

        public void AddSkill(Skill skill)
        {
            switch (skill.UsingType)
            {
                case SkillUsingType.ACTIVE:
                    activeSkills.Add(skill);
                    return;
                case SkillUsingType.CONTINUING:
                    continuingSkills.Add(skill);
                    return;
                case SkillUsingType.PASSIVE:
                    passiveSkills.Add(skill);
                    return;
                case SkillUsingType.ALL:
                    allSkills.Add(skill);
                    activeSkills.Add(skill);
                    continuingSkills.Add(skill);
                    passiveSkills.Add(skill);
                    return;
            }
#if DEBUG
            ArgumentOutOfRange.Throw("UsingType");
#endif
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("SKILLS", allSkills);
        }

        public List<Skill> this[SkillUsingType usingType]
        {
            get
            {
                switch (usingType)
                {
                    case SkillUsingType.ACTIVE:
                        return activeSkills;

                    case SkillUsingType.CONTINUING:
                        return continuingSkills;

                    case SkillUsingType.PASSIVE:
                        return passiveSkills;

                    case SkillUsingType.ALL:
                        return allSkills;
                }
#if DEBUG
                ArgumentOutOfRange.Throw("UsingType");
#endif
                return null;
            }
        }

        public List<Skill> Skills
        {
            get { return allSkills; }
        }
    }
}
