
using Verse;
using System;
using RimWorld;
using System.Collections.Generic;
using System.Linq;


namespace VFEProps
{
    [StaticConstructorOnStartup]
    public static class StaticCollections
    {

        static StaticCollections()
        {

            foreach (HiddenDesignatorsDef hiddenDesignatorDef in DefDatabase<HiddenDesignatorsDef>.AllDefsListForReading)
            {
                foreach(BuildableDef thing in hiddenDesignatorDef.hiddenDesignators)
                {

                    hidden_designators.Add(thing);
                }
            }

            foreach (PropDef prop in DefDatabase<PropDef>.AllDefsListForReading)
            {
                if (prop.dontPopUpStupidGraphicErrors)
                {
                    stupidErrors_Things.Add(prop.prop);
                }
            }

            foreach (PropCategoryDef category in DefDatabase<PropCategoryDef>.AllDefsListForReading)
            {
                List<PropDef> props = (from x in DefDatabase<PropDef>.AllDefsListForReading
                                       where (x.category == category || x.categories?.Contains(category) == true)
                                       select x).ToList();
                if (props.Count > 0)
                {
                    visibleCategories.Add(category);
                }
                
               
            }


        }

        //This static class stores lists for different things.

        // A list of designators that shouldn't appear on the architect menu
        public static HashSet<BuildableDef> hidden_designators = new HashSet<BuildableDef>();

        // A list of things that usually pop up stupid errors in-game
        public static HashSet<ThingDef> stupidErrors_Things = new HashSet<ThingDef>();

        // A list of categories that should be shown. Categories without props will be hidden
        public static HashSet<PropCategoryDef> visibleCategories = new HashSet<PropCategoryDef>();
    }
}
