using System;
using System.Collections.Generic;
using Sims3.Gameplay;
using Sims3.Gameplay.Abstracts;
using Sims3.Gameplay.ActorSystems;
using Sims3.Gameplay.Autonomy;
using Sims3.Gameplay.Objects.RabbitHoles;
using Sims3.Gameplay.Socializing;
using Sims3.Gameplay.UI;
using Sims3.Gameplay.Utilities;
using Sims3.SimIFace;
using Sims3.SimIFace.CAS;
using Sims3.UI.Hud;

namespace S3_Passion
{
	public class CustomBuff : Buff
	{
		public static class Names
		{
			public const BuffNames Invalid = BuffNames.Undefined;

			public const BuffNames NRaasWorryAboutWoohoo = (BuffNames)5912255412026328145uL;

			public const BuffNames HadFirstWoohoo = (BuffNames)9944098001884692765uL;

			public const BuffNames HadWoohoo = (BuffNames)2111502432315482727uL;

			public const BuffNames LibidoSatisfied = (BuffNames)8185339104921261200uL;

			public const BuffNames LibidoSlightlyPositive = (BuffNames)16512105510841219241uL;

			public const BuffNames LibidoModeratelyPositive = (BuffNames)10626820509964641423uL;

			public const BuffNames LibidoVeryPositive = (BuffNames)16341761339885008577uL;

			public const BuffNames LibidoExtremelyPositive = (BuffNames)16113274642161440716uL;

			public const BuffNames LibidoSlightlyNegative = (BuffNames)7244019685987188093uL;

			public const BuffNames LibidoModeratelyNegative = (BuffNames)13910300093031145699uL;

			public const BuffNames LibidoVeryNegative = (BuffNames)12415407305475427397uL;

			public const BuffNames LibidoExtremelyNegative = (BuffNames)1358929223039794148uL;

			public const BuffNames STDSimydia = (BuffNames)12465744095023444500uL;

			public const BuffNames STDSimnorrhea = (BuffNames)16328498947224254690uL;

			public const BuffNames STDSimphilis = (BuffNames)9183286871710270222uL;

			public const BuffNames STDSimphilisSecondary = (BuffNames)4642255024717194076uL;

			public const BuffNames STDSimphilisTertiary = (BuffNames)3348796355468607868uL;

			public const BuffNames STDSimphilisPermanent = (BuffNames)7014284627137684376uL;

			public const BuffNames STDSIV = (BuffNames)3802937897211410794uL;

			public const BuffNames STDSIVManaged = (BuffNames)6514717556347855497uL;

			public const BuffNames STDSIVComplications = (BuffNames)16680654480998469135uL;

			public const BuffNames STDSimerpes = (BuffNames)14409719855781998876uL;

			public const BuffNames STDSimerpesMild = (BuffNames)8648471817152268810uL;

			public static readonly List<BuffNames> List;

			public static readonly List<BuffNames> Woohoo;

			public static readonly List<BuffNames> Libido;

			public static readonly List<BuffNames> STD;

			public static string ToString(BuffNames buff)
			{
				switch (buff)
				{
				case (BuffNames)2111502432315482727uL:
					return "Had Woohoo";
				case (BuffNames)9944098001884692765uL:
					return "Had First Woohoo";
				case (BuffNames)10626820509964641423uL:
				case (BuffNames)12415407305475427397uL:
				case (BuffNames)13910300093031145699uL:
				case (BuffNames)16113274642161440716uL:
				case (BuffNames)16341761339885008577uL:
				case (BuffNames)16512105510841219241uL:
				case (BuffNames)1358929223039794148uL:
				case (BuffNames)7244019685987188093uL:
				case (BuffNames)8185339104921261200uL:
					return "Libido";
				case (BuffNames)12465744095023444500uL:
					return "Simydia";
				case (BuffNames)16328498947224254690uL:
					return "Simnorrhea";
				case (BuffNames)3348796355468607868uL:
				case (BuffNames)4642255024717194076uL:
				case (BuffNames)7014284627137684376uL:
				case (BuffNames)9183286871710270222uL:
					return "Simphilis";
				case (BuffNames)16680654480998469135uL:
				case (BuffNames)3802937897211410794uL:
				case (BuffNames)6514717556347855497uL:
					return "Sim Immunodeficiency Virus";
				case (BuffNames)14409719855781998876uL:
				case (BuffNames)8648471817152268810uL:
					return "Simerpes";
				default:
					return "";
				}
			}

			static Names()
			{
				List<BuffNames> list = new List<BuffNames>();
				list.Add((BuffNames)2111502432315482727uL);
				list.Add((BuffNames)9944098001884692765uL);
				list.Add((BuffNames)8185339104921261200uL);
				list.Add((BuffNames)16512105510841219241uL);
				list.Add((BuffNames)10626820509964641423uL);
				list.Add((BuffNames)16341761339885008577uL);
				list.Add((BuffNames)16113274642161440716uL);
				list.Add((BuffNames)7244019685987188093uL);
				list.Add((BuffNames)13910300093031145699uL);
				list.Add((BuffNames)12415407305475427397uL);
				list.Add((BuffNames)1358929223039794148uL);
				list.Add((BuffNames)12465744095023444500uL);
				list.Add((BuffNames)16328498947224254690uL);
				list.Add((BuffNames)9183286871710270222uL);
				list.Add((BuffNames)4642255024717194076uL);
				list.Add((BuffNames)3348796355468607868uL);
				list.Add((BuffNames)3802937897211410794uL);
				list.Add((BuffNames)6514717556347855497uL);
				list.Add((BuffNames)16680654480998469135uL);
				list.Add((BuffNames)14409719855781998876uL);
				list.Add((BuffNames)8648471817152268810uL);
				List = list;
				List<BuffNames> list2 = new List<BuffNames>();
				list2.Add((BuffNames)2111502432315482727uL);
				list2.Add((BuffNames)9944098001884692765uL);
				Woohoo = list2;
				List<BuffNames> list3 = new List<BuffNames>();
				list3.Add((BuffNames)8185339104921261200uL);
				list3.Add((BuffNames)16512105510841219241uL);
				list3.Add((BuffNames)10626820509964641423uL);
				list3.Add((BuffNames)16341761339885008577uL);
				list3.Add((BuffNames)16113274642161440716uL);
				list3.Add((BuffNames)7244019685987188093uL);
				list3.Add((BuffNames)13910300093031145699uL);
				list3.Add((BuffNames)12415407305475427397uL);
				list3.Add((BuffNames)1358929223039794148uL);
				Libido = list3;
				List<BuffNames> list4 = new List<BuffNames>();
				list4.Add((BuffNames)12465744095023444500uL);
				list4.Add((BuffNames)16328498947224254690uL);
				list4.Add((BuffNames)9183286871710270222uL);
				list4.Add((BuffNames)4642255024717194076uL);
				list4.Add((BuffNames)3348796355468607868uL);
				list4.Add((BuffNames)3802937897211410794uL);
				list4.Add((BuffNames)6514717556347855497uL);
				list4.Add((BuffNames)16680654480998469135uL);
				list4.Add((BuffNames)14409719855781998876uL);
				list4.Add((BuffNames)8648471817152268810uL);
				STD = list4;
			}
		}

		protected static int DayTicks
		{
			get
			{
				return (int)Math.Floor(LifespanModifier * 53280f);
			}
		}

		protected static float LifespanModifier
		{
			get
			{
				uint value = 2u;
				try
				{
					OptionsModel.GetOptionSetting("AgingInterval", out value);
				}
				catch
				{
				}
				switch (value)
				{
				case 0u:
					return 0.1f;
				case 1u:
					return 0.2f;
				case 2u:
					return 0.4f;
				case 3u:
					return 0.7f;
				default:
					return 1f;
				}
			}
		}

		public CustomBuff(BuffData info)
			: base(info)
		{
		}

		public static BuffData GetDefaultBuffData()
		{
			BuffData buffData = new BuffData();
			buffData.mBuffName = "";
			buffData.mDescription = "";
			buffData.mHelpText = "";
			buffData.mThumbString = "";
			buffData.mTopic = "";
			buffData.mBuffCategory = BuffCategory.Social;
			buffData.mMoodletColor = MoodColor.Fair;
			buffData.mAxisEffected = MoodAxis.Happy;
			buffData.mSolveCommodity = CommodityKind.None;
			buffData.mSolveTime = 0f;
			buffData.mDelayTimer = 0f;
			buffData.mTimeoutSimMinutes = 0f;
			buffData.mEffectValue = 0;
			buffData.mFacialIdles = null;
			buffData.mIncreasedEffectivenessList = new List<SocialManager.SocialEffectiveness>();
			buffData.mReducedEffectivenessList = new List<SocialManager.SocialEffectiveness>();
			buffData.mPolarityOverride = Polarity.NoOverride;
			buffData.mProductVersion = ProductVersion.BaseGame;
			buffData.mDisallowedOccults = new List<OccultTypes>();
			buffData.mAvailabilityFlags = CASAGSAvailabilityFlags.All;
			buffData.mNeededTraitList = new List<TraitNames>();
			buffData.mVersion = 1.0;
			return buffData;
		}

		public static BuffData GetBuffData(BuffNames buff)
		{
			BuffData defaultBuffData = GetDefaultBuffData();
			defaultBuffData.mBuffGuid = buff;
			switch (buff)
			{
			case (BuffNames)2111502432315482727uL:
				defaultBuffData.mBuffName = "lHadWoohoo";
				defaultBuffData.mDescription = "dHadWoohoo";
				defaultBuffData.mHelpText = "hHadWoohoo";
				defaultBuffData.mThumbString = "moodlet_woohoo_s";
				defaultBuffData.mBuffCategory = BuffCategory.Social;
				defaultBuffData.mMoodletColor = MoodColor.Good;
				defaultBuffData.mAxisEffected = MoodAxis.Happy;
				defaultBuffData.mTimeoutSimMinutes = 720f;
				defaultBuffData.mEffectValue = 15;
				break;
			case (BuffNames)9944098001884692765uL:
				defaultBuffData.mBuffName = "lHadFirstWoohoo";
				defaultBuffData.mDescription = "dHadFirstWoohoo";
				defaultBuffData.mHelpText = "hHadFirstWoohoo";
				defaultBuffData.mThumbString = "moodlet_firstwoohoo_s";
				defaultBuffData.mBuffCategory = BuffCategory.Social;
				defaultBuffData.mMoodletColor = MoodColor.ReallyGood;
				defaultBuffData.mAxisEffected = MoodAxis.Happy;
				defaultBuffData.mTimeoutSimMinutes = 10080f;
				defaultBuffData.mEffectValue = 100;
				break;
			case (BuffNames)8185339104921261200uL:
				defaultBuffData.mBuffName = "lLibidoSatisfied";
				defaultBuffData.mDescription = "dLibidoSatisfied";
				defaultBuffData.mHelpText = "hLibidoSatisfied";
				defaultBuffData.mThumbString = "moodlet_libido_Satisfied";
				defaultBuffData.mBuffCategory = BuffCategory.Social;
				defaultBuffData.mMoodletColor = MoodColor.Fair;
				defaultBuffData.mAxisEffected = MoodAxis.None;
				defaultBuffData.mPolarityOverride = Polarity.Neutral;
				defaultBuffData.mTimeoutSimMinutes = 4320f;
				defaultBuffData.mEffectValue = 0;
				break;
			case (BuffNames)16512105510841219241uL:
				defaultBuffData.mBuffName = "lLibidoSlightlyPositive";
				defaultBuffData.mDescription = "dLibidoSlightlyPositive";
				defaultBuffData.mHelpText = "hLibidoSlightlyPositive";
				defaultBuffData.mThumbString = "moodlet_libido_SlightlyPositive";
				defaultBuffData.mBuffCategory = BuffCategory.Social;
				defaultBuffData.mMoodletColor = MoodColor.Good;
				defaultBuffData.mAxisEffected = MoodAxis.Happy;
				defaultBuffData.mTimeoutSimMinutes = 1440f;
				defaultBuffData.mEffectValue = 5;
				break;
			case (BuffNames)10626820509964641423uL:
				defaultBuffData.mBuffName = "lLibidoModeratelyPositive";
				defaultBuffData.mDescription = "dLibidoModeratelyPositive";
				defaultBuffData.mHelpText = "hLibidoModeratelyPositive";
				defaultBuffData.mThumbString = "moodlet_libido_ModeratelyPositive";
				defaultBuffData.mBuffCategory = BuffCategory.Social;
				defaultBuffData.mMoodletColor = MoodColor.Good;
				defaultBuffData.mAxisEffected = MoodAxis.Happy;
				defaultBuffData.mTimeoutSimMinutes = 1440f;
				defaultBuffData.mEffectValue = 10;
				break;
			case (BuffNames)16341761339885008577uL:
				defaultBuffData.mBuffName = "lLibidoVeryPositive";
				defaultBuffData.mDescription = "dLibidoVeryPositive";
				defaultBuffData.mHelpText = "hLibidoVeryPositive";
				defaultBuffData.mThumbString = "moodlet_libido_VeryPositive";
				defaultBuffData.mBuffCategory = BuffCategory.Social;
				defaultBuffData.mMoodletColor = MoodColor.ReallyGood;
				defaultBuffData.mAxisEffected = MoodAxis.Happy;
				defaultBuffData.mTimeoutSimMinutes = 2880f;
				defaultBuffData.mEffectValue = 20;
				break;
			case (BuffNames)16113274642161440716uL:
				defaultBuffData.mBuffName = "lLibidoExtremelyPositive";
				defaultBuffData.mDescription = "dLibidoExtremelyPositive";
				defaultBuffData.mHelpText = "hLibidoExtremelyPositive";
				defaultBuffData.mThumbString = "moodlet_libido_Positive";
				defaultBuffData.mBuffCategory = BuffCategory.Social;
				defaultBuffData.mMoodletColor = MoodColor.Good;
				defaultBuffData.mAxisEffected = MoodAxis.Happy;
				defaultBuffData.mTimeoutSimMinutes = 4320f;
				defaultBuffData.mEffectValue = 40;
				break;
			case (BuffNames)7244019685987188093uL:
				defaultBuffData.mBuffName = "lLibidoSlightlyNegative";
				defaultBuffData.mDescription = "dLibidoSlightlyNegative";
				defaultBuffData.mHelpText = "hLibidoSlightlyNegative";
				defaultBuffData.mThumbString = "moodlet_libido_SlightlyNegative";
				defaultBuffData.mBuffCategory = BuffCategory.Physical;
				defaultBuffData.mMoodletColor = MoodColor.Bad;
				defaultBuffData.mAxisEffected = MoodAxis.Stressed;
				defaultBuffData.mPolarityOverride = Polarity.Negative;
				defaultBuffData.mTimeoutSimMinutes = 1440f;
				defaultBuffData.mEffectValue = -5;
				break;
			case (BuffNames)13910300093031145699uL:
				defaultBuffData.mBuffName = "lLibidoModeratelyNegative";
				defaultBuffData.mDescription = "dLibidoModeratelyNegative";
				defaultBuffData.mHelpText = "hLibidoModeratelyNegative";
				defaultBuffData.mThumbString = "moodlet_libido_ModeratelyNegative";
				defaultBuffData.mBuffCategory = BuffCategory.Physical;
				defaultBuffData.mMoodletColor = MoodColor.Bad;
				defaultBuffData.mAxisEffected = MoodAxis.Stressed;
				defaultBuffData.mPolarityOverride = Polarity.Negative;
				defaultBuffData.mTimeoutSimMinutes = 2880f;
				defaultBuffData.mEffectValue = -10;
				break;
			case (BuffNames)12415407305475427397uL:
				defaultBuffData.mBuffName = "lLibidoVeryNegative";
				defaultBuffData.mDescription = "dLibidoVeryNegative";
				defaultBuffData.mHelpText = "hLibidoVeryNegative";
				defaultBuffData.mThumbString = "moodlet_libido_VeryNegative";
				defaultBuffData.mBuffCategory = BuffCategory.Physical;
				defaultBuffData.mMoodletColor = MoodColor.ReallyBad;
				defaultBuffData.mAxisEffected = MoodAxis.Stressed;
				defaultBuffData.mPolarityOverride = Polarity.Negative;
				defaultBuffData.mTimeoutSimMinutes = 4320f;
				defaultBuffData.mEffectValue = -20;
				break;
			case (BuffNames)1358929223039794148uL:
				defaultBuffData.mBuffName = "lLibidoExtremelyNegative";
				defaultBuffData.mDescription = "dLibidoExtremelyNegative";
				defaultBuffData.mHelpText = "hLibidoExtremelyNegative";
				defaultBuffData.mThumbString = "moodlet_libido_negative";
				defaultBuffData.mBuffCategory = BuffCategory.Physical;
				defaultBuffData.mMoodletColor = MoodColor.ReallyBad;
				defaultBuffData.mAxisEffected = MoodAxis.Stressed;
				defaultBuffData.mPolarityOverride = Polarity.Negative;
				defaultBuffData.mTimeoutSimMinutes = -1f;
				defaultBuffData.mEffectValue = -40;
				break;
			case (BuffNames)12465744095023444500uL:
				defaultBuffData.mBuffName = "lSTDSimydia";
				defaultBuffData.mDescription = "dSTDSimydia";
				defaultBuffData.mHelpText = "hSTDSimydia";
				defaultBuffData.mThumbString = "moodlet_std_simydia";
				defaultBuffData.mBuffCategory = BuffCategory.Physical;
				defaultBuffData.mMoodletColor = MoodColor.Bad;
				defaultBuffData.mAxisEffected = MoodAxis.Uncomfortable;
				defaultBuffData.mPolarityOverride = Polarity.Negative;
				defaultBuffData.mTimeoutSimMinutes = 20160f;
				defaultBuffData.mEffectValue = -20;
				break;
			case (BuffNames)16328498947224254690uL:
				defaultBuffData.mBuffName = "lSTDSimnorrhea";
				defaultBuffData.mDescription = "dSTDSimnorrhea";
				defaultBuffData.mHelpText = "hSTDSimnorrhea";
				defaultBuffData.mThumbString = "moodlet_std_simnorrhea";
				defaultBuffData.mBuffCategory = BuffCategory.Physical;
				defaultBuffData.mMoodletColor = MoodColor.Bad;
				defaultBuffData.mAxisEffected = MoodAxis.Uncomfortable;
				defaultBuffData.mPolarityOverride = Polarity.Negative;
				defaultBuffData.mTimeoutSimMinutes = 20160f;
				defaultBuffData.mEffectValue = -20;
				break;
			case (BuffNames)9183286871710270222uL:
				defaultBuffData.mBuffName = "lSTDSimphilis";
				defaultBuffData.mDescription = "dSTDSimphilis";
				defaultBuffData.mHelpText = "hSTDSimphilis";
				defaultBuffData.mThumbString = "moodlet_std_simphilis";
				defaultBuffData.mBuffCategory = BuffCategory.Mental;
				defaultBuffData.mMoodletColor = MoodColor.Bad;
				defaultBuffData.mAxisEffected = MoodAxis.Uncomfortable;
				defaultBuffData.mPolarityOverride = Polarity.Negative;
				defaultBuffData.mTimeoutSimMinutes = 20160f;
				defaultBuffData.mEffectValue = -5;
				break;
			case (BuffNames)4642255024717194076uL:
				defaultBuffData.mBuffName = "lSTDSimphilisSecondary";
				defaultBuffData.mDescription = "dSTDSimphilisSecondary";
				defaultBuffData.mHelpText = "hSTDSimphilisSecondary";
				defaultBuffData.mThumbString = "moodlet_std_simphilis";
				defaultBuffData.mBuffCategory = BuffCategory.Physical;
				defaultBuffData.mMoodletColor = MoodColor.ReallyBad;
				defaultBuffData.mAxisEffected = MoodAxis.Uncomfortable;
				defaultBuffData.mPolarityOverride = Polarity.Negative;
				defaultBuffData.mTimeoutSimMinutes = 20160f;
				defaultBuffData.mEffectValue = -20;
				break;
			case (BuffNames)3348796355468607868uL:
				defaultBuffData.mBuffName = "lSTDSimphilisTertiary";
				defaultBuffData.mDescription = "dSTDSimphilisTertiary";
				defaultBuffData.mHelpText = "hSTDSimphilisTertiary";
				defaultBuffData.mThumbString = "moodlet_std_simphilis";
				defaultBuffData.mBuffCategory = BuffCategory.Physical;
				defaultBuffData.mMoodletColor = MoodColor.ReallyBad;
				defaultBuffData.mAxisEffected = MoodAxis.Uncomfortable;
				defaultBuffData.mPolarityOverride = Polarity.Negative;
				defaultBuffData.mTimeoutSimMinutes = -1f;
				defaultBuffData.mEffectValue = -50;
				break;
			case (BuffNames)7014284627137684376uL:
				defaultBuffData.mBuffName = "lSTDSimphilisPermanent";
				defaultBuffData.mDescription = "dSTDSimphilisPermanent";
				defaultBuffData.mHelpText = "hSTDSimphilisPermanent";
				defaultBuffData.mThumbString = "moodlet_std_simphilis";
				defaultBuffData.mBuffCategory = BuffCategory.Physical;
				defaultBuffData.mMoodletColor = MoodColor.Bad;
				defaultBuffData.mAxisEffected = MoodAxis.Uncomfortable;
				defaultBuffData.mPolarityOverride = Polarity.Negative;
				defaultBuffData.mTimeoutSimMinutes = -1f;
				defaultBuffData.mEffectValue = -25;
				break;
			case (BuffNames)3802937897211410794uL:
				defaultBuffData.mBuffName = "lSTDSIV";
				defaultBuffData.mDescription = "dSTDSIV";
				defaultBuffData.mHelpText = "hSTDSIV";
				defaultBuffData.mThumbString = "moodlet_std_siv";
				defaultBuffData.mBuffCategory = BuffCategory.Mental;
				defaultBuffData.mMoodletColor = MoodColor.Bad;
				defaultBuffData.mAxisEffected = MoodAxis.Uncomfortable;
				defaultBuffData.mPolarityOverride = Polarity.Negative;
				defaultBuffData.mTimeoutSimMinutes = -1f;
				defaultBuffData.mEffectValue = -5;
				break;
			case (BuffNames)16680654480998469135uL:
				defaultBuffData.mBuffName = "lSTDSIVComplications";
				defaultBuffData.mDescription = "dSTDSIVComplications";
				defaultBuffData.mHelpText = "hSTDSIVComplications";
				defaultBuffData.mThumbString = "moodlet_std_siv";
				defaultBuffData.mBuffCategory = BuffCategory.Physical;
				defaultBuffData.mMoodletColor = MoodColor.Bad;
				defaultBuffData.mAxisEffected = MoodAxis.Uncomfortable;
				defaultBuffData.mPolarityOverride = Polarity.Negative;
				defaultBuffData.mTimeoutSimMinutes = -1f;
				defaultBuffData.mEffectValue = -50;
				break;
			case (BuffNames)6514717556347855497uL:
				defaultBuffData.mBuffName = "lSTDSIVManaged";
				defaultBuffData.mDescription = "dSTDSIVManaged";
				defaultBuffData.mHelpText = "hSTDSIVManaged";
				defaultBuffData.mThumbString = "moodlet_std_siv";
				defaultBuffData.mBuffCategory = BuffCategory.Physical;
				defaultBuffData.mMoodletColor = MoodColor.Fair;
				defaultBuffData.mAxisEffected = MoodAxis.Uncomfortable;
				defaultBuffData.mPolarityOverride = Polarity.Negative;
				defaultBuffData.mTimeoutSimMinutes = 43200f;
				defaultBuffData.mEffectValue = 0;
				break;
			case (BuffNames)14409719855781998876uL:
				defaultBuffData.mBuffName = "lSTDSimerpes";
				defaultBuffData.mDescription = "dSTDSimerpes";
				defaultBuffData.mHelpText = "hSTDSimerpes";
				defaultBuffData.mThumbString = "moodlet_std_simerpes";
				defaultBuffData.mBuffCategory = BuffCategory.Physical;
				defaultBuffData.mMoodletColor = MoodColor.Bad;
				defaultBuffData.mAxisEffected = MoodAxis.Uncomfortable;
				defaultBuffData.mPolarityOverride = Polarity.Negative;
				defaultBuffData.mTimeoutSimMinutes = 7200f;
				defaultBuffData.mEffectValue = -15;
				break;
			case (BuffNames)8648471817152268810uL:
				defaultBuffData.mBuffName = "lSTDSimerpesMild";
				defaultBuffData.mDescription = "dSTDSimerpesMild";
				defaultBuffData.mHelpText = "hSTDSimerpesMild";
				defaultBuffData.mThumbString = "moodlet_std_simerpes";
				defaultBuffData.mBuffCategory = BuffCategory.Physical;
				defaultBuffData.mMoodletColor = MoodColor.Bad;
				defaultBuffData.mAxisEffected = MoodAxis.Uncomfortable;
				defaultBuffData.mPolarityOverride = Polarity.Negative;
				defaultBuffData.mTimeoutSimMinutes = 4320f;
				defaultBuffData.mEffectValue = -5;
				break;
			}
			defaultBuffData.mThumbKey = ResourceKey.CreatePNGKey(defaultBuffData.mThumbString, 0u);
			defaultBuffData.mJazzStateSuffix = defaultBuffData.mBuffName;
			return defaultBuffData;
		}

		public static void Load(ResourceKey[] resourceKeys)
		{
			Load();
		}

		public static void Load()
		{
			try
			{
				foreach (BuffNames item in Names.List)
				{
					if (item != BuffNames.Undefined)
					{
						BuffData buffData = GetBuffData(item);
						Buff buff = (Names.Libido.Contains(item) ? new Libido(buffData) : ((!Names.STD.Contains(item)) ? new CustomBuff(buffData) : new STD(buffData)));
						Load(buff, buffData);
					}
				}
			}
			catch
			{
			}
		}

		public static void AddInteractions()
		{
			Hospital[] objects = Sims3.Gameplay.Queries.GetObjects<Hospital>();
			foreach (Hospital hospital in objects)
			{
				if (hospital != null)
				{
					hospital.AddInteraction(STD.GetTreated.Singleton, true);
				}
			}
			ScienceLab[] objects2 = Sims3.Gameplay.Queries.GetObjects<ScienceLab>();
			foreach (ScienceLab scienceLab in objects2)
			{
				if (scienceLab != null)
				{
					scienceLab.AddInteraction(STD.GetTreated.Singleton, true);
				}
			}
			ComboHospitalScienceLab[] objects3 = Sims3.Gameplay.Queries.GetObjects<ComboHospitalScienceLab>();
			foreach (ComboHospitalScienceLab comboHospitalScienceLab in objects3)
			{
				if (comboHospitalScienceLab == null)
				{
					continue;
				}
				foreach (KeyValuePair<RabbitHole, string> containedRabbithole in comboHospitalScienceLab.ContainedRabbitholes)
				{
					if (containedRabbithole.Key is Hospital || containedRabbithole.Key is ScienceLab)
					{
						containedRabbithole.Key.AddInteraction(STD.GetTreated.Singleton, true);
					}
				}
			}
		}

		public static void Load(Buff buff, BuffData info)
		{
			BuffInstance value = new BuffInstance(buff, info.mBuffGuid, info.mEffectValue, info.mTimeoutSimMinutes);
			if (GenericManager<BuffNames, BuffInstance, BuffInstance>.sDictionary.ContainsKey((ulong)info.mBuffGuid))
			{
				GenericManager<BuffNames, BuffInstance, BuffInstance>.sDictionary[(ulong)info.mBuffGuid] = value;
			}
			else
			{
				GenericManager<BuffNames, BuffInstance, BuffInstance>.sDictionary.Add((ulong)info.mBuffGuid, value);
			}
		}

		public override void OnAddition(BuffManager bm, BuffInstance bi, bool travelReaddition)
		{
		}

		public override void OnPostAddition(BuffManager bm, BuffInstance bi)
		{
		}

		public override bool OnReAddition(BuffManager bm, BuffInstance existingBuff, BuffInstance newBuff, BuffNames guid, Origin origin, bool timeoutPaused)
		{
			return true;
		}

		public override void OnRemoval(BuffManager bm, BuffInstance bi)
		{
		}

		public override void OnTimeout(BuffManager bm, BuffInstance bi, OnTimeoutReasons reason)
		{
		}
	}
}
