using System;
using System.Collections.Generic;
using UnityEngine;


public class PlaceData
{
    public string State;
    public string County;
    public double Longitude;
    public double Latitude;
    public int StartingCases;
    public int CurrentCases;
    public float ChanceToGain;
    public float ChanceToRecruit;
    public List<PlaceData> Connected_Places;
    
    public PlaceData(string state, string county, double longitude, double latitude, float confirmed, float deaths, float incidentRate)
    {
        
        State = state;
        County = county;
        Longitude = longitude;
        Latitude = latitude;
        
        StartingCases = SetStarting(confirmed);
        CurrentCases = StartingCases;
        ChanceToGain = SetGainMore(incidentRate);
        ChanceToRecruit = SetSucceedRecruit(deaths);
        
        Connected_Places = new List<PlaceData>();
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
        
        // also makes sure each place doesn't have a crazy amount of connections
        if (distance < 10 && Connected_Places.Count < 4 && b.Connected_Places.Count < 4)
        {
            this.Connected_Places.Add(b);
            b.Connected_Places.Add(this);
            //
            //Debug.Log(County + " and " + b.County + " connected");
        }
    }

    public bool IsConnected(PlaceData otherPlace)
    {
        return Connected_Places.Contains(otherPlace);
    }

    public int SetStarting(float confirmed)
    {
        // starting amount depends on the confirmed number of COVID cases
        if (confirmed < 300000)
        {
            return 0;
        }
        else if (confirmed < 500000)
        {
            return 1;
        }
        else if (confirmed < 1000000)
        {
            return 2;
        }
        else
        {
            return 3;
        }
    }
    
    public float SetGainMore(float incidentRate)
    {
        // the higher the incident rate, the more likely it is to gain more cases naturally
        if (incidentRate < 25000)
        {
            return 10;
        }
        else if (incidentRate < 35000)
        {
            return 15;
        }
        else if (incidentRate < 38000)
        {
            return 30;
        }
        else
        {
            return 45;
        }
    }
    
    public float SetSucceedRecruit(float deaths)
    {
        // the more deaths there are, the less likely "Recruit" is to succeed
        if (deaths < 1000)
        {
            return 90;
        }
        else if (deaths < 5000)
        {
            return 75;
        }
        else if (deaths < 10000)
        {
            return 50;
        }
        else
        {
            return 40;
        }
    }
    
}
