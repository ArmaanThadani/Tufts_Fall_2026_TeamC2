using UnityEngine;

public enum UpgradeType 
{
    PunchDamage,    
    Speed,
    MaxHealth       
}

[CreateAssetMenu(fileName = "NewUpgradeCard", 
                 menuName = "Upgrades/CardStats")]
public class UpgradeCardStats : ScriptableObject
{
    public string cardName; 
    [TextArea] public string description;

    public UpgradeType upgradeType;

    //determins the stats of the upgrade if necessary, so the MaxHealth upgrade
    //will use this to determine how much health to give.
    public float upgradeValue1;
    //we might neeed more upgrade values.

}
