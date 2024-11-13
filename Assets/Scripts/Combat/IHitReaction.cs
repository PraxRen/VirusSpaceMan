public interface IHitReaction
{
    void Handle(IWeaponReadOnly weapon, float damage);
}