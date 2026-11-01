
using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon", menuName = "Game/Weapon")]
public class WeaponDefinition : ScriptableObject
{
    public string weaponName = "New Weapon";

    [Header("Visuals")]
    public GameObject worldPrefab;
    public GameObject heldPrefab;

    [Header("Stats")]
    public float damage = 10f;
}
