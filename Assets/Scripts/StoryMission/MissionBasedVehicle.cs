﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionBasedVehicle : Vehicle
{
    public override void DestroyVehicle()
    {
        base.DestroyVehicle();
        if(GameManager.instance.currentMission)
        {
            GameManager.instance.currentMission.MissionComplete();
        }
    }
}
