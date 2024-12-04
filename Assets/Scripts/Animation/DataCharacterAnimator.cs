using UnityEngine;

public class DataCharacterAnimator
{
    public static class Params
    {
        public static readonly int VelocityX = Animator.StringToHash("VelocityX");
        public static readonly int VelocityZ = Animator.StringToHash("VelocityZ");
        public static readonly int IsAttack = Animator.StringToHash("IsAttack");
        public static readonly int IndexAttack = Animator.StringToHash("IndexAttack");
        public static readonly int IsInteractive = Animator.StringToHash("IsInteractive");
        public static readonly int IndexInteractive = Animator.StringToHash("IndexInteractive");
    }

    public static class Names
    {
        public const string FaceUpStandUp = "FaceUpStandUp";
        public const string FaceDownStandUp = "FaceDownStandUp";
    }
}