using UnityEngine;

public class SimpleVerticalKeypad : MonoBehaviour, IComplexObjectInteraction
{
    [SerializeField] private Transform _startPoint;
    [SerializeField] private float _radiusStartPoint;

    public ITarget StartPoint { get; private set; }
    public int IndexAnimation => 0;
    public TypeAnimationLayer TypeAnimationLayer => TypeAnimationLayer.Default;


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(_startPoint.position, _radiusStartPoint);
    }

    private void Awake()
    {
        StartPoint = new TargetTransform(_startPoint, _radiusStartPoint);
    }

    public void InteractBefore()
    {
        Debug.Log("InteractBefore");
    }
    
    public void Interact()
    {
        Debug.Log("Interact");
    }

    public void InteractAfter()
    {
        Debug.Log("InteractAfter");
    }
}