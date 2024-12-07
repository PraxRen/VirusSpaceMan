using UnityEngine;

public class SimpleVerticalKeypad : MonoBehaviour, IObjectInteraction
{
    [SerializeField] private ObjectInteractionConfig _config;
    [SerializeField] private Transform _startPoint;
    [SerializeField] private Transform _lookAtPoint;
    [SerializeField] private float _radiusStartPoint;

    private GameObject _gameObject;
    private int _currentIndexAnimations;

    public ObjectInteractionConfig Config => _config;
    public ITarget StartPoint { get; private set; }
    public ITarget LookAtPoint { get; private set; }
    public int AnimationInteractiveIndex => _config.AnimationInteractiveIndexes[_currentIndexAnimations];
    public int Layer => _gameObject.layer;
    public bool IsActive {  get; private set; }

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

    public void StartInteract() 
    {
        IsActive = true;
    }

    public void InteractBefore() { }
    
    public void Interact() { }

    public void InteractAfter() 
    {
        int nextIndex = _currentIndexAnimations + 1;
        _currentIndexAnimations = nextIndex >= _config.AnimationInteractiveIndexes.Count ? 0 : nextIndex;
    }

    public void StopInteract()
    {
        _currentIndexAnimations = 0;
        IsActive = false;
    }
}