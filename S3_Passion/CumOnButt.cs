using System;
using Sims3.Gameplay.Actors;
using Sims3.Gameplay.Autonomy;
using Sims3.Gameplay.CAS;
using Sims3.Gameplay.Core;
using Sims3.Gameplay.Interactions;
using Sims3.Gameplay.Utilities;
using Sims3.SimIFace;
using Sims3.SimIFace.CAS;
using Sims3.UI;

namespace Passion.S3_Passion
{
	internal sealed class CumOnButt : ImmediateInteraction<Sim, Sim>
	{
		[DoesntRequireTuning]
		private sealed class Definition : ImmediateInteractionDefinition<Sim, Sim, CumOnButt>
		{
			public override string GetInteractionName(Sim actor, Sim target, InteractionObjectPair interaction)
			{
				return Localization.LocalizeString("CumOnButt");
			}

			public override string[] GetPath(bool isFemale)
			{
				return new string[] { Localization.LocalizeString("Ejaculate") };
			}

			public override bool Test(Sim actor, Sim target, bool isAutonomous, ref GreyedOutTooltipCallback greyedOutTooltipCallback)
			{
				return Passion.CumInteractions;
			}
		}

		public static readonly InteractionDefinition Singleton = new Definition();

		private Sim.SwitchOutfitHelper _mSwitchOutfitHelper;

		private Sim.SwitchOutfitHelper SwitchOutfitHelper
		{
			get
			{
				return _mSwitchOutfitHelper;
			}
			set
			{
				if (_mSwitchOutfitHelper != value)
				{
					if (_mSwitchOutfitHelper != null)
					{
						_mSwitchOutfitHelper.Dispose();
					}
					_mSwitchOutfitHelper = value;
				}
			}
		}

		public override bool Run()
		{
			if ((Actor.IsMale && Actor == Target) || (IsTransfem(Actor) && IsTransfem(Target)))
			{
				SimMessage(Localization.LocalizeString("You want me ejaculate on my Butt now? Show me how you do it?"), Actor);
			}
			if (Actor.IsFemale && Target.IsFemale && !IsTransfem(Actor) && !IsTransfem(Target))
			{
				SimMessage(Localization.LocalizeString("I don't have the ability to do that..."), Actor);
			}
			if ((Actor.IsMale && Actor != Target && Passion.Settings.UseCondom && Passion.Settings.CondomIsBroken) || (Actor.IsMale && Actor != Target && !Passion.Settings.UseCondom) || (IsTransfem(Actor) && !IsTransfem(Target) && Passion.Settings.UseCondom && Passion.Settings.CondomIsBroken) || (IsTransfem(Actor) && !IsTransfem(Target) && !Passion.Settings.UseCondom))
			{
				ResourceKey kInvalidResourceKey = ResourceKey.kInvalidResourceKey;
				ResourceKey resourceKey = ResourceKey.FromString("0x034AEECB-0x00000000-0x0970A7D67C90A462");
				SimDescription simDescription = Actor.SimDescription;
				if (resourceKey == kInvalidResourceKey) return false;
				if (!IsSmPeener(Actor))
				{
					try
					{
						SimOutfit simOutfit = new SimOutfit(ResourceKey.FromString("0x025ED6F4-0x00000000-0x5DA2AF30FC4A0425"));
						SimOutfit resultOutfit;
						if (OutfitUtils.TryApplyUniformToOutfit(simDescription.GetOutfit(OutfitCategories.Naked, 0), simOutfit, simDescription, "CumShot", out resultOutfit))
						{
							simDescription.AddOutfit(resultOutfit, OutfitCategories.Naked, true);
						}
						SwitchOutfitHelper = new Sim.SwitchOutfitHelper(Actor, OutfitCategories.Naked, 0);
						SwitchOutfitHelper.Start();
						SwitchOutfitHelper.Wait(false);
						try
						{
							Actor.SwitchToOutfitWithoutSpin(OutfitCategories.Naked, resultOutfit, 0);
						}
						catch
						{
							// ignored
						}

						SwitchOutfitHelper = new Sim.SwitchOutfitHelper(Actor, OutfitCategories.Naked, 1);
						SwitchOutfitHelper.Start();
						SwitchOutfitHelper.Wait(false);
						Wait(3u);
						try
						{
							Actor.SwitchToOutfitWithoutSpin(OutfitCategories.Naked, 1);
						}
						catch
						{
							// ignored
						}

						SwitchOutfitHelper = new Sim.SwitchOutfitHelper(Actor, OutfitCategories.Naked, 0);
						SwitchOutfitHelper.Start();
						SwitchOutfitHelper.Wait(false);
						Wait(13u);
						try
						{
							Actor.SwitchToOutfitWithoutSpin(OutfitCategories.Naked, resultOutfit, 0);
						}
						catch
						{
							// ignored
						}

						SwitchOutfitHelper = new Sim.SwitchOutfitHelper(Actor, OutfitCategories.Naked, 1);
						SwitchOutfitHelper.Start();
						SwitchOutfitHelper.Wait(false);
						Wait(3u);
						try
						{
							Actor.SwitchToOutfitWithoutSpin(OutfitCategories.Naked, 1);
						}
						catch
						{
							// ignored
						}

						SwitchOutfitHelper = new Sim.SwitchOutfitHelper(Actor, OutfitCategories.Naked, 0);
						SwitchOutfitHelper.Start();
						SwitchOutfitHelper.Wait(false);
						Wait(5u);
						try
						{
							Actor.SwitchToOutfitWithoutSpin(OutfitCategories.Naked, resultOutfit, 0);
						}
						catch
						{
							// ignored
						}

						SwitchOutfitHelper = new Sim.SwitchOutfitHelper(Actor, OutfitCategories.Naked, 1);
						SwitchOutfitHelper.Start();
						SwitchOutfitHelper.Wait(false);
						Wait(3u);
						try
						{
							Actor.SwitchToOutfitWithoutSpin(OutfitCategories.Naked, 1);
						}
						catch
						{
							// ignored
						}

						SwitchOutfitHelper = new Sim.SwitchOutfitHelper(Actor, OutfitCategories.Naked, 0);
						SwitchOutfitHelper.Start();
						SwitchOutfitHelper.Wait(false);
						Wait(5u);
						try
						{
							Actor.SwitchToOutfitWithoutSpin(OutfitCategories.Naked, resultOutfit, 0);
						}
						catch
						{
							// ignored
						}

						Wait(2u);
						if (simDescription.GetOutfitCount(OutfitCategories.Naked) != 1)
						{
							while (simDescription.GetOutfitCount(OutfitCategories.Naked) > 1)
							{
								simDescription.RemoveOutfit(OutfitCategories.Naked, 0, true);
							}
							try
							{
								Actor.SwitchToOutfitWithoutSpin(OutfitCategories.Naked, 0);
							}
							catch
							{
								// ignored
							}
						}
					}
					catch
					{
						// ignored
					}
				}
				ResourceKey resourceKey2 = ResourceKey.FromString("0x034AEECB-0x00000000-0x25D284A71C97A1E3");
				SimDescription simDescription2 = Target.SimDescription;
				SimOutfit simOutfit2 = null;
				if (resourceKey2 != kInvalidResourceKey)
				{
					simOutfit2 = new SimOutfit(ResourceKey.FromString("0x025ED6F4-0x00000000-0x827C13C3F52C09B3"));
				}
				if (simOutfit2 != null && Target.CurrentOutfitCategory == OutfitCategories.Naked)
				{
					SimOutfit resultOutfit2;
					if (OutfitUtils.TryApplyUniformToOutfit(simDescription2.GetOutfit(OutfitCategories.Naked, 0), simOutfit2, simDescription2, "CumMask", out resultOutfit2))
					{
						simDescription2.AddOutfit(resultOutfit2, OutfitCategories.Naked, true);
					}
					try
					{
						Target.SwitchToOutfitWithoutSpin(OutfitCategories.Naked, resultOutfit2, 0);
					}
					catch
					{
						// ignored
					}

					if (simDescription2.GetOutfitCount(OutfitCategories.Naked) <= 2) return true;
					while (simDescription2.GetOutfitCount(OutfitCategories.Naked) > 2)
					{
						simDescription2.RemoveOutfit(OutfitCategories.Naked, 1, true);
					}
				}
				return true;
			}

			if ((!Actor.IsFemale || IsTransfem(Actor) || !Target.IsMale || !Passion.Settings.UseCondom ||
			     !Passion.Settings.CondomIsBroken) &&
			    (!Actor.IsFemale || IsTransfem(Actor) || !Target.IsMale || Passion.Settings.UseCondom) &&
			    (!Actor.IsFemale || IsTransfem(Actor) || !IsTransfem(Target) || !Passion.Settings.UseCondom ||
			     !Passion.Settings.CondomIsBroken) &&
			    (!Actor.IsFemale || IsTransfem(Actor) || !IsTransfem(Target) || Passion.Settings.UseCondom))
				return false;
			ResourceKey kInvalidResourceKey2 = ResourceKey.kInvalidResourceKey;
			ResourceKey resourceKey3 = ResourceKey.FromString("0x034AEECB-0x00000000-0x0970A7D67C90A462");
			SimDescription simDescription3 = Target.SimDescription;
			if (resourceKey3 == kInvalidResourceKey2) return false;
			if (!IsSmPeener(Target))
			{
				try
				{
					SimOutfit simOutfit3 = new SimOutfit(ResourceKey.FromString("0x025ED6F4-0x00000000-0x5DA2AF30FC4A0425"));
					SimOutfit resultOutfit3;
					if (OutfitUtils.TryApplyUniformToOutfit(simDescription3.GetOutfit(OutfitCategories.Naked, 0), simOutfit3, simDescription3, "CumShot", out resultOutfit3))
					{
						simDescription3.AddOutfit(resultOutfit3, OutfitCategories.Naked, true);
					}
					SwitchOutfitHelper = new Sim.SwitchOutfitHelper(Target, OutfitCategories.Naked, 0);
					SwitchOutfitHelper.Start();
					SwitchOutfitHelper.Wait(false);
					try
					{
						Target.SwitchToOutfitWithoutSpin(OutfitCategories.Naked, resultOutfit3, 0);
					}
					catch
					{
						// ignored
					}

					SwitchOutfitHelper = new Sim.SwitchOutfitHelper(Target, OutfitCategories.Naked, 1);
					SwitchOutfitHelper.Start();
					SwitchOutfitHelper.Wait(false);
					Wait(3u);
					try
					{
						Target.SwitchToOutfitWithoutSpin(OutfitCategories.Naked, 1);
					}
					catch
					{
						// ignored
					}

					SwitchOutfitHelper = new Sim.SwitchOutfitHelper(Target, OutfitCategories.Naked, 0);
					SwitchOutfitHelper.Start();
					SwitchOutfitHelper.Wait(false);
					Wait(13u);
					try
					{
						Target.SwitchToOutfitWithoutSpin(OutfitCategories.Naked, resultOutfit3, 0);
					}
					catch
					{
						// ignored
					}

					SwitchOutfitHelper = new Sim.SwitchOutfitHelper(Target, OutfitCategories.Naked, 1);
					SwitchOutfitHelper.Start();
					SwitchOutfitHelper.Wait(false);
					Wait(3u);
					try
					{
						Target.SwitchToOutfitWithoutSpin(OutfitCategories.Naked, 1);
					}
					catch
					{
						// ignored
					}

					SwitchOutfitHelper = new Sim.SwitchOutfitHelper(Target, OutfitCategories.Naked, 0);
					SwitchOutfitHelper.Start();
					SwitchOutfitHelper.Wait(false);
					Wait(5u);
					try
					{
						Target.SwitchToOutfitWithoutSpin(OutfitCategories.Naked, resultOutfit3, 0);
					}
					catch
					{
						// ignored
					}

					SwitchOutfitHelper = new Sim.SwitchOutfitHelper(Target, OutfitCategories.Naked, 1);
					SwitchOutfitHelper.Start();
					SwitchOutfitHelper.Wait(false);
					Wait(3u);
					try
					{
						Target.SwitchToOutfitWithoutSpin(OutfitCategories.Naked, 1);
					}
					catch
					{
						// ignored
					}

					SwitchOutfitHelper = new Sim.SwitchOutfitHelper(Target, OutfitCategories.Naked, 0);
					SwitchOutfitHelper.Start();
					SwitchOutfitHelper.Wait(false);
					Wait(5u);
					try
					{
						Target.SwitchToOutfitWithoutSpin(OutfitCategories.Naked, resultOutfit3, 0);
					}
					catch
					{
						// ignored
					}

					Wait(2u);
					if (simDescription3.GetOutfitCount(OutfitCategories.Naked) != 1)
					{
						while (simDescription3.GetOutfitCount(OutfitCategories.Naked) > 1)
						{
							simDescription3.RemoveOutfit(OutfitCategories.Naked, 0, true);
						}
						try
						{
							Target.SwitchToOutfitWithoutSpin(OutfitCategories.Naked, 0);
						}
						catch
						{
							// ignored
						}
					}
				}
				catch
				{
					// ignored
				}
			}
			ResourceKey resourceKey4 = ResourceKey.FromString("0x034AEECB-0x00000000-0x25D284A71C97A1E3");
			SimDescription simDescription4 = Actor.SimDescription;
			SimOutfit simOutfit4 = null;
			if (resourceKey4 != kInvalidResourceKey2)
			{
				simOutfit4 = new SimOutfit(ResourceKey.FromString("0x025ED6F4-0x00000000-0x827C13C3F52C09B3"));
			}

			if (simOutfit4 == null || Actor.CurrentOutfitCategory != OutfitCategories.Naked) return true;
			SimOutfit resultOutfit4;
			if (OutfitUtils.TryApplyUniformToOutfit(simDescription4.GetOutfit(OutfitCategories.Naked, 0), simOutfit4, simDescription4, "CumMask", out resultOutfit4))
			{
				simDescription4.AddOutfit(resultOutfit4, OutfitCategories.Naked, true);
			}
			SwitchOutfitHelper = new Sim.SwitchOutfitHelper(Actor, OutfitCategories.Naked, 0);
			SwitchOutfitHelper.Start();
			SwitchOutfitHelper.Wait(false);
			try
			{
				SwitchOutfitHelper.ChangeOutfit();
			}
			catch
			{
				// ignored
			}

			if (simDescription4.GetOutfitCount(OutfitCategories.Naked) <= 2) return true;
			while (simDescription4.GetOutfitCount(OutfitCategories.Naked) > 2)
			{
				simDescription4.RemoveOutfit(OutfitCategories.Naked, 1, true);
			}
			return true;
		}

		private static void Wait()
		{
			Wait(1u);
		}

		public static void Wait(int least, int greatest)
		{
			Wait(RandomUtil.GetInt(least, greatest));
		}

		private static void Wait(int pause)
		{
			try
			{
				Wait(Convert.ToUInt32(pause));
			}
			catch
			{
				Wait();
			}
		}

		private static void Wait(uint pause)
		{
			try
			{
				Simulator.Sleep(pause);
			}
			catch
			{
				// ignored
			}
		}

		private static void Message(string message, StyledNotification.NotificationStyle style, Sim speaker, Sim target)
		{
			if (speaker != null)
			{
				StyledNotification.Show(target != null
					? new StyledNotification.Format(message, speaker.ObjectId, target.ObjectId, style)
					: new StyledNotification.Format(message, speaker.ObjectId, style));
			}
			else
			{
				StyledNotification.Show(new StyledNotification.Format(message, style));
			}
		}

		private static void SimMessage(string message, Sim speaker)
		{
			Message(message, StyledNotification.NotificationStyle.kSimTalking, speaker, null);
		}

		private static bool IsTransfem(Sim sim)
		{
			SimDescription simDescription = sim.SimDescription;
			if (simDescription.GetOutfit(OutfitCategories.Naked, 0).GetPartPreset(ResourceKey.FromString("0x034AEECB-0x00000000-0xB25D1F4F442041E6")) != null)
			{
				return true;
			}
			if (simDescription.GetOutfit(OutfitCategories.Naked, 0).GetPartPreset(ResourceKey.FromString("0x034AEECB-0x00000000-0x23697088F9BC3EA8")) != null)
			{
				return true;
			}
			return simDescription.GetOutfit(OutfitCategories.Naked, 0).GetPartPreset(ResourceKey.FromString("0x034AEECB-0x00000000-0x49CBFB1B775EC86E")) != null;
		}

		private static bool IsSmPeener(Sim sim)
		{
			SimDescription simDescription = sim.SimDescription;
			return simDescription.GetOutfit(OutfitCategories.Naked, 0).GetPartPreset(ResourceKey.FromString("0x034AEECB-0x00000000-0x0603B3F0BE3C7883")) != null;
		}
	}
}
