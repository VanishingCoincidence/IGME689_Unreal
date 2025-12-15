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
using UnityEngine.Rendering;

public class PlaceManager : MonoBehaviour
{
    [Header("ArcGIS")]
    public ArcGISMapComponent arcgisMap;
    public Transform spawnPosition;
    
    [Header("Prefabs")]
    public GameObject placePrefab;
    public GameObject riverPrefab;
    public GameObject roadPrefab;
    
    [Header("Data")]
    private DataLoader dataLoader;
    public List<Place> places;
    
    //public List<Person> people = new List<Person>();
    //public GameObject personPrefab;

    void Awake()
    {
        arcgisMap = FindFirstObjectByType<ArcGISMapComponent>();
        places = new List<Place>();
        dataLoader = new DataLoader();
        StartCoroutine(dataLoader.GetCityFeatures());
        StartCoroutine(dataLoader.GetRiverFeatures());
        StartCoroutine(dataLoader.GetRoadFeatures());
    }

    void Start()
    {
        // this would sometimes not load, so this forced it to wait until (hopefully) GetFeatures is done
        StartCoroutine(DelayedAction());
    }
    
    public IEnumerator DelayedAction()
    {
        yield return new WaitForSeconds(5f); 

        SpawnPlaces();
        SpawnRoads();
        SpawnRivers();
    }

    private void SpawnPlaces()
    {
        
        foreach (var city in dataLoader.placeDataList)
        {
            Place place = Instantiate(placePrefab, spawnPosition).GetComponent<Place>();
            place.placeData = city;
            
            places.Add(place);
        }
    }

    
    private void SpawnRivers()
    {
        foreach (var river in dataLoader.riverList)
        {
            foreach (var riverList in river.CoordinatesList)
            {
                LineRenderer connectionPath = Instantiate(riverPrefab, spawnPosition).GetComponent<LineRenderer>();
                connectionPath.positionCount = riverList.Count;
                
                int count = 0;

                foreach(var coord in riverList)
                {
                    var arcPosition = new ArcGISPoint(coord[0], coord[1], 300, ArcGISSpatialReference.WGS84());
                    var position = arcgisMap.GeographicToEngine(arcPosition);
                
                    connectionPath.SetPosition(count, position);

                    count++;
                }
            }
            
        }
    }
    private void SpawnRoads()
    {
        foreach (var road in dataLoader.roadList)
        {
            LineRenderer connectionPath = Instantiate(roadPrefab, spawnPosition).GetComponent<LineRenderer>();
            connectionPath.positionCount = road.Coordinates.Count;
            
            int count = 0;
            
            foreach (var coordinate in road.Coordinates)
            {
                var arcPosition = new ArcGISPoint(coordinate[0], coordinate[1], 300, ArcGISSpatialReference.WGS84());
                var position = arcgisMap.GeographicToEngine(arcPosition);
                
                connectionPath.SetPosition(count, position);
            
                count++;
            }
        }
    }
    
    
}
