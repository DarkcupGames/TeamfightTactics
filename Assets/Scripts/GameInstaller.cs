using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class GameInstaller : MonoInstaller
{
    [Inject] private GamePlayController gamePlayController;
    public override void InstallBindings()
    {
        Container.Bind<Gameplay>().FromComponentInHierarchy().AsSingle();
        Container.Bind<GameData>().FromComponentInHierarchy().AsSingle();
        Container.BindFactory<Gameplay, FactoryGamePlay>().FromComponentInNewPrefabResource(nameof(GamePlayController)).AsSingle();

    }
    public class Settings
    {
        public GameObject minionPrefab;
    }
}
public class FactoryGamePlay : PlaceholderFactory<Gameplay>
{
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
