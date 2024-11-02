using System.Text;
using Sims3.Gameplay.Abstracts;
using Sims3.Gameplay.Actors;
using Sims3.Gameplay.ActorSystems;
using Sims3.Gameplay.Autonomy;
using Sims3.Gameplay.Interactions;
using Sims3.Gameplay.Socializing;
using Sims3.Gameplay.Utilities;
using Sims3.SimIFace;

namespace S3_Passion
{
	internal sealed class BeHypnoticGazeFuck : SocialInteraction
	{
		[DoesntRequireTuning]
		private sealed class Definition : InteractionDefinition<Sim, Sim, BeHypnoticGazeFuck>, IHasTraitIcon, IHasMenuPathIcon
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
				return Localization.LocalizeSpeciesString(actor, target.IsFemale, "Look Deep Into My Eyes And Listen to my Voice", actor.SimDescription, target.SimDescription);
			}

			public override string[] GetPath(bool bPath)
			{
				return new string[1] { Localization.LocalizeString("Vampire...") };
			}

			public override bool Test(Sim actor, Sim target, bool isAutonomous, ref GreyedOutTooltipCallback greyedOutTooltipCallback)
			{
				if (GameUtils.IsInstalled(ProductVersion.EP7) && target.SimDescription.IsVampire && !actor.SimDescription.IsRobot && !target.Posture.Satisfies(CommodityKind.SwimmingInPool, null) && !actor.Posture.Satisfies(CommodityKind.SwimmingInPool, null) && target != actor && !actor.BuffManager.HasElement(BuffNames.Dazed) && !actor.SimDescription.ChildOrBelow && !actor.SimDescription.IsPet && actor.InteractionQueue.GetCurrentInteraction() != Singleton && Passion.Settings.VampireInteractions)
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

		public override bool Run()
		{
			Definition definition = base.InteractionDefinition as Definition;
			Actor.SynchronizationLevel = Sim.SyncLevel.NotStarted;
			Target.SynchronizationLevel = Sim.SyncLevel.NotStarted;
			if (BeginSocialInteraction(new SocialInteractionB.Definition(), false, 1f, true))
			{
				Actor.RouteTurnToFace(Target.Position);
				Target.RouteTurnToFace(Actor.Position);
				StandardEntry();
				BeginCommodityUpdates();
				StartSocialContext();
				AcquireStateMachine("vampirehypnoticgaze");
				EnterStateMachine("vampirehypnoticgaze", "Enter", "x", "y");
				SetActor("x", Target);
				SetActor("y", Actor);
				EnterState("x", "Enter");
				EnterState("y", "Enter");
				AnimateJoinSims("Success");
				StandardExit();
				WaitForSyncComplete(1.5f);
				FinishLinkedInteraction(true);
				EndCommodityUpdates(true);
				Actor.BuffManager.AddElement(BuffNames.Dazed, Origin.FromVampire);
				Passion.Player player = Passion.GetPlayer(Target);
				Passion.Player player2 = Passion.GetPlayer(Actor);
				Passion.Settings.AutonomyNotify = true;
				Actor.InteractionQueue.AddAfterCheckingForDuplicates(Passion.Interactions.UseSimForPassion.Singleton.CreateInstance(Actor, Target, new InteractionPriority(InteractionPriorityLevel.RequiredNPCBehavior), false, true));
				Actor.BuffManager.AddElement(BuffNames.Obsessed, Origin.FromVampire);
				Actor.BuffManager.RemoveElement(BuffNames.ImminentNemesis);
				if (Passion.Settings.AutonomyChance > 0 && Passion.Settings.AutonomyNotify)
				{
					player.IsAutonomous = true;
					player2.IsAutonomous = true;
					if (Target != Actor)
					{
						try
						{
							StringBuilder stringBuilder = new StringBuilder(PassionCommon.Localize("S3_Passion.Terms.AutonomyNotifySimMessage"));
							stringBuilder.Replace("[player]", player.Name);
							stringBuilder.Replace("[label]", Passion.Settings.ActiveLabel.ToLower());
							stringBuilder.Replace("[partner]", player2.Name);
							stringBuilder.Replace("[address]", player.Actor.LotCurrent.Name);
							PassionCommon.SimMessage(stringBuilder.ToString(), player.Actor, player2.Actor);
						}
						catch
						{
						}
					}
				}
				Cleanup();
				return true;
			}
			Cleanup();
			return false;
		}
	}
}
