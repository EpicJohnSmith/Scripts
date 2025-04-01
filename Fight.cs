using UnityEngine;

public class Fight
{
    private Inhabitant attacker;
    private Inhabitant defender;
    private Monster theMonster;
    
    private GameObject playerGO;
    private GameObject monsterGO;
    
    private float timeSinceLastAttack = 0f;
    private float attackInterval = 1.0f; // This will now only apply to monster attacks
    private bool fightInProgress = false;
    private bool fightOver = false;
    private bool waitingForPlayerAction = false;
    
    // New variables for attack types
    public enum AttackType { Normal, Power, Potion }
    private AttackType currentPlayerAttackType = AttackType.Normal;
    
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
            waitingForPlayerAction = false;
        }
        else
        {
            Debug.Log("Player goes first");
            this.attacker = Core.thePlayer;
            this.defender = m;
            waitingForPlayerAction = true;
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
        
        // Show appropriate UI based on who goes first
        if (waitingForPlayerAction)
        {
            sceneManager.ShowPlayerActionUI(true);
            sceneManager.UpdateAttackTypeUI("Normal Attack");
        }
    }
    
    public void UpdateFight(float deltaTime)
    {
        if (!fightInProgress || fightOver)
            return;
        
        // Check for player input if it's the player's turn
        if (waitingForPlayerAction)
        {
            CheckPlayerInput();
            return; // Don't proceed with monster attack logic while waiting for player
        }
        
        // Monster's turn - proceed with timer-based attack
        timeSinceLastAttack += deltaTime;
        
        // It's time for the monster's attack
        if (timeSinceLastAttack >= attackInterval)
        {
            ExecuteMonsterAttack();
            timeSinceLastAttack = 0f;
            
            // Now it's player's turn
            waitingForPlayerAction = true;
            sceneManager.ShowPlayerActionUI(true);
            
            // Reset the attack type UI for next turn
            currentPlayerAttackType = AttackType.Normal;
            sceneManager.UpdateAttackTypeUI("Normal Attack");
        }
    }
    
    private void CheckPlayerInput()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Keypad1))
        {
            currentPlayerAttackType = AttackType.Power;
            Debug.Log("Julian selected Power Attack");
            sceneManager.UpdateAttackTypeUI("Power Attack");
            ExecutePlayerAction();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.Keypad2))
        {
            currentPlayerAttackType = AttackType.Normal;
            Debug.Log("Julian selected Normal Attack");
            sceneManager.UpdateAttackTypeUI("Normal Attack");
            ExecutePlayerAction();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3) || Input.GetKeyDown(KeyCode.Keypad3))
        {
            currentPlayerAttackType = AttackType.Potion;
            Debug.Log("Julian + selected Healing Potion");
            sceneManager.UpdateAttackTypeUI("Healing Potion");
            ExecutePlayerAction();
        }
    }
    
    private void ExecutePlayerAction()
    {
        // Hide the player action UI while executing
        sceneManager.ShowPlayerActionUI(false);
        
        // Execute the selected action
        if (currentPlayerAttackType == AttackType.Potion)
        {
            HealingPotion();
        }
        else
        {
            // Execute normal or power attack
            ExecutePlayerAttack();
        }
        
        // Switch to monster's turn
        waitingForPlayerAction = false;
        
        // Check if fight ended after player's action
        if (fightOver)
            return;
            
        // Reset the timer for monster's next attack
        timeSinceLastAttack = 0f;
        
        // DON'T reset the attack type here - we'll reset it when the monster's turn is over
        // and the player gets their next turn
    }
    
    private void ExecutePlayerAttack()
    {
        int attackRoll = Random.Range(0, 20) + 1;
        int targetAC = defender.getAC();
        
        // Apply attack modifiers for power attack
        if (currentPlayerAttackType == AttackType.Power)
        {
            // Reduce attack roll by 25% for power attack
            attackRoll = Mathf.RoundToInt(attackRoll * 0.75f);
            Debug.Log($"Power Attack roll: {attackRoll} vs AC: {targetAC}");
        }
        
        if (attackRoll >= targetAC)
        {
            // Player hits the monster
            int damage = Random.Range(1, 6); // Base damage is 1-5
            
            // If it's a power attack, increase damage by 50%
            if (currentPlayerAttackType == AttackType.Power)
            {
                damage = Mathf.RoundToInt(damage * 1.5f);
                Debug.Log($"Power Attack deals increased damage: {damage}");
            }
            
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
    }
    
    private void ExecuteMonsterAttack()
    {
        int attackRoll = Random.Range(0, 20) + 1;
        
        if (attackRoll >= defender.getAC())
        {
            // Monster hits the player
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
    }
    
    private void HealingPotion()
    {
        Player player = (Player)Core.thePlayer;
        
        // Calculate healing amount (25% of max health)
        int maxHealth = player.getMaxHP();
        int healAmount = Mathf.RoundToInt(maxHealth * 0.25f);
        
        // Get current health using reflection
        int currentHealth = GetInhabitantHP(player);
        
        // Apply healing without exceeding max health
        int newHealth = Mathf.Min(currentHealth + healAmount, maxHealth);
        int actualHealAmount = newHealth - currentHealth;
        
        // Set the new health value using reflection
        System.Reflection.FieldInfo field = typeof(Inhabitant).GetField("currHp", 
            System.Reflection.BindingFlags.Instance | 
            System.Reflection.BindingFlags.NonPublic);
            
        if (field != null)
        {
            field.SetValue(player, newHealth);
        }
        
        Debug.Log($"{player.getName()} drank a potion and healed for {actualHealAmount} HP!");
        
        // Update the UI
        UpdateHealthUI();
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
        
        // Hide the action UI
        sceneManager.ShowPlayerActionUI(false);
        
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
    
    // Getter for attack type - can be used by UI
    public AttackType GetCurrentPlayerAttackType()
    {
        return currentPlayerAttackType;
    }
    
    // Getter to check if it's player's turn
    public bool IsPlayersTurn()
    {
        return waitingForPlayerAction;
    }
}