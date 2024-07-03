using Unity.Cinemachine;
using Zenject;

using TLT.CharacterManager;
using TLT.CameraManager;

namespace TLT.Installers
{
  public class GameplaySceneInstaller : MonoInstaller
  {
    public override void InstallBindings()
    {
      Container.Bind<LevelManager>().FromComponentInHierarchy().AsSingle().NonLazy();

      Container.Bind<CameraController>().FromComponentInHierarchy().AsSingle().NonLazy();

      Container.Bind<CameraShake>().FromComponentInHierarchy().AsSingle().NonLazy();

      Container.Bind<CinemachineCamera>().FromComponentInHierarchy().AsSingle().NonLazy();

      Container.Bind<CinemachinePositionComposer>().FromComponentInHierarchy().AsSingle().NonLazy();

      Container.Bind<Character>().FromComponentInHierarchy().AsSingle().NonLazy();
    }
  }
}