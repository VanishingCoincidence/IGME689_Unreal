using System;
using System.Collections.Generic;
using UnityEngine;


public class PlaceData
{
    public string Name;
    public double Longitude;
    public double Latitude;
    public int Population;
    public double PopPerMile;
    public string Ocean;
    public string River;
    public List<PlaceData> Connected_Road_Places;
    public List<PlaceData> Connected_Water_Places;

    public int Money = 10;
    public string HomeResource;
    public bool IsPLayer = false;
    
    public PlaceData(string name, double longitude, double latitude, int population, double popPerMile, string ocean, string river)
    {
        
        Name = name;
        Longitude = longitude;
        Latitude = latitude;
        Population = population;
        PopPerMile = popPerMile;
        Ocean = ocean;
        River = river;
        
        Connected_Road_Places = new List<PlaceData>();
        Connected_Water_Places = new List<PlaceData>();
    }
    
    public void ConnectPlaces(PlaceData b)
    {
        // see if the two places are close enough to one another
        double x = b.Latitude - Latitude;
        x = Math.Pow(x, 2);
        
        double y = b.Longitude - Longitude;
        y = Math.Pow(y, 2);

        double distance = Math.Sqrt(x + y);
        distance = Math.Abs(distance);
        
        
        if (!this.River.Equals(" n") && this.River.Equals(b.River))
        {
            this.Connected_Water_Places.Add(b);
            b.Connected_Water_Places.Add(this);
        }
        else if (!this.Ocean.Equals(" n") && this.Ocean.Equals(b.Ocean))
        {
            this.Connected_Water_Places.Add(b);
            b.Connected_Water_Places.Add(this);
        }
        else
        {
            this.Connected_Road_Places.Add(b);
            b.Connected_Road_Places.Add(this);
        }
        
    }

    public bool IsConnectedWater(PlaceData otherPlace)
    {
        return this.Connected_Water_Places.Contains(otherPlace);
    }

    public bool IsConnectedRoad(PlaceData otherPlace)
    {
        return this.Connected_Road_Places.Contains(otherPlace);
    }
    
    
}
