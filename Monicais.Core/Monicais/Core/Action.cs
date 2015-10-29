
namespace Monicais.Core
{

    public interface IAction
    {

        void Suspend(IEntity entity);

        void TestAndDoAction(IEntity entity, IActionArgs args);

        void Update(IEntity entity);

        bool IsProcessing { get; }

        string Name { get; }
    }

    public interface IActionArgs { }
}
