using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Zenject;

public class GameInstaller : MonoInstaller<GameInstaller>
{
    private Settings settings;

    public override void InstallBindings()
    {
        Container.Bind<Gameplay>().FromComponentInHierarchy().AsSingle();
        Container.Bind<AIOpponent>().FromComponentInHierarchy().AsSingle();
        Container.Bind<Map>().FromComponentInHierarchy().AsSingle();

        Container.BindFactory<Minion, Minion.Factory>()
                  .FromComponentInNewPrefab(settings.minionPrefab)
                  .WithGameObjectName("Minion")
                  .AsTransient();
    }
    [Inject]
    public void Construct(Settings settings)
    {
        this.settings = settings;
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
