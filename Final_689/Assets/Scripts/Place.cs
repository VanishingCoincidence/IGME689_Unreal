using Esri.ArcGISMapsSDK.Components;
using Esri.ArcGISMapsSDK.Utils.GeoCoord;
using Esri.GameEngine.Geometry;
using UnityEngine;

public class Place : MonoBehaviour
{
    public PlaceData placeData;
    public ArcGISLocationComponent arcgisLocation;
    
    private Renderer renderer;

    void Awake()
    {
        arcgisLocation = GetComponent<ArcGISLocationComponent>();
        renderer = GetComponent<Renderer>();
    }

    void Start()
    {
        arcgisLocation.Position = new ArcGISPoint(placeData.Longitude, placeData.Latitude, 100, ArcGISSpatialReference.WGS84());
        renderer.material.color = new Color32(179, 33, 52, 255);
    }

    private void OnMouseEnter()
    {
        renderer.material.color = new Color32(220, 89, 135, 255);
    }

    private void OnMouseExit()
    {
        renderer.material.color = new Color32(179, 33, 52, 255);
    }
}
