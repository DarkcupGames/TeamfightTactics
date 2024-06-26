using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controlls Enemy champions
/// </summary>
public class AIopponent : MonoBehaviour
{
    public ChampionShop championShop;
    public Map map;
    public UIController uIController;
    public GamePlayController gamePlayController;
    public GameObject[,] gridChampionsArray;
    public Dictionary<ChampionType, int> championTypeCount;
    public List<ChampionBonus> activeBonusList;

    public int championDamage = 2;

    public void OnMapReady()
    {
        gridChampionsArray = new GameObject[Map.hexMapSizeX, Map.hexMapSizeZ / 2];
        AddRandomChampion();
        // AddRandomChampion();
    }

    public void OnGameStageComplate(GameStage stage)
    {
        if (stage == GameStage.Preparation)
        {
            for (int x = 0; x < Map.hexMapSizeX; x++)
            {
                for (int z = 0; z < Map.hexMapSizeZ / 2; z++)
                {
                    if (gridChampionsArray[x, z] != null)
                    {
                        ChampionController championController = gridChampionsArray[x, z].GetComponent<ChampionController>();
                        championController.OnCombatStart();
                    }
                }
            }
        }

        if (stage == GameStage.Combat)
        {
            int damage = 0;

            for (int x = 0; x < Map.hexMapSizeX; x++)
            {
                for (int z = 0; z < Map.hexMapSizeZ / 2; z++)
                {
                    if (gridChampionsArray[x, z] != null)
                    {
                        ChampionController championController = gridChampionsArray[x, z].GetComponent<ChampionController>();
                        if (championController.currentHealth > 0)
                            damage += championDamage;
                    }
                }
            }

            gamePlayController.TakeDamage(damage);

            ResetChampions();

            AddRandomChampion();
            //  AddRandomChampion();
        }
    }

    private void GetEmptySlot(out int emptyIndexX, out int emptyIndexZ)
    {
        emptyIndexX = -1;
        emptyIndexZ = -1;

        for (int x = 0; x < Map.hexMapSizeX; x++)
        {
            for (int z = 0; z < Map.hexMapSizeZ / 2; z++)
            {
                if (gridChampionsArray[x, z] == null)
                {
                    emptyIndexX = x;
                    emptyIndexZ = z;
                    break;
                }
            }
        }
    }

    public void AddRandomChampion()
    {
        int indexX;
        int indexZ;
        GetEmptySlot(out indexX, out indexZ);

        if (indexX == -1 || indexZ == -1)
            return;

        Champion champion = championShop.GetRandomChampionInfo();

        GameObject championPrefab = Instantiate(champion.prefab);

        gridChampionsArray[indexX, indexZ] = championPrefab;

        ChampionController championController = championPrefab.GetComponent<ChampionController>();

        championController.Init(champion, ChampionController.TEAMID_AI);

        championController.SetGridPosition(Map.GRIDTYPE_HEXA_MAP, indexX, indexZ + 4);

        championController.SetWorldPosition();
        championController.SetWorldRotation();

        List<ChampionController> championList_lvl_1 = new List<ChampionController>();
        List<ChampionController> championList_lvl_2 = new List<ChampionController>();

        for (int x = 0; x < Map.hexMapSizeX; x++)
        {
            for (int z = 0; z < Map.hexMapSizeZ / 2; z++)
            {
                if (gridChampionsArray[x, z] != null)
                {
                    ChampionController cc = gridChampionsArray[x, z].GetComponent<ChampionController>();
                    if (cc.champion == champion)
                    {
                        if (cc.lvl == 1)
                            championList_lvl_1.Add(cc);
                        else if (cc.lvl == 2)
                            championList_lvl_2.Add(cc);
                    }
                }
            }
        }

        if (championList_lvl_1.Count == 3)
        {
            championList_lvl_1[2].UpgradeLevel();
            Destroy(championList_lvl_1[0].gameObject);
            Destroy(championList_lvl_1[1].gameObject);

            if (championList_lvl_2.Count == 2)
            {
                championList_lvl_1[2].UpgradeLevel();
                Destroy(championList_lvl_2[0].gameObject);
                Destroy(championList_lvl_2[1].gameObject);
            }
        }

        CalculateBonuses();
    }

    private void ResetChampions()
    {
        for (int x = 0; x < Map.hexMapSizeX; x++)
        {
            for (int z = 0; z < Map.hexMapSizeZ / 2; z++)
            {
                if (gridChampionsArray[x, z] != null)
                {
                    ChampionController championController = gridChampionsArray[x, z].GetComponent<ChampionController>();
                    championController.Reset();
                }
            }
        }
    }

    public void Restart()
    {
        for (int x = 0; x < Map.hexMapSizeX; x++)
        {
            for (int z = 0; z < Map.hexMapSizeZ / 2; z++)
            {
                if (gridChampionsArray[x, z] != null)
                {
                    ChampionController championController = gridChampionsArray[x, z].GetComponent<ChampionController>();
                    Destroy(championController.gameObject);
                    gridChampionsArray[x, z] = null;
                }
            }
        }

        AddRandomChampion();
        //AddRandomChampion();
    }

    public void OnChampionDeath()
    {
        bool allDead = IsAllChampionDead();

        if (allDead)
            gamePlayController.EndRound();
    }

    private bool IsAllChampionDead()
    {
        int championCount = 0;
        int championDead = 0;
        for (int x = 0; x < Map.hexMapSizeX; x++)
        {
            for (int z = 0; z < Map.hexMapSizeZ / 2; z++)
            {
                if (gridChampionsArray[x, z] != null)
                {
                    ChampionController championController = gridChampionsArray[x, z].GetComponent<ChampionController>();
                    championCount++;
                    if (championController.isDead)
                        championDead++;
                }
            }
        }

        if (championDead == championCount)
            return true;

        return false;
    }

    private void CalculateBonuses()
    {
        championTypeCount = new Dictionary<ChampionType, int>();

        for (int x = 0; x < Map.hexMapSizeX; x++)
        {
            for (int z = 0; z < Map.hexMapSizeZ / 2; z++)
            {
                if (gridChampionsArray[x, z] != null)
                {
                    Champion c = gridChampionsArray[x, z].GetComponent<ChampionController>().champion;

                    if (championTypeCount.ContainsKey(c.type1))
                    {
                        int cCount = 0;
                        championTypeCount.TryGetValue(c.type1, out cCount);
                        cCount++;
                        championTypeCount[c.type1] = cCount;
                    }
                    else
                    {
                        championTypeCount.Add(c.type1, 1);
                    }

                    if (championTypeCount.ContainsKey(c.type2))
                    {
                        int cCount = 0;
                        championTypeCount.TryGetValue(c.type2, out cCount);
                        cCount++;
                        championTypeCount[c.type2] = cCount;
                    }
                    else
                    {
                        championTypeCount.Add(c.type2, 1);
                    }
                }
            }
        }

        activeBonusList = new List<ChampionBonus>();

        foreach (KeyValuePair<ChampionType, int> m in championTypeCount)
        {
            ChampionBonus championBonus = m.Key.championBonus;

            if (m.Value >= championBonus.championCount)
            {
                activeBonusList.Add(championBonus);
            }
        }
    }
}
