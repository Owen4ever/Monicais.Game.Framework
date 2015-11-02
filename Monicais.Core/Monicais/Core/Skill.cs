
using Monicais.Property;
using Monicais.ThrowHelper;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

namespace Monicais.Core
{

    [Serializable]
    public enum SkillActionUsingType : byte
    {
        ACTIVE = 1,
        CONTINUING = 2,
        PASSIVE = 4,
        ALL = 7
    }

    public interface SkillUpgrader : ISerializable
    {

        bool Used(bool success);

        int CurrentExp { get; }

        int CurrentLevel { get; }

        int MaxExp { get; }

        int MaxLevel { get; }
    }

    [Serializable]
    public abstract class ISkillCondition
    {

        protected ISkillCondition() { }

        public abstract bool DoAction(IEntity entity);

        public abstract bool Test(IEntity entity);

        public string Description
        {
            get { return desc; }
            set { desc = value ?? ""; }
        }
        private string desc;
    }

    public class DefaultSkillCondition : ISkillCondition
    {
        private Action @do;
        private Action test;

        public DefaultSkillCondition(Action test, Action @do, string description)
        {
            if (test == null)
                ArgumentNull.Throw("test");
            if (@do == null)
                ArgumentNull.Throw("do");
            this.test = test;
            this.@do = @do;
            Description = description;
        }

        public override bool DoAction(IEntity entity)
        {
            return @do(entity);
        }

        public override bool Test(IEntity entity)
        {
            return test(entity);
        }

        public delegate bool Action(IEntity entity);
    }

    [Serializable]
    public class SkillArgs : IActionArgs { }

    [Serializable]
    public class SkillAction : NonNullDisplayable, IAction
    {

        private List<IEffect> effects;

        public SkillAction(string name, ISkillCondition condition, SkillActionUsingType usingType,
            ActionUsingProcess process, SkillUpgrader upgrader,
            List<IEffect> effectList, Attributes attributes)
            : this(name, null, condition, usingType, process, upgrader, effectList, attributes)
        { }

        public SkillAction(string name, string description, ISkillCondition condition, SkillActionUsingType usingType,
            ActionUsingProcess process, SkillUpgrader upgrader,
            List<IEffect> effectList, Attributes attributes) : base(name, description)
        {
            if (process == null)
                ArgumentNull.Throw("process");
            if (condition == null)
                ArgumentNull.Throw("condition");
            if (upgrader == null)
                ArgumentNull.Throw("upgrader");
            if (effectList == null || effectList.Count == 0)
                ArgumentNull.Throw("effects");
            if (attributes == null)
                ArgumentNull.Throw("attributes");
            UsingType = usingType;
            Process = process;
            Condition = condition;
            Upgrader = upgrader;
            effects = effectList;
            Attributes = attributes;
        }

        private ISkillCondition Condition { get; set; }

        public void Suspend(IEntity entity)
        {
            if (IsProcessing)
                Process.Suspend(entity);
        }

        public void TestAndDoAction(IEntity entity, IActionArgs args)
        {
        }

        public void Update(IEntity entity)
        {
            if (IsProcessing) ;
        }

        public Attributes Attributes { get; private set; }

        public List<IEffect> Effects
        {
            get { return effects; }
        }

        public bool IsProcessing
        {
            get { return Process.IsProcessing; }
            private set { Process.IsProcessing = true; }
        }

        public SkillUpgrader Upgrader { get; private set; }

        public ActionUsingProcess Process { get; private set; }

        public SkillActionUsingType UsingType { get; private set; }
    }

    public class Skill : INameable
    {

        public Skill() { }

        public string Name
        {
            get { return name; }
            set { name = value ?? ""; }
        }
        private string name;

        public List<SkillAction> Actions { get; private set; }

        public void AddAction(SkillAction action) {  }
    }
}
