
using Monicais.ThrowHelper;
using System;

namespace Monicais.Core
{

    public class MonoAction : NonNullDisplayable
    {

        public MonoAction(string name, ActionProcess process) : base(name)
        {
            Process = process;
        }

        public MonoAction(string name, string description, ActionProcess process) : base(name, description)
        {
            Process = process;
        }

        public void Suspend(IEntity entity, ActionSuspendType suspendType)
        {
            Process.Suspend(entity, suspendType);
        }

        public void DoAction(IEntity entity)
        {
            Process.BeginProcess();
        }

        public void Update(IEntity entity)
        {
            if (!IsProcessing)
                if (Process.Current(entity))
                    Process.MoveNext();
        }

        public bool IsProcessing { get { return Process.IsProcessing; } }

        public ActionProcess Process { get; private set; }
    }

    public interface IActionArgs { }

    [Serializable]
    public sealed class ActionProcess
    {

        public ActionProcess(MonoActionProcess process)
        {
            if (process == null)
                ArgumentNull.Throw("process");
            actions = process;
            invokeList = process.GetInvocationList();
            len = invokeList.Length;
            Reset();
        }

        public ActionProcess(params MonoActionProcess[] processes)
        {
            if (processes == null)
                ArgumentNull.Throw("processes");
            foreach (var process in processes)
                if (actions != null)
                    actions += process;
                else
                    actions = process;
            invokeList = actions.GetInvocationList();
            len = invokeList.Length;
            Reset();
        }

        public bool HasNext { get { return index + 1 < len; } }

        public void BeginProcess()
        {
            if (!IsProcessing)
            {
                Reset();
                IsProcessing = true;
            }
        }

        public bool MoveNext()
        {
            if (IsProcessing)
                if ((++index) < len)
                    return IsProcessing = false;
                else
                    return true;
            else
                return false;
        }

        public static ActionProcess operator +(ActionProcess mode, ActionProcess addition)
        {
            return mode + addition.actions;
        }

        public static ActionProcess operator +(ActionProcess mode, MonoActionProcess addition)
        {
            return new ActionProcess(mode.actions + addition);
        }

        public void Reset()
        {
            index = 0;
        }

        public void Suspend(IEntity entity, ActionSuspendType suspendType)
        {
            if (IsProcessing)
            {
                if (MoveNext())
                    Current(entity, (ActionStatus) suspendType);
                IsProcessing = false;
                Reset();
            }
        }

        internal MonoActionProcess Current
        {
            get { return (MonoActionProcess) invokeList[index]; }
        }

        public bool IsProcessing { get; internal set; }

        private readonly MonoActionProcess actions;
        private readonly Delegate[] invokeList;
        private int index;
        private int len;
    }

    [Serializable]
    public delegate bool MonoActionProcess(IEntity entity, ActionStatus status = ActionStatus.CONTINUE);

    [Serializable]
    public enum ActionStatus : uint
    {
        CONTINUE = 0,
        SUSPEND = 1,
        FORCE_SUSPEND = 2
    }

    public enum ActionSuspendType : uint
    {
        NORMAL = ActionStatus.SUSPEND,
        FORCE = ActionStatus.FORCE_SUSPEND
    }
}
