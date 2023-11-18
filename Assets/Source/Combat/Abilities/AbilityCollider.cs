using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAbilityCollider
{
    public abstract bool isColliding { get; protected set; }
}
