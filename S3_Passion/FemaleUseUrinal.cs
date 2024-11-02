using System.Collections.Generic;
using Sims3.Gameplay;
using Sims3.Gameplay.Abstracts;
using Sims3.Gameplay.Actors;
using Sims3.Gameplay.ActorSystems;
using Sims3.Gameplay.Autonomy;
using Sims3.Gameplay.Core;
using Sims3.Gameplay.Interactions;
using Sims3.Gameplay.Interfaces;
using Sims3.Gameplay.Objects.CookingObjects;
using Sims3.Gameplay.Objects.Plumbing;
using Sims3.Gameplay.ThoughtBalloons;
using Sims3.Gameplay.Utilities;
using Sims3.SimIFace;

namespace S3_Passion
{
	public sealed class FemaleUseUrinal : Interaction<Sim, Urinal>
	{
		private sealed class Definition : InteractionDefinition<Sim, Urinal, FemaleUseUrinal>
		{
			public override string GetInteractionName(Sim actor, Urinal target, InteractionObjectPair iop)
			{
				return Localization.LocalizeString(actor.IsFemale, "Gameplay/Objects/Plumbing/Toilet/UseToilet:InteractionName");
			}

			public override bool Test(Sim sim, Urinal target, bool isAutonomous, ref GreyedOutTooltipCallback greyedOutTooltipCallback)
			{
				return sim.IsFemale;
			}
		}

		private const int mDrinkPopInHandEventID = 100;

		private const int mGrabDrinkStartEventID = 104;

		private const int mTurnOffCensorEventID = 120;

		public static readonly InteractionDefinition Singleton = new Definition();

		private Glass mDrinkInHand;

		private bool mHasFarted;

		private float mFartTime;

		public override bool Run()
		{
			BuffInstance element = Actor.BuffManager.GetElement(BuffNames.ReallyHasToPee);
			if (element != null && element.mTimeoutCount <= 30f)
			{
				RequestWalkStyle(Sim.WalkStyle.Run);
			}
			if (!Target.Line.WaitForTurn(this, SimQueue.WaitBehavior.DefaultAllowSubstitution, ExitReason.Default, 20f))
			{
				return false;
			}
			if (!Target.RouteToUrinalAndCheckInUse(Actor))
			{
				return false;
			}
			ClearRequestedWalkStyles();
			if (!Target.RouteToUrinalAndCheckInUse(Actor))
			{
				return false;
			}
			base.CancellableByPlayer = false;
			StandardEntry();
			mCurrentStateMachine = Target.GetStateMachine(Actor);
			Glass.CarryingGlassPosture carryingGlassPosture = Actor.Posture as Glass.CarryingGlassPosture;
			if (carryingGlassPosture != null)
			{
				mDrinkInHand = carryingGlassPosture.ObjectBeingCarried as Glass;
				CarrySystem.ExitCarry(Actor);
				mDrinkInHand.FadeOut(true);
				mDrinkInHand.UnParent();
				Actor.PopPosture();
				SetParameter("hasDrink", true);
				SetActor("drink", mDrinkInHand);
				if (Target.HasDrinkSlot && Target.GetContainedObject(Slot.ContainmentSlot_0) == null)
				{
					mDrinkInHand.ParentToSlot(Target, Slot.ContainmentSlot_0);
					mDrinkInHand.FadeIn();
				}
			}
			AddOneShotScriptEventHandler(120u, OnAnimationEvent);
			AnimateSim("use");
			if (element != null)
			{
				element.mTimeoutPaused = true;
			}
			mFartTime = RandomUtil.RandomFloatGaussianDistribution(0.1f, 0.9f);
			BeginCommodityUpdate(CommodityKind.Bladder, 0f);
			BeginCommodityUpdates();
			Actor.Motives.LerpToFill(this, CommodityKind.Bladder, 10f);
			StartStages();
			bool flag = DoLoop(~(ExitReason.HigherPriorityNext | ExitReason.MaxSkillPointsReached | ExitReason.BuffFailureState | ExitReason.PlayIdle | ExitReason.ObjectStateChanged | ExitReason.MidRoutePushRequested | ExitReason.Replan), LoopFunc, mCurrentStateMachine);
			EndCommodityUpdates(flag);
			if (flag)
			{
				Motive motive = Actor.Motives.GetMotive(CommodityKind.Bladder);
				if (motive != null)
				{
					motive.PotionBladderDecayOverride = false;
				}
			}
			if (element != null)
			{
				element.mTimeoutPaused = false;
			}
			if (Target.IsCleanable)
			{
				Target.Cleanable.DirtyInc(Actor);
			}
			bool flag2 = Target.Line.MemberCount() > 1;
			InteractionInstance interactionInstance = null;
			AddOneShotScriptEventHandler(104u, OnAnimationEvent);
			AddOneShotScriptEventHandler(100u, OnAnimationEvent);
			AnimateSim("exit");
			if (mDrinkInHand != null)
			{
				CarrySystem.EnterWhileHolding(Actor, mDrinkInHand);
				Actor.Posture = new Glass.CarryingGlassPosture(Actor, mDrinkInHand);
				mDrinkInHand.FadeIn();
			}
			StandardExit();
			if (interactionInstance != null)
			{
				Actor.InteractionQueue.PushAsContinuation(interactionInstance, true);
			}
			if (!flag2 && interactionInstance == null)
			{
				Actor.RouteAway(2.5f, 2.5f, false, GetPriority(), true, true, true, RouteDistancePreference.NoPreference);
			}
			return flag;
		}

		private void ReactToFartCallback(Sim s, ReactionBroadcaster rb)
		{
			GameObject gameObject = rb.BroadcastingObject as GameObject;
			if (gameObject != s)
			{
				if (s.HasTrait(TraitNames.Inappropriate))
				{
					s.PlayReaction(ReactionTypes.SmellYummy, gameObject, ReactionSpeed.NowOrLater);
				}
				else
				{
					s.PlayReaction(ReactionTypes.Smelly, gameObject, ReactionSpeed.NowOrLater);
				}
			}
		}

		private void LoopFunc(StateMachineClient smc, LoopData loopData)
		{
			if (!mHasFarted)
			{
				float num = base.ActiveStage.PortionComplete(this);
				if (num >= mFartTime)
				{
					mHasFarted = true;
					AnimateSim("fart");
					ReactionBroadcaster.CreateOneShotPulse(Actor, new ReactionBroadcasterParams(), ReactToFartCallback);
				}
			}
			ThoughtBalloonManager.BalloonData balloonData = null;
			if (Actor.ThoughtBalloonManager.CurrentBalloon != null)
			{
				return;
			}
			bool flag = true;
			List<IToiletOrUrinal> list = new List<IToiletOrUrinal>(Sims3.Gameplay.Queries.GetObjects<IToiletOrUrinal>(Actor.LotCurrent, Actor.RoomId));
			List<Sim> list2 = new List<Sim>();
			foreach (IToiletOrUrinal item in list)
			{
				if (item is Urinal && item.InUse)
				{
					Urinal urinal = item as Urinal;
					if (urinal.ActorsUsingMe[0] != Actor)
					{
						list2.Add(urinal.ActorsUsingMe[0]);
					}
				}
			}
			if (list2.Count > 0)
			{
				int @int = RandomUtil.GetInt(list2.Count - 1);
				balloonData = new ThoughtBalloonManager.BalloonData(list2[@int].GetThumbnailKey());
			}
			if (balloonData == null)
			{
				balloonData = ThoughtBalloonManager.GetBalloonData("Urinal", Actor);
			}
			Actor.ThoughtBalloonManager.ShowBalloon(balloonData);
		}

		private void OnAnimationEvent(StateMachineClient smc, IEvent evt)
		{
			switch (evt.EventId)
			{
			case 120u:
				Actor.AutoEnableCensor();
				break;
			case 104u:
				if (mDrinkInHand != null)
				{
					mDrinkInHand.FadeOut();
					mDrinkInHand.UnParent();
				}
				break;
			case 100u:
				if (mDrinkInHand != null)
				{
					mDrinkInHand.FadeIn();
				}
				break;
			}
		}

		public override void ConfigureInteraction()
		{
			TimedStage timedStage = new TimedStage(GetInteractionName(), 10f, false, true, false);
			base.Stages = new List<Stage>(new Stage[1] { timedStage });
		}

		public override void Cleanup()
		{
			base.Cleanup();
		}
	}
}
