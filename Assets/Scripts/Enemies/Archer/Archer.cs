using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Archer : Entity
{
    public Archer_MoveState moveState { get; private set; }
    public Archer_IdleState idleState { get; private set; }

    [SerializeField]
    private D_MoveState moveStateData;

    [SerializeField]
    private D_IdleState idleStateData;

    private void Start()
    {
        
    }

}
