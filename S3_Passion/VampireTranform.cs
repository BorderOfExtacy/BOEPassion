using Sims3.Gameplay.Abstracts;
using Sims3.Gameplay.Actors;
using Sims3.Gameplay.ActorSystems;
using Sims3.Gameplay.Autonomy;
using Sims3.Gameplay.CelebritySystem;
using Sims3.Gameplay.EventSystem;
using Sims3.Gameplay.Interactions;
using Sims3.Gameplay.Socializing;
using Sims3.Gameplay.Utilities;
using Sims3.SimIFace;
using Sims3.UI.Hud;

namespace Passion.S3_Passion
{
	public class VampireTranform : SocialInteraction
	{
		[DoesntRequireTuning]
		public sealed class Definition : InteractionDefinition<Sim, Sim, VampireTranform>, IHasTraitIcon, IHasMenuPathIcon
		{
			public ResourceKey GetPathIcon(Sim actor, GameObject target)
			{
				return ResourceKey.CreatePNGKey("trait_nocturnal_s_ep7", 0u);
			}

			public ResourceKey GetTraitIcon(Sim actor, GameObject target)
			{
				return ResourceKey.CreatePNGKey("trait_nocturnal_s_ep7", 0u);
			}

			public override string GetInteractionName(Sim actor, Sim target, InteractionObjectPair interaction)
			{
				return Localization.LocalizeString(target.IsFemale, "Vampire Tranform", target.SimDescription, actor.SimDescription);
			}

			public override string[] GetPath(bool isFemale)
			{
				return new string[1] { Localization.LocalizeString(isFemale, "Vampire...") };
			}

			public override bool Test(Sim a, Sim target, bool isAutonomous, ref GreyedOutTooltipCallback greyedOutTooltipCallback)
			{
				return GameUtils.IsInstalled(ProductVersion.EP7) && target != a && a.TraitManager.HasElement(TraitNames.SuperVampire) && !target.IsActiveSim && !target.SimDescription.IsServicePerson && !target.Posture.Satisfies(CommodityKind.SwimmingInPool, null) && !target.SimDescription.IsGhost && !target.SimDescription.ChildOrBelow && !target.SimDescription.IsPet && !target.SimDescription.IsVampire && a.SimDescription.IsVampire && !a.SimDescription.TeenOrBelow && !a.Posture.Satisfies(CommodityKind.SwimmingInPool, null) && Passion.Settings.VampireInteractions;
			}
		}

		public static readonly InteractionDefinition Singleton = new Definition();

		public static ResourceKey VampireiconResourceKey = new ResourceKey(11874435978993716202uL, 796721156u, 0u);

		public override bool Run()
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
				WaitForSyncComplete(1f);
				AcquireStateMachine("Vampire");
				EnterStateMachine("Vampire", "Enter", "x", "y");
				SetActor("y", Actor);
				SetActor("x", Target);
				EnterSim("Join");
				AnimateJoinSims("Bite");
				WaitForSyncComplete(1f);
				AcquireStateMachine("Vampire");
				EnterStateMachine("Vampire", "Enter", "x");
				SetActor("x", Target);
				EnterSim("Join");
				AnimateSim("Transform");
				FinishLinkedInteraction(true);
				EndCommodityUpdates(true);
				StandardExit();
				WaitForSyncComplete(1.5f);
			}
			VampireDrinkSuccess(Actor);
			VampVictBuffs(Actor, Target);
			if (!Target.SimDescription.IsVampire)
			{
				Target.OccultManager.MergeOccult(OccultTypes.Vampire);
			}
			if (Target.SimDescription.IsFairy)
			{
				Actor.BuffManager.AddElement(BuffNames.DrankFromAFairy, Origin.FromReceivingVampireNutrients);
			}
			EventTracker.SendEvent(EventTypeId.kVampireDrankFromSim, Actor, Target);
			EventTracker.SendEvent(new VampireLifetimeEvent(EventTypeId.kVampireLifetimeEvent, Actor.SimDescription, true, Target.SimDescription.SimDescriptionId));
			Cleanup();
			return true;
		}

		public static void SetMaxMotive(Sim actor, CommodityKind type)
		{
			actor.Motives.SetValue(type, 100f);
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
			SetMaxMotive(actor, CommodityKind.VampireThirst);
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

		public override ThumbnailKey GetIconKey()
		{
			return new ThumbnailKey(VampireiconResourceKey, ThumbnailSize.Large);
		}
	}
}
