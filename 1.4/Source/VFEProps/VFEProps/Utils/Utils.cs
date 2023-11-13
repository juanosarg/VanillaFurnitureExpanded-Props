using System.Collections.Generic;
using Verse;
using System.Linq;
using UnityEngine;
using System;



namespace VFEProps
{
    public static class Utils

    {

      public static int CostCalculator(ThingDef def) {

            return (int)(7.5 * ((float)def.BaseMaxHitPoints / 300) * (def.fillPercent / 0.55f) * (def.Size.x * def.Size.z)); 
        }

    }










}
