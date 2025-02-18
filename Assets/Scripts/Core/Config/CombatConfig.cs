using UnityEngine;

[System.Serializable]
public class CombatConfig
{
    [SerializeField] private float _maxValueAccuracy;
    [SerializeField] private float _maxValueDamage;
    [SerializeField] private float _maxValueDistance;
    [Range(0f,1f)][SerializeField] private float _maxValueArmor;

    public CombatConfig(float maxValueAccuracy, float maxValueDamage, float maxValueDistance, float maxValueArmor)
    {
        _maxValueAccuracy = maxValueAccuracy;
        _maxValueDamage = maxValueDamage;
        _maxValueDistance = maxValueDistance;
        _maxValueArmor = maxValueArmor;
    }

    public float MaxValueAccuracy => _maxValueAccuracy;
    public float MaxValueDamage => _maxValueDamage;
    public float MaxValueDistance => _maxValueDistance;
    public float MaxValueArmor => _maxValueArmor;
}