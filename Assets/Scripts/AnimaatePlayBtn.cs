using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AnimaateSubTitle : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        LeanTween.scale(gameObject, new Vector3(1.2f, 1.2f, 1.2f), 5f).setEaseOutQuart().setLoopPingPong(); ;
    }

}
