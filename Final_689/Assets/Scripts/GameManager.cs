using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using Esri.GameEngine.Geometry;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private int currentPlayerPoints = 6;
    private int totalPlayerPoints = 6;
    private int maxCases = 100;
    private int totalCases;
    
    private List<Person> people = new List<Person>();
    public Transform spawnPosition;
    public GameObject personPrefab;
    
    public Camera camera;
    public PlaceManager placeManager;

    public Canvas placeCanvas;
    public TMP_Text placeInfo;
    public TMP_Text currentCaseInfo;
    public TMP_Text connectedInfo;
    
    public Canvas personCanvas;
    public TMP_Text personPlaceInfo;
    public TMP_Text personCurrentCaseInfo;
    public TMP_Text personCanTravelInfo;
    public TMP_Dropdown moveDropdown;
    public Button cureButton;
    public Button researchButton;
    public Button gainToolsButton;
    public Button recruitButton;

    public Canvas tutorialCanvas;
    public Button proceedButton;
    
    public TMP_Text pointsInfo;
    public TMP_Text totalCaseInfo;
    public Button endTurnButton;
    private int cureMod = 1;
    private Place currentPlace = null;

    public Canvas legendCanvas;
    public Button legendButton;

    public Image winImage;
    public Image loseImage;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //placeCanvas.enabled = false;
        //loseImage.enabled = false;
        //winImage.enabled = false;
        //legendCanvas.enabled = false;
        //personCanvas.enabled = false;
        //tutorialCanvas.enabled = true;
        
        //proceedButton.onClick.AddListener(Proceed);
        
        //StartCoroutine(DelayedAction());
    }
    
    public IEnumerator DelayedAction()
    {
        yield return new WaitForSeconds(1.3f); 

        //UpdateTotalCases();
    }
    
    // Update is called once per frame
    void Update()
    {
        if (currentPlayerPoints <= 0)
        {
            //EndTurn();
        }
        
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePosition = Input.mousePosition;
            
            Ray ray = camera.ScreenPointToRay(mousePosition);
            RaycastHit hit;
            
            bool isHit = Physics.Raycast(ray, out hit);

            if (!EventSystem.current.IsPointerOverGameObject())
            {
                if (isHit && hit.collider.gameObject.GetComponent<Place>() != null)
                {
                    //placeCanvas.enabled = true;
                    FixPlaceInfo(hit.collider.gameObject.GetComponent<Place>());
                }
                else if (isHit && hit.collider.gameObject.GetComponent<Person>() != null)
                {
                    //personCanvas.enabled = true;
                }
                else
                {
                    //placeCanvas.enabled = false;
                    //personCanvas.enabled = false;
                }
            }

        }
    }


    void FixPlaceInfo(Place place)
    {
        //foreach (Place p in placeManager.places)
        //{
        //    if (p.placeData.County == place.placeData.County)
        //    {
        //        string placesCanTravel = "Places can travel: ";
        //
        //        foreach (PlaceData placeData in p.placeData.Connected_Places)
        //        {
        //            placesCanTravel += " " + placeData.County + ", " + placeData.State + " |";
        //        }
        //        
        //        connectedInfo.text = placesCanTravel;
        //        placeInfo.text = p.placeData.County + ", " + p.placeData.State;
        //        currentCaseInfo.text = "Cases: " + p.placeData.CurrentCases;
        //        break;
        //    }
        //}
    }

    void UpdatePoints()
    {
        pointsInfo.text = "Points: " + currentPlayerPoints;
    }

    void Proceed()
    {
        tutorialCanvas.enabled = false;
    }
    
    
}
