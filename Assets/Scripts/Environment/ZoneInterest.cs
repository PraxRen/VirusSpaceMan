using System.Linq;
using UnityEngine;

public class ZoneInterest : MonoBehaviour
{
    [SerializeField][ReadOnly] private PlaceInterest[] _places;
    [SerializeField] private LayerMask _layerMaskCharacters;
    [SerializeField] private float timeWaitUpdateCollision;
#if UNITY_EDITOR
    [Space(1f)]
    [Header("Gizmos")]
    [SerializeField] private Vector3 _gizmosPositionZone;
    [SerializeField] private Vector3 _gizmosSizeZone;
    [SerializeField] private Color _gizmosColorZone;
#endif

    private WaitForSeconds _waitUpdateCollision;

    public LayerMask LayerMaskCharacters => _layerMaskCharacters;
    public Transform Transform { get; private set; }

#if UNITY_EDITOR
    [ContextMenu("FindPlaces")]
    private void FindPlaces()
    {
        _places = GetComponentsInChildren<PlaceInterest>();
    }
#endif

    private void Awake()
    {
        Transform = transform;
        _waitUpdateCollision = new WaitForSeconds(timeWaitUpdateCollision);

        foreach (PlaceInterest place in _places) 
        {
            place.Initialize(this, _waitUpdateCollision);
        }
    }

    private void OnDrawGizmosSelected()
    {
        float _radiusCenterPoint = 0.15f;
        Vector3 center = transform.position + _gizmosPositionZone;
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(center, _radiusCenterPoint);
        Gizmos.color = _gizmosColorZone;
        Gizmos.DrawCube(center, _gizmosSizeZone);
    }

    public bool TryTakeEmptyPlace(IReadOnlyCharacter character, out IReadOnlyPlaceInterest placeInteres)
    {
        placeInteres = null;

        if (enabled == false)
            return false;

        if (_places.Any(place => place.Character != null && place.Character.Id == character.Id))
            return false;

        PlaceInterest[] places = _places.Where(place => place.CanSetCharacter(character)).ToArray();

        if (places.Length == 0)
            return false;

        PlaceInterest place = places[Random.Range(0, places.Length - 1)];
        place.SetCharacter(character);
        placeInteres = place;

        return true;
    }
}