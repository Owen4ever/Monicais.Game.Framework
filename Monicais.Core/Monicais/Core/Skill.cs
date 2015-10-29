
using Monicais.Property;
using Monicais.ThrowHelper;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

namespace Monicais.Core
{

    [Serializable]
    public sealed class SkillInfo : NonNullDisplayable
    {
        public SkillInfo(string name) : base(name, null) { }

        public SkillInfo(string name, string description) : base(name, description) { }
    }

    [Serializable]
    public enum SkillUsingType : byte
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
    public sealed class SkillUsingProcess
    {

        private readonly MonoProcess actions;
        private int index;
        private readonly Delegate[] invokeList;
        private int len;

        public SkillUsingProcess(MonoProcess process)
        {
            if (process == null)
                ArgumentNull.Throw("process");
            actions = process;
            invokeList = process.GetInvocationList();
            len = invokeList.Length;
            Reset();
        }

        public bool MoveNext()
        {
            if (IsProcessing)
            {
                if (index + 1 == len)
                    return IsProcessing = false;
                else
                {
                    ++index;
                    return true;
                }
            } else
                return false;
        }

        public static SkillUsingProcess operator +(SkillUsingProcess mode, SkillUsingProcess addition)
        {
            return mode + addition.actions;
        }

        public static SkillUsingProcess operator +(SkillUsingProcess mode, MonoProcess addition)
        {
            return new SkillUsingProcess(mode.actions + addition);
        }

        public void Reset()
        {
            index = 0;
        }

        public void Suspend(IEntity entity)
        {
            if (MoveNext())
                Current(entity, new Vector3(0f, 0f), true);
            IsProcessing = false;
            Reset();
        }

        public MonoProcess Current
        {
            get { return (MonoProcess) invokeList[index]; }
        }

        public bool IsProcessing { get; internal set; }

        [Serializable]
        public delegate bool MonoProcess(IEntity player, Vector3 playerObj, bool suspend = false);
    }

    [Serializable]
    public class SkillArgs : IActionArgs { }

    [Serializable]
    public delegate void SkillCallBack(IEntity entity, int originalDamage, int finalDamage);

    [Serializable]
    public class Skill : IAction, IDisplayable, INameable
    {

        private List<IEffect> effects;

        public Skill(SkillInfo info, ISkillCondition condition, SkillUsingType usingType,
            SkillUsingProcess process, SkillCallBack callback, SkillUpgrader upgrader,
            List<IEffect> effectList, Attributes attributes)
        {
            if (info == null)
                ArgumentNull.Throw("info");
            if (process == null)
                ArgumentNull.Throw("process");
            if (callback == null)
                ArgumentNull.Throw("callback");
            if (upgrader == null)
                ArgumentNull.Throw("upgrader");
            if (effectList == null || effectList.Count == 0)
                ArgumentNull.Throw("effects");
            if (attributes == null)
                ArgumentNull.Throw("attributes");
            Info = info;
            UsingType = usingType;
            UsingProcess = process;
            CallBack = callback;
            Upgrader = upgrader;
            effects = effectList;
            Attributes = attributes;
        }

        public void Suspend(IEntity entity)
        {
            if (UsingProcess.IsProcessing)
                UsingProcess.Suspend(entity);
        }

        public void TestAndDoAction(IEntity entity, IActionArgs args)
        {
        }

        public void Update(IEntity entity)
        {
        }

        public Attributes Attributes { get; private set; }

        public SkillCallBack CallBack { get; private set; }

        public string Description
        {
            get { return Info.Description; }
            set { Info.Description = value; }
        }

        public List<IEffect> Effects
        {
            get { return effects; }
        }

        public SkillInfo Info { get; private set; }

        public bool IsProcessing
        {
            get { return UsingProcess.IsProcessing; }
            private set { UsingProcess.IsProcessing = true; }
        }

        public string Name
        {
            get { return Info.Name; }
            set { Info.Name = value; }
        }

        public SkillUpgrader Upgrader { get; private set; }

        public SkillUsingProcess UsingMode { get; private set; }

        public SkillUsingProcess UsingProcess { get; private set; }

        public SkillUsingType UsingType { get; private set; }
    }
}
