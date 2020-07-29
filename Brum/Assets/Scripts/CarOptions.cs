using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarOptions : MonoBehaviour
{
    public GameObject Car;
    Renderer Rend;
    Material[] Mat;
    float Red;
    float Green;
    float Blue;
    void Awake()
    {
        
        Red = PlayerPrefs.GetFloat("Red");
        Green = PlayerPrefs.GetFloat("Green");
        Blue = PlayerPrefs.GetFloat("Blue");
        Rend = Car.GetComponent<Renderer>();
        Mat = Rend.materials;
        Mat[0].color = new Color(Red / 255, Green / 255, Blue / 255);
    }


}
