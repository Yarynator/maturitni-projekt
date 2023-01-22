using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class VersionInfo : MonoBehaviour
{

    private void Start()
    {
        GetComponent<TextMeshProUGUI>().text = Application.productName + ", " + Application.version; 
    }

}
