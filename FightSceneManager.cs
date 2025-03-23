using System;
using UnityEngine;

public class fightSceneManager : MonoBehaviour
{
    public GameObject player;
    
    public GameObject monster;

    private Monster theMonster;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        this.theMonster = new Monster("Goblin");
        Fight f = new Fight(this.theMonster);
        f.startFight(player, monster); //we need this to be experienced over time, so we need this to be repersented in update
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}