using UnityEngine;

public class Wagon
{
    public string Destination;
    public int Amount;
    public string Resource;
    public int TurnsLeft;
    
    public Wagon(string destination, string resource, int turnsLeft, int amount = 5)
    {
        Destination = destination;
        Resource = resource;
        TurnsLeft = turnsLeft;
        Amount = amount;
    }
}
