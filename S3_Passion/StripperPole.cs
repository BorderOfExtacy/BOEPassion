using System.Collections.Generic;
using Sims3.Gameplay;
using Sims3.Gameplay.Abstracts;
using Sims3.Gameplay.Actors;
using Sims3.Gameplay.ActorSystems;
using Sims3.Gameplay.Autonomy;
using Sims3.Gameplay.Core;
using Sims3.Gameplay.Interactions;
using Sims3.Gameplay.Objects.Decorations;
using Sims3.Gameplay.Objects.Decorations.Mimics;
using Sims3.Gameplay.Objects.Electronics;
using Sims3.Gameplay.Services;
using Sims3.SimIFace;
using Sims3.SimIFace.CAS;
using Sims3.UI;

namespace S3_Passion
{
	public class StripperPole : PassionCommon
	{
		public class Dance : Interaction<Sim, SculptureFloorGunShow>
		{
			[DoesntRequireTuning]
			private sealed class Definition : InteractionDefinition<Sim, SculptureFloorGunShow, Dance>
			{
				public override string GetInteractionName(Sim actor, SculptureFloorGunShow target, InteractionObjectPair interaction)
				{
					return PassionCommon.Localize("S3_Passion.Terms.Dance");
				}

				public override bool Test(Sim actor, SculptureFloorGunShow target, bool IsAutonomous, ref GreyedOutTooltipCallback greyedOutTooltipCallback)
				{
					if (!IsAutonomous && actor != null && target != null && target.UseCount < 1 && actor.SimDescription.IsHuman && actor.SimDescription.TeenOrAbove)
					{
						return true;
					}
					return false;
				}
			}

			public static readonly InteractionDefinition Singleton = new Definition();

			public override bool Run()
			{
				List<Stereo> list = new List<Stereo>(Sims3.Gameplay.Queries.GetObjects<Stereo>(Actor.LotCurrent, Actor.RoomId));
				Stereo stereo = null;
				foreach (Stereo item in list)
				{
					if (item.TurnedOn)
					{
						stereo = item;
						break;
					}
				}
				if (Actor.RouteToObjectRadius(Target, 1f) && Target.UseCount < 1 && stereo != null)
				{
					Vector3 position = new Vector3(Actor.Position);
					OutfitCategories currentOutfitCategory = Actor.CurrentOutfitCategory;
					int currentOutfitIndex = Actor.CurrentOutfitIndex;
					Target.ClearUseList();
					Target.AddToUseList(Actor);
					Actor.SetPosition(Target.Position);
					Actor.SetForward(Target.ForwardVector);
					float x = Target.Position.x;
					float z = Target.Position.z;
					float y = Target.Position.y;
					int num = 2;
					WatchersHaveRouteToStripper = true;
					GetStripWatchers(Actor, Target);
					while (Actor.HasNoExitReason() && !Actor.Motives.CheckMotivesForTimeToLeave(Actor.Motives, (InteractionInstance)this, false, base.Autonomous))
					{
						StripperIsOnPole = true;
						Actor.PlaySoloAnimation(RandomUtil.GetRandomObjectFromList(Animations));
						if (num == 0)
						{
							if (Actor.CurrentOutfitCategory == OutfitCategories.Everyday)
							{
								Actor.SwitchToOutfitWithoutSpin(OutfitCategories.Swimwear, 0);
								num = 1;
							}
							else if (Actor.CurrentOutfitCategory == OutfitCategories.Swimwear)
							{
								Actor.SwitchToOutfitWithoutSpin(OutfitCategories.Sleepwear, 0);
								num = 0;
							}
							else if (Actor.CurrentOutfitCategory == OutfitCategories.Sleepwear)
							{
								Actor.SwitchToOutfitWithoutSpin(OutfitCategories.Naked, 0);
							}
							else if (Actor.CurrentOutfitCategory != OutfitCategories.Naked)
							{
								Actor.SwitchToOutfitWithoutSpin(OutfitCategories.Swimwear, 0);
								num = 2;
							}
						}
						if (num > 0)
						{
							num--;
						}
						// Libido.PartialSatisfaction(Actor);
						PassionCommon.Wait(2);
					}
					Target.RemoveFromUseList(Actor);
					Actor.SetPosition(position);
					StripperIsOnPole = false;
					WatchersHaveRouteToStripper = false;
					Actor.SwitchToOutfitWithoutSpin(currentOutfitCategory, currentOutfitIndex);
				}
				return true;
			}
		}

		public class Dance2 : Interaction<Sim, SculptureFloorGunShow>
		{
			[DoesntRequireTuning]
			private sealed class Definition : InteractionDefinition<Sim, SculptureFloorGunShow, Dance2>
			{
				public override string GetInteractionName(Sim actor, SculptureFloorGunShow target, InteractionObjectPair interaction)
				{
					return PassionCommon.Localize("S3_Passion.Terms.Dance2");
				}

				public override bool Test(Sim actor, SculptureFloorGunShow target, bool IsAutonomous, ref GreyedOutTooltipCallback greyedOutTooltipCallback)
				{
					if (!IsAutonomous && actor != null && target != null && target.UseCount < 1 && actor.SimDescription.IsHuman && actor.SimDescription.TeenOrAbove)
					{
						return true;
					}
					return false;
				}
			}

			public static readonly InteractionDefinition Singleton = new Definition();

			public override bool Run()
			{
				List<Stereo> list = new List<Stereo>(Sims3.Gameplay.Queries.GetObjects<Stereo>(Actor.LotCurrent, Actor.RoomId));
				Stereo stereo = null;
				foreach (Stereo item in list)
				{
					if (item.TurnedOn)
					{
						stereo = item;
						break;
					}
				}
				if (Actor.RouteToObjectRadius(Target, 1f) && Target.UseCount < 1 && stereo != null)
				{
					Vector3 position = new Vector3(Actor.Position);
					OutfitCategories currentOutfitCategory = Actor.CurrentOutfitCategory;
					int currentOutfitIndex = Actor.CurrentOutfitIndex;
					Target.ClearUseList();
					Target.AddToUseList(Actor);
					float x = Target.Position.x - (Target.ForwardVector.z * 0f + Target.ForwardVector.x * 0.795f);
					float y = Target.Position.y;
					float z = Target.Position.z - (Target.ForwardVector.z * 0f - Target.ForwardVector.z * -0.795f);
					Actor.SetForward(Target.ForwardVector);
					Vector3 position2 = new Vector3(x, y, z);
					Actor.SetPosition(position2);
					int num = 3;
					WatchersHaveRouteToStripper = true;
					GetStripWatchers(Actor, Target);
					while (Actor.HasNoExitReason() && !Actor.Motives.CheckMotivesForTimeToLeave(Actor.Motives, (InteractionInstance)this, false, base.Autonomous))
					{
						StripperIsOnPole = true;
						Actor.PlaySoloAnimation(RandomUtil.GetRandomObjectFromList(Animations2));
						if (num == 0)
						{
							if (Actor.CurrentOutfitCategory == OutfitCategories.Everyday)
							{
								Actor.SwitchToOutfitWithoutSpin(OutfitCategories.Swimwear, 0);
								num = 2;
							}
							else if (Actor.CurrentOutfitCategory == OutfitCategories.Swimwear)
							{
								Actor.SwitchToOutfitWithoutSpin(OutfitCategories.Sleepwear, 0);
								num = 1;
							}
							else if (Actor.CurrentOutfitCategory == OutfitCategories.Sleepwear)
							{
								Actor.SwitchToOutfitWithoutSpin(OutfitCategories.Naked, 0);
							}
							else if (Actor.CurrentOutfitCategory != OutfitCategories.Naked)
							{
								Actor.SwitchToOutfitWithoutSpin(OutfitCategories.Swimwear, 0);
								num = 3;
							}
						}
						if (num > 0)
						{
							num--;
						}
						// Libido.PartialSatisfaction(Actor);
						PassionCommon.Wait(2);
					}
					Target.RemoveFromUseList(Actor);
					Actor.SetPosition(position);
					StripperIsOnPole = false;
					WatchersHaveRouteToStripper = false;
					Actor.SwitchToOutfitWithoutSpin(currentOutfitCategory, currentOutfitIndex);
				}
				return true;
			}
		}

		public class AutoDance : Interaction<Sim, SculptureFloorGunShow>
		{
			[DoesntRequireTuning]
			private sealed class Definition : InteractionDefinition<Sim, SculptureFloorGunShow, AutoDance>
			{
				public override string GetInteractionName(Sim actor, SculptureFloorGunShow target, InteractionObjectPair interaction)
				{
					return PassionCommon.Localize("S3_Passion.Terms.Dance");
				}

				public override bool Test(Sim actor, SculptureFloorGunShow target, bool IsAutonomous, ref GreyedOutTooltipCallback greyedOutTooltipCallback)
				{
					if (!IsAutonomous && actor != null && target != null && target.UseCount < 1 && actor.SimDescription.TeenOrAbove && actor.SimDescription.IsHuman)
					{
						return true;
					}
					return false;
				}
			}

			public static readonly InteractionDefinition Singleton = new Definition();

			public override bool Run()
			{
				List<Stereo> list = new List<Stereo>(Sims3.Gameplay.Queries.GetObjects<Stereo>(Actor.LotCurrent, Actor.RoomId));
				Stereo stereo = null;
				foreach (Stereo item in list)
				{
					if (item.TurnedOn)
					{
						stereo = item;
						break;
					}
				}
				if (Actor.RouteToObjectRadius(Target, 1f) && Target.UseCount < 1 && stereo != null)
				{
					Vector3 position = new Vector3(Actor.Position);
					OutfitCategories currentOutfitCategory = Actor.CurrentOutfitCategory;
					int currentOutfitIndex = Actor.CurrentOutfitIndex;
					Target.ClearUseList();
					Target.AddToUseList(Actor);
					float x = Target.Position.x - (Target.ForwardVector.z * 0f + Target.ForwardVector.x * 0.795f);
					float y = Target.Position.y;
					float z = Target.Position.z - (Target.ForwardVector.z * 0f - Target.ForwardVector.z * -0.795f);
					Actor.SetForward(Target.ForwardVector);
					Vector3 position2 = new Vector3(x, y, z);
					Actor.SetPosition(position2);
					int num = 3;
					WatchersHaveRouteToStripper = true;
					GetStripWatchers(Actor, Target);
					while (Actor.HasNoExitReason() && !Actor.Motives.CheckMotivesForTimeToLeave(Actor.Motives, (InteractionInstance)this, false, base.Autonomous))
					{
						StripperIsOnPole = true;
						Actor.PlaySoloAnimation(RandomUtil.GetRandomObjectFromList(Animations2));
						if (num == 0)
						{
							if (Actor.CurrentOutfitCategory == OutfitCategories.Everyday)
							{
								Actor.SwitchToOutfitWithoutSpin(OutfitCategories.Swimwear, 0);
								num = 2;
							}
							else if (Actor.CurrentOutfitCategory == OutfitCategories.Swimwear)
							{
								Actor.SwitchToOutfitWithoutSpin(OutfitCategories.Sleepwear, 0);
								num = 1;
							}
							else if (Actor.CurrentOutfitCategory == OutfitCategories.Sleepwear)
							{
								Actor.SwitchToOutfitWithoutSpin(OutfitCategories.Naked, 0);
								num = 2;
							}
							else if (Actor.CurrentOutfitCategory != OutfitCategories.Naked)
							{
								Actor.SwitchToOutfitWithoutSpin(OutfitCategories.Swimwear, 0);
								num = 3;
							}
							else if (Actor.CurrentOutfitCategory == OutfitCategories.Naked && num <= 0)
							{
								break;
							}
						}
						if (num > 0)
						{
							num--;
						}
						// Libido.PartialSatisfaction(Actor);
						PassionCommon.Wait(2u);
					}
					Target.RemoveFromUseList(Actor);
					Actor.SetPosition(position);
					StripperIsOnPole = false;
					WatchersHaveRouteToStripper = false;
					if (Actor.SimDescription.IsServicePerson && !(Actor.Service is Butler) && !(Actor.Service is Maid))
					{
						Actor.SwitchToOutfitWithoutSpin(Passion.MyOutfit);
					}
					else
					{
						Actor.SwitchToOutfitWithoutSpin(currentOutfitCategory, currentOutfitIndex);
					}
				}
				Sim actor = Actor;
				if (Actor.LotCurrent.CountObjects<DanceFloor>() != 0)
				{
					Sim sim = Actor;
					GameObject gameObject = Target;
					List<DanceFloor> list2 = new List<DanceFloor>(Sims3.Gameplay.Queries.GetObjects<DanceFloor>(sim.LotCurrent, sim.RoomId));
					if (!sim.SimDescription.IsServicePerson || sim.Service is Butler || sim.Service is Maid)
					{
						foreach (Sim sim2 in sim.LotCurrent.GetSims())
						{
							if (sim2.SimDescription.TeenOrAbove && sim2.SimDescription.IsHuman && !sim2.LotCurrent.IsWorldLot)
							{
								sim = sim2;
							}
							foreach (DanceFloor item2 in list2)
							{
								gameObject = item2;
							}
							if (GameUtils.IsInstalled(ProductVersion.EP3) && gameObject != null && sim != null && !sim.SimDescription.IsServicePerson && RandomUtil.CoinFlip())
							{
								sim.InteractionQueue.AddAfterCheckingForDuplicates(DanceFloor.Dance.Singleton.CreateInstance(gameObject, sim, new InteractionPriority(InteractionPriorityLevel.UserDirected), false, true));
							}
						}
					}
				}
				Actor = actor;
				return true;
			}
		}

		internal sealed class AskToDanceOnPole : ImmediateInteraction<Sim, SculptureFloorGunShow>
		{
			[DoesntRequireTuning]
			private sealed class Definition : ImmediateInteractionDefinition<Sim, SculptureFloorGunShow, AskToDanceOnPole>
			{
				public override string GetInteractionName(Sim actor, SculptureFloorGunShow target, InteractionObjectPair interaction)
				{
					return PassionCommon.Localize("S3_Passion.Terms.AskToDance");
				}

				public override void PopulatePieMenuPicker(ref InteractionInstanceParameters parameters, out List<ObjectPicker.TabInfo> listObjs, out List<ObjectPicker.HeaderInfo> headers, out int NumSelectableRows)
				{
					NumSelectableRows = 1;
					PopulateSimPicker(ref parameters, out listObjs, out headers, GetThePeople(parameters.Actor as Sim, parameters.Actor.LotCurrent), false);
				}

				public override bool Test(Sim actor, SculptureFloorGunShow target, bool IsAutonomous, ref GreyedOutTooltipCallback greyedOutTooltipCallback)
				{
					return actor.SimDescription.TeenOrAbove && actor.SimDescription.IsHuman && target.UseCount == 0;
				}
			}

			public static readonly InteractionDefinition Singleton = new Definition();

			public override bool Run()
			{
				Sim sim = GetSelectedObject() as Sim;
				sim.InteractionQueue.AddNextIfPossibleAfterCheckingForDuplicates(Dance.Singleton.CreateInstance(Target, sim, new InteractionPriority(InteractionPriorityLevel.UserDirected), false, true));
				return true;
			}
		}

		internal sealed class AskToDanceOnPole2 : ImmediateInteraction<Sim, SculptureFloorGunShow>
		{
			[DoesntRequireTuning]
			private sealed class Definition : ImmediateInteractionDefinition<Sim, SculptureFloorGunShow, AskToDanceOnPole2>
			{
				public override string GetInteractionName(Sim actor, SculptureFloorGunShow target, InteractionObjectPair interaction)
				{
					return PassionCommon.Localize("S3_Passion.Terms.AskToDance2");
				}

				public override void PopulatePieMenuPicker(ref InteractionInstanceParameters parameters, out List<ObjectPicker.TabInfo> listObjs, out List<ObjectPicker.HeaderInfo> headers, out int NumSelectableRows)
				{
					NumSelectableRows = 1;
					PopulateSimPicker(ref parameters, out listObjs, out headers, GetThePeople(parameters.Actor as Sim, parameters.Actor.LotCurrent), false);
				}

				public override bool Test(Sim actor, SculptureFloorGunShow target, bool IsAutonomous, ref GreyedOutTooltipCallback greyedOutTooltipCallback)
				{
					return actor.SimDescription.TeenOrAbove && actor.SimDescription.IsHuman && target.UseCount == 0;
				}
			}

			public static readonly InteractionDefinition Singleton = new Definition();

			public override bool Run()
			{
				Sim sim = GetSelectedObject() as Sim;
				sim.InteractionQueue.AddNextIfPossibleAfterCheckingForDuplicates(Dance2.Singleton.CreateInstance(Target, sim, new InteractionPriority(InteractionPriorityLevel.UserDirected), false, true));
				return true;
			}
		}

		internal sealed class DanceOnPoleStop : ImmediateInteraction<Sim, Sim>
		{
			[DoesntRequireTuning]
			private sealed class Definition : ImmediateInteractionDefinition<Sim, Sim, DanceOnPoleStop>
			{
				public override string GetInteractionName(Sim actor, Sim target, InteractionObjectPair interaction)
				{
					return PassionCommon.Localize("S3_Passion.Terms.StopDance");
				}

				public override bool Test(Sim actor, Sim target, bool IsAutonomous, ref GreyedOutTooltipCallback greyedOutTooltipCallback)
				{
					return target.InteractionQueue.GetCurrentInteraction() != null && (target.InteractionQueue.GetCurrentInteraction().InteractionDefinition == Dance.Singleton || target.InteractionQueue.GetCurrentInteraction().InteractionDefinition == Dance2.Singleton);
				}
			}

			public static readonly InteractionDefinition Singleton = new Definition();

			public override bool Run()
			{
				Target.InteractionQueue.CancelInteraction(Target.InteractionQueue.GetCurrentInteraction().Id, ExitReason.UserCanceled);
				StripperIsOnPole = false;
				return true;
			}
		}

		internal sealed class Strip : ImmediateInteraction<Sim, Sim>
		{
			[DoesntRequireTuning]
			private sealed class Definition : ImmediateInteractionDefinition<Sim, Sim, Strip>
			{
				public override string GetInteractionName(Sim actor, Sim target, InteractionObjectPair interaction)
				{
					return PassionCommon.Localize("S3_Passion.Terms.Strip");
				}

				public override bool Test(Sim actor, Sim target, bool IsAutonomous, ref GreyedOutTooltipCallback greyedOutTooltipCallback)
				{
					try
					{
						if (!IsAutonomous && actor == target && actor.SimDescription.IsHuman && actor.CurrentInteraction != null && actor.CurrentInteraction.InteractionDefinition == Dance.Singleton)
						{
							return true;
						}
						if (!IsAutonomous && actor == target && actor.SimDescription.IsHuman && actor.CurrentInteraction != null && actor.CurrentInteraction.InteractionDefinition == Dance2.Singleton)
						{
							return true;
						}
					}
					catch
					{
					}
					return false;
				}
			}

			public static readonly InteractionDefinition Singleton = new Definition();

			public override bool Run()
			{
				Actor.SwitchToOutfitWithoutSpin(OutfitCategories.Naked);
				return true;
			}
		}

		public class WatchStrip : Interaction<Sim, SculptureFloorGunShow>
		{
			[DoesntRequireTuning]
			private sealed class Definition : InteractionDefinition<Sim, SculptureFloorGunShow, WatchStrip>
			{
				public override string GetInteractionName(Sim actor, SculptureFloorGunShow target, InteractionObjectPair interaction)
				{
					return PassionCommon.Localize("S3_Passion.Terms.Watch") + " " + PassionCommon.Localize("S3_Passion.Terms.Strip");
				}

				public override bool Test(Sim actor, SculptureFloorGunShow target, bool IsAutonomous, ref GreyedOutTooltipCallback greyedOutTooltipCallback)
				{
					if (!IsAutonomous && actor != null && actor != Sim.ActiveActor && target != null && target.UseCount > 0 && !actor.SimDescription.IsServicePerson && actor.SimDescription.IsHuman)
					{
						return true;
					}
					return false;
				}
			}

			public static float kMinRouteDistance = 1f;

			public static float kMaxRouteDistance = 5f;

			public static float kMinTimeBetweenReaction = 4f;

			public static float kMaxTimeBetweenReaction = 5f;

			public float mTimeSinceLastReaction = 1f;

			public float mTimeBetweenReaction;

			public WatchStrip mWatchedInteraction;

			[Persistable(false)]
			public List<ReactionTypes> mReactions = new List<ReactionTypes>(new ReactionTypes[4]
			{
				ReactionTypes.Cheer,
				ReactionTypes.PumpFist,
				ReactionTypes.Giggle,
				ReactionTypes.Fascinated
			});

			public static readonly InteractionDefinition Singleton = new Definition();

			public override bool Run()
			{
				List<Stereo> list = new List<Stereo>(Sims3.Gameplay.Queries.GetObjects<Stereo>(Actor.LotCurrent, Actor.RoomId));
				Stereo stereo = null;
				foreach (Stereo item in list)
				{
					if (item.TurnedOn)
					{
						stereo = item;
						break;
					}
				}
				Sim sim = ((Target.ActorsUsingMe.Count > 0) ? Target.ActorsUsingMe[0] : null);
				if (sim != null && stereo != null)
				{
					Passion.GetPlayer(Actor).Watch(sim);
					Route route = Actor.CreateRoute();
					route.PlanToPointRadialRange(Target.Position, kMinRouteDistance, kMaxRouteDistance, RouteDistancePreference.NoPreference, RouteOrientationPreference.TowardsObject, Target.LotCurrent.LotId, new int[1] { Target.RoomId });
					if (!Actor.DoRoute(route) || Actor.InteractionQueue.GetCurrentInteraction().InteractionDefinition == Passion.Interactions.UseObjectForPassion.Singleton || Actor.InteractionQueue.GetCurrentInteraction().InteractionDefinition == Passion.Interactions.UseSimForPassion.Singleton || Actor.InteractionQueue.GetCurrentInteraction().InteractionDefinition == Passion.Interactions.ActiveJoinPassion.Singleton || Actor.InteractionQueue.GetCurrentInteraction().InteractionDefinition == Passion.Interactions.JoinPassion.Singleton || Actor.InteractionQueue.GetCurrentInteraction().InteractionDefinition == Passion.Interactions.AskToJoinPassion.Singleton || Actor.InteractionQueue.GetCurrentInteraction().InteractionDefinition == Passion.Interactions.AskToSoloPassion.Singleton || Actor.InteractionQueue.GetCurrentInteraction().InteractionDefinition == Passion.Interactions.AskToPassionOther.Singleton || Actor.InteractionQueue.GetCurrentInteraction().InteractionDefinition == Passion.Interactions.AskToWatchPassion.Singleton || Actor.InteractionQueue.GetCurrentInteraction().InteractionDefinition == Passion.Interactions.WatchPassion.Singleton || Actor.InteractionQueue.GetCurrentInteraction().InteractionDefinition == Passion.Interactions.WatchMasturbate.Singleton)
					{
						return false;
					}
					BeginCommodityUpdates();
					Actor.LoopIdle();
					bool flag = DoLoop(ExitReason.Default, WatchLoop, mCurrentStateMachine);
					EndCommodityUpdates(flag);
					return flag;
				}
				return false;
			}

			public void WatchLoop(StateMachineClient smc, LoopData ld)
			{
				if (Actor.HasExitReason(ExitReason.Canceled) || !StripperIsOnPole)
				{
					Actor.AddExitReason(ExitReason.Finished);
					return;
				}
				mTimeSinceLastReaction += ld.mDeltaTime;
				if (mTimeBetweenReaction < mTimeSinceLastReaction)
				{
					ReactionTypes randomObjectFromList = RandomUtil.GetRandomObjectFromList(mReactions);
					Actor.PlayReaction(randomObjectFromList, ReactionSpeed.ImmediateWithoutOverlay);
					mTimeSinceLastReaction = 0f;
					mTimeBetweenReaction = RandomUtil.GetFloat(kMinTimeBetweenReaction, kMaxTimeBetweenReaction);
				}
			}
		}

		protected static bool StripperIsOnPole = false;

		protected static bool WatchersHaveRouteToStripper = false;

		public static List<string> Animations;

		public static List<string> Animations2;

		private static List<Sim> GetThePeople(Sim actor, Lot targetLot)
		{
			List<Sim> list = new List<Sim>();
			foreach (Sim sim in targetLot.GetSims())
			{
				if (actor != sim && sim.SimDescription.TeenOrAbove && sim.SimDescription.IsHuman && !sim.LotCurrent.IsWorldLot)
				{
					list.Add(sim);
				}
			}
			return list;
		}

		public static void GetStripWatchers(Sim actor, GameObject pole)
		{
			foreach (Sim sim in actor.LotCurrent.GetSims())
			{
				if (sim != actor && !sim.SimDescription.IsServicePerson)
				{
					sim.InteractionQueue.AddAfterCheckingForDuplicates(WatchStrip.Singleton.CreateInstance(pole, sim, new InteractionPriority(InteractionPriorityLevel.UserDirected), false, true));
				}
			}
		}

		static StripperPole()
		{
			List<string> list = new List<string>();
			list.Add("a_mmd_doax2_poledance_1");
			list.Add("a_mmd_doax2_poledance_2");
			Animations = list;
			List<string> list2 = new List<string>();
			list2.Add("a2o_DancePole_Practice_Start_x");
			list2.Add("a2o_DancePole_Practice_Loop_1_x");
			list2.Add("a2o_DancePole_Practice_Loop_1_x");
			list2.Add("a2o_DancePole_Practice_Loop_2_x");
			list2.Add("a2o_DancePole_Practice_Loop_2_x");
			list2.Add("a2o_DancePole_Practice_Stop_x");
			Animations2 = list2;
		}
	}
}
