using Sims3.Gameplay.Abstracts;
using Sims3.Gameplay.Actors;
using Sims3.Gameplay.ActorSystems;
using Sims3.Gameplay.Autonomy;
using Sims3.Gameplay.Interactions;
using Sims3.Gameplay.Skills;
using Sims3.Gameplay.Socializing;
using Sims3.SimIFace;

namespace Passion.S3_Passion
{
	public class VampireAttacked : SocialInteraction
	{
		[DoesntRequireTuning]
		public sealed class Definition : InteractionDefinition<Sim, Sim, VampireAttacked>, IHasTraitIcon, IHasMenuPathIcon
		{
			public ResourceKey GetPathIcon(Sim actor, GameObject target)
			{
				return ResourceKey.CreatePNGKey("trait_nocturnal_s_ep7", 0u);
			}

			public ResourceKey GetTraitIcon(Sim actor, GameObject target)
			{
				return ResourceKey.CreatePNGKey("trait_nocturnal_s_ep7", 0u);
			}

			public override bool Test(Sim a, Sim target, bool isAutonomous, ref GreyedOutTooltipCallback greyedOutTooltipCallback)
			{
				return GameUtils.IsInstalled(ProductVersion.EP7) && Passion.Settings.VampireInteractions;
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
			if (!Target.BuffManager.HasElement(BuffNames.WhatJustHappened))
			{
				if (BeginSocialInteraction(new SocialInteractionB.Definition(), false, 0.65f, true))
				{
					Actor.RouteTurnToFace(Target.Position);
					Target.RouteTurnToFace(Actor.Position);
					Target.LookAtManager.DisableLookAts();
					Actor.LookAtManager.DisableLookAts();
					StandardEntry();
					BeginCommodityUpdates();
					StartSocialContext();
					AcquireStateMachine("Vampire");
					EnterStateMachine("Vampire", "Enter", "x", "y");
					SetActor("y", Actor);
					SetActor("x", Target);
					EnterSim("Join");
					AnimateJoinSims("LaughAt");
					FinishLinkedInteraction(true);
					EndCommodityUpdates(true);
					WaitForSyncComplete(0.5f);
					StandardExit();
					Relationship relationship = Relationship.Get(Target, Actor, true);
					relationship.LTR.UpdateLiking(20f);
				}
			}
			else if (BeginSocialInteraction(new SocialInteractionB.Definition(), false, 0.65f, true))
			{
				Actor.RouteTurnToFace(Target.Position);
				Target.RouteTurnToFace(Actor.Position);
				Target.LookAtManager.DisableLookAts();
				Actor.LookAtManager.DisableLookAts();
				StandardEntry();
				BeginCommodityUpdates();
				StartSocialContext();
				AcquireStateMachine("Vampire");
				EnterStateMachine("Vampire", "Enter", "x", "y");
				SetActor("y", Actor);
				SetActor("x", Target);
				EnterSim("Join");
				AnimateJoinSims("Reject");
				FinishLinkedInteraction(true);
				EndCommodityUpdates(true);
				WaitForSyncComplete(0.5f);
				StandardExit();
				Relationship relationship2 = Relationship.Get(Target, Actor, true);
				relationship2.LTR.UpdateLiking(-20f);
			}
			if (!Target.BuffManager.HasElement(BuffNames.WhatJustHappened) && Actor.BuffManager.HasElement(BuffNames.VampiricVigor) && !Actor.BuffManager.HasElement(BuffNames.AdrenalineRush) && (long)Actor.SkillManager.GetSkillLevel(SkillNames.Athletic) != 10)
			{
				Skill skill = Actor.SkillManager.AddElement(SkillNames.Athletic);
				skill.ForceSkillLevelUp(1 + Actor.SkillManager.GetSkillLevel(SkillNames.Athletic));
				Actor.BuffManager.AddElement(BuffNames.AdrenalineRush, Origin.FromUnknown);
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
			return new ThumbnailKey(VampireiconResourceKey, ThumbnailSize.Large);
		}
	}
}
