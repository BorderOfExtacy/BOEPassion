using Sims3.Gameplay.Abstracts;
using Sims3.Gameplay.Actors;
using Sims3.Gameplay.ActorSystems;
using Sims3.Gameplay.Autonomy;
using Sims3.Gameplay.EventSystem;
using Sims3.Gameplay.Interactions;
using Sims3.Gameplay.Socializing;
using Sims3.Gameplay.Utilities;
using Sims3.SimIFace;

namespace S3_Passion
{
	internal sealed class VampireMindControl : SocialInteraction
	{
		[DoesntRequireTuning]
		private sealed class Definition : InteractionDefinition<Sim, Sim, VampireMindControl>, IHasTraitIcon, IHasMenuPathIcon
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
				return Localization.LocalizeString(target.IsFemale, "Vampire Mind Control", target.SimDescription, actor.SimDescription);
			}

			public override string[] GetPath(bool bPath)
			{
				return new string[1] { Localization.LocalizeString("Vampire...") };
			}

			protected override bool Test(Sim actor, Sim target, bool isAutonomous, ref GreyedOutTooltipCallback greyedOutTooltipCallback)
			{
				if (GameUtils.IsInstalled(ProductVersion.EP7) && actor.TraitManager.HasElement(TraitNames.SuperVampire) && actor.SimDescription.IsVampire && !actor.Posture.Satisfies(CommodityKind.SwimmingInPool, null) && !target.Posture.Satisfies(CommodityKind.SwimmingInPool, null) && !target.SimDescription.IsRobot && target != actor && !target.SimDescription.IsEP11Bot && target.Household != actor.Household && !target.BuffManager.HasElement(BuffNames.WeddingDay) && !target.SimDescription.IsPregnant && !target.BuffManager.HasElement(BuffNames.Ensorcelled) && !target.SimDescription.IsServicePerson && !target.SimDescription.ChildOrBelow && !target.SimDescription.IsBonehilda && !target.SimDescription.IsPet && !target.TraitManager.HasElement(TraitNames.SuperVampire) && Passion.Settings.VampireInteractions)
				{
					return true;
				}
				return false;
			}
		}

		public static readonly InteractionDefinition Singleton = new Definition();

		public static ResourceKey vampireiconResourceKey = new ResourceKey(11874435978993716202uL, 796721156u, 0u);

		public override ThumbnailKey GetIconKey()
		{
			return new ThumbnailKey(vampireiconResourceKey, ThumbnailSize.Large);
		}

		protected override bool Run()
		{
			Definition definition = base.InteractionDefinition as Definition;
			Actor.SynchronizationLevel = Sim.SyncLevel.NotStarted;
			Target.SynchronizationLevel = Sim.SyncLevel.NotStarted;
			Target.InteractionQueue.CancelAllInteractions();
			if (BeginSocialInteraction(new SocialInteractionB.Definition(), false, 1f, true))
			{
				Actor.RouteTurnToFace(Target.Position);
				Target.RouteTurnToFace(Actor.Position);
				StandardEntry();
				BeginCommodityUpdates();
				StartSocialContext();
				AcquireStateMachine("vampirehypnoticgaze");
				EnterStateMachine("vampirehypnoticgaze", "Enter", "x", "y");
				SetActor("x", Actor);
				SetActor("y", Target);
				EnterState("x", "Enter");
				EnterState("y", "Enter");
				AnimateJoinSims("Success");
				StandardExit();
				WaitForSyncComplete(1.5f);
				FinishLinkedInteraction(true);
				EventTracker.SendEvent(EventTypeId.kEnsorcelSim, Actor, Target);
				EndCommodityUpdates(true);
				Target.BuffManager.AddElement(BuffNames.Ensorcelled, Origin.None);
				Cleanup();
				return true;
			}
			Cleanup();
			return false;
		}
	}
}
