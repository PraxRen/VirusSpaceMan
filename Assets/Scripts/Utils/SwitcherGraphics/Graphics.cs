using UnityEngine;

public class Graphics : MonoBehaviour
{
    [SerializeField] private TypeGraphics _type;
    [SerializeField] private Renderer _renderer;

    public TypeGraphics Type => _type;

    public void Activate()
    {
        _renderer.enabled = true;
    }

    public void Deactivate() 
    {
        _renderer.enabled = false;
    }
}