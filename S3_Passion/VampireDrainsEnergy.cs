using Sims3.Gameplay.Abstracts;
using Sims3.Gameplay.Actors;
using Sims3.Gameplay.ActorSystems;
using Sims3.Gameplay.Autonomy;
using Sims3.Gameplay.Interactions;
using Sims3.Gameplay.Socializing;
using Sims3.Gameplay.ThoughtBalloons;
using Sims3.Gameplay.Utilities;
using Sims3.SimIFace;

namespace S3_Passion
{
	internal sealed class VampireDrainsEnergy : SocialInteraction
	{
		[DoesntRequireTuning]
		private sealed class Definition : InteractionDefinition<Sim, Sim, VampireDrainsEnergy>, IHasTraitIcon, IHasMenuPathIcon
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
				return Localization.LocalizeString(target.IsFemale, "Drain Energy", target.SimDescription, actor.SimDescription);
			}

			public override string[] GetPath(bool bPath)
			{
				return new string[1] { Localization.LocalizeString("Vampire...") };
			}

			protected override bool Test(Sim actor, Sim target, bool isAutonomous, ref GreyedOutTooltipCallback greyedOutTooltipCallback)
			{
				if (GameUtils.IsInstalled(ProductVersion.EP7) && actor.SimDescription.IsVampire && !actor.Posture.Satisfies(CommodityKind.SwimmingInPool, null) && !target.Posture.Satisfies(CommodityKind.SwimmingInPool, null) && !target.SimDescription.IsRobot && target != actor && !target.TraitManager.HasElement(TraitNames.SuperVampire) && !target.SimDescription.IsEP11Bot && !target.SimDescription.ChildOrBelow && !target.SimDescription.IsBonehilda && !target.SimDescription.IsPet && Passion.Settings.VampireInteractions)
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
			Actor.RouteTurnToFace(Target.Position);
			ThoughtBalloonManager.BalloonData balloonData = new ThoughtBalloonManager.BalloonData(Target.GetThumbnailKey());
			balloonData.BalloonType = ThoughtBalloonTypes.kThoughtBalloon;
			balloonData.Duration = ThoughtBalloonDuration.Medium;
			balloonData.mPriority = ThoughtBalloonPriority.High;
			Actor.ThoughtBalloonManager.ShowBalloon(balloonData);
			VisualEffect visualEffect = VisualEffect.Create("ep3VampireReadMindOut");
			visualEffect.ParentTo(Target, Sim.FXJoints.Head);
			visualEffect.SubmitOneShotEffect(VisualEffect.TransitionType.SoftTransition);
			VisualEffect visualEffect2 = VisualEffect.Create("ep3VampireReadMindIn");
			visualEffect2.ParentTo(Actor, Sim.FXJoints.Head);
			visualEffect2.SubmitOneShotEffect(VisualEffect.TransitionType.SoftTransition);
			EnterStateMachine("VampireHunt", "Enter", "x");
			AnimateSim("Hunt Loop");
			AnimateSim("Exit");
			WaitForSyncComplete(1f);
			FinishLinkedInteraction(true);
			Target.Motives.SetValue(CommodityKind.Energy, -90f);
			Actor.Motives.SetMax(CommodityKind.Energy);
			Target.BuffManager.AddElement(BuffNames.KnockedOut, Origin.FromUnfriendlyVampire);
			Cleanup();
			return true;
		}
	}
}
