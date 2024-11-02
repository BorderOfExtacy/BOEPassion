using System.Collections.Generic;
using Sims3.Gameplay;
using Sims3.Gameplay.Abstracts;
using Sims3.Gameplay.Actors;
using Sims3.Gameplay.ActorSystems;
using Sims3.Gameplay.Autonomy;
using Sims3.Gameplay.CAS;
using Sims3.Gameplay.Core;
using Sims3.Gameplay.Interactions;
using Sims3.Gameplay.Utilities;
using Sims3.SimIFace;
using Sims3.UI;

namespace S3_Passion
{
	public class STD : CustomBuff
	{
		public class Infection
		{
			public SimDescription CarrierDescription;

			public BuffNames Disease;

			public long Onset;

			public AlarmHandle IncubationAlarm;

			public bool Reinfection;

			public bool IsValid;

			public Sim Carrier
			{
				get
				{
					if (CarrierDescription != null)
					{
						return CarrierDescription.CreatedSim;
					}
					return null;
				}
				set
				{
					if (value != null)
					{
						CarrierDescription = value.SimDescription;
					}
					else
					{
						CarrierDescription = null;
					}
				}
			}

			public Infection(Sim carrier, BuffNames disease, long onset)
			{
				Initialize(carrier, disease, onset, true);
			}

			public Infection(Sim carrier, BuffNames disease, long onset, bool reinfection)
			{
				Initialize(carrier, disease, onset, reinfection);
			}

			private void Initialize(Sim carrier, BuffNames disease, long onset, bool reinfection)
			{
				Carrier = carrier;
				Disease = disease;
				Onset = onset;
				Reinfection = reinfection;
				IsValid = true;
				IncubationAlarm = Carrier.AddAlarmRepeating(5f, TimeUnit.Minutes, Callback, 7f, TimeUnit.Hours, "Periodic STD Check", AlarmType.AlwaysPersisted);
			}

			public void Invalidate()
			{
				IsValid = false;
				if (Carrier != null)
				{
					Carrier.RemoveAlarm(IncubationAlarm);
				}
			}

			public void Callback()
			{
				if (SimClock.CurrentTicks >= Onset)
				{
					if (Reinfection)
					{
						Carrier.BuffManager.AddElement(Disease, Origin.None);
					}
					Invalidate();
					BuffNames disease = Disease;
					if (disease == (BuffNames)3348796355468607868uL || disease == (BuffNames)3802937897211410794uL || disease == (BuffNames)16680654480998469135uL)
					{
						Dethkloks.Add(new Dethklok(Carrier, Disease));
					}
				}
			}
		}

		public class Dethklok
		{
			public SimDescription CarrierDescription;

			public BuffNames Disease;

			public long NextCheck;

			public AlarmHandle DethklokAlarm;

			public bool IsValid;

			public Sim Carrier
			{
				get
				{
					if (CarrierDescription != null)
					{
						return CarrierDescription.CreatedSim;
					}
					return null;
				}
				set
				{
					if (value != null)
					{
						CarrierDescription = value.SimDescription;
					}
					else
					{
						CarrierDescription = null;
					}
				}
			}

			public Dethklok(Sim carrier, BuffNames disease)
			{
				Carrier = carrier;
				Disease = disease;
				SetNextCheck();
				IsValid = true;
				DethklokAlarm = Carrier.AddAlarmRepeating(1f, TimeUnit.Days, Callback, 5f, TimeUnit.Hours, "Dethklok Alarm", AlarmType.AlwaysPersisted);
			}

			public void Invalidate()
			{
				IsValid = false;
				if (Carrier != null)
				{
					Carrier.RemoveAlarm(DethklokAlarm);
				}
			}

			public void SetNextCheck()
			{
				switch (Disease)
				{
				case (BuffNames)3802937897211410794uL:
					NextCheck = SimClock.CurrentTicks + (long)((float)CustomBuff.DayTicks * RandomUtil.GetFloat(1f, 20f));
					break;
				case (BuffNames)16680654480998469135uL:
					NextCheck = SimClock.CurrentTicks + (long)((float)CustomBuff.DayTicks * RandomUtil.GetFloat(2f, 7f));
					break;
				case (BuffNames)3348796355468607868uL:
					NextCheck = SimClock.CurrentTicks + (long)((float)CustomBuff.DayTicks * RandomUtil.GetFloat(5f, 16f));
					break;
				}
			}

			public void Callback()
			{
				if (Carrier == null || !Carrier.BuffManager.HasElement(Disease))
				{
					Invalidate();
				}
				if (!IsValid)
				{
					return;
				}
				switch (Disease)
				{
				case (BuffNames)3348796355468607868uL:
					if (!Carrier.TraitManager.HasElement(TraitNames.Insane) && RandomUtil.RandomChance(20f))
					{
						try
						{
							Carrier.TraitManager.RemoveElement(Carrier.TraitManager.GetRandomElement().Guid);
							Carrier.TraitManager.AddElement(TraitNames.Insane);
						}
						catch
						{
							PassionCommon.SystemMessage("STDSimphilisTertiary: Failed to trigger Dementia");
						}
						SetNextCheck();
					}
					else if (RandomUtil.RandomChance(10f))
					{
						Invalidate();
						Carrier.InteractionQueue.CancelAllInteractions();
						Carrier.Kill(SimDescription.DeathType.OldAge);
					}
					else
					{
						SetNextCheck();
					}
					break;
				case (BuffNames)3802937897211410794uL:
					if (RandomUtil.RandomChance(15f))
					{
						Disease = (BuffNames)16680654480998469135uL;
						Carrier.BuffManager.RemoveElement((BuffNames)3802937897211410794uL);
						Carrier.BuffManager.AddElement((BuffNames)16680654480998469135uL, Origin.None);
					}
					SetNextCheck();
					break;
				case (BuffNames)16680654480998469135uL:
					if (RandomUtil.RandomChance(30f))
					{
						Invalidate();
						Carrier.InteractionQueue.CancelAllInteractions();
						Carrier.Kill(SimDescription.DeathType.OldAge);
					}
					else
					{
						SetNextCheck();
					}
					break;
				}
			}
		}

		public class GetTreated : RabbitHole.RabbitHoleInteraction<Sim, RabbitHole>
		{
			protected sealed class Definition : InteractionDefinition<Sim, RabbitHole, GetTreated>
			{
				public override bool Test(Sim sim, RabbitHole target, bool isAutonomous, ref GreyedOutTooltipCallback greyedOutTooltipCallback)
				{
					if (HasTreatableSTDs(sim))
					{
						return true;
					}
					return false;
				}

				public override string GetInteractionName(Sim actor, RabbitHole target, InteractionObjectPair iop)
				{
					return Localization.LocalizeString("STD.GetTreated:InteractionName") + "(" + UIUtils.FormatMoney(kCostOfTreatment) + ")";
				}
			}

			private static int kSimMinutesForTreatment = 120;

			private static int kCostOfTreatment = 500;

			public static readonly InteractionDefinition Singleton = new Definition();

			public override void ConfigureInteraction()
			{
				base.ConfigureInteraction();
				TimedStage timedStage = new TimedStage(GetInteractionName(), kSimMinutesForTreatment, false, true, true);
				base.Stages = new List<Stage>(new Stage[1] { timedStage });
				base.ActiveStage = timedStage;
			}

			public override bool InRabbitHole()
			{
				StartStages();
				BeginCommodityUpdates();
				bool flag = DoLoop(ExitReason.Default);
				if (Actor.HasExitReason(ExitReason.StageComplete))
				{
					if (Actor.FamilyFunds >= kCostOfTreatment)
					{
						Actor.ModifyFunds(-kCostOfTreatment);
					}
					else if (!GameUtils.IsFutureWorld())
					{
						Actor.UnpaidBills += kCostOfTreatment;
					}
					Treatment(Actor);
				}
				else
				{
					flag = false;
				}
				EndCommodityUpdates(flag);
				return flag;
			}
		}

		[PersistableStatic]
		private static List<Infection> mPendingInfections;

		public static List<Infection> PendingInfectionsBackup = new List<Infection>();

		[PersistableStatic]
		private static List<Dethklok> mDethkloks;

		public static List<Dethklok> DethkloksBackup = new List<Dethklok>();

		public static List<Infection> PendingInfections
		{
			get
			{
				if (mPendingInfections == null)
				{
					mPendingInfections = new List<Infection>();
				}
				return mPendingInfections;
			}
			set
			{
				mPendingInfections = value;
			}
		}

		public static List<Dethklok> Dethkloks
		{
			get
			{
				if (mDethkloks == null)
				{
					mDethkloks = new List<Dethklok>();
				}
				return mDethkloks;
			}
			set
			{
				mDethkloks = value;
			}
		}

		public STD(BuffData info)
			: base(info)
		{
		}

		public static void ClearData()
		{
			if (mPendingInfections != null)
			{
				PendingInfectionsBackup = new List<Infection>(mPendingInfections);
				mPendingInfections.Clear();
				mPendingInfections = null;
			}
			if (mDethkloks != null)
			{
				DethkloksBackup = new List<Dethklok>(mDethkloks);
				mDethkloks.Clear();
				mDethkloks = null;
			}
		}

		public static long GetOnset(BuffNames std)
		{
			long num = 0L;
			switch (std)
			{
			case (BuffNames)12465744095023444500uL:
			case (BuffNames)14409719855781998876uL:
			case (BuffNames)16328498947224254690uL:
			case (BuffNames)9183286871710270222uL:
				num = CustomBuff.DayTicks * RandomUtil.GetInt(2, 4);
				break;
			case (BuffNames)4642255024717194076uL:
				num = CustomBuff.DayTicks * RandomUtil.GetInt(20, 30);
				break;
			case (BuffNames)3348796355468607868uL:
				num = CustomBuff.DayTicks * RandomUtil.GetInt(40, 80);
				break;
			case (BuffNames)8648471817152268810uL:
				num = CustomBuff.DayTicks * RandomUtil.GetInt(21, 35);
				break;
			case (BuffNames)3802937897211410794uL:
				num = CustomBuff.DayTicks * RandomUtil.GetInt(20, 60);
				break;
			default:
				num = CustomBuff.DayTicks;
				break;
			}
			return SimClock.CurrentTicks + num;
		}

		public static List<BuffNames> GetSTDs(Sim sim)
		{
			List<BuffNames> list = new List<BuffNames>();
			if (sim != null)
			{
				foreach (BuffNames item in Names.STD)
				{
					if (sim.BuffManager.HasElement(item))
					{
						list.Add(item);
					}
				}
			}
			return list;
		}

		public static bool HasTreatableSTDs(Sim sim)
		{
			using (List<BuffNames>.Enumerator enumerator = GetSTDs(sim).GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					switch (enumerator.Current)
					{
					case (BuffNames)12465744095023444500uL:
					case (BuffNames)14409719855781998876uL:
					case (BuffNames)16328498947224254690uL:
					case (BuffNames)16680654480998469135uL:
					case (BuffNames)3802937897211410794uL:
					case (BuffNames)4642255024717194076uL:
					case (BuffNames)6514717556347855497uL:
					case (BuffNames)9183286871710270222uL:
						return true;
					}
				}
			}
			return false;
		}

		public static bool HasSIV(Sim sim)
		{
			foreach (BuffNames sTD in GetSTDs(sim))
			{
				BuffNames buffNames = sTD;
				if (buffNames != (BuffNames)3802937897211410794uL && buffNames != (BuffNames)6514717556347855497uL && buffNames != (BuffNames)16680654480998469135uL)
				{
					continue;
				}
				return true;
			}
			foreach (Infection pendingInfection in PendingInfections)
			{
				BuffNames disease = pendingInfection.Disease;
				if (disease != (BuffNames)3802937897211410794uL && disease != (BuffNames)6514717556347855497uL && disease != (BuffNames)16680654480998469135uL)
				{
					continue;
				}
				return true;
			}
			return false;
		}

		public static bool HasSimphilis(Sim sim)
		{
			using (List<BuffNames>.Enumerator enumerator = GetSTDs(sim).GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					switch (enumerator.Current)
					{
					case (BuffNames)3348796355468607868uL:
					case (BuffNames)4642255024717194076uL:
					case (BuffNames)7014284627137684376uL:
					case (BuffNames)9183286871710270222uL:
						return true;
					}
				}
			}
			foreach (Infection pendingInfection in PendingInfections)
			{
				switch (pendingInfection.Disease)
				{
				case (BuffNames)3348796355468607868uL:
				case (BuffNames)4642255024717194076uL:
				case (BuffNames)7014284627137684376uL:
				case (BuffNames)9183286871710270222uL:
					return true;
				}
			}
			return false;
		}

		public static bool HasSimerpes(Sim sim)
		{
			foreach (BuffNames sTD in GetSTDs(sim))
			{
				BuffNames buffNames = sTD;
				if (buffNames != (BuffNames)8648471817152268810uL && buffNames != (BuffNames)14409719855781998876uL)
				{
					continue;
				}
				return true;
			}
			foreach (Infection pendingInfection in PendingInfections)
			{
				BuffNames disease = pendingInfection.Disease;
				if (disease != (BuffNames)8648471817152268810uL && disease != (BuffNames)14409719855781998876uL)
				{
					continue;
				}
				return true;
			}
			return false;
		}

		public static bool SimmunityPreventsInfection(Sim sim)
		{
			return sim == null || (Passion.Settings.STDSimmunity == STDImmunity.Immune && sim.HasTrait(TraitNames.Simmunity));
		}

		public static bool InfectionRandomization(Sim sim, BuffNames std)
		{
			bool result = false;
			if (sim != null)
			{
				if (Passion.Settings.STDSimmunity == STDImmunity.Resistant && sim.HasTrait(TraitNames.Simmunity))
				{
					switch (std)
					{
					case (BuffNames)14409719855781998876uL:
						result = RandomUtil.RandomChance(12f);
						break;
					case (BuffNames)8648471817152268810uL:
						result = RandomUtil.RandomChance(6f);
						break;
					default:
						result = RandomUtil.RandomChance(20f);
						break;
					}
				}
				else
				{
					switch (std)
					{
					case (BuffNames)14409719855781998876uL:
						result = RandomUtil.RandomChance(30f);
						break;
					case (BuffNames)8648471817152268810uL:
						result = RandomUtil.RandomChance(15f);
						break;
					default:
						result = RandomUtil.CoinFlip();
						break;
					}
				}
			}
			return result;
		}

		public static void Process(Sim sim)
		{
			Process(Passion.GetPlayer(sim));
		}

		public static void Process(Passion.Player player)
		{
			if (player != null && player.IsValid && Passion.Settings.STD && !SimmunityPreventsInfection(player.Actor))
			{
				try
				{
					Sim actor = player.Actor;
					Passion.Part part = player.Part;
					if (part != null)
					{
						foreach (Passion.Player value in part.Players.Values)
						{
							if (value == null || value.Actor == null || value.Actor == actor)
							{
								continue;
							}
							try
							{
								foreach (BuffNames sTD in GetSTDs(value.Actor))
								{
									switch (sTD)
									{
									case (BuffNames)12465744095023444500uL:
									case (BuffNames)16328498947224254690uL:
										if (InfectionRandomization(actor, sTD) && !actor.BuffManager.HasElement(sTD))
										{
											PendingInfections.Add(new Infection(actor, sTD, GetOnset(sTD)));
										}
										break;
									case (BuffNames)4642255024717194076uL:
									case (BuffNames)9183286871710270222uL:
										if (InfectionRandomization(actor, sTD) && !HasSimphilis(actor))
										{
											PendingInfections.Add(new Infection(actor, (BuffNames)9183286871710270222uL, GetOnset((BuffNames)9183286871710270222uL)));
										}
										break;
									case (BuffNames)16680654480998469135uL:
									case (BuffNames)3802937897211410794uL:
									case (BuffNames)6514717556347855497uL:
										if (InfectionRandomization(actor, sTD) && !HasSIV(actor))
										{
											PendingInfections.Add(new Infection(actor, (BuffNames)3802937897211410794uL, GetOnset((BuffNames)3802937897211410794uL)));
										}
										break;
									case (BuffNames)14409719855781998876uL:
									case (BuffNames)8648471817152268810uL:
										if (InfectionRandomization(actor, sTD) && !HasSimerpes(actor))
										{
											PendingInfections.Add(new Infection(actor, (BuffNames)14409719855781998876uL, GetOnset((BuffNames)14409719855781998876uL)));
										}
										break;
									}
								}
							}
							catch
							{
							}
							try
							{
								foreach (Infection pendingInfection in PendingInfections)
								{
									if (!pendingInfection.IsValid || pendingInfection.Carrier != value.Actor)
									{
										continue;
									}
									switch (pendingInfection.Disease)
									{
									case (BuffNames)12465744095023444500uL:
									case (BuffNames)16328498947224254690uL:
										if (InfectionRandomization(actor, pendingInfection.Disease) && !actor.BuffManager.HasElement(pendingInfection.Disease))
										{
											PendingInfections.Add(new Infection(actor, pendingInfection.Disease, GetOnset(pendingInfection.Disease)));
										}
										break;
									case (BuffNames)3348796355468607868uL:
									case (BuffNames)4642255024717194076uL:
									case (BuffNames)9183286871710270222uL:
										if (InfectionRandomization(actor, pendingInfection.Disease) && !HasSimphilis(actor))
										{
											PendingInfections.Add(new Infection(actor, (BuffNames)9183286871710270222uL, GetOnset((BuffNames)9183286871710270222uL)));
										}
										break;
									case (BuffNames)16680654480998469135uL:
									case (BuffNames)3802937897211410794uL:
									case (BuffNames)6514717556347855497uL:
										if (InfectionRandomization(actor, pendingInfection.Disease) && !HasSIV(actor))
										{
											PendingInfections.Add(new Infection(actor, (BuffNames)3802937897211410794uL, GetOnset((BuffNames)3802937897211410794uL)));
										}
										break;
									case (BuffNames)14409719855781998876uL:
										if (InfectionRandomization(actor, pendingInfection.Disease) && !HasSimerpes(actor))
										{
											PendingInfections.Add(new Infection(actor, (BuffNames)14409719855781998876uL, GetOnset((BuffNames)14409719855781998876uL)));
										}
										break;
									case (BuffNames)8648471817152268810uL:
										if (InfectionRandomization(actor, pendingInfection.Disease) && !HasSimerpes(actor))
										{
											PendingInfections.Add(new Infection(actor, (BuffNames)14409719855781998876uL, GetOnset((BuffNames)14409719855781998876uL)));
										}
										break;
									}
								}
							}
							catch
							{
							}
						}
					}
				}
				catch
				{
				}
			}
			List<Infection> list = new List<Infection>();
			foreach (Infection pendingInfection2 in PendingInfections)
			{
				if (pendingInfection2.IsValid)
				{
					list.Add(pendingInfection2);
				}
			}
			if (list.Count != PendingInfections.Count)
			{
				PendingInfections.Clear();
				PendingInfections = list;
			}
		}

		public static void Progression(BuffManager bm, BuffInstance bi)
		{
			switch (bi.Guid)
			{
			case (BuffNames)12465744095023444500uL:
				PendingInfections.Add(new Infection(bm.Actor, (BuffNames)12465744095023444500uL, SimClock.CurrentTicks + CustomBuff.DayTicks * RandomUtil.GetInt(30, 45), false));
				break;
			case (BuffNames)9183286871710270222uL:
				PendingInfections.Add(new Infection(bm.Actor, (BuffNames)4642255024717194076uL, GetOnset((BuffNames)4642255024717194076uL)));
				break;
			case (BuffNames)4642255024717194076uL:
				PendingInfections.Add(new Infection(bm.Actor, (BuffNames)3348796355468607868uL, GetOnset((BuffNames)3348796355468607868uL)));
				break;
			case (BuffNames)6514717556347855497uL:
				bm.AddElement((BuffNames)3802937897211410794uL, Origin.None);
				Dethkloks.Add(new Dethklok(bm.Actor, (BuffNames)3802937897211410794uL));
				break;
			case (BuffNames)14409719855781998876uL:
			case (BuffNames)8648471817152268810uL:
				PendingInfections.Add(new Infection(bm.Actor, RandomUtil.RandomChance(15f) ? ((BuffNames)14409719855781998876uL) : ((BuffNames)8648471817152268810uL), GetOnset((BuffNames)8648471817152268810uL)));
				break;
			default:
				if (!bm.Actor.Household.IsActive)
				{
					PendingInfections.Add(new Infection(bm.Actor, bi.Guid, GetOnset(BuffNames.Undefined)));
				}
				break;
			}
			List<Dethklok> list = new List<Dethklok>();
			foreach (Dethklok dethklok in Dethkloks)
			{
				if (dethklok.IsValid)
				{
					list.Add(dethklok);
				}
			}
			if (list.Count != Dethkloks.Count)
			{
				Dethkloks.Clear();
				Dethkloks = list;
			}
		}

		public static void Treatment(Sim sim)
		{
			if (sim == null)
			{
				return;
			}
			foreach (BuffNames sTD in GetSTDs(sim))
			{
				switch (sTD)
				{
				case (BuffNames)12465744095023444500uL:
				case (BuffNames)16328498947224254690uL:
				case (BuffNames)4642255024717194076uL:
				case (BuffNames)9183286871710270222uL:
					sim.BuffManager.RemoveElement(sTD);
					break;
				case (BuffNames)3348796355468607868uL:
					sim.BuffManager.RemoveElement(sTD);
					sim.BuffManager.AddElement((BuffNames)7014284627137684376uL, Origin.None);
					break;
				case (BuffNames)3802937897211410794uL:
					sim.BuffManager.RemoveElement(sTD);
					sim.BuffManager.AddElement((BuffNames)6514717556347855497uL, Origin.None);
					foreach (Dethklok dethklok in Dethkloks)
					{
						if (dethklok.Carrier == sim && dethklok.Disease == (BuffNames)3802937897211410794uL)
						{
							dethklok.Invalidate();
						}
					}
					break;
				case (BuffNames)16680654480998469135uL:
					if (!RandomUtil.RandomChance(30f))
					{
						break;
					}
					sim.BuffManager.RemoveElement(sTD);
					sim.BuffManager.AddElement((BuffNames)6514717556347855497uL, Origin.None);
					foreach (Dethklok dethklok2 in Dethkloks)
					{
						if (dethklok2.Carrier == sim && dethklok2.Disease == (BuffNames)16680654480998469135uL)
						{
							dethklok2.Invalidate();
						}
					}
					break;
				case (BuffNames)14409719855781998876uL:
					sim.BuffManager.RemoveElement(sTD);
					sim.BuffManager.AddElement((BuffNames)8648471817152268810uL, Origin.None);
					break;
				}
			}
			List<Infection> list = new List<Infection>();
			foreach (Infection pendingInfection in PendingInfections)
			{
				if (pendingInfection.Carrier == sim)
				{
					switch (pendingInfection.Disease)
					{
					case (BuffNames)12465744095023444500uL:
					case (BuffNames)16328498947224254690uL:
					case (BuffNames)3348796355468607868uL:
					case (BuffNames)4642255024717194076uL:
					case (BuffNames)9183286871710270222uL:
						pendingInfection.Invalidate();
						break;
					}
				}
			}
		}

		public static void AddRandomToAll()
		{
			AddRandomToAll(false, true);
		}

		public static void AddRandomToAll(bool includeActive, bool messages)
		{
			Sim[] globalObjects = Sims3.Gameplay.Queries.GetGlobalObjects<Sim>();
			PendingInfections = new List<Infection>();
			int num = 0;
			int num2 = 0;
			Sim[] array = globalObjects;
			foreach (Sim sim in array)
			{
				if (sim == null || !sim.IsHuman || (!includeActive && sim.Household.IsActive) || !Passion.IsValidAge(sim) || SimmunityPreventsInfection(sim))
				{
					continue;
				}
				bool flag = false;
				if (messages)
				{
					PassionCommon.BufferClear();
					PassionCommon.BufferLine(sim.Name + ":");
				}
				if (RandomUtil.RandomChance(15f) && !sim.BuffManager.HasElement((BuffNames)12465744095023444500uL))
				{
					if (messages)
					{
						PassionCommon.BufferLine("  " + PassionCommon.Localize("Gameplay/Excel/buffs/BuffList:lSTDSimydia"));
					}
					sim.BuffManager.AddElement((BuffNames)12465744095023444500uL, Origin.None);
					flag = true;
					num2++;
				}
				if (RandomUtil.RandomChance(15f) && !sim.BuffManager.HasElement((BuffNames)16328498947224254690uL))
				{
					if (messages)
					{
						PassionCommon.BufferLine("  " + PassionCommon.Localize("Gameplay/Excel/buffs/BuffList:lSTDSimnorrhea"));
					}
					sim.BuffManager.AddElement((BuffNames)16328498947224254690uL, Origin.None);
					flag = true;
					num2++;
				}
				if (RandomUtil.RandomChance(15f) && !HasSimerpes(sim))
				{
					if (messages)
					{
						PassionCommon.BufferLine("  " + PassionCommon.Localize("Gameplay/Excel/buffs/BuffList:lSTDSimerpes"));
					}
					sim.BuffManager.AddElement((BuffNames)14409719855781998876uL, Origin.None);
					flag = true;
					num2++;
				}
				if (RandomUtil.RandomChance(15f) && !HasSimphilis(sim))
				{
					if (messages)
					{
						PassionCommon.BufferLine("  " + PassionCommon.Localize("Gameplay/Excel/buffs/BuffList:lSTDSimphilis"));
					}
					sim.BuffManager.AddElement((BuffNames)9183286871710270222uL, Origin.None);
					flag = true;
					num2++;
				}
				if (RandomUtil.RandomChance(5f) && !HasSIV(sim))
				{
					if (messages)
					{
						PassionCommon.BufferLine("  " + PassionCommon.Localize("Gameplay/Excel/buffs/BuffList:lSTDSIV"));
					}
					sim.BuffManager.AddElement((BuffNames)3802937897211410794uL, Origin.None);
					Dethkloks.Add(new Dethklok(sim, (BuffNames)3802937897211410794uL));
					flag = true;
					num2++;
				}
				if (flag)
				{
					if (messages)
					{
						PassionCommon.SimMessage(sim);
					}
					num++;
				}
			}
			PassionCommon.SystemMessage("Randomly added " + num2 + " STD's to " + num + " Sims.");
		}

		public static void RemoveFromAll()
		{
			Sim[] globalObjects = Sims3.Gameplay.Queries.GetGlobalObjects<Sim>();
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			Sim[] array = globalObjects;
			foreach (Sim sim in array)
			{
				if (sim == null || !sim.IsHuman || !Passion.IsValidAge(sim, true))
				{
					continue;
				}
				foreach (BuffNames item in Names.STD)
				{
					if (sim.BuffManager.HasElement(item))
					{
						sim.BuffManager.RemoveElement(item);
						num++;
					}
				}
			}
			foreach (Infection pendingInfection in PendingInfections)
			{
				pendingInfection.Invalidate();
				num2++;
			}
			PendingInfections.Clear();
			foreach (Dethklok dethklok in Dethkloks)
			{
				dethklok.Invalidate();
				num3++;
			}
			Dethkloks.Clear();
			PassionCommon.SystemMessage("Removed " + num + " diseases, " + num2 + " incubating infections, and " + num3 + " active death clocks.");
		}

		public static void RemoveFrom(Sim carrier)
		{
			if (carrier == null)
			{
				return;
			}
			foreach (BuffNames item in Names.STD)
			{
				if (carrier.BuffManager.HasElement(item))
				{
					carrier.BuffManager.RemoveElement(item);
				}
			}
			foreach (Infection pendingInfection in PendingInfections)
			{
				if (pendingInfection.Carrier == carrier)
				{
					pendingInfection.Invalidate();
				}
			}
			foreach (Dethklok dethklok in Dethkloks)
			{
				if (dethklok.Carrier == carrier)
				{
					dethklok.Invalidate();
				}
			}
		}

		public override void OnTimeout(BuffManager bm, BuffInstance bi, OnTimeoutReasons reason)
		{
			Progression(bm, bi);
		}
	}
}
