using System;
using UnityEngine;

public abstract class DisplayerStorageMonoBehaviour<T> : MonoBehaviour, IDisplayerStorage<T> where T : IObjectItem
{
    [SerializeField] private MonoBehaviour _storageMonoBehaviour;
    [SerializeField] private ScriptableObject _displayerSlotFactoryScriptableObject;

    private DisplayerStorage<T> _displayerStorage;
    private IReadOnlyStorage<T> _storage;
    private IDisplayerSlotFactory<T> _displayerSlotFactory;

    public event Action<IReadOnlyDisplayerSlot<T>> ActiveDisplayerSlotChanged;

    private void OnValidate()
    {
        if (_storageMonoBehaviour != null)
        {
            _storage = _storageMonoBehaviour as IReadOnlyStorage<T>;

            if (_storage == null)
            {
                _storageMonoBehaviour = null;
                Debug.LogWarning($"{nameof(_storageMonoBehaviour)} is not {nameof(IReadOnlyStorage<T>)}");
            }
        }

        if (_displayerSlotFactoryScriptableObject != null)
        {
            _displayerSlotFactory = _displayerSlotFactoryScriptableObject as IDisplayerSlotFactory<T>;

            if (_displayerSlotFactory == null)
            {
                _displayerSlotFactoryScriptableObject = null;
                Debug.LogWarning($"{nameof(_displayerSlotFactoryScriptableObject)} is not {nameof(IDisplayerSlotFactory<T>)}");
            }
        }
    }

    private void Awake()
    {
        _displayerStorage = new DisplayerStorage<T>();
    }

    private void OnEnable()
    {
        _displayerStorage.ActiveDisplayerSlotChanged += OnActiveDisplayerSlotChanged;
    }

    private void OnDisable()
    {
        _displayerStorage.ActiveDisplayerSlotChanged -= OnActiveDisplayerSlotChanged;
    }

    private void Start()
    {
        Initilize(_storage, _displayerSlotFactory);
    }

    public IReadOnlyDisplayerSlot<T> ActiveDisplayerSlot => _displayerStorage.ActiveDisplayerSlot;

    public void Initilize(IReadOnlyStorage<T> storage, IDisplayerSlotFactory<T> displayerSlotFactory) => _displayerStorage.Initilize(storage, displayerSlotFactory);

    public void Next() => _displayerStorage.Next();

    public void Previous() => _displayerStorage.Previous();

    private void OnActiveDisplayerSlotChanged(IReadOnlyDisplayerSlot<T> displayerSlot) => ActiveDisplayerSlotChanged?.Invoke(displayerSlot);
}