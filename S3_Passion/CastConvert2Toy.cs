using Sims3.Gameplay;
using Sims3.Gameplay.Abstracts;
using Sims3.Gameplay.Actors;
using Sims3.Gameplay.ActorSystems;
using Sims3.Gameplay.Autonomy;
using Sims3.Gameplay.Core;
using Sims3.Gameplay.EventSystem;
using Sims3.Gameplay.Interactions;
using Sims3.Gameplay.Interfaces;
using Sims3.Gameplay.ObjectComponents;
using Sims3.Gameplay.Objects.Alchemy;
using Sims3.Gameplay.Objects.Fishing;
using Sims3.Gameplay.Objects.FoodObjects;
using Sims3.Gameplay.Objects.Gardening;
using Sims3.Gameplay.Objects.Spawners;
using Sims3.Gameplay.Skills;
using Sims3.Gameplay.Utilities;
using Sims3.SimIFace;

namespace S3_Passion
{
	public class CastConvert2Toy : Interaction<Sim, GameObject>
	{
		public class Definition : InteractionDefinition<Sim, GameObject, CastConvert2Toy>, IOverridesVisualType, IHasTraitIcon, IHasMenuPathIcon
		{
			public InteractionVisualTypes GetVisualType
			{
				get
				{
					return InteractionVisualTypes.Trait;
				}
			}

			public override string[] GetPath(bool isFemale)
			{
				return new string[1] { MagicWand.LocalizeString(isFemale, "CastSpell", new object[0]) + Localization.Ellipsis };
			}

			protected override string GetInteractionName(Sim actor, GameObject target, InteractionObjectPair interaction)
			{
				return Localization.LocalizeString("Toy Spell");
			}

			protected override bool Test(Sim actor, GameObject target, bool isAutonomous, ref GreyedOutTooltipCallback greyedOutTooltipCallback)
			{
				return actor.HasTrait(TraitNames.WitchHiddenTrait) && MagicWand.HasWand(actor) && actor.SkillManager.GetSkillLevel(SkillNames.Spellcasting) >= MagicWand.kSpellLevels[10] && target.GetContainedObject(Slot.ContainmentSlot_0) == null;
			}

			public ResourceKey GetTraitIcon(Sim actor, GameObject target)
			{
				return ResourceKey.CreatePNGKey("trait_SpellcastingTalent_s", ResourceUtils.ProductVersionToGroupId(ProductVersion.EP7));
			}

			public ResourceKey GetPathIcon(Sim actor, GameObject target)
			{
				return ResourceKey.CreatePNGKey("trait_SpellcastingTalent_s", ResourceUtils.ProductVersionToGroupId(ProductVersion.EP7));
			}
		}

		public const string kFishBowlMedatorName = "FishBowl";

		public MagicWand mWand;

		public VisualEffect mSpellEffect;

		public VisualEffect mPoofEffect;

		public static float kMotiveDrain = 5f;

		public static float kRoutingDistance = 1f;

		public static float kExtraChanceOfEP7Item = 20f;

		public static int[] kMaxConversionValue = new int[11]
		{
			1000, 2000, 3000, 4000, 5000, 6000, 7000, 8000, 9000, 10000,
			2147483647
		};

		public static InteractionDefinition Singleton = new Definition();

		public override ThumbnailKey GetIconKey()
		{
			return new ThumbnailKey(ResourceKey.CreatePNGKey("w_cast_spell", ResourceUtils.ProductVersionToGroupId(ProductVersion.EP7)), ThumbnailSize.Large);
		}

		protected override bool RunFromInventory()
		{
			return Run();
		}

		protected override bool Run()
		{
			bool flag = true;
			Actor.SkillManager.AddElement(SkillNames.Spellcasting);
			SpellcastingSkill spellcastingSkill = Actor.SkillManager.GetElement(SkillNames.Spellcasting) as SpellcastingSkill;
			mWand = MagicWand.GetWandToUse(Actor, spellcastingSkill);
			if (mWand == null)
			{
				return false;
			}
			mWand.PrepareForUse(Actor);
			if (Target.InInventory)
			{
				if (Target is Fish)
				{
					Fish fish = Target as Fish;
					Target = MagicWand.CastConvert.CreateFishBowlAndAddFish(fish.Type);
					if (Target == null)
					{
						return false;
					}
					Actor.Inventory.TryToRemove(fish);
				}
				InteractionInstance interactionInstance = PutDown.DisallowCarrySingleton.CreateInstance(Target, Actor, GetPriority(), base.Autonomous, base.CancellableByPlayer);
				flag = interactionInstance.RunInteraction();
			}
			else if (!Actor.RouteToPointRadius(Target.Position, MagicWand.CastConvert.kRoutingDistance))
			{
				return false;
			}
			if (!flag || Actor.HasExitReason())
			{
				return false;
			}
			StandardEntry();
			BeginCommodityUpdates();
			EnterStateMachine("FireIceBlast", "Enter", "x");
			if (mWand is MagicHands)
			{
				SetParameter("noWand", true);
			}
			else
			{
				SetParameter("noWand", false);
			}
			SetActor("wand", mWand);
			SetParameter("isSkilled", spellcastingSkill.SkillLevel >= MagicWand.kExpertLevel);
			AddOneShotScriptEventHandler(101u, ShowSuccessVfx);
			AddOneShotScriptEventHandler(102u, ShowFailVfx);
			AddOneShotScriptEventHandler(103u, ShowEpicFailVfx);
			DrainMotives();
			bool flag2 = false;
			IGameObject gameObject = TransformedObject();
			if ((gameObject is Fish && !(Target.Parent is ISurface)) || gameObject is FailureObject)
			{
				gameObject.Destroy();
				gameObject = null;
				flag2 = true;
			}
			if (gameObject != null)
			{
				int num = MagicWand.CastConvert.kMaxConversionValue[1];
				int skillLevel = Actor.SkillManager.GetSkillLevel(SkillNames.Spellcasting);
				if (skillLevel >= MagicWand.kSpellLevels[11] && skillLevel <= 10)
				{
					num = MagicWand.CastConvert.kMaxConversionValue[skillLevel];
				}
				if (gameObject.Value > num)
				{
					flag2 = true;
				}
			}
			if (!flag2 && RandomUtil.RandomChance(mWand.SuccessChance(Actor, MagicWand.SpellType.Convert, spellcastingSkill.SkillLevel)))
			{
				flag = true;
				AnimateSim("SuccessIdle");
				AnimateSim("Success");
				OnSpellSuccess(gameObject);
			}
			else if (RandomUtil.RandomChance(mWand.EpicFailChance(Actor)))
			{
				flag = false;
				AnimateSim("EpicFail");
			}
			else
			{
				flag = false;
				AnimateSim("Fail");
			}
			if (!flag && gameObject != null)
			{
				gameObject.Destroy();
			}
			EventTracker.SendEvent(EventTypeId.kCastSpell, Actor);
			EndCommodityUpdates(flag);
			StandardExit();
			return flag;
		}

		public void DrainMotives()
		{
			mWand.DrainMotive(Actor, CommodityKind.MagicFatigue, 0f - MagicWand.CastConvert.kMotiveDrain);
		}

		public void OnSpellSuccess(IGameObject conjuredObject)
		{
			Vector3 position = Target.Position;
			Vector3 forwardVector = Target.ForwardVector;
			IGameObject parent = Target.Parent;
			SurfaceSlot surfaceSlot = null;
			ISurface surface = parent as ISurface;
			if (surface != null)
			{
				surfaceSlot = surface.Surface.GetSurfaceSlotFromContainedObject(Target);
			}
			mPoofEffect = VisualEffect.Create("ep7WandRestorationLot_main");
			mPoofEffect.SetPosAndOrient(position, forwardVector, Vector3.UnitY);
			mPoofEffect.Start();
			Fish.IFishBowl fishBowl = Target as Fish.IFishBowl;
			string text = null;
			Ingredient ingredient = Target as Ingredient;
			if (ingredient != null)
			{
				text = ingredient.Key;
			}
			PlantableNonIngredient plantableNonIngredient = Target as PlantableNonIngredient;
			if (plantableNonIngredient != null)
			{
				text = plantableNonIngredient.Key;
			}
			if (text != null)
			{
				string[] kPlantIngredients = MagicWand.kPlantIngredients;
				foreach (string text2 in kPlantIngredients)
				{
					if (text2.Equals(text))
					{
						break;
					}
				}
			}
			Target.FadeOut(false, true);
			while (Target != null && Target.ObjectId != ObjectGuid.InvalidObjectGuid)
			{
				Simulator.Sleep(1u);
			}
			Fish fish = conjuredObject as Fish;
			if (fish != null)
			{
				FishType type = fish.Type;
				conjuredObject.Destroy();
				conjuredObject = MagicWand.CastConvert.CreateFishBowlAndAddFish(type);
			}
			if (conjuredObject != null)
			{
				conjuredObject.SetPosition(position);
				conjuredObject.SetForward(forwardVector);
				World.AddObjectToScene(conjuredObject.ObjectId);
				conjuredObject.AddToWorld();
				if (surface != null && surfaceSlot != null)
				{
					surface.Surface.TriggerSlotUsed(surfaceSlot, conjuredObject, SurfaceAddOn.Action.ItemPlaced);
					conjuredObject.ParentToSlot(parent, surfaceSlot.ContainmentSlot);
				}
				conjuredObject.FadeIn();
			}
		}

		public IGameObject TransformedObject()
		{
			IGameObject result = null;
			switch (RandomUtil.GetInt(4))
			{
			case 0:
				result = Metal.MakeSmeltedMetal(RockGemMetal.Gold, true);
				break;
			case 1:
				result = Metal.MakeSmeltedMetal(RockGemMetal.Gold, false);
				break;
			case 2:
				result = Metal.MakeSmeltedMetal(RockGemMetal.Gold, true);
				break;
			case 3:
				result = Metal.MakeSmeltedMetal(RockGemMetal.Gold, false);
				break;
			case 4:
				result = Metal.MakeSmeltedMetal(RockGemMetal.Gold, true);
				break;
			}
			return result;
		}

		public void OnSpellEpicFailure()
		{
			mPoofEffect = VisualEffect.Create("ep7WandRestorationLotNeg_main");
			mPoofEffect.SetPosAndOrient(Target.Position, Target.ForwardVector, Vector3.UnitY);
			mPoofEffect.Start();
			Target.FadeOut(true, true);
		}

		public void ShowSuccessVfx(StateMachineClient sender, IEvent evt)
		{
			mSpellEffect = VisualEffect.Create("ep7WandRestoration_main");
			mSpellEffect.ParentTo(mWand, Slot.FXJoint_0);
			mSpellEffect.Start();
		}

		public void ShowFailVfx(StateMachineClient sender, IEvent evt)
		{
			mSpellEffect = VisualEffect.Create("ep7WandFail_main");
			mSpellEffect.ParentTo(mWand, Slot.FXJoint_0);
			mSpellEffect.Start();
		}

		public void ShowEpicFailVfx(StateMachineClient sender, IEvent evt)
		{
			mSpellEffect = VisualEffect.Create("ep7WandRestorationCriticalFail_main");
			mSpellEffect.ParentTo(mWand, Slot.FXJoint_0);
			mSpellEffect.Start();
			OnSpellEpicFailure();
		}

		public override void Cleanup()
		{
			if (mWand != null)
			{
				mWand.FinishUsing(Actor);
			}
			if (mSpellEffect != null)
			{
				mSpellEffect.Stop();
				mSpellEffect.Dispose();
				mSpellEffect = null;
			}
			if (mPoofEffect != null)
			{
				mPoofEffect.Stop();
				mPoofEffect.Dispose();
				mPoofEffect = null;
			}
			base.Cleanup();
		}

		public static GameObject CreateFishBowlAndAddFish(FishType fishtype)
		{
			IGameObject gameObject = null;
			FishInitParameters fishInitParameters = new FishInitParameters(fishtype);
			if (fishInitParameters != null)
			{
				fishInitParameters.IsBornInTank = true;
				gameObject = GlobalFunctions.CreateObjectOutOfWorld("FishBowl", null, fishInitParameters);
			}
			if (gameObject is FailureObject)
			{
				gameObject.Destroy();
				return null;
			}
			return gameObject as GameObject;
		}
	}
}
