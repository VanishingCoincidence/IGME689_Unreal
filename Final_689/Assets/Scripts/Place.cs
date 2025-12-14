using Esri.ArcGISMapsSDK.Components;
using Esri.ArcGISMapsSDK.Utils.GeoCoord;
using Esri.GameEngine.Geometry;
using UnityEngine;

public class Place : MonoBehaviour
{
    public PlaceData placeData;
    public ArcGISLocationComponent arcgisLocation;
    
    private Renderer renderer;
    
    public Color32 colorDefault = new Color32(179, 33, 52, 255);
    public Color32 colorHover = new Color32(220, 89, 135, 255);
    
    public Color32 colorPlayerDefault = new Color32(75, 0, 130, 255);
    public Color32 colorPlayerHover = new Color32(147, 112, 219, 255);

    void Awake()
    {
        arcgisLocation = GetComponent<ArcGISLocationComponent>();
        renderer = GetComponent<Renderer>();
    }

    void Start()
    {
        arcgisLocation.Position = new ArcGISPoint(placeData.Longitude, placeData.Latitude, 100, ArcGISSpatialReference.WGS84());
        renderer.material.color = colorDefault;
    }

    public void UpdateColor()
    {
        if (placeData.IsPlayer)
        {
            renderer.material.color = new Color32(75, 0, 130, 255); 
        }
        else
        {
            renderer.material.color = colorDefault; 
        }
    }

    private void OnMouseEnter()
    {
        if (placeData.IsPlayer)
        {
            renderer.material.color = new Color32(147, 112, 219, 255); 
        }
        else
        {
            renderer.material.color = colorHover; 
        }
        
    }

    private void OnMouseExit()
    {
        if (placeData.IsPlayer)
        {
            renderer.material.color = new Color32(75, 0, 130, 255); 
        }
        else
        {
            renderer.material.color = colorDefault; 
        }
    }
}
