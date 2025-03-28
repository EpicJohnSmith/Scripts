using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FightSceneManager : MonoBehaviour
{
    public GameObject player;
    public GameObject monster;
    
    // UI references
    public TextMeshProUGUI playerHealthText;
    public TextMeshProUGUI monsterHealthText;
    public Slider playerHealthBar;
    public Slider monsterHealthBar;
    
    // Default values - adjust these to match your game design
    private int playerMaxHealth = 20; 
    private int monsterMaxHealth = 15;
    
    private Monster theMonster;
    private Fight currentFight;
    private bool fightActive = false;
    
    void Start()
    {
        // Create the monster
        theMonster = new Monster("Goblin");
        
        // Get initial monster HP for max health reference
        monsterMaxHealth = theMonster.getCurrHP();
        
        // This is a workaround since we don't have direct access to Player's max health
        // Check if there's a "currHp" field in Inhabitant using reflection
        System.Reflection.FieldInfo field = typeof(Inhabitant).GetField("currHp", 
            System.Reflection.BindingFlags.Instance | 
            System.Reflection.BindingFlags.NonPublic);
            
        if (field != null)
        {
            playerMaxHealth = (int)field.GetValue(Core.thePlayer);
        }
        
        // Set up UI
        SetupUI();
        
        // Create and start the fight
        currentFight = new Fight(theMonster, this);
        currentFight.startFight(player, monster);
    }
    
    private void SetupUI()
    {
        // Set max values for health bars
        if (playerHealthBar != null)
        {
            playerHealthBar.maxValue = playerMaxHealth;
            playerHealthBar.value = playerMaxHealth; // Start with full health
        }
        
        if (monsterHealthBar != null)
        {
            monsterHealthBar.maxValue = monsterMaxHealth;
            monsterHealthBar.value = monsterMaxHealth; // Start with full health
        }
        
        // Initialize health text
        if (playerHealthText != null)
            playerHealthText.text = $"{Core.thePlayer.getName()}: {playerMaxHealth}/{playerMaxHealth} HP";
        
        if (monsterHealthText != null)
            monsterHealthText.text = $"{theMonster.getName()}: {monsterMaxHealth}/{monsterMaxHealth} HP";
    }
    
    public void UpdateHealthUI(int playerHP, int monsterHP)
    {
        // Update text
        if (playerHealthText != null)
            playerHealthText.text = $"{Core.thePlayer.getName()}: {playerHP}/{playerMaxHealth} HP";
        
        if (monsterHealthText != null)
            monsterHealthText.text = $"{theMonster.getName()}: {monsterHP}/{monsterMaxHealth} HP";
        
        // Update health bars
        if (playerHealthBar != null)
            playerHealthBar.value = playerHP;
        
        if (monsterHealthBar != null)
            monsterHealthBar.value = monsterHP;
        
        // Debug log to check values
        Debug.Log($"Updating UI - Player HP: {playerHP}/{playerMaxHealth}, Monster HP: {monsterHP}/{monsterMaxHealth}");
    }
    
    public void FightStarted(Fight fight)
    {
        currentFight = fight;
        fightActive = true;
    }
    
    public void FightEnded()
    {
        fightActive = false;
    }
    
    void Update()
    {
        // Update the fight if active
        if (fightActive && currentFight != null)
        {
            currentFight.UpdateFight(Time.deltaTime);
        }
    }
}