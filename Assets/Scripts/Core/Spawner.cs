using System;
using UnityEngine;

public abstract class Spawner<T> : MonoBehaviour where T : MonoBehaviour
{
#if UNITY_EDITOR

    [ReadOnly][SerializeField] private int _countActiveObjects;
    [ReadOnly][SerializeField] private int _countFullObjects;

#endif

    public event Action<T> CreatedSpawnObject;

    public int Capacity { get; private set; }
    public IReadOnlyObjectPool<T> ReadOnlyObjectPool => Pool;
    protected ObjectPool<T> Pool { get; private set; }

    private void Awake()
    {
        AwakeAddon();
    }

    private void OnEnable()
    {
        EnableAddon();
    }

    private void OnDisable()
    {
        DisableAddon();
    }

    private void Start()
    {
        StartAddon();
    }

#if UNITY_EDITOR

    private void Update()
    {
        _countActiveObjects = Pool.CountActiveObjects;
        _countFullObjects = Pool.CountFullObjects;
    }

#endif

    public void Initilize()
    {
        Capacity = InitilizeCapacity();
        Pool = InitilizePool();
    }

    public bool CanSpawn()
    {
        return Pool.CanGet();
    }

    public T Spawn()
    {
        if (Pool.TryGet(out T spawnObject) == false)
        {
            Debug.LogWarning($"{GetType().Name} �� ����� ������������ {typeof(T).Name}! �� �������� ��� � ��� ����������� {typeof(T).Name}!");
        }

        return spawnObject;
    }

    private T CreateSpawnObject()
    {
        T spawnObject = CreateSpawnObjectAddon();
        CreatedSpawnObject?.Invoke(spawnObject);
        return spawnObject;
    }

    protected abstract T CreateSpawnObjectAddon();

    protected abstract void GetSpawnObject(T spawnObject);

    protected abstract void RefundSpawnObject(T spawnObject);

    protected abstract int InitilizeCapacity();

    protected virtual bool TryFindObjectToPoolForGet(T objectToPool)
    {
        return !objectToPool.gameObject.activeSelf;
    }

    protected virtual bool EqualsObjectToPool(T objectToPoolOne, T objectToPoolTwo)
    {
        return objectToPoolOne.Equals(objectToPoolTwo);
    }

    protected virtual void AwakeAddon() { }

    protected virtual void EnableAddon() { }

    protected virtual void DisableAddon() { }

    protected virtual void StartAddon() { }

    private ObjectPool<T> InitilizePool()
    {
        return new ObjectPool<T>(CreateSpawnObject, GetSpawnObject, RefundSpawnObject, TryFindObjectToPoolForGet, EqualsObjectToPool, Capacity);
    }
}