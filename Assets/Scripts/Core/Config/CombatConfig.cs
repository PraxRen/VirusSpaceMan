using UnityEngine;

[System.Serializable]
public class CombatConfig
{
    [SerializeField] private float _maxValueAccuracy;
    [SerializeField] private float _maxValueDamage;
    [SerializeField] private float _maxValueDistance;

    public CombatConfig(float maxValueAccuracy, float maxValueDamage, float maxValueDistance)
    {
        _maxValueAccuracy = maxValueAccuracy;
        _maxValueDamage = maxValueDamage;
        _maxValueDistance = maxValueDistance;
    }

    public float MaxValueAccuracy => _maxValueAccuracy;
    public float MaxValueDamage => _maxValueDamage;
    public float MaxValueDistance => _maxValueDistance;
}