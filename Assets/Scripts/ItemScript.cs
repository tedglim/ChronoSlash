﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemScript : MonoBehaviour
{
    public int value;

    public void GetDestroyed()
    {
        Destroy(gameObject);
    }
}
