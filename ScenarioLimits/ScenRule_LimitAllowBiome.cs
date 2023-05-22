using RimWorld;
using System.Collections.Generic;
using Verse;
using System.Linq;
using System;

namespace ScenarioLimits
{
    class ScenRule_LimitAllowBiome : ScenRule_LimitBiome
    {
        public override List<BiomeDef> listBiomes
        {
            get
            {
                return DefDatabase<BiomeDef>.AllDefs.Where<BiomeDef>((Func<BiomeDef, bool>)(biome =>
                {
                    return allowedBiomeNames.Contains(biome.defName);
                })).ToList();
            }
        }

        public override bool CanCoexistWith(ScenPart other)
        {
            if ((other is ScenRule_LimitBiome)) { return false; }
            return true;
        }

        public List<string> allowedBiomeNames = new List<string>();


        protected override void toggleBiome(BiomeDef localKind)
        {
            if (this.allowedBiomeNames.Contains(localKind.defName))
            {
                this.allowedBiomeNames.Remove(localKind.defName);
            }
            else
            {
                this.allowedBiomeNames.Add(localKind.defName);
            }
        }
        public override void ExposeData()
        {
            base.ExposeData();
            Log.Message("Allowed Biomes[0]: " + string.Join(", ", allowedBiomeNames.Select(s => $"'{s}'")));
            Scribe_Collections.Look<string>(ref this.allowedBiomeNames, "allowedBiomeNames", LookMode.Value);
            Log.Message("Allowed Biomes[1]: " + string.Join(", ", allowedBiomeNames.Select(s => $"'{s}'")));
        }
    }
}