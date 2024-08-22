namespace Model
{
    public class Inventary : IInventaryService
    {
        public Weapon Weapon { get; private set; }

        public Inventary BindWeapon(Weapon weapon)
        {
            Weapon = weapon;
            return this;
        }
    }
}