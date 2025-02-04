using System.Collections;
using System.Linq;
using System;
using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;

public class SwitcherRagdoll : MonoBehaviour
{
    [SerializeField] private Mover _mover;
    [SerializeField] private Animator _animator;
    [SerializeField] private NavMeshAgent _agent;
    [SerializeField] private Collider _collider;
    [SerializeField] private SwitcherAnimationLayer _switcherAnimationLayer;
    [SerializeField] private Transform _hipsBone;
    [SerializeField] private Limb[] _limbs;
    [SerializeField] private float _timeToResetBones;
    [SerializeField] private float _timeWaitAlignPositionToHips;
    [SerializeField] private LayerMask _ragdollLayer;
    [SerializeField] private Vector3 _offsetStartPositionRaycast;

    private Transform _transform;
    private Coroutine _jobAlignPositionToHips;
    private Coroutine _jobWaitResetBones;
    private BoneTransform[] _faceUpStandUpBoneTransforms;
    private BoneTransform[] _faceDownStandUpBoneTransforms;
    private BoneTransform[] _ragdollBoneTransforms;
    private Transform[] _bones;
    private float _elapsedResetBonesTime;
    private bool _isFacingUp;
    private WaitForSeconds _waitAlignPositionToHips;
    private bool _isUpdateAlignTransformToHips;

    private LayerMask _groundLayer;

    public event Action Activated;
    public event Action Deactivated;

    public bool IsActivated { get; private set; }
    public bool IsWaitAnimationStandUp { get; private set; }

    private void Awake()
    {
        _transform = transform;
        _groundLayer = _mover.GroundLayer;
        _groundLayer &= ~_ragdollLayer;
        _waitAlignPositionToHips = new WaitForSeconds(_timeWaitAlignPositionToHips);
        _bones = _hipsBone.GetComponentsInChildren<Transform>();
        _faceUpStandUpBoneTransforms = new BoneTransform[_bones.Length];
        _faceDownStandUpBoneTransforms = new BoneTransform[_bones.Length];
        _ragdollBoneTransforms = new BoneTransform[_bones.Length];

        for (int i = 0; i < _bones.Length; i++)
        {
            _faceUpStandUpBoneTransforms[i] = new BoneTransform();
            _faceDownStandUpBoneTransforms[i] = new BoneTransform();
            _ragdollBoneTransforms[i] = new BoneTransform();
        }

        PopulateAnimationStartBoneTransforms(DataCharacterAnimator.Names.FaceUpStandUp, _faceUpStandUpBoneTransforms);
        PopulateAnimationStartBoneTransforms(DataCharacterAnimator.Names.FaceDownStandUp, _faceDownStandUpBoneTransforms);
    }

    private void OnDisable()
    {
        if (_jobWaitResetBones != null)
        {
            StopCoroutine(_jobWaitResetBones);
            _jobWaitResetBones = null;
        }

        if (_jobAlignPositionToHips != null)
        {
            StopCoroutine(_jobAlignPositionToHips);
            _jobAlignPositionToHips = null;
        }
    }

    public void SetIgnoreColliders(IEnumerable<Collider> colliders, bool isIgnore, float timeReset = 0f)
    {
        foreach (Collider collider in colliders)
        {
            foreach (Limb limb in _limbs)
            {
                Physics.IgnoreCollision(collider, limb.Collider, isIgnore);
            }
        }

        if (timeReset > 0)
        {
            StartCoroutine(RunTimerForResetIgnoreCollision(colliders, timeReset));
        }
    }

    public void ApplyHit(Vector3 force, Vector3 hitPoint)
    {
        if (IsActivated == false)
            return;

        Rigidbody hitRigidbody = _limbs.OrderBy(limb => Vector3.Distance(limb.Transform.position, hitPoint)).First().Rigidbody;
        hitRigidbody.AddForceAtPosition(force, hitPoint, ForceMode.Impulse);
    }

    public void Activete()
    {
        if (_jobWaitResetBones != null)
        {
            StopCoroutine(_jobWaitResetBones);
            _jobWaitResetBones = null;
        }

        IsActivated = true;
        _mover.enabled = false;
        _animator.enabled = false;
        _collider.enabled = false;

        if (_agent != null)
            _agent.enabled = false;

        foreach (Limb limb in _limbs)
            limb.ActivateRagdoll(true);

        if (_jobAlignPositionToHips != null)
        {
            StopCoroutine(_jobAlignPositionToHips);
            _jobAlignPositionToHips = null;
        }

        _jobAlignPositionToHips = StartCoroutine(AlignTransformToHips());
        Activated?.Invoke();
    }

    public void Deactivete()
    {
        if (IsActivated == false)
            return;

        _jobWaitResetBones = StartCoroutine(WaitResetBones());
    }

    private IEnumerator AlignTransformToHips()
    {
        _isUpdateAlignTransformToHips = true;

        while (_isUpdateAlignTransformToHips)
        {
            Vector3 originalHipsPosition = _hipsBone.position;
            Quaternion originalHipsRotation = _hipsBone.rotation;

            // Вычисление направления для поворота персонажа
            _isFacingUp = _hipsBone.up.y > 0;
            Vector3 desiredDirection = _isFacingUp ? _hipsBone.right : -_hipsBone.right;
            desiredDirection.y = 0;
            desiredDirection.Normalize();
            Quaternion targetRotation = Quaternion.LookRotation(desiredDirection);
            _transform.rotation = Quaternion.Euler(0, targetRotation.eulerAngles.y, 0);
            _transform.position = _hipsBone.position;

            // Коррекция позиции относительно костей standUp анимации
            Vector3 positionOffset = GetStandUpBonesTransform()[0].Position;
            positionOffset.y = 0;
            positionOffset = _transform.rotation * positionOffset;
            _transform.position -= positionOffset;
            UpdateCorrectPositionY();

            _hipsBone.position = originalHipsPosition;
            _hipsBone.rotation = originalHipsRotation;

            yield return _waitAlignPositionToHips;
        }

        UpdateCorrectPositionY();
        _jobAlignPositionToHips = null;
    }

    private void UpdateCorrectPositionY()
    {
        if (Physics.Raycast(_transform.position + _offsetStartPositionRaycast, Vector3.down, out RaycastHit raycastHit, Mathf.Infinity, _groundLayer))
        {
            _transform.position = new Vector3(_transform.position.x, raycastHit.point.y, _transform.position.z);
        }
    }

    private IEnumerator WaitResetBones()
    {
        _isUpdateAlignTransformToHips = false;
        yield return _jobAlignPositionToHips;

        PopulateBoneTransform(_ragdollBoneTransforms);
        _elapsedResetBonesTime = 0f;
        float elapsedPercentage = 0f;
        BoneTransform[] standUpBonesTransform = GetStandUpBonesTransform();

        while (elapsedPercentage < 1)
        {
            _elapsedResetBonesTime += Time.deltaTime;
            elapsedPercentage = _elapsedResetBonesTime / _timeToResetBones;

            for (int i = 0; i < _bones.Length; i++)
            {
                _bones[i].localPosition = Vector3.Lerp(_ragdollBoneTransforms[i].Position, standUpBonesTransform[i].Position, elapsedPercentage);
                _bones[i].localRotation = Quaternion.Slerp(_ragdollBoneTransforms[i].Rotation, standUpBonesTransform[i].Rotation, elapsedPercentage);
            }

            yield return null;
        }

        RunAnimationStandUp();
        _jobWaitResetBones = null;
    }

    private void RunAnimationStandUp()
    {
        //_collider.enabled = true;
        _animator.enabled = true;
        int indexAnimatorLayer = _switcherAnimationLayer.GetIndexCurrentSetting();
        _animator.Play(GetNameAnimationStandUp(), indexAnimatorLayer, 0);
        IsWaitAnimationStandUp = true;
    }

    private void Cancel()
    {
        foreach (Limb limb in _limbs)
            limb.ActivateRagdoll(false);

        if (_agent != null)
            _agent.enabled = true;

        _collider.enabled = true;
        _mover.enabled = true;
        IsActivated = false;
    }

    private void PopulateBoneTransform(BoneTransform[] boneTransforms)
    {
        for (int i = 0; i < _bones.Length; i++)
        {
            boneTransforms[i].Position = _bones[i].localPosition;
            boneTransforms[i].Rotation = _bones[i].localRotation;
        }
    }

    private void PopulateAnimationStartBoneTransforms(string clipName, BoneTransform[] boneTransforms)
    {
        Vector3 positionBeforeSampling = _transform.position;
        Quaternion rotationBeforeSampling = _transform.rotation;

        foreach (AnimationClip clip in _animator.runtimeAnimatorController.animationClips)
        {
            if (clip.name == clipName)
            {
                clip.SampleAnimation(gameObject, 0);
                PopulateBoneTransform(boneTransforms);
                break;
            }
        }

        _transform.position = positionBeforeSampling;
        _transform.rotation = rotationBeforeSampling;
    }

    private string GetNameAnimationStandUp() => _isFacingUp ? DataCharacterAnimator.Names.FaceUpStandUp : DataCharacterAnimator.Names.FaceDownStandUp;

    private BoneTransform[] GetStandUpBonesTransform() => _isFacingUp ? _faceUpStandUpBoneTransforms : _faceDownStandUpBoneTransforms;

    private IEnumerator RunTimerForResetIgnoreCollision(IEnumerable<Collider> colliders, float timeReset)
    {
        yield return new WaitForSeconds(timeReset);
        SetIgnoreColliders(colliders, false);
    }

    //AnimationEvent
    private void OnStopDeactivatedAnimationEvent()
    {
        if (IsWaitAnimationStandUp == false)
            return;

        Cancel();
        IsWaitAnimationStandUp = false;
        Deactivated?.Invoke();
    }
}