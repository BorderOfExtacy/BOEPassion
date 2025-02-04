using Sims3.Gameplay.Abstracts;
using Sims3.Gameplay.Actors;
using Sims3.Gameplay.ActorSystems;
using Sims3.Gameplay.Autonomy;
using Sims3.Gameplay.Interactions;
using Sims3.Gameplay.Socializing;
using Sims3.Gameplay.Utilities;
using Sims3.SimIFace;

namespace Passion.S3_Passion
{
	internal sealed class VampireFireBlast : SocialInteraction
	{
		[DoesntRequireTuning]
		private sealed class Definition : InteractionDefinition<Sim, Sim, VampireFireBlast>, IHasTraitIcon, IHasMenuPathIcon
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
				return Localization.LocalizeString(target.IsFemale, "Vampire Fire Blast", target.SimDescription, actor.SimDescription);
			}

			public override string[] GetPath(bool bPath)
			{
				return new string[1] { Localization.LocalizeString("Vampire...") };
			}

			public override bool Test(Sim actor, Sim target, bool isAutonomous, ref GreyedOutTooltipCallback greyedOutTooltipCallback)
			{
				if (GameUtils.IsInstalled(ProductVersion.EP7) && actor.SimDescription.IsVampire && target != actor && actor.TraitManager.HasElement(TraitNames.SuperVampire) && !target.SimDescription.IsServicePerson && !target.SimDescription.ChildOrBelow && !target.SimDescription.IsPet && !target.SimDescription.IsWildAnimal && !target.SimDescription.IsPregnant && Passion.Settings.VampireInteractions && !actor.Posture.Satisfies(CommodityKind.SwimmingInPool, null))
				{
					return true;
				}
				return false;
			}
		}

		public static readonly InteractionDefinition Singleton = new Definition();

		public static ResourceKey VampireiconResourceKey = new ResourceKey(11874435978993716202uL, 796721156u, 0u);

		public override ThumbnailKey GetIconKey()
		{
			return new ThumbnailKey(VampireiconResourceKey, ThumbnailSize.Large);
		}

		public override bool Run()
		{
			Actor.RouteTurnToFace(Target.Position);
			StandardEntry();
			BeginCommodityUpdates();
			Actor.PlaySoloAnimation("a_genie_clap_x", true, ProductVersion.EP6);
			EndCommodityUpdates(true);
			VisualEffect visualEffect = VisualEffect.Create("ep3VampireThinkAbout");
			visualEffect.ParentTo(Target, Sim.FXJoints.Head);
			visualEffect.SubmitOneShotEffect(VisualEffect.TransitionType.SoftTransition);
			PassionCommon.Wait(50);
			VisualEffect visualEffect2 = VisualEffect.Create("ep6GenieTargetSimwhirlwind_main");
			visualEffect2.ParentTo(Target, Sim.FXJoints.Spine0);
			visualEffect2.SubmitOneShotEffect(VisualEffect.TransitionType.SoftTransition);
			PassionCommon.Wait(50);
			Target.BuffManager.AddElement(BuffNames.OnFire, Origin.FromUnfriendlyVampire);
			Cleanup();
			return true;
		}
	}
}
