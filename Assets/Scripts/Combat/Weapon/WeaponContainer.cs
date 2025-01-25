using System.Linq;
using UnityEngine;

public class WeaponContainer : MonoBehaviour
{
    [SerializeField][ReadOnly] private Weapon[] _weapons;

#if UNITY_EDITOR
    [ContextMenu("Find Weapons")]
    private void FindWeapons()
    {
        _weapons = GetComponentsInChildren<Weapon>(true);
    }
#endif

    public Weapon GetWeapon(string idWeapon)
    {
        return _weapons.FirstOrDefault(weapon => weapon.Id == idWeapon);
    }
}