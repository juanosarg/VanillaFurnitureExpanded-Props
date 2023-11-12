using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using HarmonyLib;
using RimWorld;
using UnityEngine;
using Verse;
using Verse.Noise;
using static System.Collections.Specialized.BitVector32;

namespace VFEProps
{
    public class Window_PropsListing : Window
    {

        public PropCategoryDef category;
        public override Vector2 InitialSize => new Vector2(620f, 500f);
        private Vector2 scrollPosition = new Vector2(0, 0);
        public int columnCount = 8;
        private static readonly Color borderColor = new Color(0.13f, 0.13f, 0.13f);
        private static readonly Color fillColor = new Color(0, 0, 0, 0.1f);
        private string searchKey;

        public Window_PropsListing(PropCategoryDef category)
        {
            this.category = category;
            draggable = true;
            resizeable = true;
            preventCameraMotion = false;
        }

        public void OpenCategoriesWindow()
        {
            Window_PropsCategories categoriesWindow = new Window_PropsCategories();
            Find.WindowStack.Add(categoriesWindow);
            categoriesWindow.windowRect = this.windowRect;
            Close();
        }

        public void CreateDesignator(ThingDef thingdef)
        {
            Designator_Build designator = new Designator_Build(thingdef);
            Find.DesignatorManager.Select(designator);
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



        public override void DoWindowContents(Rect inRect)
        {
            Text.Font = GameFont.Small;
            var outRect = new Rect(inRect);
            outRect.yMin += 20f;
            outRect.yMax -= 40f;
            outRect.width -= 16f;

            var arrowRect = new Rect(0f, 0f, 32f, 32f);
            GUI.DrawTexture(arrowRect, ContentFinder<Texture2D>.Get("UI/AP_GoBack", true), ScaleMode.ScaleToFit, alphaBlend: true, 0f, Color.white, 0f, 0f);
            if (Widgets.ButtonInvisible(arrowRect))
            {
                OpenCategoriesWindow();
            }

            var GoBackTextRect = new Rect(40, 5, 100f, 32f);
            Widgets.Label(GoBackTextRect, "VFE_GoBack".Translate());
            if (Widgets.ButtonInvisible(GoBackTextRect))
            {
                OpenCategoriesWindow();
            }

            var searchRect = new Rect(160, 5, 150, 24);
            searchKey = Widgets.TextField(searchRect, searchKey);
            var searchLabel = new Rect(320, 5, 100, 32);
            Widgets.Label(searchLabel, "VFE_PrefabSearch".Translate());

            if (Widgets.ButtonImage(new Rect(outRect.xMax - 18f - 4f, 2f, 18f, 18f), TexButton.CloseXSmall))
            {
                Close();
            }

            outRect.yMin += 20f;
            List<PropDef> props = (from x in DefDatabase<PropDef>.AllDefsListForReading
                                   where (x.category == category || x.categories?.Contains(category) == true) && x.prop.label.ToLower().Contains(searchKey.ToLower())

                                   select x).OrderBy(x => x.priority).ToList();


            var viewRect = new Rect(0f, 40, outRect.width - 16f, 104 * ((props.Count / columnCount) + 1)+20);
            Widgets.BeginScrollView(outRect, ref scrollPosition, viewRect);
            try
            {

                for (var i = 0; i < props.Count; i++)
                {

                    Rect rectIcon = new Rect((64 * (i % columnCount)) + 5 * (i % columnCount), viewRect.y + (84 * (i / columnCount) + 20 * ((i / columnCount) + 1)), 64, 64);

                    Widgets.DrawBoxSolidWithOutline(rectIcon, fillColor, borderColor, 2);
                    Rect rectIconInside = rectIcon.ContractedBy(2);

                    GUI.DrawTexture(rectIconInside, props[i].prop.uiIcon, ScaleMode.ScaleAndCrop, alphaBlend: true, 0f, props[i].prop.uiIconColor, 0f, 0f);

                    TooltipHandler.TipRegion(rectIcon, props[i].prop.LabelCap + ": " + props[i].prop.description);
                    if (Widgets.ButtonInvisible(rectIcon))
                    {

                        if (CheckSilverInMap(props[i].silverCost))
                        {
                            CreateDesignator(props[i].prop);
                        }
                        else
                        {
                            Messages.Message("VFE_NoSilver".Translate(props[i].silverCost), null, MessageTypeDefOf.RejectInput);
                        }
                    }

                    Text.Font = GameFont.Tiny;
                    var prefabTextRect = new Rect((64 * (i % columnCount)) + 5 * (i % columnCount), viewRect.y + 64 + (84 * (i / columnCount) + 20 * ((i / columnCount) + 1)), 64, 20);
                    string label;
                    if (!props[i].shortLabel.NullOrEmpty())
                    {
                        label = props[i].shortLabel.CapitalizeFirst();
                    }
                    else
                    {
                        label = props[i].prop.LabelCap;
                    }         
                    
                    Widgets.Label(prefabTextRect, label);

                    Rect silverIcon = new Rect((64 * (i % columnCount)) + 5 * (i % columnCount), viewRect.y + 79 + (84 * (i / columnCount) + 20 * ((i / columnCount) + 1)), 20, 20);
                    GUI.DrawTexture(silverIcon, ContentFinder<Texture2D>.Get("Things/Item/Resource/Silver/Silver_c", true), ScaleMode.ScaleToFit, alphaBlend: true, 0f, Color.white, 0f, 0f);
                    Rect silverDetails = new Rect((64 * (i % columnCount)) + 5 * (i % columnCount) + 24, viewRect.y + 79 + (84 * (i / columnCount) + 20 * ((i / columnCount) + 1)), 64, 20);
                    Widgets.Label(silverDetails, (props[i].silverCost).ToString());
                }
            }
            finally
            {
                Widgets.EndScrollView();
            }
        }

        
    }
}