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

    public bool tryToTakeExit(string direction)
    {
        Exit theExit = this.getExit(direction);
        if(theExit != null)
        {
            //remove the player from the current room
            Core.thePlayer.getCurrentRoom().removePlayer();

            //place them in the destination room in that direction
            Room destinationRoom = theExit.getDestination();
            destinationRoom.setPlayer(Core.thePlayer);
            
            //update the room the player is currently in so the room exits visually update
            return true;
        }
        else
        {
            Debug.Log("No Exit In This Direction");
            return false;
        }
    }

    public Exit getExit(string direction)
    {
        if(this.hasExit(direction))
        {
            for(int i = 0; i < this.currNumberOfExits; i++)
            {
                if(String.Equals(this.availableExits[i].getDirection(), direction))
                {
                    return this.availableExits[i];
                }
            }
        }
        return null;
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

    public void removePlayer()
    {
        this.thePlayer = null;
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