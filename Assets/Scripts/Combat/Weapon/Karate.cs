using UnityEngine;

public class Karate : Weapon
{
    [SerializeField] private WeaponConfig _configX;

    private void Awake()
    {
        Init(_configX, GetComponent<IFighterReadOnly>());
    }
}