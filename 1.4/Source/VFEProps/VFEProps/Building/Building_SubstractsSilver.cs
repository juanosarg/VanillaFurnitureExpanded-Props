
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
namespace VFEProps
{
    public class Building_SubstractsSilver : Building
    {

       

        public override void SpawnSetup(Map map, bool respawningAfterLoad)
        {
            base.SpawnSetup(map,respawningAfterLoad);
            if (!respawningAfterLoad)
            {
                int cost = GetSilverCost();

                if (CheckSilverInMap(cost))
                {
                    RemoveSilverFromMap(cost);
                }
                else
                {
                    Messages.Message("VFE_NoSilver".Translate(cost), null, MessageTypeDefOf.RejectInput);
                    this.DeSpawn();
                  

                }
            }
        }

       
        public int GetSilverCost()
        {
            int cost = (from x in DefDatabase<PropDef>.AllDefsListForReading
                        where x.prop == this.def
                        select x).First().silverCost;

            return cost;
        }

        public bool CheckSilverInMap(int cost)
        {
            int totalSilver = 0;
            List<SlotGroup> allGroupsListForReading = Find.CurrentMap.haulDestinationManager.AllGroupsListForReading;
            for (int i = 0; i < allGroupsListForReading.Count; i++)
            {
                foreach (Thing heldThing in allGroupsListForReading[i].HeldThings)
                {
                    Thing innerIfMinified = heldThing.GetInnerIfMinified();
                    if (innerIfMinified.def.CountAsResource)
                    {
                        if (innerIfMinified.def == ThingDefOf.Silver)
                        {
                            totalSilver += innerIfMinified.stackCount;
                        }

                    }
                }
            }
            if (totalSilver >= cost)
            {
                return true;
            }

            return false;
        }

        public void RemoveSilverFromMap(int cost)
        {
            int silverLeftToRemove = cost;
            List<SlotGroup> allGroupsListForReading = Find.CurrentMap.haulDestinationManager.AllGroupsListForReading;
            while (silverLeftToRemove > 0)
            {
                for (int i = 0; i < allGroupsListForReading.Count; i++)
                {
                    foreach (Thing heldThing in allGroupsListForReading[i].HeldThings)
                    {
                        Thing innerIfMinified = heldThing.GetInnerIfMinified();
                        if (innerIfMinified.def.CountAsResource)
                        {
                            if (innerIfMinified.def == ThingDefOf.Silver)
                            {
                                int num = Math.Min(cost, innerIfMinified.stackCount);
                                innerIfMinified.SplitOff(num).Destroy();
                                silverLeftToRemove -= num;
                            }

                        }
                    }
                }
            }


        }


    }


}
