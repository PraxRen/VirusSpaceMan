using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class HelperSkinnedMeshRenderer : MonoBehaviour
{

    [SerializeField] private bool _canChangeCurrentBones;
    [SerializeField] private SkinnedMeshRenderer _donorSkinnedMeshRenderer;
    [SerializeField] private SkinnedMeshRenderer _currentSkinnedMeshRenderer;
    [SerializeField] private Transform _donorRoot;
    [SerializeField] private List<Transform> _donorBones;
    [SerializeField] private Transform _root;
    [SerializeField] private List<Transform> _bones;

#if UNITY_EDITOR
    [ContextMenu("FindDonorBones")]
    private void FindDonorBones()
    {
        if (_donorSkinnedMeshRenderer == null)
            return;

        _donorBones = _donorSkinnedMeshRenderer.bones.ToList();
        _donorRoot = _donorSkinnedMeshRenderer.rootBone;
    }

    [ContextMenu("FindCurrentBones")]
    private void FindCurrentBones()
    {
        if (_donorBones == null || _donorRoot == null)
            return;

        if (_root == null)
            return;

        _bones.Clear();
        FindBones(_root);

        foreach (Transform bone in _bones.ToArray())
        {
            Transform donorTransform = _donorBones.FirstOrDefault(donorBone => donorBone.name == bone.name);

            if (donorTransform == null)
            {
                _bones.Remove(bone);
                continue;
            }

            if (_canChangeCurrentBones)
            {
                bone.localPosition = donorTransform.localPosition;
                bone.localRotation = donorTransform.localRotation;
            }
        }

        _bones = _bones.OrderBy(item => _donorBones.FindIndex(refItem => refItem.name == item.name)).ToList();
    }


    [ContextMenu("ResetSkinnedMeshRenderer")]
    private void ResetSkinnedMeshRenderer()
    {
        if (_currentSkinnedMeshRenderer == null)
            return;

        if (_bones == null)
            return;

        _currentSkinnedMeshRenderer.bones = _bones.ToArray();
        _currentSkinnedMeshRenderer.rootBone = _root;
    }

    private void FindBones(Transform parent)
    {
        _bones.Add(parent);

        foreach (Transform child in parent)
        {
            FindBones(child);
        }
    }
#endif
}