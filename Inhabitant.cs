// Inhabitant.cs
using UnityEngine;

public abstract class Inhabitant
{
    protected int currHp;
    protected int maxHp;
    protected int ac;
    protected string name;

    // Add property for Name to make it accessible
    public string Name { get { return name; } }

    public Inhabitant(string name)
    {
        this.name = name;
        this.maxHp = Random.Range(30, 50);
        this.currHp = this.maxHp;
        this.ac = Random.Range(10, 20);
    }

    // Add IsAlive method
    public bool IsAlive()
    {
        return currHp > 0;
    }

    // Add Attack method
    public virtual void Attack(Inhabitant target)
    {
        // Calculate hit chance based on a d20 roll
        int attackRoll = Random.Range(1, 21);
        
        // If roll is greater than or equal to target's AC, the attack hits
        if (attackRoll >= target.ac)
        {
            // Calculate damage between 5-10
            int damage = Random.Range(5, 11);
            target.currHp -= damage;
            
            // Make sure HP doesn't go below 0
            if (target.currHp < 0)
                target.currHp = 0;
                
            Debug.Log($"{Name} hits {target.Name} for {damage} damage! {target.Name} has {target.currHp} HP left.");
        }
        else
        {
            Debug.Log($"{Name} misses {target.Name}!");
        }
    }
    
    // Add a method to get AC if needed for extensibility
    public int GetAC()
    {
        return ac;
    }
}