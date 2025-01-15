using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(Collider))]
public class Limb : MonoBehaviour
{
    private const float Drag = 1.2f;
    private const float AngularDrag = 1.2f;
    private const float Spring = 100f;
    private const string PathToPhysicMaterial = "RagdollDefaultPhysicMaterial";

    public Transform Transform { get; private set; }
    public Rigidbody Rigidbody { get; private set; }
    public Collider Collider { get; private set; }

#if UNITY_EDITOR
    [ContextMenu("ResetDefaultConfig")]
    private void ResetDefaultConfig()
    {
        Rigidbody = GetComponent<Rigidbody>();
        Collider = GetComponent<Collider>();
        CharacterJoint joint = GetComponent<CharacterJoint>();

        if (joint != null)
        {
            joint.enableProjection = true;
            joint.twistLimitSpring = new SoftJointLimitSpring() { spring = Spring };
        }


        Rigidbody.drag = Drag;
        Rigidbody.angularDrag = AngularDrag;
        Rigidbody.collisionDetectionMode = CollisionDetectionMode.Continuous;
        PhysicMaterial ragdollDefaultPhysicMaterial = Resources.Load<PhysicMaterial>(PathToPhysicMaterial);
        Collider.material = ragdollDefaultPhysicMaterial;
        ActivateRagdoll(false);
    }
#endif

    private void Awake()
    {
        Transform = transform;
        Rigidbody = GetComponent<Rigidbody>();
        Collider = GetComponent<Collider>();
    }

    public void ActivateRagdoll(bool isEnable)
    {
        Rigidbody.isKinematic = !isEnable;
        Collider.enabled = isEnable;
    }
}