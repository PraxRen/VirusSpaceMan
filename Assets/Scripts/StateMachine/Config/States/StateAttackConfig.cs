using UnityEngine;

[CreateAssetMenu(fileName = "NewStateAttackConfig", menuName = "StateMachine/States/StateAttackConfig")]
public class StateAttackConfig : StateConfig
{
    [SerializeField] private ModeMover _modeMover;
    [SerializeField] private LayerMask _layerMaskSimpleEventAttack;

    public override State CreateState(AICharacter character) => new StateAttack(Id, character, TimeSecondsWaitUpdate, _modeMover, _layerMaskSimpleEventAttack);
}