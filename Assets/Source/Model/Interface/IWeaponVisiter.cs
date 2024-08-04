public interface IWeaponVisiter
{
    void Visit(Automaton visit);
    void Visit(Gun visit);
    void Visit(Shotgun visit);
    void Visit(GrenadeLauncher visit);
}