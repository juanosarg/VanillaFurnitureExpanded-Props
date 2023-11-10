using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KCSG;
using RimWorld;
using Verse;

namespace VFEProps
{
    public class PropDef : Def
    { 
        public float priority;
        public PropCategoryDef category;
        public List<PropCategoryDef> categories;
        public ThingDef prop;
        public int silverCost=0;
        
    }
}