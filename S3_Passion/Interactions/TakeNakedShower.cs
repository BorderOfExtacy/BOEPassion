using System.Collections.Generic;
using Sims3.Gameplay.Abstracts;
using Sims3.Gameplay.Actors;
using Sims3.Gameplay.ActorSystems;
using Sims3.Gameplay.Autonomy;
using Sims3.Gameplay.Core;
using Sims3.Gameplay.DreamsAndPromises;
using Sims3.Gameplay.EventSystem;
using Sims3.Gameplay.Interactions;
using Sims3.Gameplay.Objects.Plumbing;
using Sims3.Gameplay.Skills;
using Sims3.Gameplay.Utilities;
using Sims3.SimIFace;
using Sims3.SimIFace.CAS;
using Sims3.Store.Objects;

namespace Passion.S3_Passion.Interactions
{
	public class TakeNakedShower : Interaction<Sim, ShowerPublic_Dance>
	{
		private sealed class Definition : InteractionDefinition<Sim, ShowerPublic_Dance, TakeNakedShower>
		{
			public override string GetInteractionName(Sim actor, ShowerPublic_Dance target, InteractionObjectPair iop)
			{
				if (actor.HasTrait(TraitNames.NaturalBornPerformer))
				{
					return LocalizeString("NBPInteractionName");
				}
				if (actor.HasTrait(TraitNames.Evil))
				{
					return LocalizeString("EvilInteractionName");
				}
				if (actor.HasTrait(TraitNames.EnvironmentallyConscious))
				{
					return LocalizeString("EnvironmentallyConsciousInteractionName");
				}
				return actor.HasTrait(TraitNames.Good) ? LocalizeString("GoodInteractionName") : LocalizeString(actor.IsFemale, actor.TraitManager.HasElement(TraitNames.Daredevil) ? "DaredevilInteractionName" : "InteractionName");
			}

			public override bool Test(Sim a, ShowerPublic_Dance target, bool isAutonomous, ref GreyedOutTooltipCallback greyedOutTooltipCallback)
			{
				return !isAutonomous || (a.Autonomy.Motives.GetValue(CommodityKind.Hygiene) < 100f && !a.SimDescription.IsFrankenstein);
			}
		}

		//private const string SLocalizationKey = "Gameplay/Objects/Plumbing/Shower/TakeShower";

		private ObjectSound _mSingInShowerSound;

		private ObjectSound _mShoweringSound;

		private Sim.SwitchOutfitHelper _mSwitchOutfitHelper;

		private TimedStage _mShowerStage;

		public static readonly InteractionDefinition Singleton = new Definition();

		private static string LocalizeString(string name, params object[] parameters)
		{
			return Localization.LocalizeString("Gameplay/Objects/Plumbing/Shower/TakeShower:" + name, parameters);
		}

		private static string LocalizeString(bool isFemale, string name, params object[] parameters)
		{
			return Localization.LocalizeString(isFemale, "Gameplay/Objects/Plumbing/Shower/TakeShower:" + name, parameters);
		}

		public void EventCallbackStartSinging(StateMachineClient sender, IEvent evt)
		{
			string name = (Actor.SimDescription.ChildOrBelow ? "vo_shower_singC" : "vo_shower_singA");
			_mSingInShowerSound = new ObjectSound(Actor.ObjectId, name);
			_mSingInShowerSound.StartLoop();
		}

		public void EventCallbackStopSinging(StateMachineClient sender, IEvent evt)
		{
			StopSingInShowerSound();
		}

		private void StopSingInShowerSound()
		{
			if (_mSingInShowerSound != null)
			{
				_mSingInShowerSound.Stop();
				_mSingInShowerSound.Dispose();
				_mSingInShowerSound = null;
			}
		}

		public void EventCallbackStartShoweringSound(StateMachineClient sender, IEvent evt)
		{
			if (_mShoweringSound == null)
			{
				_mShoweringSound = new ObjectSound(Target.ObjectId, "shower_running_lp");
				_mShoweringSound.StartLoop();
			}
		}

		public void EventCallbackStopShoweringSound(StateMachineClient sender, IEvent evt)
		{
			StopShoweringSound();
		}

		private void StopShoweringSound()
		{
			if (_mShoweringSound != null)
			{
				_mShoweringSound.Stop();
				_mShoweringSound.Dispose();
				_mShoweringSound = null;
			}
		}

		public float GetShowerTime()
		{
			return 15f;
		}

		public override void ConfigureInteraction()
		{
			float showerTime = GetShowerTime();
			_mShowerStage = new TimedStage(GetInteractionName(), showerTime, false, true, true);
			Stages = new List<Stage>(new Stage[] { _mShowerStage });
		}

		private static void WaitToLeaveShower(Sim actor, IShowerable shower)
		{
			GameObject gameObject = shower as GameObject;
			if (gameObject != null)
			{
				gameObject.EnableFootprintAndPushSims(824351308u, actor);
				while (gameObject.IsRoutingSlotObstructed(actor, Slot.RoutingSlot_0))
				{
					Simulator.Sleep(5u);
				}
			}
		}

		private static void ApplyPostShowerEffects(Sim actor, IShowerable shower)
		{
			BuffManager buffManager = actor.BuffManager;
			buffManager.RemoveElement(BuffNames.Singed);
			buffManager.RemoveElement(BuffNames.SingedElectricity);
			buffManager.RemoveElement(BuffNames.GarlicBreath);
			if (actor.SimDescription.IsMummy)
			{
				buffManager.AddElement(BuffNames.Soaked, Origin.FromShower);
			}
			if (RandomUtil.RandomChance(shower.TuningShower.ChanceOfExhileratingShowerBuff))
			{
				buffManager.AddElement(BuffNames.ExhilaratingShower, Origin.FromNiceShower);
			}
			if (actor.HasTrait(TraitNames.Hydrophobic))
			{
				actor.PlayReaction(ReactionTypes.Cry, shower as GameObject, ReactionSpeed.AfterInteraction);
			}
			else if (shower.ShouldGetColdShower)
			{
				actor.BuffManager.AddElement(BuffNames.ColdShower, Origin.FromCheapShower);
				EventTracker.SendEvent(EventTypeId.kGotColdShowerBuff, actor, shower);
			}
		}

		public override bool Run()
		{
			if (!Target.SimLine.WaitForTurn(this, SimQueue.WaitBehavior.DefaultEvict, ExitReason.Default, Shower.kTimeToWaitToEvict))
			{
				return false;
			}
			_mSwitchOutfitHelper = new Sim.SwitchOutfitHelper(Actor, Sim.ClothesChangeReason.GoingToBathe);
			_mSwitchOutfitHelper.Start();
			if (Actor.HasTrait(TraitNames.Hydrophobic))
			{
				Actor.PlayReaction(ReactionTypes.WhyMe, Target, ReactionSpeed.ImmediateWithoutOverlay);
			}
			if (Actor.HasTrait(TraitNames.Daredevil))
			{
				TraitTipsManager.ShowTraitTip(13271263770231521888uL, Actor, TraitTipsManager.TraitTipCounterIndex.Daredevil, TraitTipsManager.kDaredevilCountOfShowersTaken);
			}
			if (!Actor.RouteToSlotAndCheckInUse(Target, Slot.RoutingSlot_0))
			{
				return false;
			}
			StandardEntry();
			if (!Actor.RouteToSlot(Target, Slot.RoutingSlot_0))
			{
				StandardExit();
				return false;
			}
			if (Autonomous)
			{
				mPriority = new InteractionPriority(InteractionPriorityLevel.UserDirected);
			}
			_mSwitchOutfitHelper.Wait(true);
			bool daredevilPerforming = Actor.DaredevilPerforming;
			bool flag = Actor.GetCurrentOutfitCategoryFromOutfitInGameObject() == OutfitCategories.Singed;
			EnterStateMachine("Shower", "Enter", "x");
			SetActor("Shower", Target);
			SetParameter("IsShowerTub", Target.IsShowerTub);
			SetParameter("SimShouldCloseDoor", true);
			SetParameter("SimShouldClothesChange", !daredevilPerforming && !flag && !Actor.OccultManager.DisallowClothesChange() && _mSwitchOutfitHelper.WillChange);
			SetParameter("isBoobyTrapped", false);
			_mSwitchOutfitHelper.AddScriptEventHandler(this);
			AddOneShotScriptEventHandler(1001u, EventCallbackStartShoweringSound);
			if (Actor.HasTrait(TraitNames.Virtuoso) || RandomUtil.RandomChance(Target.TuningShower.ChanceOfSinging))
			{
				AddOneShotScriptEventHandler(200u, EventCallbackStartSinging);
			}
			PetStartleBehavior.CheckForStartle(Target, StartleType.ShowerOn);
			AnimateSim("Loop Shower");
			Actor.BuffManager.AddElement(BuffNames.SavingWater, Origin.FromShower, ProductVersion.EP2, TraitNames.EnvironmentallyConscious);
			StartStages();
			BeginCommodityUpdate(
				new CommodityChange(CommodityKind.Hygiene, 1000f, false, 1000f, OutputUpdateType.First, false, true,
					UpdateAboveAndBelowZeroType.Either),
				Actor.HasTrait(TraitNames.EnvironmentallyConscious)
					? ShowerPublic_Dance.kEnvironmentallyConsciousShowerSpeedMultiplier
					: 1f);
			bool flag2;
			try
			{
				Target.SimInShower = Actor;
				flag2 = DoLoop(ExitReason.Default, DuringShower, null);
			}
			finally
			{
				Target.SimInShower = null;
			}
			EndCommodityUpdates(flag2);
			WaitToLeaveShower(Actor, Target);
			if (flag2)
			{
				ApplyPostShowerEffects(Actor, Target);
			}
			if (flag && flag2)
			{
				_mSwitchOutfitHelper.Dispose();
				_mSwitchOutfitHelper = new Sim.SwitchOutfitHelper(Actor, Sim.ClothesChangeReason.GoingToSwim);
				_mSwitchOutfitHelper.Start();
				_mSwitchOutfitHelper.Wait(false);
				_mSwitchOutfitHelper.ChangeOutfit();
			}
			bool flag3 = false;
			if ((flag && flag2) || (!flag && !daredevilPerforming))
			{
				SetParameter("SimShouldClothesChange", !Actor.OccultManager.DisallowClothesChange());
				_mSwitchOutfitHelper.Dispose();
				_mSwitchOutfitHelper = new Sim.SwitchOutfitHelper(Actor, Sim.ClothesChangeReason.GettingOutOfBath);
				_mSwitchOutfitHelper.Start();
				_mSwitchOutfitHelper.AddScriptEventHandler(this);
				_mSwitchOutfitHelper.Wait(false);
				flag3 = true;
			}
			AddOneShotScriptEventHandler(201u, EventCallbackStopSinging);
			AddOneShotScriptEventHandler(1002u, EventCallbackStopShoweringSound);
			if (flag3 && InventingSkill.IsBeingDetonated(Target))
			{
				SetParameter("SimShouldClothesChange", false);
				_mSwitchOutfitHelper.Abort();
				_mSwitchOutfitHelper.Dispose();
			}
			AnimateSim("Exit Working");
			if (flag2)
			{
				Actor.BuffManager.RemoveElement(BuffNames.GotFleasHuman);
			}
			StandardExit();
			return flag2;
		}

		private void DuringShower(StateMachineClient smc, LoopData loopData)
		{
			if (Actor.SimDescription.IsFrankenstein)
			{
				OccultFrankenstein.PushFrankensteinShortOut(Actor);
			}
			if (Actor.Motives.IsMax(CommodityKind.Hygiene))
			{
				Actor.AddExitReason(ExitReason.Finished);
			}
			Actor.TrySinging();
			EventTracker.SendEvent(EventTypeId.kEventTakeBath, Actor, Target);
			EventTracker.SendEvent(EventTypeId.kEventTakeShower, Actor, Target);
		}

		public override void AddExcludedDreams(ICollection<DreamNames> excludedDreams)
		{
			base.AddExcludedDreams(excludedDreams);
			AddExcludedDream(DreamNames.bathe);
		}

		public override void Cleanup()
		{
			GameObject target = Target;
			if (target != null)
			{
				target.DisableFootprint(824351308u);
			}
			StopShoweringSound();
			StopSingInShowerSound();
			if (_mSwitchOutfitHelper != null)
			{
				_mSwitchOutfitHelper.Dispose();
				_mSwitchOutfitHelper = null;
			}
			base.Cleanup();
		}
	}
}
