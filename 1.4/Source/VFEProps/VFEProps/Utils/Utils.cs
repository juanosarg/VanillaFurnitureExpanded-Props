using System.Collections.Generic;
using Verse;
using System.Linq;
using UnityEngine;
using System;



namespace VFEProps
{
    public static class Utils

    {

      public static int CostCalculator(BuildableDef def) {

            ThingDef thingDef = def as ThingDef;
            if (def != null)
            {
                return Math.Max(5, (int)(7.5 * ((float)thingDef.BaseMaxHitPoints / 300) * (thingDef.fillPercent / 0.55f) * (def.Size.x * def.Size.z)));

            }
            else return 0;

        }

    }










}
