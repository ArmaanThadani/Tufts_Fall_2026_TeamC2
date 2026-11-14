using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "UpgradesDatabase", 
                 menuName = "Upgrades/UpgradesDatabase")]
public class UpgradesDatabase : ScriptableObject
{
    //now you can make a UpgradeCardStats as an object as an asset
    public List<UpgradeCardStats> allUpgrades 
        = new List<UpgradeCardStats>();
}