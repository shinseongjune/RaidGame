using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombStoneControlComponent : ControlComponent
{
    public override void Die()
    {
        Destroy(gameObject);
    }

    public override void EndMovement()
    {

    }
}
