using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UserControll : MonoBehaviour
{

    private CopyFactory copyFactory;
    private ThirdPersonCamera thirdPersonCamera;
    private BlueprintPlanet planet;
    private SeedGenerator seedGenerator;
    [SerializeField]
    private Text text1, text2;
    [SerializeField]
    private InputField inputField;
    private bool textfieldIsActive;

    private OrbitPainter orbitPainter;

    [Header("Cameras")]
    [SerializeField]
    private GameObject observerCamera;
    [SerializeField]
    private GameObject mainCam;

    void Start()
    {
        copyFactory = GetComponent<CopyFactory>();
        thirdPersonCamera = FindObjectOfType<ThirdPersonCamera>();
        planet = FindObjectOfType<BlueprintPlanet>();
        orbitPainter = FindObjectOfType<OrbitPainter>();
        seedGenerator = FindObjectOfType<SeedGenerator>();

        foreach (MeshRenderer mesh in planet.GetComponentsInChildren<MeshRenderer>())
        {
            mesh.enabled = false;
        }

        inputField.gameObject.SetActive(false);
        textfieldIsActive = false;
    }

    private void Update()
    {
        if (!textfieldIsActive)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                // Randomize whole Planet
                planet.RandomizePlanetColor();
                planet.RandomizePlanetShape();
                planet.GeneratePlanet();
                var fplanet = thirdPersonCamera.targets[thirdPersonCamera.index].GetComponentInChildren<FinalPlanet>();
                copyFactory.UpdateParameters(fplanet);
            }
            if (Input.GetKeyDown(KeyCode.T))
            {
                // Randomize Planet Color
                planet.RandomizePlanetColor();
                planet.GeneratePlanet();
                copyFactory.UpdateParameters(thirdPersonCamera.targets[thirdPersonCamera.index].GetComponentInChildren<FinalPlanet>());
            }
            if (Input.GetKeyDown(KeyCode.Y))
            {
                // Randomize Planet Shape
                planet.RandomizePlanetShape();
                planet.GeneratePlanet();
                copyFactory.UpdateParameters(thirdPersonCamera.targets[thirdPersonCamera.index].GetComponentInChildren<FinalPlanet>());
            }

            if (Input.GetKeyDown(KeyCode.U))
            {
                // turn UI text on off
                text1.enabled = !text1.enabled;
                text2.enabled = !text2.enabled;
            }

            if (Input.GetKeyDown(KeyCode.I))
            {
                // show system instead of single Planets            
                if (mainCam.activeSelf)
                {
                    mainCam.SetActive(false);
                    observerCamera.SetActive(true);
                }
                else if (!mainCam.activeSelf)
                {
                    observerCamera.SetActive(false);
                    mainCam.SetActive(true);
                }
            }
            if (Input.GetKeyDown(KeyCode.O))
            {
                // Show Flightpath
                orbitPainter.ShowOrbit();
            }
        }

        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }

        if (Input.GetButtonDown("Submit"))
        {
            textfieldIsActive = !textfieldIsActive;

            if (textfieldIsActive)
            {
                inputField.gameObject.SetActive(true);
                inputField.ActivateInputField();
            }
            else
            {
                inputField.gameObject.SetActive(false);
            }
        }
    }

    public void UseSeedToGeneratePlanet()
    {
        // not on first enterPress
        if (textfieldIsActive)
        {
            seedGenerator.InputSeed(inputField.text);
            // Calculate BluePrintPlanet
            planet.RandomizePlanetShape();
            planet.RandomizePlanetColor();
            planet.GeneratePlanet();
            // set to FinalePlanetInFocus
            copyFactory.UpdateParameters(thirdPersonCamera.targets[thirdPersonCamera.index].GetComponentInChildren<FinalPlanet>());
        }
    }
}
