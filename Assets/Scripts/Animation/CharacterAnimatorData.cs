using UnityEngine;

public class CharacterAnimatorData
{
    public static class Params
    {
        public static readonly int VelocityX = Animator.StringToHash("VelocityX");
        public static readonly int VelocityZ = Animator.StringToHash("VelocityZ");
        public static readonly int IsAttack = Animator.StringToHash("IsAttack");
        public static readonly int IndexAttack = Animator.StringToHash("IndexAttack");
    }
}