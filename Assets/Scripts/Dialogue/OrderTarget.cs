using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "OrderTarget", menuName = "ScriptableObjects/OrderTarget")]
public class OrderTarget : ScriptableObject
{
    public string orderName;
    public string nodeTarget;
    public List<string> allowedOrders;
}
