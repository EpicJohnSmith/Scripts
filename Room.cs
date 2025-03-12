using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Room
{
    private Player thePlayer;

    private GameObject[] theDoors;
    private Exit[] availableExits = new Exit[4];
    private int currNumberOfExits = 0;

    private string name;

    public Room(string name)
    {
        this.name = name;
        this.thePlayer = null;
    }

    public string getName()
    {
        return this.name;
    }

public void tryToTakeExit(string direction)
{
    if (this.hasExit(direction))
    {
        // Find the exit that matches the direction
        for (int i = 0; i < this.currNumberOfExits; i++)
        {
            if (String.Equals(this.availableExits[i].getDirection(), direction))
            {
                // Get the destination room from the exit
                Room destinationRoom = this.availableExits[i].getDestination();

                // Remove the player from the current room
                this.thePlayer.setCurrentRoom(null);

                // Place the player in the destination room
                destinationRoom.setPlayer(this.thePlayer);

                // Update the player's current room reference
                this.thePlayer.setCurrentRoom(destinationRoom);

                Debug.Log($"Player moved to {destinationRoom.getName()}");
                return;
            }
        }
    }
    else
    {
        Debug.Log("No Exit In This Direction!");
    }
}

    public bool hasExit(string direction)
    {
        for(int i = 0; i < this.currNumberOfExits; i++)
        {
            if(String.Equals(this.availableExits[i].getDirection(), direction))
            {
                return true;
            }
        }
        return false;
    }
    public void setPlayer(Player p)
    {
        this.thePlayer = p;
        this.thePlayer.setCurrentRoom(this);
    }
    public void addExit(string direction, Room destination)
    {
        if(this.currNumberOfExits <= 3)
        {
            Exit e = new Exit(direction, destination);
            this.availableExits[this.currNumberOfExits] = e;
            this.currNumberOfExits++;
        }
        else
        {
            Console.Error.WriteLine("there are too many exits!!!!");
        }
    }

}