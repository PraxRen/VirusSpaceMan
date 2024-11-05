using System.Linq;
using UnityEngine;

public class StorageWeapon : MonoBehaviour
{
    [SerializeField][ReadOnly] private Weapon[] _weapons;

    private void Awake()
    {
        _weapons = GetComponentsInChildren<Weapon>(true);
    }

    public Weapon GetWeapon(string idWeapon)
    {
        return _weapons.FirstOrDefault(weapon => weapon.Id == idWeapon);
    }
}
