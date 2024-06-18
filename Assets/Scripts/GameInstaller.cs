using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class GameInstaller : MonoInstaller
{
    [Inject] private GamePlayController gamePlayController;
    public override void InstallBindings()
    {
        Container.Bind<GamePlayController>().FromComponentInHierarchy().AsSingle();
        Container.Bind<GameData>().FromComponentInHierarchy().AsSingle();
        Container.BindFactory<GamePlayController, FactoryGamePlayController>().FromComponentInNewPrefabResource(nameof(GamePlayController)).AsSingle();

    }
}
public class FactoryGamePlayController : PlaceholderFactory<GamePlayController>
{
}
public interface IGamePlayController
{
    GameStage CurrentGameStage { get; set; }
    GameObject[,] GridChampionsArray { get; set; }
    List<ChampionBonus> ActiveBonusList { get; set; }
    bool BuyChampionFromShop(Champion1 champion);
    void StartDrag();
    void StopDrag();
    void Buylvl();
    void TakeDamage(int damage);
    void RestartGame();
    void EndRound();
    void OnChampionDeath();
}
