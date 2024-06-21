using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
[CreateAssetMenu(menuName = "Asteroids/GameData")]
public class GameDataMinion : ScriptableObjectInstaller<GameDataMinion>
{
    public GameInstaller.Settings GameInstaller;
    public MinionSettings Minion;
    [Serializable]
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
