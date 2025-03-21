using UnityEngine;

public class Fight
{
    private Inhabitant attacker;
    private Inhabitant defender;

    public Fight()
    {
        int roll = Random.Range(0, 20) + 1;
        if (roll <= 10)
        {
            Debug.Log("Monster goes first");
        }
        else
        {
            Debug.Log("Player goes first");
        }

    }

    public void startFight()
{
    while (attacker.IsAlive() && defender.IsAlive())
    {
        attacker.Attack(defender);

        if (!defender.IsAlive())
        {
            Debug.Log($"{defender.Name} has died. {attacker.Name} wins!");
            break;
        }

        // Swap roles for the next round
        Inhabitant temp = attacker;
        attacker = defender;
        defender = temp;
    }
}
}