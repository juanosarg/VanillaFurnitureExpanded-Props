using System.Collections.Generic;
using Verse;
using System.Linq;
using UnityEngine;
using System;



namespace VFEProps
{
    public class VFEProps_Settings : ModSettings

    {

        public static float costMultiplier = baseCostMultiplier;
        public const float baseCostMultiplier = 1f;

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref costMultiplier, "costMultiplier", baseCostMultiplier);
        }

        public static void DoWindowContents(Rect inRect)
        {
            Listing_Standard ls = new Listing_Standard();


            ls.Begin(inRect);
            ls.Gap(10f);

            var costLabel = ls.LabelPlusButton("VFE_PropCostMultiplier".Translate() + ": " + costMultiplier, "VFE_PropCostMultiplierDesc".Translate());
            costMultiplier = (float)Math.Round(ls.Slider(costMultiplier, 0f, 5), 1);

            if (ls.Settings_Button("VFE_Reset".Translate(), new Rect(0f, costLabel.position.y + 35, 250f, 29f)))
            {
                costMultiplier = baseCostMultiplier;
            }


            ls.End();
        }



    }










}
