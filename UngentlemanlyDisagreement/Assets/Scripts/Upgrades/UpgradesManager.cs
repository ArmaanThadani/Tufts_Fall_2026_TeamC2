using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class UpgradesManager : MonoBehaviour
{
    private UpgradeCardSpacing upgradeCardSpacing;
    public UpgradesDatabase upgradesDatabase;

    void Awake()
    {
        upgradeCardSpacing = GetComponent<UpgradeCardSpacing>();
    }

    private void Start() {
        StartUpgradePhase();
    }

    void StartUpgradePhase() 
    {
        List<UpgradeCardStats> cards = GetRandomUpgradeCards(9);
        upgradeCardSpacing.SpawnAndCenterCards(cards);

    }

    //draws (count) number of random cards to be displayed
    public List<UpgradeCardStats> GetRandomUpgradeCards(int count)
    {
        List<UpgradeCardStats> randomCards = new List<UpgradeCardStats>();
        
        List<UpgradeCardStats> pool = 
            new List<UpgradeCardStats>(upgradesDatabase.allUpgrades);

        for (int i = 0; i < count; i++)
        {
            int randomIndex = Random.Range(0, pool.Count);
            randomCards.Add(pool[randomIndex]);
        }

        return randomCards;
    }
}
