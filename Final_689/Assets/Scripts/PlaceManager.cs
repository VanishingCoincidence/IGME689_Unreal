using Esri.ArcGISMapsSDK.Components;
using Esri.ArcGISMapsSDK.Utils.GeoCoord;
using Esri.GameEngine.Geometry;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System;
using UnityEngine.Networking;
using UnityEngine;

public class PlaceManager : MonoBehaviour
{
    public ArcGISMapComponent arcgisMap;
    public GameObject placePrefab;
    public GameObject linePrefab;
    
    private DataLoader dataLoader;
    public List<Place> places;
    public Transform spawnPosition;
    
    public List<Person> people = new List<Person>();
    public GameObject personPrefab;

    void Awake()
    {
        arcgisMap = FindFirstObjectByType<ArcGISMapComponent>();
        places = new List<Place>();
        dataLoader = new DataLoader();
        StartCoroutine(dataLoader.GetFeatures());
    }

    void Start()
    {
        // this would sometimes not load, so this forced it to wait until (hopefully) GetFeatures is done
        StartCoroutine(DelayedAction());
    }
    
    public IEnumerator DelayedAction()
    {
        yield return new WaitForSeconds(1f); 

        SpawnPlaces();
        SpawnConnections();
        InitialPersonSpawn();
    }

    private void SpawnPlaces()
    {
        foreach (var county in dataLoader.placeDataList)
        {
            Place place = Instantiate(placePrefab, spawnPosition).GetComponent<Place>();
            place.placeData = county;
            places.Add(place);
        }
    }
    
    public void SpawnPerson(string county)
    {
        foreach (Place p in places)
        {
            if (p.placeData.County == county)
            {
                Person person = Instantiate(personPrefab, spawnPosition).GetComponent<Person>();
                person.currentCounty = p;
                person.latitude = p.placeData.Latitude;
                person.longitude = p.placeData.Longitude;
                people.Add(person);
                break;
            }

        }
    }
    
    void InitialPersonSpawn()
    {
        // first two people spawn in Harris county and Washington DC
        SpawnPerson(" Harris");
        SpawnPerson(" District of Columbia");
    }
    
    private void SpawnConnections()
    {
        // go through each county
        foreach (var county in dataLoader.placeDataList)
        {
            // go through each county that the county being looked at is connected to
            foreach (var connection in county.Connected_Places)
            {
                LineRenderer connectionPath = Instantiate(linePrefab, spawnPosition).GetComponent<LineRenderer>();
                
                var arcPosition = new ArcGISPoint(county.Longitude, county.Latitude, 90, ArcGISSpatialReference.WGS84());
                var position = arcgisMap.GeographicToEngine(arcPosition);
                var arcPosition2 = new ArcGISPoint(connection.Longitude, connection.Latitude, 90, ArcGISSpatialReference.WGS84());
                var position2 = arcgisMap.GeographicToEngine(arcPosition2);
                
                // draw a line between the two counties
                connectionPath.positionCount = 2;
                connectionPath.SetPosition(0, position);
                connectionPath.SetPosition(1, position2);
                //Debug.Log(position + " " + position2);
            }
        }
    }
    
}
