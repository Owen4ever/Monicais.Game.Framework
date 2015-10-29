namespace Monicais.Core
{
    using System;

    public interface IAction
    {
        void Suspend(IEntity entity);
        void TestAndDoAction(IEntity entity, IActionArgs args);
        void Update();

        bool IsProcessing { get; }

        string Name { get; }
    }
}

