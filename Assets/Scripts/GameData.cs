using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
[CreateAssetMenu(menuName = "Asteroids/GameData")]
public class GameData : ScriptableObjectInstaller<GameData>
{
    public GameInstaller.Settings GameInstaller;
    public MinionSettings Minion;
    public class MinionSettings
    {
        public Minion.Settings General;
    }
    public override void InstallBindings()
    {
        Container.BindInstance(Minion.General);
        Container.BindInstance(GameInstaller);
    }

}
