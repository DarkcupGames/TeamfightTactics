using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class MinionSpawner : MonoBehaviour
{
    [Inject]
    Minion.Factory minionFactory;
    [Inject]
    Minion.Settings settings;
    private void Start()
    {
        minionFactory.Create(settings);
    }
}
