using RimWorld;
using Verse;
using HarmonyLib;
using System.Linq;
using System;

namespace ScenarioLimits
{
    [StaticConstructorOnStartup]
    public class ScenarioLimits
    {
        static ScenarioLimits() 
        {            
            var harmony = new Harmony("uk.co.dbyz.rim.scenariolimits");
            harmony.PatchAll();
        }
    }

    [HarmonyPatch(typeof(Page_SelectStartingSite))]
    [HarmonyPatch("CanDoNext")] 
    class TileSelectionPatch
    {
        static void Postfix(Page_SelectStartingSite __instance, ref bool __result)
        {
            if (!__result) return;

            RimWorld.Planet.Tile tile = Find.WorldGrid[Find.WorldInterface.SelectedTile];

            try
            {
                var rule = (ScenRule_LimitAllowBiome)Find.Scenario.AllParts.Where((sp) => sp is ScenRule_LimitAllowBiome).First();
                if (rule.listBiomes.NullOrEmpty() || rule.listBiomes.Contains(tile.biome))
                {
                    __result = true;
                    return;
                }

                __result = false;
                Messages.Message("You must start on one of these biomes: " + string.Join(", ", rule.listBiomes.Select(s => $"'{s.defName}'")), MessageTypeDefOf.RejectInput, false);
            }
            catch (InvalidOperationException)
            {
                // No rule found, ignore
            }

            try
            {
                var rule = (ScenRule_LimitDisallowBiome)Find.Scenario.AllParts.Where((sp) => sp is ScenRule_LimitDisallowBiome).First();
                if (rule.listBiomes.NullOrEmpty() || !rule.listBiomes.Contains(tile.biome))
                {
                    __result = true;
                    return;
                }

                __result = false;
                Messages.Message("You can't start on one of these biomes: " + string.Join(", ", rule.listBiomes.Select(s => $"'{s.defName}'")), MessageTypeDefOf.RejectInput, false);
            }
            catch (InvalidOperationException)
            {
                // No rule found, ignore
            }
        }
        
    }
}