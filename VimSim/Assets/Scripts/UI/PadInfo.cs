using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Pad", menuName = "Pad")]
public class PadInfo : ScriptableObject
{
    public string code;
    public InventoryItem item;
}