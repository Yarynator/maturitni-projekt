using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cookie : MonoBehaviour
{
    
    public static Cookie Create(Vector2 position)
    {
        return Instantiate(Resources.Load<Cookie>("Cookie"), position, Quaternion.identity);
    }

}
