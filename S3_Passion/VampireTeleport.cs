using Sims3.Gameplay.Abstracts;
using Sims3.Gameplay.Actors;
using Sims3.Gameplay.Autonomy;
using Sims3.Gameplay.Core;
using Sims3.Gameplay.Interactions;
using Sims3.Gameplay.Interfaces;
using Sims3.Gameplay.Utilities;
using Sims3.SimIFace;
using Sims3.UI.Hud;

namespace Passion.S3_Passion
{
	public sealed class VampireTeleport : TerrainInteraction
	{
		public sealed class Definition : InteractionDefinition<Sim, Terrain, VampireTeleport>, IHasTraitIcon, IHasMenuPathIcon
		{
			public ResourceKey GetPathIcon(Sim actor, GameObject target)
			{
				return ResourceKey.CreatePNGKey("trait_nocturnal_s_ep7", 0u);
			}

			public ResourceKey GetTraitIcon(Sim actor, GameObject target)
			{
				return ResourceKey.CreatePNGKey("trait_nocturnal_s_ep7", 0u);
			}

			public override string GetInteractionName(Sim actor, Terrain target, InteractionObjectPair interaction)
			{
				return Localization.LocalizeString(actor.IsFemale, "Vampire Teleport", target, actor.SimDescription);
			}

			public override string[] GetPath(bool isFemale)
			{
				return new string[1] { Localization.LocalizeString(isFemale, "Vampire...") };
			}

			public override InteractionTestResult Test(ref InteractionInstanceParameters parameters, ref GreyedOutTooltipCallback greyedOutTooltipCallback)
			{
				if (!Test(parameters.Actor as Sim, parameters.Target as Terrain, parameters.Autonomous, ref greyedOutTooltipCallback))
				{
					return InteractionTestResult.Special_IsPerformingShow;
				}
				return Terrain.CanSimTeleport(parameters.Hit, parameters.Actor as Sim);
			}

			public override bool Test(Sim actor, Terrain target, bool isAutonomous, ref GreyedOutTooltipCallback greyedOutTooltipCallback)
			{
				actor.SimDescription.OccultManager.GetOccultType(OccultTypes.Vampire);
				return actor.SimDescription.IsVampire && GameUtils.IsInstalled(ProductVersion.EP7) && Passion.Settings.VampireInteractions;
			}
		}

		public ITeleporter MTeleporter;

		public static Vector3 Destination;

		public static readonly InteractionDefinition Singleton = new Definition();

		public static ResourceKey VampireiconResourceKey = new ResourceKey(11874435978993716202uL, 796721156u, 0u);

		public override ThumbnailKey GetIconKey()
		{
			return new ThumbnailKey(VampireiconResourceKey, ThumbnailSize.Large);
		}

		public override bool Run()
		{
			Sim actor = Actor;
			if (!actor.Posture.Satisfies(CommodityKind.SwimmingInPool, null) && !actor.Posture.Satisfies(CommodityKind.KeepSwimming, null))
			{
				TeleportVamp(actor, base.Destination, null, true, true);
				base.Cleanup();
				return true;
			}
			string effectName = "ep3vampiretransition";
			actor.InteractionQueue.CancelAllInteractions();
			actor.PopPosture();
			actor.SimRoutingComponent.DisableDynamicFootprint();
			VisualEffect visualEffect = VisualEffect.Create(effectName);
			actor.FadeOut(true, false, 0.5f);
			actor.SetPosition(base.Destination);
			visualEffect.SetPosAndOrient(actor.Position, actor.ForwardVector, actor.UpVector);
			visualEffect.SubmitOneShotEffect(VisualEffect.TransitionType.SoftTransition);
			actor.FadeIn(true, 0.5f);
			base.Cleanup();
			return false;
		}

		public static void TeleportVamp(Sim actor, Vector3 destination, ITeleporter teleporter, bool bPlayVfXvamp, bool fadeSim)
		{
			string effectName = null;
			if (bPlayVfXvamp)
			{
				effectName = "ep3vampiretransition";
				VisualEffect visualEffect = VisualEffect.Create(effectName);
				visualEffect.SetPosAndOrient(actor.Position, actor.ForwardVector, actor.UpVector);
				visualEffect.SubmitOneShotEffect(VisualEffect.TransitionType.SoftTransition);
				if (actor.SimDescription.Child)
				{
					actor.PlaySoloAnimation("c_floating_x", false);
				}
				else
				{
					actor.PlaySoloAnimation("a_floating_x", false);
				}
			}
			if (fadeSim)
			{
				actor.FadeOut(true, false, 1.3f);
			}
			actor.SimRoutingComponent.DisableDynamicFootprint();
			actor.SetPosition(destination);
			if (bPlayVfXvamp)
			{
				VisualEffect visualEffect2 = VisualEffect.Create(effectName);
				visualEffect2.SetPosAndOrient(actor.Position, actor.ForwardVector, actor.UpVector);
				visualEffect2.SubmitOneShotEffect(VisualEffect.TransitionType.SoftTransition);
			}
			actor.SimRoutingComponent.EnableDynamicFootprint();
			if (fadeSim)
			{
				actor.FadeOut(true, false, 0.5f);
			}
			if (fadeSim)
			{
				actor.FadeIn(true, 0.9f);
			}
		}
	}
}
