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
    [SerializeField]
    private GameObject exitScreen;

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
        exitScreen.SetActive(false);
    }

    private void Update()
    {
        if (!textfieldIsActive)
        {
            // Randomize whole Planet
            if (Input.GetKeyDown(KeyCode.R))
            {
                planet.RandomizePlanetColor();
                planet.RandomizePlanetShape();
                planet.GeneratePlanet();
                var fplanet = thirdPersonCamera.targets[thirdPersonCamera.index].GetComponentInChildren<FinalPlanet>();
                copyFactory.UpdateParameters(fplanet);
            }
            // Randomize Planet Color
            if (Input.GetKeyDown(KeyCode.T))
            {
                planet.RandomizePlanetColor();
                planet.GeneratePlanet();
                copyFactory.UpdateParameters(thirdPersonCamera.targets[thirdPersonCamera.index].GetComponentInChildren<FinalPlanet>());
            }
            // Randomize Planet Shape
            if (Input.GetKeyDown(KeyCode.Y))
            {
                planet.RandomizePlanetShape();
                planet.GeneratePlanet();
                copyFactory.UpdateParameters(thirdPersonCamera.targets[thirdPersonCamera.index].GetComponentInChildren<FinalPlanet>());
            }
            // turn UI text on off
            if (Input.GetKeyDown(KeyCode.U))
            {
                text1.enabled = !text1.enabled;
                text2.enabled = !text2.enabled;
            }
            // swap between systemview and singlePlanet view     
            if (Input.GetKeyDown(KeyCode.I))
            {
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
            // Show Flightpath
            if (Input.GetKeyDown(KeyCode.O))
            {
                orbitPainter.ShowOrbit();
            }
        }
        // show Exit 
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!exitScreen.activeSelf)
            {
                exitScreen.SetActive(true);
                Cursor.lockState = CursorLockMode.Confined;
                Cursor.visible = true;
            }
            else
            {
                exitScreen.SetActive(false);
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }
        // Show inputfield to set seed to planet
        if (Input.GetButtonDown("Submits"))
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

    public void ExitApplication(bool yes)
    {
        if (yes)
        {
            Debug.Log("close Application");
            Application.Quit();
        }
        else
        {
            exitScreen.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }


}
