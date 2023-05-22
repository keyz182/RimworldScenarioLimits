using RimWorld;
using System.Collections.Generic;
using Verse;
using System.Linq;
using System;
using UnityEngine;

namespace ScenarioLimits
{
    abstract class ScenRule_LimitBiome : ScenPart
    {
        public List<BiomeDef> PossibleBiomes => DefDatabase<BiomeDef>.AllDefs.Where<BiomeDef>((Func<BiomeDef, bool>)(biome =>
        {
            return biome.canBuildBase && biome.implemented && !listBiomes.Contains(biome);
        })).ToList();

        public abstract List<BiomeDef> listBiomes
        {
            get;
        }

        protected abstract void toggleBiome(BiomeDef biome);

        public override void DoEditInterface(Listing_ScenEdit listing)
        {
            int rows = listBiomes.Count + 1;
            Rect scenPartRect = listing.GetScenPartRect((ScenPart)this, ScenPart.RowHeight * rows);

            if (Widgets.ButtonText(new Rect(scenPartRect.x, scenPartRect.y, scenPartRect.width, scenPartRect.height / rows), "Select Biome", true, true, true))
            {
                List<FloatMenuOption> list = new List<FloatMenuOption>();
                foreach (BiomeDef possibleBiome in PossibleBiomes)
                {
                    BiomeDef localKind = possibleBiome;

                    list.Add(new FloatMenuOption((string)localKind.LabelCap, delegate ()
                    {
                        toggleBiome(localKind);
                    }, MenuOptionPriority.Default, null, null, 0f, null, null));
                }
                Find.WindowStack.Add(new FloatMenu(list));
            }

            int i = 1;
            foreach (BiomeDef selectedBiome in listBiomes)
            {
                Rect r = new Rect(scenPartRect.x, scenPartRect.y + (float)((double)scenPartRect.height * i++ / rows), scenPartRect.width, scenPartRect.height / rows);
                if (Widgets.ButtonText(r, selectedBiome.LabelCap))
                {
                    listBiomes.Remove(selectedBiome);
                }
            }
        }
    }
}