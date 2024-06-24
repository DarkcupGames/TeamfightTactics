using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
[CreateAssetMenu(menuName = "Asteroids/GameData")]
public class GameDataMinion : ScriptableObjectInstaller<GameDataMinion>
{
    public MinionSettings Minion;
    [Serializable]
    public class MinionSettings
    {
        public GameInstaller.Settings GameInstaller;
        public Minion.Settings General;
    }
    public override void InstallBindings()
    {
        Container.BindInstance(Minion.GameInstaller);
        Container.BindInstance(Minion.General);
    }

}
