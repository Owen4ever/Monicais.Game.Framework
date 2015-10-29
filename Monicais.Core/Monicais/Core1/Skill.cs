namespace Monicais.Core
{
    using Monicais.Property;
    using Monicais.ThrowHelper;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;

    [Serializable]
    public class Skill : IAction, IDisplayable, INameable
    {
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Monicais.Property.Attributes <Attributes>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private SkillCallBack <CallBack>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private SkillInfo <Info>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private SkillUpgrader <Upgrader>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private SkillUsingProcess <UsingMode>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private SkillUsingProcess <UsingProcess>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private SkillUsingType <UsingType>k__BackingField;
        private List<IEffect> effects;

        public Skill(SkillInfo info, ISkillCondition condition, SkillUsingType usingType, SkillUsingProcess process, SkillCallBack callback, SkillUpgrader upgrader, List<IEffect> effects, Monicais.Property.Attributes attributes)
        {
            if (info == null)
            {
                ArgumentNull.Throw("info");
            }
            if (process == null)
            {
                ArgumentNull.Throw("process");
            }
            if (callback == null)
            {
                ArgumentNull.Throw("callback");
            }
            if (upgrader == null)
            {
                ArgumentNull.Throw("upgrader");
            }
            if ((effects == null) || (effects.Count == 0))
            {
                ArgumentNull.Throw("effects");
            }
            if (attributes == null)
            {
                ArgumentNull.Throw("attributes");
            }
            this.Info = info;
            this.UsingType = usingType;
            this.UsingProcess = process;
            this.CallBack = callback;
            this.Upgrader = this.Upgrader;
            this.effects = effects;
            this.Attributes = attributes;
        }

        public void Suspend(IEntity entity)
        {
            if (this.UsingProcess.IsProcessing)
            {
                this.UsingProcess.Suspend(entity);
            }
        }

        public void TestAndDoAction(IEntity entity, IActionArgs args)
        {
        }

        public void Update()
        {
        }

        public Monicais.Property.Attributes Attributes { get; private set; }

        public SkillCallBack CallBack { get; private set; }

        public string Description
        {
            get
            {
                return this.Info.Description;
            }
            set
            {
                this.Info.Description = value;
            }
        }

        public List<IEffect> Effects
        {
            get
            {
                return this.effects;
            }
        }

        public SkillInfo Info { get; private set; }

        public bool IsProcessing
        {
            get
            {
                return this.UsingProcess.IsProcessing;
            }
            private set
            {
                this.UsingProcess.IsProcessing = true;
            }
        }

        public string Name
        {
            get
            {
                return this.Info.Name;
            }
            set
            {
                this.Info.Name = value;
            }
        }

        public SkillUpgrader Upgrader { get; private set; }

        public SkillUsingProcess UsingMode { get; private set; }

        public SkillUsingProcess UsingProcess { get; private set; }

        public SkillUsingType UsingType { get; private set; }
    }
}

