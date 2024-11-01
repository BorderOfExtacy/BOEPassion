using Sims3.Gameplay.Abstracts;
using Sims3.Gameplay.Actors;
using Sims3.Gameplay.ActorSystems;
using Sims3.Gameplay.Autonomy;
using Sims3.Gameplay.CelebritySystem;
using Sims3.Gameplay.EventSystem;
using Sims3.Gameplay.Interactions;
using Sims3.Gameplay.Skills;
using Sims3.Gameplay.Socializing;
using Sims3.Gameplay.Utilities;
using Sims3.SimIFace;

namespace S3_Passion
{
	public class VampireBites : SocialInteraction
	{
		[DoesntRequireTuning]
		public sealed class Definition : InteractionDefinition<Sim, Sim, VampireBites>, IHasTraitIcon, IHasMenuPathIcon
		{
			public ResourceKey GetPathIcon(Sim actor, GameObject target)
			{
				return ResourceKey.CreatePNGKey("trait_nocturnal_s_ep7", 0u);
			}

			public ResourceKey GetTraitIcon(Sim actor, GameObject target)
			{
				return ResourceKey.CreatePNGKey("trait_nocturnal_s_ep7", 0u);
			}

			protected override string GetInteractionName(Sim actor, Sim target, InteractionObjectPair interaction)
			{
				return Localization.LocalizeString(target.IsFemale, "Vampire Bite", target.SimDescription, actor.SimDescription);
			}

			public override string[] GetPath(bool isFemale)
			{
				return new string[1] { Localization.LocalizeString(isFemale, "Vampire...") };
			}

			protected override bool Test(Sim a, Sim target, bool isAutonomous, ref GreyedOutTooltipCallback greyedOutTooltipCallback)
			{
				return GameUtils.IsInstalled(ProductVersion.EP7) && target != a && !target.Posture.Satisfies(CommodityKind.SwimmingInPool, null) && !target.SimDescription.IsGhost && !target.SimDescription.ChildOrBelow && !target.SimDescription.IsPet && a.SimDescription.IsVampire && !a.SimDescription.TeenOrBelow && !target.TraitManager.HasElement(TraitNames.SuperVampire) && !a.Posture.Satisfies(CommodityKind.SwimmingInPool, null) && !target.BuffManager.HasElement(BuffNames.WhatJustHappened) && a.BuffManager.HasElement(BuffNames.VampiricVigor) && Passion.Settings.VampireInteractions;
			}
		}

		public static readonly InteractionDefinition Singleton = new Definition();

		public static ResourceKey vampireiconResourceKey = new ResourceKey(11874435978993716202uL, 796721156u, 0u);

		protected override bool Run()
		{
			Definition definition = base.InteractionDefinition as Definition;
			Actor.SynchronizationLevel = Sim.SyncLevel.NotStarted;
			Target.SynchronizationLevel = Sim.SyncLevel.NotStarted;
			Target.InteractionQueue.CancelAllInteractions();
			if (BeginSocialInteraction(new SocialInteractionB.Definition(), false, 0.75f, true))
			{
				Target.LookAtManager.DisableLookAts();
				Actor.LookAtManager.DisableLookAts();
				Actor.RouteTurnToFace(Target.Position);
				Target.RouteTurnToFace(Actor.Position);
				StandardEntry();
				BeginCommodityUpdates();
				StartSocialContext();
				AcquireStateMachine("Vampire");
				EnterStateMachine("Vampire", "Enter", "x", "y");
				SetActor("x", Actor);
				SetActor("y", Target);
				EnterSim("Join");
				AnimateJoinSims("Amorus Bite");
				FinishLinkedInteraction(true);
				EndCommodityUpdates(true);
				StandardExit();
				WaitForSyncComplete(1f);
				VampireDrinkSuccess(Actor);
				VampVictBuffs(Actor, Target);
				if (Target.SimDescription.IsFairy)
				{
					Actor.BuffManager.AddElement(BuffNames.DrankFromAFairy, Origin.FromReceivingVampireNutrients);
				}
				EventTracker.SendEvent(EventTypeId.kVampireDrankFromSim, Actor, Target);
				EventTracker.SendEvent(new VampireLifetimeEvent(EventTypeId.kVampireLifetimeEvent, Actor.SimDescription, false, Target.SimDescription.SimDescriptionId));
			}
			if (!Target.BuffManager.HasElement(BuffNames.WhatJustHappened) && Actor.BuffManager.HasElement(BuffNames.VampiricVigor) && !Actor.BuffManager.HasElement(BuffNames.VampireBite) && (long)Actor.SkillManager.GetSkillLevel(SkillNames.Charisma) != 10)
			{
				Skill skill = Actor.SkillManager.AddElement(SkillNames.Charisma);
				skill.ForceSkillLevelUp(1 + Actor.SkillManager.GetSkillLevel(SkillNames.Charisma));
				Actor.BuffManager.AddElement(BuffNames.VampireBite, Origin.FromUnknown);
				Target.BuffManager.AddElement(BuffNames.WhatJustHappened, Origin.FromVampire);
			}
			else
			{
				Target.BuffManager.AddElement(BuffNames.WhatJustHappened, Origin.FromVampire);
			}
			Cleanup();
			return true;
		}

		public override ThumbnailKey GetIconKey()
		{
			return new ThumbnailKey(vampireiconResourceKey, ThumbnailSize.Large);
		}

		public static void VampVictBuffs(Sim vamp, Sim vic)
		{
			vic.BuffManager.AddElement(BuffNames.Weakened, Origin.FromProvidingVampireNutrients);
			vic.BuffManager.AddElement(BuffNames.Bitten, Origin.FromProvidingVampireNutrients);
		}

		public static void VampireDrinkSuccess(Sim actor)
		{
			if (actor.Motives.IsFullEnoughForStuffedBuff())
			{
				actor.BuffManager.AddElement(BuffNames.Stuffed, Origin.FromCarnivorousBehavior);
			}
			setMaxMotive(actor, CommodityKind.VampireThirst);
			if (actor.LotCurrent.IsCommunityLot)
			{
				DisgracefulActionEvent e = new DisgracefulActionEvent(EventTypeId.kSimCommittedDisgracefulAction, actor, DisgracefulActionType.BiteSomeoneInPublic);
				EventTracker.SendEvent(e);
			}
			if (actor.TraitManager.HasElement(TraitNames.Vegetarian))
			{
				actor.BuffManager.AddElement(BuffNames.Nauseous, Origin.FromCarnivorousBehavior);
			}
			else
			{
				actor.BuffManager.AddElement(BuffNames.Sated, Origin.FromReceivingVampireNutrients);
			}
		}

		public static void setMaxMotive(Sim actor, CommodityKind type)
		{
			actor.Motives.SetValue(type, 200f);
		}
	}
}
