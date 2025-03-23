using Unity.VisualScripting;
using UnityEngine;

public class Monster : Inhabitant
{
    public Monster(string name) : base(name)
    {
    }

    public int getCurrHP()
    {
        return this.currHp;
    }

    public string getName()
    {
        return this.name;
    }
}