
using Monicais.ThrowHelper;
using System;

namespace Monicais.Core
{

    public class MonoAction : NonNullDisplayable
    {

        public MonoAction(string name, ActionProcesses process) : base(name)
        {
            Process = process;
        }

        public MonoAction(string name, string description, ActionProcesses process) : base(name, description)
        {
            Process = process;
        }

        public void Suspend(IEntity entity, ActionSuspendType suspendType)
        {
            Process.Suspend(entity, suspendType);
        }

        public void Begin(IEntity entity)
        {
            Process.BeginProcess();
            EntityToDo = entity;
        }

        public void Update()
        {
            if (!IsProcessing)
                if (Process.Current(EntityToDo))
                    Process.MoveNext();
        }

        public bool IsProcessing { get { return Process.IsProcessing; } }

        public ActionProcesses Process { get; private set; }

        private IEntity EntityToDo { get; set; }
    }

    public interface IActionArgs { }

    [Serializable]
    public sealed class ActionProcesses
    {

        public ActionProcesses(MonoActionProcess process)
        {
            if (process == null)
                ArgumentNull.Throw("process");
            actions = process;
            invokeList = process.GetInvocationList();
            len = invokeList.Length;
            Reset();
        }

        public ActionProcesses(params MonoActionProcess[] processes)
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

        public ActionProcesses(params IMonoActionProcess[] processes)
        {
            if (processes == null)
                ArgumentNull.Throw("processes");
            foreach (var process in processes)
                if (actions != null)
                    actions += process.MonoActionProcess;
                else
                    actions = process.MonoActionProcess;
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

        public static ActionProcesses operator +(ActionProcesses mode, ActionProcesses addition)
        {
            return mode + addition.actions;
        }

        public static ActionProcesses operator +(ActionProcesses mode, MonoActionProcess addition)
        {
            return new ActionProcesses(mode.actions + addition);
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

    public interface IMonoActionProcess
    {

        bool MonoActionProcess(IEntity entity, ActionStatus status = ActionStatus.CONTINUE);
    }

    [Serializable]
    public enum ActionStatus : uint
    {
        BEGIN = 1,
        CONTINUE = 2,
        SUSPEND = 4,
        FORCE_SUSPEND = 8
    }

    public enum ActionSuspendType : uint
    {
        NORMAL = ActionStatus.SUSPEND,
        FORCE = ActionStatus.FORCE_SUSPEND
    }
}
