
using Monicais.ThrowHelper;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Monicais.Core
{

    [Serializable]
    public class SkillList : ISerializable
    {

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
            activeSkills = allSkills.FindAll(s => s.UsingType == SkillActionUsingType.ACTIVE);
            continuingSkills = allSkills.FindAll(s => s.UsingType == SkillActionUsingType.CONTINUING);
            passiveSkills = allSkills.FindAll(s => s.UsingType == SkillActionUsingType.PASSIVE);
        }

        public void AddSkill(Skill skill)
        {
            switch (skill.UsingType)
            {
                case SkillActionUsingType.ACTIVE:
                    activeSkills.Add(skill);
                    return;
                case SkillActionUsingType.CONTINUING:
                    continuingSkills.Add(skill);
                    return;
                case SkillActionUsingType.PASSIVE:
                    passiveSkills.Add(skill);
                    return;
                case SkillActionUsingType.ALL:
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

        public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("SKILLS", allSkills);
        }

        public List<Skill> this[SkillActionUsingType usingType]
        {
            get
            {
                switch (usingType)
                {
                    case SkillActionUsingType.ACTIVE:
                        return activeSkills;
                    case SkillActionUsingType.CONTINUING:
                        return continuingSkills;
                    case SkillActionUsingType.PASSIVE:
                        return passiveSkills;
                    case SkillActionUsingType.ALL:
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

        private List<Skill> allSkills;
        [NonSerialized]
        private List<Skill> activeSkills;
        [NonSerialized]
        private List<Skill> continuingSkills;
        [NonSerialized]
        private List<Skill> passiveSkills;
    }
}
