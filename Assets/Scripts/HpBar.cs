using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HpBar : MonoBehaviour
{


    public GameObject barObject;


    public void SetState(float percentage)
    {
        if(barObject == null) return;
        barObject.transform.localScale =  new Vector3(percentage,1,1);
    }


    public void SetActive(bool active)
    {
        gameObject.SetActive(active);
    }
}
