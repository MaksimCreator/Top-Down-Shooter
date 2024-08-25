namespace Model
{
    public class Inventary : IService
    {
        public Weapon Weapon { get; private set; }

        public Inventary BindWeapon(Weapon weapon)
        {
            Weapon = weapon;
            return this;
        }
    }
}