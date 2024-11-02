using System.Collections.Generic;
using Sims3.Gameplay;
using Sims3.Gameplay.Actors;
using Sims3.Gameplay.Autonomy;
using Sims3.Gameplay.Core;
using Sims3.Gameplay.Interactions;
using Sims3.Gameplay.Objects.Decorations;
using Sims3.Gameplay.Objects.Electronics;
using Sims3.Gameplay.Utilities;
using Sims3.SimIFace;

namespace S3_Passion.PassionDance
{
	public class CustomDance : Interaction<Sim, DanceFloor>
	{
		public class NightFeaver : Interaction<Sim, DanceFloor>
		{
			[DoesntRequireTuning]
			private sealed class Definition : InteractionDefinition<Sim, DanceFloor, NightFeaver>
			{
				public override string GetInteractionName(Sim actor, DanceFloor target, InteractionObjectPair interaction)
				{
					return Localization.LocalizeString("S3_Passion.Terms.NightFeaver");
				}

				public override string[] GetPath(bool isFemale)
				{
					return new string[1] { Localization.LocalizeString("CustomDance") };
				}

				public override bool Test(Sim actor, DanceFloor target, bool IsAutonomous, ref GreyedOutTooltipCallback greyedOutTooltipCallback)
				{
					if (!IsAutonomous && actor != null && target != null && actor.SimDescription.TeenOrAbove)
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
				if (Actor.RouteToObjectRadius(Target, 1f) && stereo != null && Actor.SimDescription.TeenOrAbove)
				{
					Vector3 position = new Vector3(Actor.Position);
					Target.ClearUseList();
					Target.AddToUseList(Actor);
					foreach (Stereo item2 in list)
					{
						if (item2.TurnedOn)
						{
							stereo = item2;
							break;
						}
						stereo = null;
					}
					while (stereo != null && Actor.HasNoExitReason() && !Actor.Motives.CheckMotivesForTimeToLeave(Actor.Motives, (InteractionInstance)this, false, base.Autonomous))
					{
						foreach (Stereo item3 in list)
						{
							if (item3.TurnedOn)
							{
								stereo = item3;
								break;
							}
							stereo = null;
						}
						Actor.PlaySoloAnimation(RandomUtil.GetRandomObjectFromList(NightFeaverAnimations));
						Target.RemoveFromUseList(Actor);
						Actor.SetPosition(position);
					}
				}
				return true;
			}
		}

		public class DiscoDance : Interaction<Sim, DanceFloor>
		{
			[DoesntRequireTuning]
			private sealed class Definition : InteractionDefinition<Sim, DanceFloor, DiscoDance>
			{
				public override string GetInteractionName(Sim actor, DanceFloor target, InteractionObjectPair interaction)
				{
					return Localization.LocalizeString("S3_Passion.Terms.DiscoDance");
				}

				public override string[] GetPath(bool isFemale)
				{
					return new string[1] { Localization.LocalizeString("CustomDance") };
				}

				public override bool Test(Sim actor, DanceFloor target, bool IsAutonomous, ref GreyedOutTooltipCallback greyedOutTooltipCallback)
				{
					if (!IsAutonomous && actor != null && target != null && actor.SimDescription.TeenOrAbove)
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
				if (Actor.RouteToObjectRadius(Target, 1f) && stereo != null && Actor.SimDescription.TeenOrAbove)
				{
					Vector3 position = new Vector3(Actor.Position);
					Target.ClearUseList();
					Target.AddToUseList(Actor);
					foreach (Stereo item2 in list)
					{
						if (item2.TurnedOn)
						{
							stereo = item2;
							break;
						}
						stereo = null;
					}
					while (stereo != null && Actor.HasNoExitReason() && !Actor.Motives.CheckMotivesForTimeToLeave(Actor.Motives, (InteractionInstance)this, false, base.Autonomous))
					{
						foreach (Stereo item3 in list)
						{
							if (item3.TurnedOn)
							{
								stereo = item3;
								break;
							}
							stereo = null;
						}
						Actor.PlaySoloAnimation(RandomUtil.GetRandomObjectFromList(DiscoAnimations));
						Target.RemoveFromUseList(Actor);
						Actor.SetPosition(position);
					}
				}
				return true;
			}
		}

		public class Hifiraver : Interaction<Sim, DanceFloor>
		{
			[DoesntRequireTuning]
			private sealed class Definition : InteractionDefinition<Sim, DanceFloor, Hifiraver>
			{
				public override string GetInteractionName(Sim actor, DanceFloor target, InteractionObjectPair interaction)
				{
					return Localization.LocalizeString("S3_Passion.Terms.Hifiraver");
				}

				public override string[] GetPath(bool isFemale)
				{
					return new string[1] { Localization.LocalizeString("CustomDance") };
				}

				public override bool Test(Sim actor, DanceFloor target, bool IsAutonomous, ref GreyedOutTooltipCallback greyedOutTooltipCallback)
				{
					if (!IsAutonomous && actor != null && target != null && actor.SimDescription.TeenOrAbove)
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
				if (Actor.RouteToObjectRadius(Target, 1f) && stereo != null && Actor.SimDescription.TeenOrAbove)
				{
					Vector3 position = new Vector3(Actor.Position);
					Target.ClearUseList();
					Target.AddToUseList(Actor);
					foreach (Stereo item2 in list)
					{
						if (item2.TurnedOn)
						{
							stereo = item2;
							break;
						}
						stereo = null;
					}
					while (stereo != null && Actor.HasNoExitReason() && !Actor.Motives.CheckMotivesForTimeToLeave(Actor.Motives, (InteractionInstance)this, false, base.Autonomous))
					{
						foreach (Stereo item3 in list)
						{
							if (item3.TurnedOn)
							{
								stereo = item3;
								break;
							}
							stereo = null;
						}
						Actor.PlaySoloAnimation(RandomUtil.GetRandomObjectFromList(HifiraverAnimations));
						Target.RemoveFromUseList(Actor);
						Actor.SetPosition(position);
					}
				}
				return true;
			}
		}

		public class JunjoufFighter : Interaction<Sim, DanceFloor>
		{
			[DoesntRequireTuning]
			private sealed class Definition : InteractionDefinition<Sim, DanceFloor, JunjoufFighter>
			{
				public override string GetInteractionName(Sim actor, DanceFloor target, InteractionObjectPair interaction)
				{
					return Localization.LocalizeString("S3_Passion.Terms.JunjoufFighter");
				}

				public override string[] GetPath(bool isFemale)
				{
					return new string[1] { Localization.LocalizeString("CustomDance") };
				}

				public override bool Test(Sim actor, DanceFloor target, bool IsAutonomous, ref GreyedOutTooltipCallback greyedOutTooltipCallback)
				{
					if (!IsAutonomous && actor != null && target != null && actor.SimDescription.TeenOrAbove)
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
				if (Actor.RouteToObjectRadius(Target, 1f) && stereo != null && Actor.SimDescription.TeenOrAbove)
				{
					Vector3 position = new Vector3(Actor.Position);
					Target.ClearUseList();
					Target.AddToUseList(Actor);
					foreach (Stereo item2 in list)
					{
						if (item2.TurnedOn)
						{
							stereo = item2;
							break;
						}
						stereo = null;
					}
					while (stereo != null && Actor.HasNoExitReason() && !Actor.Motives.CheckMotivesForTimeToLeave(Actor.Motives, (InteractionInstance)this, false, base.Autonomous))
					{
						foreach (Stereo item3 in list)
						{
							if (item3.TurnedOn)
							{
								stereo = item3;
								break;
							}
							stereo = null;
						}
						Actor.PlaySoloAnimation(RandomUtil.GetRandomObjectFromList(JunjoufFighterAnimations));
						Target.RemoveFromUseList(Actor);
						Actor.SetPosition(position);
					}
				}
				return true;
			}
		}

		public class TiktTok : Interaction<Sim, DanceFloor>
		{
			[DoesntRequireTuning]
			private sealed class Definition : InteractionDefinition<Sim, DanceFloor, TiktTok>
			{
				public override string GetInteractionName(Sim actor, DanceFloor target, InteractionObjectPair interaction)
				{
					return Localization.LocalizeString("S3_Passion.Terms.TiktTok");
				}

				public override string[] GetPath(bool isFemale)
				{
					return new string[1] { Localization.LocalizeString("CustomDance") };
				}

				public override bool Test(Sim actor, DanceFloor target, bool IsAutonomous, ref GreyedOutTooltipCallback greyedOutTooltipCallback)
				{
					if (!IsAutonomous && actor != null && target != null && actor.SimDescription.TeenOrAbove)
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
				if (Actor.RouteToObjectRadius(Target, 1f) && stereo != null && Actor.SimDescription.TeenOrAbove)
				{
					Vector3 position = new Vector3(Actor.Position);
					Target.ClearUseList();
					Target.AddToUseList(Actor);
					foreach (Stereo item2 in list)
					{
						if (item2.TurnedOn)
						{
							stereo = item2;
							break;
						}
						stereo = null;
					}
					while (stereo != null && Actor.HasNoExitReason() && !Actor.Motives.CheckMotivesForTimeToLeave(Actor.Motives, (InteractionInstance)this, false, base.Autonomous))
					{
						foreach (Stereo item3 in list)
						{
							if (item3.TurnedOn)
							{
								stereo = item3;
								break;
							}
							stereo = null;
						}
						Actor.PlaySoloAnimation(RandomUtil.GetRandomObjectFromList(TiktTokAnimations));
						Target.RemoveFromUseList(Actor);
						Actor.SetPosition(position);
					}
				}
				return true;
			}
		}

		public class Valenti : Interaction<Sim, DanceFloor>
		{
			[DoesntRequireTuning]
			private sealed class Definition : InteractionDefinition<Sim, DanceFloor, Valenti>
			{
				public override string GetInteractionName(Sim actor, DanceFloor target, InteractionObjectPair interaction)
				{
					return Localization.LocalizeString("S3_Passion.Terms.Valenti");
				}

				public override string[] GetPath(bool isFemale)
				{
					return new string[1] { Localization.LocalizeString("CustomDance") };
				}

				public override bool Test(Sim actor, DanceFloor target, bool IsAutonomous, ref GreyedOutTooltipCallback greyedOutTooltipCallback)
				{
					if (!IsAutonomous && actor != null && target != null && actor.SimDescription.TeenOrAbove)
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
				if (Actor.RouteToObjectRadius(Target, 1f) && stereo != null && Actor.SimDescription.TeenOrAbove)
				{
					Vector3 position = new Vector3(Actor.Position);
					Target.ClearUseList();
					Target.AddToUseList(Actor);
					foreach (Stereo item2 in list)
					{
						if (item2.TurnedOn)
						{
							stereo = item2;
							break;
						}
						stereo = null;
					}
					while (stereo != null && Actor.HasNoExitReason() && !Actor.Motives.CheckMotivesForTimeToLeave(Actor.Motives, (InteractionInstance)this, false, base.Autonomous))
					{
						foreach (Stereo item3 in list)
						{
							if (item3.TurnedOn)
							{
								stereo = item3;
								break;
							}
							stereo = null;
						}
						Actor.PlaySoloAnimation(RandomUtil.GetRandomObjectFromList(ValentiAnimations));
						Target.RemoveFromUseList(Actor);
						Actor.SetPosition(position);
					}
				}
				return true;
			}
		}

		public static List<string> NightFeaverAnimations;

		public static List<string> DiscoAnimations;

		public static List<string> HifiraverAnimations;

		public static List<string> JunjoufFighterAnimations;

		public static List<string> TiktTokAnimations;

		public static List<string> ValentiAnimations;

		static CustomDance()
		{
			List<string> list = new List<string>();
			list.Add("a_umpa_simsnightfever");
			NightFeaverAnimations = list;
			List<string> list2 = new List<string>();
			list2.Add("a_ud_disco_01");
			list2.Add("a_ud_disco_02");
			list2.Add("a_ud_disco_03");
			list2.Add("a_ud_disco_04");
			list2.Add("a_ud_disco_05");
			list2.Add("a_ud_disco_06");
			list2.Add("a_ud_disco_07");
			list2.Add("a_ud_disco_08");
			list2.Add("a_ud_disco_09");
			list2.Add("a_ud_disco_10");
			list2.Add("a_ud_disco_11");
			DiscoAnimations = list2;
			List<string> list3 = new List<string>();
			list3.Add("a_mmd_hifiraver_1");
			list3.Add("a_mmd_hifiraver_2");
			list3.Add("a_mmd_hifiraver_3");
			list3.Add("a_mmd_hifiraver_4");
			HifiraverAnimations = list3;
			List<string> list4 = new List<string>();
			list4.Add("a_mmd_junjoufighter_1");
			list4.Add("a_mmd_junjoufighter_2");
			list4.Add("a_mmd_junjoufighter_3");
			list4.Add("a_mmd_junjoufighter_4");
			JunjoufFighterAnimations = list4;
			List<string> list5 = new List<string>();
			list5.Add("a_mmd_tiktok");
			TiktTokAnimations = list5;
			List<string> list6 = new List<string>();
			list6.Add("a_mmd_valenti_1");
			list6.Add("a_mmd_valenti_2");
			ValentiAnimations = list6;
		}
	}
}
