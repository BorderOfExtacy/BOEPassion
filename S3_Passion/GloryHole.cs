using System.Collections.Generic;
using Sims3.Gameplay.Abstracts;
using Sims3.Gameplay.Actors;
using Sims3.Gameplay.ActorSystems;
using Sims3.Gameplay.Autonomy;
using Sims3.Gameplay.Interactions;
using Sims3.Gameplay.Objects.Door;
using Sims3.SimIFace;
using Sims3.UI;

namespace Passion.S3_Passion
{
	public class GloryHole : PassionCommon
	{
		[Persistable]
		public class ActiveGloryHole
		{
			public bool AnimationSwitch;

			public bool CommodityUpdates;

			public Sim Slut;

			public bool SlutReady;

			public Sim Stud;

			public bool StudReady;

			public FenceRedwood_Gate GloryHole;

			public const int Cost = 10;

			public bool HasSlut
			{
				get
				{
					return Slut != null;
				}
			}

			public bool HasStud
			{
				get
				{
					return Stud != null;
				}
			}

			public bool HasBoth
			{
				get
				{
					return HasStud && HasSlut;
				}
			}

			public bool BothReady
			{
				get
				{
					return SlutReady && StudReady;
				}
			}

			public static ActiveGloryHole Create(FenceRedwood_Gate hole)
			{
				ActiveGloryHole activeGloryHole = new ActiveGloryHole();
				activeGloryHole.GloryHole = hole;
				activeGloryHole.AnimationSwitch = false;
				activeGloryHole.CommodityUpdates = false;
				activeGloryHole.SlutReady = false;
				activeGloryHole.StudReady = false;
				return activeGloryHole;
			}
		}

		public class ServiceStrangers : Interaction<Sim, FenceRedwood_Gate>
		{
			[DoesntRequireTuning]
			private sealed class Definition : InteractionDefinition<Sim, FenceRedwood_Gate, ServiceStrangers>
			{
				public override string GetInteractionName(Sim actor, FenceRedwood_Gate target, InteractionObjectPair interaction)
				{
					return PassionCommon.Localize("S3_Passion.Terms.ServiceStrangers");
				}

				public override bool Test(Sim actor, FenceRedwood_Gate target, bool isAutonomous, ref GreyedOutTooltipCallback greyedOutTooltipCallback)
				{
					if (actor == null || target == null || isAutonomous) return false;
					ActiveGloryHole activeGloryHole = Get(target);
					return activeGloryHole != null && !activeGloryHole.HasSlut;
				}
			}

			public static readonly InteractionDefinition Singleton = new Definition();

			public override bool Run()
			{
				ActiveGloryHole activeGloryHole = Get(Target);
				if (activeGloryHole == null || !Actor.RouteToSlot(Target, Door.RoutingSlots.Door0_Rear) ||
				    activeGloryHole.HasSlut) return true;
				activeGloryHole.Slut = Actor;
				Actor.SetPosition(activeGloryHole.GloryHole.Position);
				Actor.SetForward(activeGloryHole.GloryHole.ForwardVector);
				while (Actor.HasNoExitReason() && !Actor.Motives.CheckMotivesForTimeToLeave(Actor.Motives, this, false, base.Autonomous))
				{
					activeGloryHole.SlutReady = true;
					if (activeGloryHole.AnimationSwitch)
					{
						activeGloryHole.SlutReady = false;
						Actor.PlaySoloAnimation("a_gloryhole_blowjobB_01v");
						activeGloryHole.AnimationSwitch = false;
						Libido.PartialSatisfaction(Actor);
					}
					PassionCommon.Wait(2);
				}
				if (activeGloryHole.HasStud)
				{
					activeGloryHole.Stud.CurrentInteraction.EndCommodityUpdates(false);
				}
				EndCommodityUpdates(true);
				activeGloryHole.CommodityUpdates = false;
				activeGloryHole.SlutReady = false;
				activeGloryHole.Slut = null;
				return true;
			}
		}

		public class GetSucked : Interaction<Sim, FenceRedwood_Gate>
		{
			[DoesntRequireTuning]
			private sealed class Definition : InteractionDefinition<Sim, FenceRedwood_Gate, GetSucked>
			{
				public override string GetInteractionName(Sim actor, FenceRedwood_Gate target, InteractionObjectPair interaction)
				{
					ActiveGloryHole activeGloryHole = Get(target);
					if (activeGloryHole != null)
					{
						return PassionCommon.Localize("S3_Passion.Terms.UseGloryHole") + ": " + UIUtils.FormatMoney(ActiveGloryHole.Cost);
					}
					return PassionCommon.Localize("S3_Passion.Terms.UseGloryHole");
				}

				public override bool Test(Sim actor, FenceRedwood_Gate target, bool isAutonomous, ref GreyedOutTooltipCallback greyedOutTooltipCallback)
				{
					if (actor == null || target == null || isAutonomous) return false;
					ActiveGloryHole activeGloryHole = Get(target);
					return activeGloryHole != null && !activeGloryHole.HasStud;
				}
			}

			public static readonly InteractionDefinition Singleton = new Definition();

			public override bool Run()
			{
				ActiveGloryHole activeGloryHole = Get(Target);
				if (activeGloryHole == null || !Actor.RouteToSlot(Target, Door.RoutingSlots.Door0_Front) ||
				    activeGloryHole.HasStud) return true;
				activeGloryHole.Stud = Actor;
				Actor.SetPosition(activeGloryHole.GloryHole.Position);
				Actor.SetForward(activeGloryHole.GloryHole.ForwardVector);
				while (Actor.HasNoExitReason() && !Actor.Motives.CheckMotivesForTimeToLeave(Actor.Motives, this, false, base.Autonomous))
				{
					activeGloryHole.StudReady = true;
					if (activeGloryHole.BothReady)
					{
						if (!activeGloryHole.CommodityUpdates)
						{
							BeginCommodityUpdate(new CommodityChange(CommodityKind.Fun, 300f, false, 300f, OutputUpdateType.First, false, true, UpdateAboveAndBelowZeroType.Either), 1f);
							activeGloryHole.Slut.CurrentInteraction.BeginCommodityUpdate(new CommodityChange(CommodityKind.Social, 50f, false, 50f, OutputUpdateType.First, false, true, UpdateAboveAndBelowZeroType.Either), 1f);
							activeGloryHole.Slut.CurrentInteraction.BeginCommodityUpdate(new CommodityChange(CommodityKind.Hygiene, -50f, false, -50f, OutputUpdateType.First, false, true, UpdateAboveAndBelowZeroType.Either), 1f);
							if (Actor.HasTrait(TraitNames.PartyAnimal) || Actor.HasTrait(TraitNames.Adventurous) || Actor.HasTrait(TraitNames.Slob))
							{
								activeGloryHole.Slut.CurrentInteraction.BeginCommodityUpdate(new CommodityChange(CommodityKind.Fun, 150f, false, 150f, OutputUpdateType.First, false, true, UpdateAboveAndBelowZeroType.Either), 1f);
							}
							activeGloryHole.CommodityUpdates = true;
						}
						activeGloryHole.AnimationSwitch = true;
					}
					if (activeGloryHole.AnimationSwitch)
					{
						activeGloryHole.StudReady = false;
						Actor.PlaySoloAnimation("a_gloryhole_blowjobA_01v");
						activeGloryHole.AnimationSwitch = false;
						Libido.PartialSatisfaction(Actor);
						if (activeGloryHole.HasSlut && Actor.Motives.GetMotiveValue(CommodityKind.Bladder) < -80f)
						{
							Actor.Motives.LerpToFill(this, CommodityKind.Bladder, 2f);
							activeGloryHole.Slut.Motives.LerpToFill(activeGloryHole.Slut.CurrentInteraction, CommodityKind.Hunger, 2f);
						}
					}
					PassionCommon.Wait(2);
				}
				int num = ((Actor.FamilyFunds >= ActiveGloryHole.Cost) ? ActiveGloryHole.Cost : Actor.FamilyFunds);
				if (activeGloryHole.HasSlut)
				{
					Actor.ModifyFunds(-num);
					activeGloryHole.Slut.ModifyFunds(num);
					activeGloryHole.Slut.CurrentInteraction.EndCommodityUpdates(false);
				}
				EndCommodityUpdates(true);
				activeGloryHole.CommodityUpdates = false;
				activeGloryHole.StudReady = false;
				activeGloryHole.Stud = null;
				return true;
			}
		}

		[PersistableStatic] private static Dictionary<ulong, ActiveGloryHole> _mActiveGloryHoles;

		private static Dictionary<ulong, ActiveGloryHole> ActiveGloryHoles
		{
			get { return _mActiveGloryHoles ?? (_mActiveGloryHoles = new Dictionary<ulong, ActiveGloryHole>()); }
		}

		private static ActiveGloryHole Get(FenceRedwood_Gate hole)
		{
			ActiveGloryHole activeGloryHole;
			if (hole == null) return null;
			ulong value = hole.ObjectId.Value;
			if (ActiveGloryHoles.ContainsKey(value))
			{
				if (ActiveGloryHoles[value] != null)
				{
					activeGloryHole = ActiveGloryHoles[value];
				}
				else
				{
					activeGloryHole = ActiveGloryHole.Create(hole);
					ActiveGloryHoles[value] = activeGloryHole;
				}
			}
			else
			{
				activeGloryHole = ActiveGloryHole.Create(hole);
				ActiveGloryHoles.Add(value, activeGloryHole);
			}
			return activeGloryHole;
		}
	}
}
