using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Harmony;
using BattleTech;
using BattleTech.Data;


namespace AbilityRealizer.Patches
{
    [HarmonyPatch(typeof(StarSystem), "HirePilot")]
    public static class StarSystem_HirePilot_Patch
    {
		public static bool Prefix(StarSystem __instance, PilotDef def)
		{
	//		if (!__instance.AvailablePilots.Contains(def))
			if (__instance.AvailablePilots.Any(x => x.Description.Id == def.Description.Id))

			{
				__instance.AvailablePilots.RemoveAll(x => x.Description.Id == def.Description.Id);
	//			if (__instance.PermanentRonin.Contains(def))
				if (__instance.PermanentRonin.Any(x => x.Description.Id == def.Description.Id))
				{
					__instance.PermanentRonin.RemoveAll(x=>x.Description.Id == def.Description.Id);
					__instance.Sim.UsedRoninIDs.Add(def.Description.Id);
				}
				def.SetDayOfHire(__instance.Sim.DaysPassed);
				__instance.Sim.AddPilotToRoster(def, true, false);
				int purchaseCostAfterReputationModifier = __instance.GetPurchaseCostAfterReputationModifier(__instance.Sim.GetMechWarriorHiringCost(def));
				__instance.Sim.AddFunds(-purchaseCostAfterReputationModifier, null, true, true);
			}
			return false;
		}
	}
}
