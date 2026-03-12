using System.Collections.Generic;

public class Weapon
{
    public List<Attack> Attacks { get; private set; }

    public Weapon(List<Attack> attacks)
    {
        Attacks = attacks;
    }

    public Attack GetAttack(int index = 0)
    {
        if (index < 0 || index >= Attacks.Count)
            return null;
        return Attacks[index];
    }
}