using System.Linq;
using UnityEngine;

public class HandlerCollide : MonoBehaviour, ICollidable
{
    [SerializeField] private Collider _collider;
    [SerializeField][SerializeInterface(typeof(ICollidable))] private MonoBehaviour[] _collidablesMonoBehaviour;

    private ICollidable[] _collidables;

    public Collider Collider => _collider;

    private void Awake()
    {
        _collidables = _collidablesMonoBehaviour.Cast<ICollidable>().ToArray();
    }

    public void HandleCollide(ISurface surface)
    {
        foreach (var handler in _collidables)
        {
            handler.HandleCollide(surface);
        }
    }
}