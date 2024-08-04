public interface IBulletVisiter
{
    void Visit(DefoltBullet bullet);
    void Visit(ExplosiveBullet bullet);
    void Visit(ShotgunBullet bullet);
}
