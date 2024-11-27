using System;
using System.Collections;
using UnityEngine;

public class PlaceInterest : MonoBehaviour, IReadOnlyPlaceInterest
{
    [SerializeField] private float _radius;

    private Transform _transform;
    private ZoneInterest _zoneInterest;
    private WaitForSeconds _waitUpdateCollision;
    private Coroutine _jobUpdateCollision;

    public event Action EnteredCharacter;

    public bool IsEmpty {  get; private set; }
    public bool HasCharacterInside { get; private set; }
    public IReadOnlyCharacter Character { get; private set; }
    public Vector3 Position => _transform.position;
    public Quaternion Rotation => _transform.rotation;

    private void Awake()
    {
        _transform = transform;
        IsEmpty = true;
    }

    private void OnEnable()
    {
        if (IsEmpty == false) 
        {
            _jobUpdateCollision = StartCoroutine(UpdateCollision());
        }
    }

    private void OnDisable()
    {
        if (_jobUpdateCollision != null)
        {
            StopCoroutine(_jobUpdateCollision);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, _radius);
    }

    public void Initialize(ZoneInterest zoneInterest, WaitForSeconds waitUpdateCollision)
    {
        _zoneInterest = zoneInterest;
        _waitUpdateCollision = waitUpdateCollision;
    }

    public bool CanSetCharacter(IReadOnlyCharacter character)
    {
        return IsEmpty;
    }

    public void SetCharacter(IReadOnlyCharacter character)
    {
        if (IsEmpty == false)
            throw new InvalidOperationException("Place not is empty!");

        Character = character;
        IsEmpty = false;
        _jobUpdateCollision = StartCoroutine(UpdateCollision());
    }

    public void RunInteract()
    {
        if (IsEmpty)
            throw new InvalidOperationException("Place is empty!");

        if (HasCharacterInside == false)
            throw new InvalidOperationException("The character is out of place!");

        Debug.Log("+++");
    }

    public void Clear()
    {
        Character = null;
        IsEmpty = true;
    }

    private IEnumerator UpdateCollision()
    {
        while (IsEmpty == false)
        {
            Collider[] colliders = Physics.OverlapSphere(_transform.position, _radius, _zoneInterest.LayerMaskCharacters, QueryTriggerInteraction.Ignore);
            UpdateCollide(colliders);

            yield return _waitUpdateCollision;
        }

        _jobUpdateCollision = null;
    }

    private void UpdateCollide(Collider[] colliders)
    {
        bool isCollidedCharacter = false;

        foreach (Collider collider in colliders) 
        {
            if (collider.TryGetComponent(out IReadOnlyCharacter character) == false)
                continue;

            if (character != Character)
                continue;

            isCollidedCharacter = true;
            break;
        }

        if (HasCharacterInside == false && isCollidedCharacter)
        {
            EnteredCharacter?.Invoke();
        }
        else if (HasCharacterInside && isCollidedCharacter == false)
        {
            Clear();
        }

        HasCharacterInside = isCollidedCharacter;
    }
}