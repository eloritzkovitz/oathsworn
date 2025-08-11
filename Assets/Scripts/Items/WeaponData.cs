using UnityEngine;

[CreateAssetMenu(fileName = "NewWeapon", menuName = "Weapons/Weapon")]
public class WeaponData : ScriptableObject
{
    public string weaponName = "New Weapon";
    public GameObject weaponPrefab;
    public int damage;
    public Sprite icon;
}
