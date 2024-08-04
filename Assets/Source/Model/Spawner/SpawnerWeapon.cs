public class SpawnerWeapon
{
    private readonly IPosition _entity;
    private readonly WeaponViewFactory _factory;

    public SpawnerWeapon(IPosition position, WeaponViewFactory factory)
    {
        _entity = position;
        _factory = factory;
    }

    public void Distroy(Weapon model)
    => _factory.Destroy(model);

    public void Creat(Weapon weapon,bool isParent)
    => _factory.Creat(weapon, _entity.Transform,isParent);
}