namespace Monicais.Core
{
    using Monicais.ThrowHelper;
    using System;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using UnityEngine;

    [Serializable]
    public sealed class SkillUsingProcess
    {
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool <IsProcessing>k__BackingField;
        private readonly MonoProcess actions;
        private int index;
        private readonly Delegate[] invokeList;
        private int len;

        public SkillUsingProcess(MonoProcess process)
        {
            if (process == null)
            {
                ArgumentNull.Throw("process");
            }
            this.actions = process;
            this.invokeList = process.GetInvocationList();
            this.len = this.invokeList.Length;
            this.Reset();
        }

        public bool MoveNext()
        {
            if (this.IsProcessing)
            {
                if ((this.index + 1) == this.len)
                {
                    this.IsProcessing = false;
                    return false;
                }
                int num = this.index + 1;
                this.index = num;
                return true;
            }
            return false;
        }

        public static SkillUsingProcess operator +(SkillUsingProcess mode, SkillUsingProcess addition)
        {
            return (mode + addition.actions);
        }

        public static SkillUsingProcess operator +(SkillUsingProcess mode, MonoProcess addition)
        {
            return new SkillUsingProcess((MonoProcess) Delegate.Combine(mode.actions, addition));
        }

        public void Reset()
        {
            this.index = 0;
        }

        public void Suspend(IEntity entity)
        {
            if (this.MoveNext())
            {
                this.Current(entity, new Vector3(0f, 0f), true);
            }
            this.IsProcessing = false;
            this.Reset();
        }

        public MonoProcess Current
        {
            get
            {
                return (MonoProcess) this.invokeList[this.index];
            }
        }

        public bool IsProcessing { get; internal set; }

        [Serializable]
        public delegate bool MonoProcess(IEntity player, Vector3 playerObj, bool suspend = false);
    }
}

