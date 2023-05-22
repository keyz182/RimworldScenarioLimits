using RimWorld;
using System.Collections.Generic;
using Verse;
using System.Linq;
using System;

namespace ScenarioLimits
{
    class ScenRule_LimitDisallowBiome : ScenRule_LimitBiome
    {
        public override List<BiomeDef> listBiomes
        {
            get
            {
                return DefDatabase<BiomeDef>.AllDefs.Where<BiomeDef>((Func<BiomeDef, bool>)(biome =>
                {
                    return disallowedBiomeNames.Contains(biome.defName);
                })).ToList();
            }
        }

        public override bool CanCoexistWith(ScenPart other)
        {
            if ((other is ScenRule_LimitBiome)) { return false; }
            return true;
        }

        public List<string> disallowedBiomeNames = new List<string>();


        protected override void toggleBiome(BiomeDef localKind)
        {
            if (this.disallowedBiomeNames.Contains(localKind.defName))
            {
                this.disallowedBiomeNames.Remove(localKind.defName);
            }
            else
            {
                this.disallowedBiomeNames.Add(localKind.defName);
            }
        }
        public override void ExposeData()
        {
            base.ExposeData();
            Log.Message("Allowed Biomes[0]: " + string.Join(", ", disallowedBiomeNames.Select(s => $"'{s}'")));
            Scribe_Collections.Look<string>(ref this.disallowedBiomeNames, "allowedBiomeNames", LookMode.Value);
            Log.Message("Allowed Biomes[1]: " + string.Join(", ", disallowedBiomeNames.Select(s => $"'{s}'")));
        }
    }
}