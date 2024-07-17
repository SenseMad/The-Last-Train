using Zenject;

using TLT.Input;
using TLT.Save;

namespace TLT.Installers
{
  public class GlobalInstaller : MonoInstaller
  {
    public override void InstallBindings()
    {
      Container.Bind<InputHandler>().FromNewComponentOnNewGameObject().AsSingle();

      Container.Bind<GameManager>().FromNewComponentOnNewGameObject().AsSingle();

      Container.Bind<LoadingScene>().FromNewComponentOnNewGameObject().AsSingle();
    }
  }
}