using VContainer;
using VContainer.Unity;

namespace Cupboards
{
    public class GameLifetimeScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            builder.Register<ConfigParser>(Lifetime.Singleton);
            builder.Register<FileService>(Lifetime.Singleton);
            builder.Register<PathfindingService>(Lifetime.Singleton);

            builder.RegisterComponentInHierarchy<GameOverWindow>();
        }
    }
}
