using UnityEngine;

public enum WeaponType
{
    Sword,
    DuelingPistol,
    BearTrap
}

[CreateAssetMenu(fileName = "New Weapon", menuName = "Game/Weapon")]
public class WeaponDefinition : ScriptableObject
{
    public string weaponName = "New Weapon";
    public WeaponType weaponType;

    [Header("Visuals")]
    public GameObject worldPrefab;
    public GameObject heldPrefab;

    [Header("Stats")]
    public float damage = 10f;
    public int uses = 1;

    [Header("Sword")]
    public float attackRange = 1.5f;

    [Header("Dueling Pistol")]
    public GameObject bulletPrefab;
    public float bulletSpeed = 25f;

    [Header("Bear Trap")]
    public GameObject trapPrefab;
    public float throwForce = 8f;
    public float throwUpwardForce = 4f;
    public int escapePresses = 12;
}