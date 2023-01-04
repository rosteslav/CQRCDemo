using SQRS.Core.Commands;

namespace SQRS.Core.Infrastucture
{
    public interface ICommandDispatcher
    {
        void RegusterHandler<T>(Func<T, Task> handler) where T : BaseCommand;
        Task SendAsync(BaseCommand command);
    }
}
