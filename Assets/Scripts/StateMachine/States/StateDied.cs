using UnityEngine;

public class StateDied : State
{
    private const string DefaultNameLayerMask = "Default";

    [SerializeField] private GameObject[] _objects—hangeLayerToDefault;

    protected override void EnterAfterAddon()
    {
        foreach (GameObject object—hangeLayerToDefault in _objects—hangeLayerToDefault)
        {
            object—hangeLayerToDefault.layer = LayerMask.NameToLayer(DefaultNameLayerMask);
        }

        Character.enabled = false;
    }
}