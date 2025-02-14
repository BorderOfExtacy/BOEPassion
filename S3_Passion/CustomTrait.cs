using System.Collections.Generic;
using Sims3.Gameplay.ActorSystems;
using Sims3.Gameplay.Autonomy;
using Sims3.Gameplay.Socializing;
using Sims3.Gameplay.Utilities;
using Sims3.SimIFace;
using Sims3.SimIFace.CAS;
using Sims3.UI.CAS;
using Sims3.UI.Hud;

namespace Passion.S3_Passion
{
	public class CustomTrait : Trait
	{
		private static class Names
		{
			public const TraitNames Invalid = TraitNames.Unknown;

			public static readonly List<TraitNames> List = new List<TraitNames>();
		}

		public const OccultTypes AllOccultTypes = (OccultTypes)4294967295u;

		public CustomTrait(TraitNames guid, string name, string description, string traitTipDescription, ProductVersion productVersion, CASAGSAvailabilityFlags ageSpeciesGroupVisable, OccultTypes occultAvailabiltiyFlag, int[] additionalSets, int score, float randomWeight, TraitGroup casGroup, CommodityKind commodityKind, string[] facialIdles, CommodityTypes[] autonomousDesiredCommodity, CommodityTypes[] autonomousAvoidedCommodity, List<SocialManager.SocialEffectiveness> increasedEffectiveness, List<SocialManager.SocialEffectiveness> reducedEffectiveness, string predicate, bool canBeLearnedRandomly, int intimacyLevel, string activeTopic, string preferredBookGenre, string thumbFileName, string thumbPieMenu, string thumbDislikePieMenu, string thumbPose, List<ItemType> itemsWanted, string addListenerFunction, TraitTipsManager.TraitTipCounterIndex traitTipIndex, string[] simologyHudTraitTooltip, bool learnedThroughConversation, LifetimeRewardCategories lifetimeRewardsCategory, bool hideIfDisabled, TraitReinforcementAxis axis, float reinforcementThreshold, TraitNames oppTrait, string academicEffects, int descendantFamilySizeModifier, float descendantFamilyValueModifier, bool allowOnRobots, bool robotOnly, TraitChipName requiredChip)
			: base(guid, name, description, traitTipDescription, productVersion, ageSpeciesGroupVisable, occultAvailabiltiyFlag, additionalSets, score, randomWeight, casGroup, commodityKind, facialIdles, autonomousDesiredCommodity, autonomousAvoidedCommodity, increasedEffectiveness, reducedEffectiveness, predicate, canBeLearnedRandomly, intimacyLevel, activeTopic, preferredBookGenre, thumbFileName, thumbPieMenu, thumbDislikePieMenu, thumbPose, itemsWanted, addListenerFunction, traitTipIndex, simologyHudTraitTooltip, learnedThroughConversation, lifetimeRewardsCategory, hideIfDisabled, axis, reinforcementThreshold, oppTrait, academicEffects, descendantFamilySizeModifier, descendantFamilyValueModifier, allowOnRobots, robotOnly, requiredChip)
		{
		}

		public static void Load()
		{
			foreach (TraitNames item in Names.List)
			{
				if (item == TraitNames.Unknown) continue;
				Trait trait = null;
				TraitNames traitNames = item;
				Load(trait);
			}
		}

		private static void Load(Trait trait)
		{
			if (trait == null)
			{
				return;
			}
			ulong guid = (ulong)trait.Guid;
			if (GenericManager<TraitNames, Trait, Trait>.sDictionary.ContainsKey(guid))
			{
				if (GenericManager<TraitNames, Trait, Trait>.sDictionary[guid].TraitVersion < trait.TraitVersion)
				{
					GenericManager<TraitNames, Trait, Trait>.sDictionary[guid] = trait;
				}
			}
			else
			{
				GenericManager<TraitNames, Trait, Trait>.sDictionary.Add(guid, trait);
				TraitManager.sTraitEnumValues.AddNewEnumValue(trait.TraitNameInfo, trait.Guid);
			}
		}
	}
}
