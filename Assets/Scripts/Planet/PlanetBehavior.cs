using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;   
public class PlanetBehavior : MonoBehaviour
{
    [Header("Size and Misc.")]
    [SerializeField] Vector2 sizeRange;
    [SerializeField] float firstPlanetSize;
    [SerializeField] float DistTillDestroy;
    public float density;
    public float mass;
    public float size;
    bool isFirstPlanet=false;

    [Header("Material")]
    [SerializeField] float maxSpinSpeed;
    [SerializeField] Vector2 earthNoiseRange;
    [SerializeField] Vector2 gasNoiseRange;
    [SerializeField] Vector2 gasOffsetRange;
    string[] possibleShaderNames = { "Gas", "Earth" };
    [SerializeField] Shader earthShader;
    [SerializeField] Shader gasShader;

    Transform camPos;
    GameManager gm;

    void Awake()
    {
        gm = GameObject.FindWithTag("GameController").GetComponent<GameManager>();
        camPos = Camera.main.transform;

        gm.planets.Add(this);
        if (GameObject.FindWithTag("GameController").GetComponent<PlanetSpawner>().planetsSpawned < 1 && !gm.isTutorial)
        {
            isFirstPlanet = true;
        }

        if (isFirstPlanet || gm.isTutorial)//if it is the very first planet to be spawned...
        {
            size = firstPlanetSize;//set size to constant first planet size
            GetComponentInChildren<OnOrbit>().hasAchievedPerfectOrbit = true;//has achieved perfect orbit (prevents confetti, etc.
            if (gm.isTutorial) Destroy(GetComponentInChildren<TextMeshPro>().gameObject);//Delete text ring if in tutorial
        }
        else
        {
            size = Random.Range(sizeRange.x, sizeRange.y);//set size randomly
            Destroy(GetComponentInChildren<TextMeshPro>().gameObject);//Delete text ring if not the very first planet
        }

        transform.localScale = new Vector3(size, size, 1);
        mass = density * Mathf.PI*Mathf.Pow(size/2,2);//set planets size and mass

        generateMaterial();//create random material
    }
    private void Update()
    {

        if(camPos.position.y>transform.position.y && camPos.position.y-transform.position.y > DistTillDestroy)
        {
            GameObject.FindWithTag("GameController").GetComponent<GameManager>().planets.Remove(this);
            Destroy(gameObject);//if far enough behind player, destroy
        }
    }

    void generateMaterial()
    {
        string shaderName = possibleShaderNames[Random.Range(0, possibleShaderNames.Length)];
        Material mat=new Material(earthShader); 

        Vector2 spinSpeed = new Vector2(Random.Range(-maxSpinSpeed, maxSpinSpeed), Random.Range(-maxSpinSpeed, maxSpinSpeed));
        float noiseScale;

        int numbColors;
        Color[] colors;
        float[] locations;
 


        switch (shaderName)
        {
            case "Gas":
                mat =new Material(gasShader);
                noiseScale = Random.Range(gasNoiseRange.x, gasNoiseRange.y);
                Vector2 groundOffset = new Vector2(0, Random.Range(gasOffsetRange.x, gasOffsetRange.y));//set noise stuff

                numbColors = Random.Range(2,6);
                colors = new Color[5]{ Color.black,Color.black,Color.black,Color.black,Color.black};
                //initialize colors to be black
                locations = new float[5] { 0, 2, 2, 2, 2 };
                //initialize locations- first one is always 0, when loc= '2' the color is turned off. 
                
                for(int i = 0; i < numbColors; i++)
                {
                    colors[i] = Random.ColorHSV(0f, 1f,0.5f,1f,0.6f,1f);
                }//generate colors

                for(int i = 0; i < numbColors; i++)
                {
                    locations[i] = (1f / (numbColors - 1f)) * i;
                }//set all the middle locations
                

                mat.SetFloat("_NoiseScale", noiseScale);
                mat.SetVector("_turnSpeed", spinSpeed);
                mat.SetVector("GroundOffset", groundOffset);
                setGradientColors(mat, colors, locations);//set all these values to material

                break;
            case "Earth":
                mat = new Material(earthShader);

                noiseScale = Random.Range(earthNoiseRange.x, earthNoiseRange.y);

                numbColors = Random.Range(4, 6);
                colors = new Color[5] { Color.black, Color.black, Color.black, Color.black, Color.black };
                //initialize colors to be black
                locations = new float[5] { 0.5f,0.7f,0.8f,0.9f,1.2f };
                //initialize locations- first one is always 0, when loc= '2' the color is turned off. 

                for (int i = 0; i < numbColors; i++)
                {
                    colors[i] = Random.ColorHSV(0f, 1f, 0.5f, 1f, 0.6f, 1f);
                }//generate colors


                float H, S, V;

                Color.RGBToHSV(colors[1], out H, out S, out V);//Get second color (Shallow water) 's HSV Values
                colors[0] = Color.HSVToRGB(H, S, 0.7f * V);// Override the first color to be a darker version of the second color

                Color.RGBToHSV(colors[3], out H, out S, out V);//Get third color (low land) 's HSV Values
                colors[4] = Color.HSVToRGB(H, S, 0.8f * V);// Override the last color to be a darker version of the third color

                if (numbColors < 5)
                {
                    locations[2] = 2;//if does not have all 5 colors, turn off beach color.
                }

                mat.SetFloat("_NoiseScale", noiseScale);
                mat.SetVector("_turnSpeed", spinSpeed);
                setGradientColors(mat, colors, locations);//set all these values to material
                break;

            default:
                Debug.LogError("WARNING: Planet has Invalid Shader name!");
                break;
        }


        GetComponent<SpriteRenderer>().material=mat;
    }
    void setGradientColors(Material mat, Color[] colors, float[] locations)
    {// sets all the gradient colors for custom work-around function
        mat.SetColor("Color_1", colors[0]);
        mat.SetColor("Color_2", colors[1]);
        mat.SetColor("Color_3", colors[2]);
        mat.SetColor("Color_4", colors[3]);
        mat.SetColor("Color_5", colors[4]);

        mat.SetFloat("Location_1", locations[0]);
        mat.SetFloat("Location_2", locations[1]);
        mat.SetFloat("Location_3", locations[2]);
        mat.SetFloat("Location_4", locations[3]);
        mat.SetFloat("Location_5", locations[4]);

    }

    private void OnDisable()
    {
        gm.planets.Remove(this);
    }


}
