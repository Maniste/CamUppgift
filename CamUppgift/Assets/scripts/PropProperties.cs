using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropProperties : MonoBehaviour
{
    public bool BlockingView = false;
    bool HasBeenBlocked = false;
    bool hasPassedValues = false;
    float TransparencyStrenght = 0.0f;
    float CurrentFadeTime = 0.0f;
    float CurrentFadeSpeed = 0.0f;
    [Header("Debug stuff don't mess with it")]
    public float IfRayIsStillThereCheck = 0.0f;
    Color AlphaColor;

    // Start is called before the first frame update
    void Start()
    {
        gameObject.layer = 9;                                         
    }

    public void PassValues(float FadeRate, float FadeTimeu, float TransStrenght) {
        if (!hasPassedValues)
        {
            CurrentFadeSpeed = FadeRate;
            CurrentFadeTime = FadeTimeu;
            TransparencyStrenght = TransStrenght;
            hasPassedValues = true;
        }
    }

    void MakeTransparent() {
        AlphaColor = gameObject.GetComponent<Renderer>().material.color;
        HasBeenBlocked = true;
    }
    void FadeOut()
    {
        if (CurrentFadeTime >= TransparencyStrenght)
        {
            AlphaColor.a = CurrentFadeTime;
            AlphaColor.b = CurrentFadeTime;
            CurrentFadeTime -= CurrentFadeSpeed * Time.deltaTime;
        }
    }
    void FadeIn()
    {
        if (CurrentFadeTime <= 1.0f)
        {
            AlphaColor.a = CurrentFadeTime;
            AlphaColor.b = CurrentFadeTime;
            CurrentFadeTime += CurrentFadeSpeed * Time.deltaTime;
        }
        else if (CurrentFadeTime <= 1.0f)
        {
            HasBeenBlocked = false;
        }
    }


    // Update is called once per frame
    void Update()
    {

        if (BlockingView)
        {
            MakeTransparent();
            FadeOut();
            gameObject.GetComponent<Renderer>().material.color = AlphaColor;
        }
        else if (HasBeenBlocked && !BlockingView)
        {
            FadeIn();
            gameObject.GetComponent<Renderer>().material.color = AlphaColor;
        }
        if (IfRayIsStillThereCheck <= 0)
            BlockingView = false;
        if (IfRayIsStillThereCheck > 0)
            IfRayIsStillThereCheck -= Time.deltaTime;
    }
}
