using System.Collections;
using System.Collections.Generic;
using UnityEditor.Timeline.Actions;
using UnityEngine;
using UnityEngine.UI;

public class BloodEffect : MonoBehaviour
{
    public Image bloodEffectImage;

    private float r;
    private float g;
    private float b;
    private float a;
    private bool activar = false;

    void Start()
    {
        r = bloodEffectImage.color.r;
        g = bloodEffectImage.color.g;
        b = bloodEffectImage.color.b;
        a = bloodEffectImage.color.a;
    }

    void Update()
    {
        Color c = new Color(r, g, b, a);
        bloodEffectImage.color = c;
        if (activar)
        {
            if (a < 0.45)
            {
                a += 0.1f;
            }
            else
            {
                ResetAlpha();
            }
        }

        if(a > 0f)
        {
            a -= 0.01f;
        }
    }

    public void ChangeColor()
    {
        if(activar == false)
        {
            activar = true;
        }
    }
    
    public void ResetAlpha()
    {
        activar = false;
    }
}