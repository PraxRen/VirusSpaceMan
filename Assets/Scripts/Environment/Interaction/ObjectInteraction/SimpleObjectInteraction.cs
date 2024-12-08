using UnityEngine;

public class SimpleObjectInteraction : MonoBehaviour, IObjectInteraction
{
    [SerializeField] private ObjectInteractionConfig _config;
    [SerializeField] private Transform _startPoint;
    [SerializeField] private Transform _lookAtPoint;
    [SerializeField] private float _radiusStartPoint;

    private GameObject _gameObject;
    private int _indexIteration;

    public ObjectInteractionConfig Config => _config;
    public ITarget StartPoint { get; private set; }
    public ITarget LookAtPoint { get; private set; }
    public int IdIteration => _config.SettingIterations[_indexIteration].Id;
    public int IndexIteration => _indexIteration;
    public int Layer => _gameObject.layer;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(_startPoint.position, _radiusStartPoint);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(_lookAtPoint.position, _radiusStartPoint);
    }

    private void Awake()
    {
        _gameObject = gameObject;
        StartPoint = new TargetTransform(_startPoint, _radiusStartPoint);
        LookAtPoint = new TargetTransform(_lookAtPoint, _radiusStartPoint);
    }

    public void StartInteract() { }

    public void InteractBefore() { }
    
    public void Interact() { }

    public void InteractAfter() 
    {
        int nextIndex = _indexIteration + 1;
        _indexIteration = nextIndex >= _config.SettingIterations.Count ? 0 : nextIndex;
    }

    public void StopInteract()
    {
        _indexIteration = 0;
    }
}