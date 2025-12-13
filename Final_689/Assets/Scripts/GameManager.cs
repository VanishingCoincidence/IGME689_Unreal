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
        placeCanvas.enabled = false;
        loseImage.enabled = false;
        winImage.enabled = false;
        legendCanvas.enabled = false;
        personCanvas.enabled = false;
        tutorialCanvas.enabled = true;
        
        endTurnButton.onClick.AddListener(EndTurn);
        legendButton.onClick.AddListener(ToggleLegend);
        proceedButton.onClick.AddListener(Proceed);
        
        moveDropdown.onValueChanged.AddListener(Move);
        cureButton.onClick.AddListener(Cure);
        researchButton.onClick.AddListener(Research);
        gainToolsButton.onClick.AddListener(GainTools);
        recruitButton.onClick.AddListener(Recruit);
        
        StartCoroutine(DelayedAction());
    }
    
    public IEnumerator DelayedAction()
    {
        yield return new WaitForSeconds(1.3f); 

        UpdateTotalCases();
    }
    
    // Update is called once per frame
    void Update()
    {
        if (currentPlayerPoints <= 0)
        {
            EndTurn();
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
                    placeCanvas.enabled = true;
                    FixPlaceInfo(hit.collider.gameObject.GetComponent<Place>());
                }
                else if (isHit && hit.collider.gameObject.GetComponent<Person>() != null)
                {
                    personCanvas.enabled = true;
                    FixPersonInfo(hit.collider.gameObject.GetComponent<Person>());
                }
                else
                {
                    placeCanvas.enabled = false;
                    personCanvas.enabled = false;
                }
            }

        }
    }


    void Move(int index)
    {
        string selectedCounty = moveDropdown.options[index].text;
        int placetoGoIndex = 0;
        int personToMoveIndex = 0;

        for (int i = 0; i < placeManager.places.Count; i++)
        {
            if (placeManager.places[i].placeData.County == selectedCounty)
            {
                placetoGoIndex = i;

                for (int j = 0; j < placeManager.people.Count; j++)
                {
                    if (placeManager.people[j].currentCounty.placeData.County == currentPlace.placeData.County)
                    {
                        personToMoveIndex = j;
                        //Debug.Log(placeManager.people[j].currentCounty.placeData.County);
                        break;
                    }
                }

                break;
            }
        }
        
        placeManager.people[personToMoveIndex].Move(placeManager.places[placetoGoIndex].placeData.Longitude, placeManager.places[placetoGoIndex].placeData.Latitude);
        placeManager.people[personToMoveIndex].currentCounty = placeManager.places[placetoGoIndex];
        
        currentPlayerPoints--;
        UpdatePoints();
        personCanvas.enabled = false;
    }

    void GainTools()
    {
        if (currentPlayerPoints >= 5)
        {
            personCanvas.enabled = false;
            currentPlayerPoints -= 5;
            totalPlayerPoints++;
            UpdatePoints();
        }
    }
    void Research()
    {
        if (currentPlayerPoints >= 3)
        {
            cureMod++;
            personCanvas.enabled = false;
            currentPlayerPoints -= 3;
            UpdatePoints();
        }
    }
    
    void Recruit()
    {
        if (currentPlayerPoints >= 4)
        {
            string seed = Time.time.ToString();
            System.Random rng = new System.Random(seed.GetHashCode());

            if (rng.Next(0, 100) <= currentPlace.placeData.ChanceToRecruit)
            {
                placeManager.SpawnPerson(currentPlace.placeData.County);
            }
            
            personCanvas.enabled = false;
            currentPlayerPoints -= 4;
            UpdatePoints();
        }
    }

    void Cure()
    {
        foreach (Place place in placeManager.places)
        {
            if (place.placeData.County == currentPlace.placeData.County)
            {
                if (place.placeData.CurrentCases > 0 && place.placeData.CurrentCases >= cureMod)
                {
                    if (place.placeData.CurrentCases >= cureMod)
                    {
                        place.placeData.CurrentCases -= cureMod;
                    }
                    else
                    {
                        place.placeData.CurrentCases = 0;
                    }
                    
                    place.UpdateColor();
                    currentPlayerPoints--;
                    UpdatePoints();
                    UpdateTotalCases();
                    personCanvas.enabled = false;
                    placeCanvas.enabled = false;
                    break;
                }

            }
        }
    }
    
    void FixPersonInfo(Person person)
    {
        moveDropdown.ClearOptions();
        
        foreach (Person p in placeManager.people)
        {
            if (p.currentCounty == person.currentCounty)
            {
                personPlaceInfo.text = "Current location: " + p.currentCounty.placeData.County + ", " + p.currentCounty.placeData.State;
                personCurrentCaseInfo.text = "Cases: " + p.currentCounty.placeData.CurrentCases;

                string placesCanTravel = "Places can travel: ";

                foreach (PlaceData place in person.currentCounty.placeData.Connected_Places)
                {
                    placesCanTravel += " " + place.County + ", " + place.State + " |";
                    moveDropdown.options.Add(new TMP_Dropdown.OptionData(place.County));
                }
                
                personCanTravelInfo.text = placesCanTravel;
                
                currentPlace = person.currentCounty;
                Debug.Log(currentPlace.placeData.County);
                    
                break;
            }
        }
    }

    void ToggleLegend()
    {
        if (legendCanvas.enabled)
        {
            legendCanvas.enabled = false;
        }
        else
        {
            legendCanvas.enabled = true;
        }
    }

    void EndTurn()
    {
        currentPlayerPoints = totalPlayerPoints;

        if (totalCases >= maxCases)
        {
            loseImage.enabled = true;
        }

        if (totalCases <= 0)
        {
            winImage.enabled = true;
        }
        
        placeCanvas.enabled = false;
        personCanvas.enabled = false;
        UpdatePoints();
        Spread();
    }

    void Spread()
    {
        string seed = Time.time.ToString();
        System.Random rng = new System.Random(seed.GetHashCode());

        foreach (Place p in placeManager.places)
        {
            if (rng.Next(0, 100) <= p.placeData.ChanceToGain)
            {
                p.placeData.CurrentCases++;
                p.UpdateColor();
            }

            if (p.placeData.CurrentCases > 5)
            {
                foreach (PlaceData place in p.placeData.Connected_Places)
                {
                    place.CurrentCases++;
                    p.UpdateColor();
                }
            }
        }
        
        UpdateTotalCases();
    }

    void FixPlaceInfo(Place place)
    {
        foreach (Place p in placeManager.places)
        {
            if (p.placeData.County == place.placeData.County)
            {
                string placesCanTravel = "Places can travel: ";

                foreach (PlaceData placeData in p.placeData.Connected_Places)
                {
                    placesCanTravel += " " + placeData.County + ", " + placeData.State + " |";
                }
                
                connectedInfo.text = placesCanTravel;
                placeInfo.text = p.placeData.County + ", " + p.placeData.State;
                currentCaseInfo.text = "Cases: " + p.placeData.CurrentCases;
                break;
            }
        }
    }

    void UpdateTotalCases()
    {
        int tempCases = 0;
        
        foreach (Place p in placeManager.places)
        {
            tempCases += p.placeData.CurrentCases;
        }
        
        totalCases = tempCases;
        totalCaseInfo.text = "Total Cases: " + totalCases;
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
