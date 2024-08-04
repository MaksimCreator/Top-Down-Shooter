public class ShotgunBullet : Bullet
{
    public float AngelBullet;

    public ShotgunBullet(float angelBullet,int damage, float bulletDistance) : base(damage, bulletDistance)
    {
        AngelBullet = angelBullet;
    }
}