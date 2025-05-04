using UnityEngine;
using Zenject;
using DataModel;

[CreateAssetMenu(fileName = "SettingInstaller", menuName = "Installers/SettingsInstaller")]
public class SettingsInstaller : ScriptableObjectInstaller<SettingsInstaller>
{
    [SerializeField] private GameSettings _gameSettings;

    public override void InstallBindings()
    {
        Container.BindInterfacesAndSelfTo<GameSettings>().FromInstance(_gameSettings);
    }
}