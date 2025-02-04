using Sims3.Gameplay.Actors;
using Sims3.Gameplay.ActorSystems;
using Sims3.Gameplay.Autonomy;
using Sims3.Gameplay.Interactions;
using Sims3.Gameplay.Objects.Alchemy;
using Sims3.Gameplay.Socializing;
using Sims3.Gameplay.Utilities;
using Sims3.SimIFace;

namespace Passion.S3_Passion
{
	internal sealed class WitchReleaseMindControl : SocialInteraction
	{
		[DoesntRequireTuning]
		private sealed class Definition : InteractionDefinition<Sim, Sim, WitchReleaseMindControl>
		{
			public override string[] GetPath(bool isFemale)
			{
				return new string[1] { MagicWand.LocalizeString(isFemale, "CastSpell", new object[0]) + Localization.Ellipsis };
			}

			public override string GetInteractionName(Sim actor, Sim target, InteractionObjectPair interaction)
			{
				return Localization.LocalizeString("Release Mind Control");
			}

			public override bool Test(Sim actor, Sim target, bool isAutonomous, ref GreyedOutTooltipCallback greyedOutTooltipCallback)
			{
				if (actor.SimDescription.IsWitch && target.BuffManager.HasElement(BuffNames.Ensorcelled))
				{
					return true;
				}
				return false;
			}
		}

		public static readonly InteractionDefinition Singleton = new Definition();

		public override bool Run()
		{
			Actor.RouteTurnToFace(Target.Position);
			Target.BuffManager.RemoveElement(BuffNames.Ensorcelled);
			StandardEntry();
			BeginCommodityUpdates();
			Actor.PlaySoloAnimation("a_genie_clap_x", true, ProductVersion.EP6);
			EndCommodityUpdates(true);
			VisualEffect visualEffect = VisualEffect.Create("ep3vampirereadmindout_test");
			visualEffect.ParentTo(Target, Sim.FXJoints.Head);
			visualEffect.SubmitOneShotEffect(VisualEffect.TransitionType.SoftTransition);
			Target.BuffManager.AddElement(BuffNames.KnockedOut, Origin.FromUnfriendlyVampire);
			return true;
		}
	}
}
