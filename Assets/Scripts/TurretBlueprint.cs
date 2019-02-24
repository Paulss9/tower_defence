using System.Collections;
using UnityEngine;

//we don't want it to be a monoBehaviour, but we want to be able to see the fields in the inspector
// if unity doesn't know how to save the data, it can not display it in the inspector
[System.Serializable]
public class TurretBluePrint
{
    public GameObject prefab;
    public int cost;

    public GameObject upgradedPrefab;
    public int upgradeCost;

    public int GetSellAmount ()
    {
        return cost / 2;
    }
}
