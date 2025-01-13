using System.Linq;
using UnityEngine;

public class SwitcherGraphics : MonoBehaviour
{
    [SerializeField] private Transform _mainTransform;
    [ReadOnly][SerializeField] private Graphics[] _graphics;

#if UNITY_EDITOR
    [ContextMenu("Find Graphics")]
    private void FindGraphics()
    {
        _graphics = _mainTransform.GetComponentsInChildren<Graphics>();
    }
#endif

    public void Activate(TypeGraphics typeGraphics)
    {
        Graphics result = _graphics.FirstOrDefault(graphics => graphics.Type == typeGraphics);

        if (result != null)
            result.Activate();
    }

    public void Deactivate(TypeGraphics typeGraphics) 
    {
        Graphics result = _graphics.FirstOrDefault(graphics => graphics.Type == typeGraphics);

        if (result != null)
            result.Deactivate();
    }
}