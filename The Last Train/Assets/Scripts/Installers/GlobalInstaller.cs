using Zenject;

using TLT.Input;
using TLT.CharacterManager;

namespace TLT.Installers
{
  public class GlobalInstaller : MonoInstaller
  {
    public override void InstallBindings()
    {
      Container.Bind<InputHandler>().FromNewComponentOnNewGameObject().AsSingle();
    }
  }
}