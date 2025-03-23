using UnityEngine;

public class Fight
{
    private Inhabitant attacker;
    private Inhabitant defender;
    private Monster theMonster;
    
    private GameObject playerGO;
    private GameObject monsterGO;
    
    private float timeSinceLastAttack = 0f;
    private float attackInterval = 1.0f; // Attack every second
    private bool fightInProgress = false;
    private bool fightOver = false;
    
    // Reference to the MonoBehaviour for UI updates
    private FightSceneManager sceneManager;

    public Fight(Monster m, FightSceneManager manager)
    {
        this.theMonster = m;
        this.sceneManager = manager;
        
        // Initially determine who goes first
        int roll = Random.Range(0, 20) + 1;
        if (roll <= 10)
        {
            Debug.Log("Monster goes first");
            this.attacker = m;
            this.defender = Core.thePlayer;
        }
        else
        {
            Debug.Log("Player goes first");
            this.attacker = Core.thePlayer;
            this.defender = m;
        }
    }
    
    public void startFight(GameObject playerGO, GameObject monsterGO)
    {
        this.playerGO = playerGO;
        this.monsterGO = monsterGO;
        
        // Update UI initial state
        UpdateHealthUI();
        
        // Start the fight
        fightInProgress = true;
        fightOver = false;
        
        // Inform the scene manager that the fight has started
        sceneManager.FightStarted(this);
    }
    
    public void UpdateFight(float deltaTime)
    {
        if (!fightInProgress || fightOver)
            return;
            
        timeSinceLastAttack += deltaTime;
        
        // It's time for another attack
        if (timeSinceLastAttack >= attackInterval)
        {
            ExecuteAttack();
            timeSinceLastAttack = 0f;
        }
    }
    
    private void ExecuteAttack()
    {
        int attackRoll = Random.Range(0, 20) + 1;
        
        if (attackRoll >= defender.getAC())
        {
            // Attacker hits the defender
            int damage = Random.Range(1, 6); // 1 to 5 damage
            defender.takeDamage(damage);
            
            Debug.Log($"{attacker.getName()} hit {defender.getName()} for {damage} damage!");
            
            // Update the UI
            UpdateHealthUI();
            
            if (defender.isDead())
            {
                Debug.Log(attacker.getName() + " killed " + defender.getName());
                EndFight();
                return;
            }
        }
        else
        {
            Debug.Log(attacker.getName() + " missed " + defender.getName());
        }
        
        // Swap attacker and defender
        Inhabitant temp = attacker;
        attacker = defender;
        defender = temp;
    }
    
    private void EndFight()
    {
        fightInProgress = false;
        fightOver = true;
        
        if (defender is Player)
        {
            // Player died
            Debug.Log("Player died");
            // End the game
            playerGO.SetActive(false); // Hide the player
        }
        else
        {
            // Monster died
            Debug.Log("Monster died");
            // Remove the monster from the scene
            GameObject.Destroy(monsterGO);
        }
        
        // Inform the scene manager that the fight has ended
        sceneManager.FightEnded();
    }
    
    private void UpdateHealthUI()
    {
        int playerHP = GetInhabitantHP(Core.thePlayer);
        int monsterHP = theMonster.getCurrHP();
        
        // Update health displays through the scene manager
        sceneManager.UpdateHealthUI(playerHP, monsterHP);
    }
    
    // Helper method to get HP from any Inhabitant
    private int GetInhabitantHP(Inhabitant inhabitant)
    {
        // If it's a Monster, use the getCurrHP method
        if (inhabitant is Monster monster)
        {
            return monster.getCurrHP();
        }
        
        System.Reflection.FieldInfo field = typeof(Inhabitant).GetField("currHp", 
            System.Reflection.BindingFlags.Instance | 
            System.Reflection.BindingFlags.NonPublic);
            
        if (field != null)
        {
            return (int)field.GetValue(inhabitant);
        }
        
        // If all else fails, return a default value
        return 20; // Default starting health
    }
    
    public bool IsFightOver()
    {
        return fightOver;
    }
}