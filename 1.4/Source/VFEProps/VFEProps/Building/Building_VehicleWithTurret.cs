
using RimWorld;
using Verse;
using Verse.Sound;

using UnityEngine;
using static HarmonyLib.Code;
using System;

namespace VFEProps
{
    public class Building_VehicleWithTurret : Building_SubstractsSilver
    {
        public DrawTurretExtension extension = null;

        public Graphic graphic = null;

        public DrawTurretExtension GetExtension
        {
            get
            {
                if (extension is null)
                {
                    extension = this.def.GetModExtension<DrawTurretExtension>();
                   
                }
                return extension;
            }
        }

        public Graphic GetGraphic
        {
            get
            {
                if (graphic is null)
                {
                    LongEventHandler.ExecuteWhenFinished(delegate { GetGraphicLong(); });
                    
                }
                return graphic;
            }
        }

        public void GetGraphicLong()
        {
            try
            {
                graphic = (Graphic_Single)GraphicDatabase.Get<Graphic_Single>(GetExtension.turretToDraw, ShaderDatabase.CutoutComplex, GetExtension.drawSize, this.def.graphicData.color);
            }
            catch (Exception) {  }
        }

        public override void Draw()
        {
            base.Draw();
            var vector = this.DrawPos + Altitudes.AltIncVect;
            vector.y += 5;
            vector.x += GetExtension.offset.x;
            vector.z += GetExtension.offset.y;
            GetGraphic?.DrawFromDef(vector, this.Rotation, null);

        }



    }
}