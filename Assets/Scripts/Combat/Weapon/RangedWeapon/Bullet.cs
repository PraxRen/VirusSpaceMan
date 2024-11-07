public class Bullet : Projectile
{
    protected override void AfterHandleCollideAddon() => Destroy();
}
