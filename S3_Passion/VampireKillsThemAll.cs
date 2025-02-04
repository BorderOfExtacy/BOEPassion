using System.Collections.Generic;
using Sims3.Gameplay.Abstracts;
using Sims3.Gameplay.Actors;
using Sims3.Gameplay.ActorSystems;
using Sims3.Gameplay.Autonomy;
using Sims3.Gameplay.Core;
using Sims3.Gameplay.Interactions;
using Sims3.Gameplay.Utilities;
using Sims3.SimIFace;

namespace Passion.S3_Passion
{
	public sealed class VampireKillsThemAll : TerrainInteraction
	{
		public sealed class Definition : InteractionDefinition<Sim, Terrain, VampireKillsThemAll>, IHasTraitIcon, IHasMenuPathIcon
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
				return Localization.LocalizeString("Kill Them All");
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
				if (Terrain.GoHere.SharedGoHereTests(ref parameters))
				{
					Lot lotAtPoint = LotManager.GetLotAtPoint(parameters.Hit.mPoint);
					if (lotAtPoint != parameters.Actor.LotCurrent || lotAtPoint == parameters.Actor.LotHome)
					{
						return InteractionTestResult.GenericFail;
					}
					bool flag = false;
					Sim[] objects = global::Sims3.Gameplay.Queries.GetObjects<Sim>(parameters.Hit.mPoint, 4f);
					if (objects != null && objects.Length != 0)
					{
						Sim[] array = objects;
						foreach (Sim sim in array)
						{
							if (sim != null)
							{
								flag = true;
								break;
							}
						}
					}
					int num = 0;
					foreach (Sim sim2 in parameters.Actor.LotCurrent.GetSims())
					{
						if (!sim2.SimDescription.ChildOrBelow && !sim2.SimDescription.IsRobot && sim2 != parameters.Actor && flag && !sim2.SimDescription.IsEP11Bot && !sim2.SimDescription.IsBonehilda && !sim2.SimDescription.IsPet && !parameters.Actor.LotCurrent.IsWorldLot)
						{
							num++;
						}
					}
					if (parameters.Actor.SimDescription.IsVampire && parameters.Actor.TraitManager.HasElement(TraitNames.SuperVampire) && num > 0)
					{
						return InteractionTestResult.Pass;
					}
				}
				return InteractionTestResult.GenericFail;
			}

			public override bool Test(Sim actor, Terrain target, bool isAutonomous, ref GreyedOutTooltipCallback greyedOutTooltipCallback)
			{
				if (GameUtils.IsInstalled(ProductVersion.EP7) && actor.TraitManager.HasElement(TraitNames.SuperVampire) && actor.TraitManager.HasElement(TraitNames.Evil) && Passion.Settings.VampireInteractions)
				{
					return true;
				}
				return false;
			}
		}

		public static Vector3 Destination;

		public static readonly InteractionDefinition Singleton = new Definition();

		public static ResourceKey VampireiconResourceKey = new ResourceKey(11874435978993716202uL, 796721156u, 0u);

		public override ThumbnailKey GetIconKey()
		{
			return new ThumbnailKey(VampireiconResourceKey, ThumbnailSize.Large);
		}

		public override bool Run()
		{
			Lot lotCurrent = Actor.LotCurrent;
			List<Sim> list = new List<Sim>();
			if (Actor.LotCurrent.GetAllActorsCount() != 0 && !lotCurrent.IsWorldLot)
			{
				foreach (Sim sim in lotCurrent.GetSims())
				{
					if (sim != Actor && !sim.SimDescription.ChildOrBelow && !sim.SimDescription.IsPet && !sim.SimDescription.IsPregnant && !sim.SimDescription.IsRobot && !sim.SimDescription.IsServicePerson)
					{
						Actor.InteractionQueue.PushAsContinuation(VampireBitesToKill.Singleton.CreateInstance(sim, Actor, new InteractionPriority(InteractionPriorityLevel.UserDirected), false, true), true);
					}
				}
			}
			return true;
		}
	}
}
