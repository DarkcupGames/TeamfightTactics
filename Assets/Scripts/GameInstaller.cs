using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class GameInstaller : MonoInstaller
{
    [Inject] private Settings settings = null;
    public override void InstallBindings()
    {
        Container.Bind<Gameplay>().FromComponentInHierarchy().AsSingle();
        Container.Bind<AIOpponent>().FromComponentInHierarchy().AsSingle();
        Container.BindFactory<Minion.Settings, Minion, Minion.Factory>().FromComponentInNewPrefab(settings.minionPrefab)
            .WithGameObjectName(settings.minionPrefab.name);
    }
    [Serializable]
    public class Settings
    {
        public GameObject minionPrefab;
    }
}
public interface IGamePlay
{
    GameStage CurrentGameStage { get; set; }
    GameObject[,] GridChampionsArray { get; set; }
    List<ChampionBonus> ActiveBonusList { get; set; }
    bool BuyChampionFromShop(Champion champion);
    void StartDrag();
    void StopDrag();
    void Buylvl();
    void TakeDamage(int damage);
    void RestartGame();
    void EndRound();
    void OnChampionDeath();
}
