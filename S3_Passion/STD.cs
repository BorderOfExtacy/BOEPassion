using System;
using System.Collections.Generic;
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

namespace Passion.S3_Passion
{
	public class Std : CustomBuff
	{
		public class Infection
		{
			private SimDescription _carrierDescription;

			public BuffNames Disease;

			private long _onset;

			private AlarmHandle _incubationAlarm;

			private bool _reinfection;

			public bool IsValid;

			public Sim Carrier
			{
				get
				{
					return _carrierDescription != null ? _carrierDescription.CreatedSim : null;
				}
				private set { _carrierDescription = value != null ? value.SimDescription : null; }
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
				_onset = onset;
				_reinfection = reinfection;
				IsValid = true;
				_incubationAlarm = Carrier.AddAlarmRepeating(5f, TimeUnit.Minutes, Callback, 7f, TimeUnit.Hours, "Periodic STD Check", AlarmType.AlwaysPersisted);
			}

			public void Invalidate()
			{
				IsValid = false;
				if (Carrier != null)
				{
					Carrier.RemoveAlarm(_incubationAlarm);
				}
			}

			private void Callback()
			{
				if (SimClock.CurrentTicks < _onset) return;
				if (_reinfection)
				{
					Carrier.BuffManager.AddElement(Disease, Origin.None);
				}
				Invalidate();
				BuffNames disease = Disease;
				if (disease == (BuffNames)3348796355468607868uL || disease == (BuffNames)3802937897211410794uL || disease == (BuffNames)16680654480998469135uL)
				{
					DeathClocks.Add(new DeathClock(Carrier, Disease));
				}
			}
		}

		public class DeathClock
		{
			private SimDescription _carrierDescription;

			public BuffNames Disease;

			public long NextCheck;

			private readonly AlarmHandle _deathClockAlarm;

			public bool IsValid;

			public Sim Carrier
			{
				get
				{
					return _carrierDescription != null ? _carrierDescription.CreatedSim : null;
				}
				private set { _carrierDescription = value != null ? value.SimDescription : null; }
			}

			public DeathClock(Sim carrier, BuffNames disease)
			{
				Carrier = carrier;
				Disease = disease;
				SetNextCheck();
				IsValid = true;
				_deathClockAlarm = Carrier.AddAlarmRepeating(1f, TimeUnit.Days, Callback, 5f, TimeUnit.Hours, "DeathClock Alarm", AlarmType.AlwaysPersisted);
			}

			public void Invalidate()
			{
				IsValid = false;
				if (Carrier != null)
				{
					Carrier.RemoveAlarm(_deathClockAlarm);
				}
			}

			private void SetNextCheck()
			{
				switch (Disease)
				{
				case (BuffNames)3802937897211410794uL:
					NextCheck = SimClock.CurrentTicks + (long)((float)DayTicks * RandomUtil.GetFloat(1f, 20f));
					break;
				case (BuffNames)16680654480998469135uL:
					NextCheck = SimClock.CurrentTicks + (long)((float)DayTicks * RandomUtil.GetFloat(2f, 7f));
					break;
				case (BuffNames)3348796355468607868uL:
					NextCheck = SimClock.CurrentTicks + (long)((float)DayTicks * RandomUtil.GetFloat(5f, 16f));
					break;
				default:
					throw new ArgumentOutOfRangeException();
				}
			}

			private void Callback()
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
					if (Carrier != null && !Carrier.TraitManager.HasElement(TraitNames.Insane) && RandomUtil.RandomChance(20f))
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
						if (Carrier != null)
						{
							Carrier.BuffManager.RemoveElement((BuffNames)3802937897211410794uL);
							Carrier.BuffManager.AddElement((BuffNames)16680654480998469135uL, Origin.None);
						}
					}
					SetNextCheck();
					break;
				case (BuffNames)16680654480998469135uL:
					if (RandomUtil.RandomChance(30f))
					{
						Invalidate();
						if (Carrier != null)
						{
							Carrier.InteractionQueue.CancelAllInteractions();
							Carrier.Kill(SimDescription.DeathType.OldAge);
						}
					}
					else
					{
						SetNextCheck();
					}
					break;
				default:
					throw new ArgumentOutOfRangeException();
				}
			}
		}

		public class GetTreated : RabbitHole.RabbitHoleInteraction<Sim, RabbitHole>
		{
			private sealed class Definition : InteractionDefinition<Sim, RabbitHole, GetTreated>
			{
				public override bool Test(Sim sim, RabbitHole target, bool isAutonomous, ref GreyedOutTooltipCallback greyedOutTooltipCallback)
				{
					return HasTreatableStDs(sim);
				}

				public override string GetInteractionName(Sim actor, RabbitHole target, InteractionObjectPair iop)
				{
					return Localization.LocalizeString("STD.GetTreated:InteractionName") + "(" + UIUtils.FormatMoney(_kCostOfTreatment) + ")";
				}
			}

			private static readonly int _kSimMinutesForTreatment = 120;

			private static readonly int _kCostOfTreatment = 500;

			public static readonly InteractionDefinition Singleton = new Definition();

			public override void ConfigureInteraction()
			{
				base.ConfigureInteraction();
				TimedStage timedStage = new TimedStage(GetInteractionName(), _kSimMinutesForTreatment, false, true, true);
				Stages = new List<Stage>(new Stage[1] { timedStage });
				ActiveStage = timedStage;
			}

			public override bool InRabbitHole()
			{
				StartStages();
				BeginCommodityUpdates();
				bool flag = DoLoop(ExitReason.Default);
				if (Actor.HasExitReason(ExitReason.StageComplete))
				{
					if (Actor.FamilyFunds >= _kCostOfTreatment)
					{
						Actor.ModifyFunds(-_kCostOfTreatment);
					}
					else if (!GameUtils.IsFutureWorld())
					{
						Actor.UnpaidBills += _kCostOfTreatment;
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
		private static List<Infection> _mPendingInfections;

		public static List<Infection> PendingInfectionsBackup = new List<Infection>();

		[PersistableStatic]
		private static List<DeathClock> _mDeathClocks;

		public static List<DeathClock> DeathClocksBackup = new List<DeathClock>();

		private static List<Infection> PendingInfections
		{
			get { return _mPendingInfections ?? (_mPendingInfections = new List<Infection>()); }
			set
			{
				_mPendingInfections = value;
			}
		}

		private static List<DeathClock> DeathClocks
		{
			get { return _mDeathClocks ?? (_mDeathClocks = new List<DeathClock>()); }
			set
			{
				_mDeathClocks = value;
			}
		}

		public Std(BuffData info)
			: base(info)
		{
		}

		public static void ClearData()
		{
			if (_mPendingInfections != null)
			{
				PendingInfectionsBackup = new List<Infection>(_mPendingInfections);
				_mPendingInfections.Clear();
				_mPendingInfections = null;
			}

			if (_mDeathClocks == null) return;
			DeathClocksBackup = new List<DeathClock>(_mDeathClocks);
			_mDeathClocks.Clear();
			_mDeathClocks = null;
		}

		private static long GetOnset(BuffNames std)
		{
			long num = 0L;
			switch (std)
			{
			case (BuffNames)12465744095023444500uL:
			case (BuffNames)14409719855781998876uL:
			case (BuffNames)16328498947224254690uL:
			case (BuffNames)9183286871710270222uL:
				num = DayTicks * RandomUtil.GetInt(2, 4);
				break;
			case (BuffNames)4642255024717194076uL:
				num = DayTicks * RandomUtil.GetInt(20, 30);
				break;
			case (BuffNames)3348796355468607868uL:
				num = DayTicks * RandomUtil.GetInt(40, 80);
				break;
			case (BuffNames)8648471817152268810uL:
				num = DayTicks * RandomUtil.GetInt(21, 35);
				break;
			case (BuffNames)3802937897211410794uL:
				num = DayTicks * RandomUtil.GetInt(20, 60);
				break;
			default:
				num = DayTicks;
				break;
			}
			return SimClock.CurrentTicks + num;
		}

		private static List<BuffNames> GetStDs(Sim sim)
		{
			List<BuffNames> list = new List<BuffNames>();
			if (sim == null) return list;
			foreach (BuffNames item in Names.Std)
			{
				if (sim.BuffManager.HasElement(item))
				{
					list.Add(item);
				}
			}
			return list;
		}

		private static bool HasTreatableStDs(Sim sim)
		{
			using (List<BuffNames>.Enumerator enumerator = GetStDs(sim).GetEnumerator())
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

		private static bool HasSiv(Sim sim)
		{
			foreach (BuffNames sTd in GetStDs(sim))
			{
				BuffNames buffNames = sTd;
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

		private static bool HasSimphilis(Sim sim)
		{
			using (List<BuffNames>.Enumerator enumerator = GetStDs(sim).GetEnumerator())
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
					default:
						throw new ArgumentOutOfRangeException();
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
				default:
					throw new ArgumentOutOfRangeException();
				}
			}
			return false;
		}

		private static bool HasSimerpes(Sim sim)
		{
			foreach (BuffNames sTd in GetStDs(sim))
			{
				BuffNames buffNames = sTd;
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

		private static bool SimmunityPreventsInfection(Sim sim)
		{
			return sim == null || (Passion.Settings.StdSimmunity == StdImmunity.Immune && sim.HasTrait(TraitNames.Simmunity));
		}

		private static bool InfectionRandomization(Sim sim, BuffNames std)
		{
			bool result = false;
			if (sim == null) return result;
			if (Passion.Settings.StdSimmunity == StdImmunity.Resistant && sim.HasTrait(TraitNames.Simmunity))
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
			return result;
		}

		public static void Process(Sim sim)
		{
			Process(Passion.GetPlayer(sim));
		}

		private static void Process(Passion.Player player)
		{
			if (player != null && player.IsValid && Passion.Settings.Std && !SimmunityPreventsInfection(player.Actor))
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
								foreach (BuffNames sTd in GetStDs(value.Actor))
								{
									switch (sTd)
									{
									case (BuffNames)12465744095023444500uL:
									case (BuffNames)16328498947224254690uL:
										if (InfectionRandomization(actor, sTd) && !actor.BuffManager.HasElement(sTd))
										{
											PendingInfections.Add(new Infection(actor, sTd, GetOnset(sTd)));
										}
										break;
									case (BuffNames)4642255024717194076uL:
									case (BuffNames)9183286871710270222uL:
										if (InfectionRandomization(actor, sTd) && !HasSimphilis(actor))
										{
											PendingInfections.Add(new Infection(actor, (BuffNames)9183286871710270222uL, GetOnset((BuffNames)9183286871710270222uL)));
										}
										break;
									case (BuffNames)16680654480998469135uL:
									case (BuffNames)3802937897211410794uL:
									case (BuffNames)6514717556347855497uL:
										if (InfectionRandomization(actor, sTd) && !HasSiv(actor))
										{
											PendingInfections.Add(new Infection(actor, (BuffNames)3802937897211410794uL, GetOnset((BuffNames)3802937897211410794uL)));
										}
										break;
									case (BuffNames)14409719855781998876uL:
									case (BuffNames)8648471817152268810uL:
										if (InfectionRandomization(actor, sTd) && !HasSimerpes(actor))
										{
											PendingInfections.Add(new Infection(actor, (BuffNames)14409719855781998876uL, GetOnset((BuffNames)14409719855781998876uL)));
										}
										break;
									default:
										throw new ArgumentOutOfRangeException();
									}
								}
							}
							catch
							{
								// ignored
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
										if (InfectionRandomization(actor, pendingInfection.Disease) && !HasSiv(actor))
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
									default:
										throw new ArgumentOutOfRangeException();
									}
								}
							}
							catch
							{
								// ignored
							}
						}
					}
				}
				catch
				{
					// ignored
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

			if (list.Count == PendingInfections.Count) return;
			PendingInfections.Clear();
			PendingInfections = list;
		}

		private static void Progression(BuffManager bm, BuffInstance bi)
		{
			switch (bi.Guid)
			{
			case (BuffNames)12465744095023444500uL:
				PendingInfections.Add(new Infection(bm.Actor, (BuffNames)12465744095023444500uL, SimClock.CurrentTicks + DayTicks * RandomUtil.GetInt(30, 45), false));
				break;
			case (BuffNames)9183286871710270222uL:
				PendingInfections.Add(new Infection(bm.Actor, (BuffNames)4642255024717194076uL, GetOnset((BuffNames)4642255024717194076uL)));
				break;
			case (BuffNames)4642255024717194076uL:
				PendingInfections.Add(new Infection(bm.Actor, (BuffNames)3348796355468607868uL, GetOnset((BuffNames)3348796355468607868uL)));
				break;
			case (BuffNames)6514717556347855497uL:
				bm.AddElement((BuffNames)3802937897211410794uL, Origin.None);
				DeathClocks.Add(new DeathClock(bm.Actor, (BuffNames)3802937897211410794uL));
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
			List<DeathClock> list = new List<DeathClock>();
			foreach (DeathClock dethklok in DeathClocks)
			{
				if (dethklok.IsValid)
				{
					list.Add(dethklok);
				}
			}

			if (list.Count == DeathClocks.Count) return;
			DeathClocks.Clear();
			DeathClocks = list;
		}

		private static void Treatment(Sim sim)
		{
			if (sim == null)
			{
				return;
			}
			foreach (BuffNames sTd in GetStDs(sim))
			{
				switch (sTd)
				{
				case (BuffNames)12465744095023444500uL:
				case (BuffNames)16328498947224254690uL:
				case (BuffNames)4642255024717194076uL:
				case (BuffNames)9183286871710270222uL:
					sim.BuffManager.RemoveElement(sTd);
					break;
				case (BuffNames)3348796355468607868uL:
					sim.BuffManager.RemoveElement(sTd);
					sim.BuffManager.AddElement((BuffNames)7014284627137684376uL, Origin.None);
					break;
				case (BuffNames)3802937897211410794uL:
					sim.BuffManager.RemoveElement(sTd);
					sim.BuffManager.AddElement((BuffNames)6514717556347855497uL, Origin.None);
					foreach (DeathClock dethklok in DeathClocks)
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
					sim.BuffManager.RemoveElement(sTd);
					sim.BuffManager.AddElement((BuffNames)6514717556347855497uL, Origin.None);
					foreach (DeathClock dethklok2 in DeathClocks)
					{
						if (dethklok2.Carrier == sim && dethklok2.Disease == (BuffNames)16680654480998469135uL)
						{
							dethklok2.Invalidate();
						}
					}
					break;
				case (BuffNames)14409719855781998876uL:
					sim.BuffManager.RemoveElement(sTd);
					sim.BuffManager.AddElement((BuffNames)8648471817152268810uL, Origin.None);
					break;
				default:
					throw new ArgumentOutOfRangeException();
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
					default:
						throw new ArgumentOutOfRangeException();
					}
				}
			}
		}

		public static void AddRandomToAll()
		{
			AddRandomToAll(false, true);
		}

		private static void AddRandomToAll(bool includeActive, bool messages)
		{
			Sim[] globalObjects = global::Sims3.Gameplay.Queries.GetGlobalObjects<Sim>();
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
				if (RandomUtil.RandomChance(5f) && !HasSiv(sim))
				{
					if (messages)
					{
						PassionCommon.BufferLine("  " + PassionCommon.Localize("Gameplay/Excel/buffs/BuffList:lSTDSIV"));
					}
					sim.BuffManager.AddElement((BuffNames)3802937897211410794uL, Origin.None);
					DeathClocks.Add(new DeathClock(sim, (BuffNames)3802937897211410794uL));
					flag = true;
					num2++;
				}

				if (!flag) continue;
				if (messages)
				{
					PassionCommon.SimMessage(sim);
				}
				num++;
			}
			PassionCommon.SystemMessage("Randomly added " + num2 + " STD's to " + num + " Sims.");
		}

		public static void RemoveFromAll()
		{
			Sim[] globalObjects = global::Sims3.Gameplay.Queries.GetGlobalObjects<Sim>();
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
				foreach (BuffNames item in Names.Std)
				{
					if (!sim.BuffManager.HasElement(item)) continue;
					sim.BuffManager.RemoveElement(item);
					num++;
				}
			}
			foreach (Infection pendingInfection in PendingInfections)
			{
				pendingInfection.Invalidate();
				num2++;
			}
			PendingInfections.Clear();
			foreach (DeathClock dethklok in DeathClocks)
			{
				dethklok.Invalidate();
				num3++;
			}
			DeathClocks.Clear();
			PassionCommon.SystemMessage("Removed " + num + " diseases, " + num2 + " incubating infections, and " + num3 + " active death clocks.");
		}

		public static void RemoveFrom(Sim carrier)
		{
			if (carrier == null)
			{
				return;
			}
			foreach (BuffNames item in Names.Std)
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
			foreach (DeathClock dethklok in DeathClocks)
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
