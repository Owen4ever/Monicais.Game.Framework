
using Monicais.ThrowHelper;
using System;

namespace Monicais.Core
{

    public interface IAction
    {

        void Suspend(IEntity entity);

        void TestAndDoAction(IEntity entity, IActionArgs args);

        void Update(IEntity entity);

        bool IsProcessing { get; }

        ActionUsingProcess Process { get; }

        string Name { get; }
    }

    public interface IActionArgs { }

    [Serializable]
    public sealed class ActionUsingProcess
    {

        private readonly MonoProcess actions;
        private int index;
        private readonly Delegate[] invokeList;
        private int len;

        public ActionUsingProcess(MonoProcess process)
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

        public static ActionUsingProcess operator +(ActionUsingProcess mode, ActionUsingProcess addition)
        {
            return mode + addition.actions;
        }

        public static ActionUsingProcess operator +(ActionUsingProcess mode, MonoProcess addition)
        {
            return new ActionUsingProcess(mode.actions + addition);
        }

        public void Reset()
        {
            index = 0;
        }

        public void Suspend(IEntity entity)
        {
            if (MoveNext())
                Current(entity, true);
            IsProcessing = false;
            Reset();
        }

        public MonoProcess Current
        {
            get { return (MonoProcess) invokeList[index]; }
        }

        public bool IsProcessing { get; internal set; }

        [Serializable]
        public delegate bool MonoProcess(IEntity player, bool suspend = false);
    }
}
