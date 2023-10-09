using Zenject;

public class PauseableSceneInstaller : MonoInstaller
{
    public PauseManager singleton_PauseManager;

    public override void InstallBindings()
    {
        // Singletons
        Container.Bind<PauseManager>().FromInstance(singleton_PauseManager);
    }
}
