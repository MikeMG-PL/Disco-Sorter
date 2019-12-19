using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    public int EntityNumber;                            // Numer (identyfikator) obiektu
    public enum EntityType { Slap, Smash, DontTouch };
    public EntityType entityType;
}
