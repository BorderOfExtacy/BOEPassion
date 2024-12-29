using System;
using System.Collections.Generic;
using System.Text;
using Sims3.Gameplay;
using Sims3.Gameplay.Abstracts;
using Sims3.Gameplay.Actors;
using Sims3.Gameplay.ActorSystems;
using Sims3.Gameplay.CAS;
using Sims3.Gameplay.Core;
using Sims3.Gameplay.EventSystem;
using Sims3.Gameplay.Interfaces;
using Sims3.Gameplay.Objects;
using Sims3.Gameplay.Objects.Appliances;
using Sims3.Gameplay.Objects.Beds;
using Sims3.Gameplay.Objects.Counters;
using Sims3.Gameplay.Objects.Decorations;
using Sims3.Gameplay.Objects.Entertainment;
using Sims3.Gameplay.Objects.Environment;
using Sims3.Gameplay.Objects.HobbiesSkills;
using Sims3.Gameplay.Objects.Miscellaneous;
using Sims3.Gameplay.Objects.Plumbing;
using Sims3.Gameplay.Objects.Seating;
using Sims3.Gameplay.Objects.Tables;
using Sims3.Gameplay.Objects.Toys;
using Sims3.Gameplay.Objects.Vehicles;
using Sims3.Gameplay.Utilities;
using Sims3.SimIFace;
using Sims3.SimIFace.CAS;
using Sims3.Store.Objects;
using Sims3.UI;
using Sims3.UI.CAS;

namespace S3_Passion
{
	public class PassionCommon
	{
		public class Modules
		{
			protected static List<IPassionModule> mLoaded = new List<IPassionModule>();

			public static List<IPassionModule> Loaded
			{
				get
				{
					return mLoaded;
				}
			}

			public static void Load(IPassionModule module)
			{
				if (!Loaded.Contains(module))
				{
					Loaded.Add(module);
				}
			}

			public static void Unload(IPassionModule module)
			{
				if (Loaded.Contains(module))
				{
					Loaded.Remove(module);
				}
			}

			public static void StartListeners()
			{
				foreach (IPassionModule item in mLoaded)
				{
					if (item != null)
					{
						item.StartListeners();
					}
				}
			}

			public static void Load(GameObject obj)
			{
				if (obj is Sim)
				{
					LoadSim(obj as Sim);
				}
				else
				{
					LoadObject(obj);
				}
			}

			public static void LoadSim(Sim sim)
			{
				foreach (IPassionModule item in mLoaded)
				{
					if (item != null)
					{
						item.LoadSim(sim);
					}
				}
			}

			public static void LoadObject(GameObject obj)
			{
				foreach (IPassionModule item in mLoaded)
				{
					if (item != null)
					{
						item.LoadObject(obj);
					}
				}
			}

			public static GameObject[] GetCustomObjects()
			{
				List<GameObject> list = new List<GameObject>();
				foreach (IPassionModule item in mLoaded)
				{
					if (item != null)
					{
						list.AddRange(item.GetCustomObjects());
					}
				}
				return list.ToArray();
			}

			public static Type[] GetCustomObjectTypes()
			{
				List<Type> list = new List<Type>();
				foreach (IPassionModule item in mLoaded)
				{
					if (item != null)
					{
						list.AddRange(item.GetCustomObjectTypes());
					}
				}
				return list.ToArray();
			}

			public static void PreProcessing(Sim sim)
			{
				foreach (IPassionModule item in mLoaded)
				{
					if (item != null)
					{
						item.PreProcessing(sim);
					}
				}
			}

			public static void LoopProcessing(Sim sim)
			{
				foreach (IPassionModule item in mLoaded)
				{
					if (item != null)
					{
						item.LoopProcessing(sim);
					}
				}
			}

			public static void PostProcessing(Sim sim)
			{
				if (sim == null)
				{
					return;
				}
				try
				{
					foreach (IPassionModule item in mLoaded)
					{
						if (item != null)
						{
							item.PostProcessing(sim);
						}
					}
				}
				catch
				{
				}
			}

			public static int GetPassionScoringModifier(Sim sim, Sim partner)
			{
				return GetPassionScoringModifier(sim, new Sim[1] { partner });
			}

			public static int GetPassionScoringModifier(Sim sim, Sim[] partners)
			{
				int num = 0;
				foreach (IPassionModule item in mLoaded)
				{
					if (item != null)
					{
						num += item.GetPassionScoringModifier(sim, partners);
					}
				}
				return num;
			}
		}

		public class SimPart
		{
			public class BlendValue
			{
				public FacialBlendData Blend;

				public float Value;

				public BlendValue(FacialBlendData blend, float value)
				{
					Blend = blend;
					Value = value;
				}
			}

			public class Types
			{
				public class Penis
				{
					public static readonly BlendValue[] BlankBlends = new BlendValue[2]
					{
						new BlendValue(ErectBlend, 0f),
						new BlendValue(FemaleErectBlend, 0f)
					};

					public const ulong Accessory = 328910682824383521uL;

					public static List<ulong> IDs;

					private static FacialBlendData mErectBlend;

					private static FacialBlendData mFemaleErectBlend;

					private static FacialBlendData mForeskinBlend;

					public static FacialBlendData ErectBlend
					{
						get
						{
							if (mErectBlend == null)
							{
								try
								{
									mErectBlend = new FacialBlendData(new BlendUnit(new ResourceKey(4287645100653294805uL, 3039776853u, 0u)));
								}
								catch
								{
								}
							}
							return mErectBlend;
						}
					}

					public static FacialBlendData FemaleErectBlend
					{
						get
						{
							if (mFemaleErectBlend == null)
							{
								try
								{
									mFemaleErectBlend = new FacialBlendData(new BlendUnit(new ResourceKey(4029370978840255872uL, 3039776853u, 0u)));
								}
								catch
								{
								}
							}
							return mFemaleErectBlend;
						}
					}

					public static FacialBlendData ForeskinBlend
					{
						get
						{
							if (mForeskinBlend == null)
							{
								try
								{
									mForeskinBlend = new FacialBlendData(new BlendUnit(new ResourceKey(17118777923511104575uL, 3039776853u, 0u)));
								}
								catch
								{
								}
							}
							return mForeskinBlend;
						}
					}

					public static bool ErectionLoaded
					{
						get
						{
							return ErectBlend != null || FemaleErectBlend != null;
						}
					}

					public static bool GetHard(Sim sim)
					{
						if (!ErectionLoaded)
						{
							return false;
						}
						float num = 0f;
						try
						{
							if (ForeskinBlend != null)
							{
								num = Get(sim, ForeskinBlend, sim.CurrentOutfitCategory, sim.CurrentOutfitIndex);
							}
							Set(sim, new BlendValue[2]
							{
								new BlendValue(ErectBlend, 1f - num),
								new BlendValue(FemaleErectBlend, num)
							}, sim.CurrentOutfitCategory, sim.CurrentOutfitIndex);
						}
						catch
						{
						}
						return sim.RefreshCurrentOutfit(false);
					}

					public static bool GetSoft(Sim sim)
					{
						if (!ErectionLoaded)
						{
							return false;
						}
						try
						{
							Set(sim, BlankBlends, sim.CurrentOutfitCategory, sim.CurrentOutfitIndex);
						}
						catch
						{
						}
						return sim.RefreshCurrentOutfit(false);
					}

					private static float Get(Sim sim, FacialBlendData blend, OutfitCategories cat, int ind)
					{
						float num = 0f;
						try
						{
							SimOutfit outfit = sim.SimDescription.GetOutfit(cat, ind);
							if (outfit != null)
							{
								int num2 = ((!blend.mBidirectional) ? 1 : 2);
								SimOutfit.BlendInfo[] blends = outfit.Blends;
								for (int i = 0; i < blends.Length; i++)
								{
									SimOutfit.BlendInfo blendInfo = blends[i];
									if (blends[i].key == blend.mBlend1.GetKey())
									{
										num += blends[i].amount;
										if (--num2 == 0)
										{
											break;
										}
									}
									else if (blend.mBidirectional && blends[i].key == blend.mBlend2.GetKey())
									{
										num -= blendInfo.amount;
										if (--num2 == 0)
										{
											break;
										}
									}
								}
							}
						}
						catch
						{
						}
						return num;
					}

					// I THINK THESE ARE CAS PARTS???????
					static Penis()
					{
						List<ulong> list = new List<ulong>();
						list.Add(328910682824383521uL);
						list.Add(12852463336891105766uL);
						list.Add(2551694397489823400uL);
						list.Add(5317619880426391662uL);
						list.Add(14409094506079692417uL);
						list.Add(16989481929118467994uL);
						list.Add(6288287485492957398uL);
						list.Add(6611609753548546243uL);
						list.Add(1906960305524283416uL);
						list.Add(10829650939803730647uL);
						list.Add(8828424442779924380uL);
						IDs = list;
						mErectBlend = null;
						mFemaleErectBlend = null;
						mForeskinBlend = null;
					}
				}

				public static class Cigarette
				{
					public const ulong Hand = 1113075501344031715uL;

					public const ulong Mouth = 4946428095644385211uL;

					public static List<ulong> IDs;

					public static bool Has(Sim sim)
					{
						if (sim != null)
						{
							CASPart[] parts = sim.CurrentOutfit.Parts;
							for (int i = 0; i < parts.Length; i++)
							{
								CASPart cASPart = parts[i];
								if (IDs.Contains(cASPart.Key.InstanceId))
								{
									return true;
								}
							}
						}
						return false;
					}

					static Cigarette()
					{
						List<ulong> list = new List<ulong>();
						list.Add(1113075501344031715uL);
						list.Add(4946428095644385211uL);
						IDs = list;
					}
				}

				public class Height
				{
					public const ulong ID = 18006099479972776143uL;

					public static List<ulong> IDs;

					public static float HeightMultiplier;

					protected static FacialBlendData mBlend;

					public static FacialBlendData Blend
					{
						get
						{
							if (mBlend == null)
							{
								try
								{
									mBlend = new FacialBlendData(new BlendUnit(ResourceKey.FromString("FBLN 0xB52F5055-0x00000000-0xF9E284139E1F88CF")));
								}
								catch
								{
								}
							}
							return mBlend;
						}
					}

					public static float TeenMorph(Sim sim)
					{
						float result = 0f;
						int outfitCount = sim.SimDescription.GetOutfitCount(sim.CurrentOutfitCategory);
						if (sim.CurrentOutfitCategory == OutfitCategories.Naked)
						{
							for (int i = 0; i < outfitCount; i++)
							{
								TeenMorph(sim, Blend, HeightMultiplier);
							}
						}
						if (sim.SimDescription.Teen)
						{
							result = HeightMultiplier;
						}
						return result;
					}

					public static void TeenMorph(Sim sim, FacialBlendData blend, float value)
					{
						foreach (object key in sim.SimDescription.GetCurrentOutfits().Keys)
						{
							OutfitCategories cat = (OutfitCategories)key;
							TeenMorph(sim, blend, value, cat);
						}
					}

					public static void TeenMorph(Sim sim, FacialBlendData blend, float value, OutfitCategories cat)
					{
						int outfitCount = sim.SimDescription.GetOutfitCount(cat);
						for (int i = 0; i < outfitCount; i++)
						{
							TeenMorph(sim, blend, value, cat, i);
						}
					}

					public static void TeenMorph(Sim sim, FacialBlendData blend, float value, OutfitCategories cat, int ind)
					{
						SimBuilder simBuilder = new SimBuilder();
						SimOutfit outfit = sim.SimDescription.GetOutfit(cat, ind);
						if (outfit != null)
						{
							OutfitUtils.SetOutfit(simBuilder, outfit, sim.SimDescription);
							simBuilder.SetFacialBlend(blend.mBlend1.GetKey(), Math.Max(value, 0f));
							if (blend.mBidirectional)
							{
								simBuilder.SetFacialBlend(blend.mBlend2.GetKey(), Math.Abs(Math.Min(value, 0f)));
							}
							string instanceName = sim.FirstName + sim.LastName + cat.ToString() + ind;
							SimOutfit outfit2 = new SimOutfit(simBuilder.CacheOutfit(instanceName));
							sim.SimDescription.RemoveOutfit(cat, ind, true);
							sim.SimDescription.AddOutfit(outfit2, cat, ind);
						}
					}

					public static float GetModifier(Sim sim)
					{
						float num = 0f;
						float num2 = 0f;
						if (sim != null && Blend != null)
						{
							try
							{
								SimOutfit outfit = sim.SimDescription.GetOutfit(OutfitCategories.Naked, 0);
								if (outfit != null && outfit.Blends != null)
								{
									SimOutfit.BlendInfo[] blends = outfit.Blends;
									for (int i = 0; i < blends.Length; i++)
									{
										SimOutfit.BlendInfo blendInfo = blends[i];
										if (blendInfo.key == Blend.mBlend1.GetKey())
										{
											num += blendInfo.amount;
										}
										else if (Blend.mBidirectional && blendInfo.key == Blend.mBlend2.GetKey())
										{
											num -= blendInfo.amount;
										}
									}
								}
								else
								{
									outfit = sim.SimDescription.GetOutfit(sim.CurrentOutfitCategory, sim.CurrentOutfitIndex);
									if (outfit != null && outfit.Blends != null)
									{
										SimOutfit.BlendInfo[] blends2 = outfit.Blends;
										for (int j = 0; j < blends2.Length; j++)
										{
											SimOutfit.BlendInfo blendInfo2 = blends2[j];
											if (blendInfo2.key == Blend.mBlend1.GetKey())
											{
												num += blendInfo2.amount;
											}
											else if (Blend.mBidirectional && blendInfo2.key == Blend.mBlend2.GetKey())
											{
												num -= blendInfo2.amount;
											}
										}
									}
								}
							}
							catch
							{
							}
						}
						if (sim.SimDescription.Teen)
						{
							num2 = HeightMultiplier;
						}
						if (num2 < 0f)
						{
							num2 = 0f;
						}
						return num2;
					}

					public static float GetMultiplier(Sim sim)
					{
						float num = 0f;
						float num2 = 0.1f;
						if (sim != null && Blend != null)
						{
							try
							{
								SimOutfit outfit = sim.SimDescription.GetOutfit(OutfitCategories.Naked, 0);
								if (outfit != null && outfit.Blends != null)
								{
									SimOutfit.BlendInfo[] blends = outfit.Blends;
									for (int i = 0; i < blends.Length; i++)
									{
										SimOutfit.BlendInfo blendInfo = blends[i];
										if (blendInfo.key == Blend.mBlend1.GetKey())
										{
											num += blendInfo.amount;
										}
										else if (Blend.mBidirectional && blendInfo.key == Blend.mBlend2.GetKey())
										{
											num -= blendInfo.amount;
										}
									}
								}
								else
								{
									outfit = sim.SimDescription.GetOutfit(sim.CurrentOutfitCategory, sim.CurrentOutfitIndex);
									if (outfit != null && outfit.Blends != null)
									{
										SimOutfit.BlendInfo[] blends2 = outfit.Blends;
										for (int j = 0; j < blends2.Length; j++)
										{
											SimOutfit.BlendInfo blendInfo2 = blends2[j];
											if (blendInfo2.key == Blend.mBlend1.GetKey())
											{
												num += blendInfo2.amount;
											}
											else if (Blend.mBidirectional && blendInfo2.key == Blend.mBlend2.GetKey())
											{
												num -= blendInfo2.amount;
											}
										}
									}
								}
							}
							catch
							{
							}
						}
						return num2 + num * (0f - HeightMultiplier);
					}

					public static Vector3 ApplyMultipliers(Sim sim, Vector3 variance)
					{
						return ApplyMultipliers(sim, variance, false, true, false);
					}

					public static Vector3 ApplyMultipliers(Sim sim, Vector3 variance, bool applyX, bool applyY, bool applyZ)
					{
						float multiplier = GetMultiplier(sim);
						Vector3 vector = variance;
						if (applyX)
						{
							vector.x *= multiplier;
						}
						if (applyY)
						{
							vector.y *= multiplier;
						}
						if (applyZ)
						{
							vector.z *= multiplier;
						}
						return variance;
					}

					static Height()
					{
						List<ulong> list = new List<ulong>();
						list.Add(18006099479972776143uL);
						IDs = list;
						HeightMultiplier = 0.256f;
					}
				}
			}

			public const uint FBLN = 3039776853u;

			public static List<ulong> Height = Types.Height.IDs;

			public static List<ulong> Penis = Types.Penis.IDs;

			public static List<ulong> Cigarette = Types.Cigarette.IDs;

			private static float Get(Sim sim, FacialBlendData blend, OutfitCategories cat, int ind)
			{
				float num = 0f;
				try
				{
					SimOutfit outfit = sim.SimDescription.GetOutfit(cat, ind);
					if (outfit != null)
					{
						int num2 = ((!blend.mBidirectional) ? 1 : 2);
						SimOutfit.BlendInfo[] blends = outfit.Blends;
						for (int i = 0; i < blends.Length; i++)
						{
							SimOutfit.BlendInfo blendInfo = blends[i];
							if (blends[i].key == blend.mBlend1.GetKey())
							{
								num += blends[i].amount;
								if (--num2 == 0)
								{
									break;
								}
							}
							else if (blend.mBidirectional && blends[i].key == blend.mBlend2.GetKey())
							{
								num -= blendInfo.amount;
								if (--num2 == 0)
								{
									break;
								}
							}
						}
					}
				}
				catch
				{
				}
				return num;
			}

			private static void Set(Sim sim, FacialBlendData blend, float value, OutfitCategories category, int index)
			{
				Set(sim, new BlendValue[1]
				{
					new BlendValue(blend, value)
				}, category, index);
			}

			private static void Set(Sim sim, BlendValue[] items, OutfitCategories category, int index)
			{
				try
				{
					if (sim == null || items == null || items.Length == 0)
					{
						return;
					}
					SimBuilder simBuilder = new SimBuilder();
					SimOutfit outfit = sim.SimDescription.GetOutfit(category, index);
					if (outfit == null)
					{
						return;
					}
					OutfitUtils.SetOutfit(simBuilder, outfit, sim.SimDescription);
					for (int i = 0; i < items.Length; i++)
					{
						if (items[i] != null)
						{
							simBuilder.SetFacialBlend(items[i].Blend.mBlend1.GetKey(), Math.Max(items[i].Value, 0f));
							if (items[i].Blend.mBidirectional)
							{
								simBuilder.SetFacialBlend(items[i].Blend.mBlend2.GetKey(), Math.Abs(Math.Min(items[i].Value, 0f)));
							}
						}
					}
					SimOutfit outfit2 = new SimOutfit(simBuilder.CacheOutfit(sim.FirstName + sim.LastName + category.ToString() + index));
					sim.SimDescription.RemoveOutfit(category, index, true);
					sim.SimDescription.AddOutfit(outfit2, category, index);
				}
				catch
				{
				}
			}
		}

		public static bool Testing = true;

		public static string Version = "2.7.6.7";

		public const bool ForceLoad = true;

		public const bool AlwaysList = true;

		public const bool KeepSequence = true;

		public const int TicksPerMinute = 37;

		public const long InvalidTime = 0L;

		public const float PassionRange = 5f;

		public const float Plus = 8f;

		public const float DoublePlus = 15f;

		public const float Minus = -8f;

		public const float DoubleMinus = -15f;

		public const ulong GloryHoleNameKey = 5329506766876672555uL;

		public const ulong StripperPoleNameKey = 13788670039724095860uL;

		public const ulong PrimoCoffeeTableNameKey = 15946527783677180907uL;

		public const ulong AltaraCoffeeTableNameKey = 2598734301466468606uL;

		public static readonly string NewLine = System.Environment.NewLine;

		public static readonly string X = "x";

		public static readonly List<string> DefaultXMLFiles;

		public static readonly List<ReactionTypes> PassionReactions;

		public static readonly List<Type> PreferredTypes;

		public static readonly TraitNames[] PassionFriendlyTraits;

		protected static List<string> mBufferedMessages;

		protected static string mBufferedMessageChunk;

		public static ReactionTypes RandomReaction
		{
			get
			{
				return RandomUtil.GetRandomObjectFromList(PassionReactions);
			}
		}

		protected static List<string> BufferedMessages
		{
			get
			{
				if (mBufferedMessages == null)
				{
					mBufferedMessages = new List<string>();
				}
				return mBufferedMessages;
			}
		}

		public static bool MessageBufferEmpty
		{
			get
			{
				return string.IsNullOrEmpty(mBufferedMessageChunk);
			}
		}

		public static void Wait()
		{
			Wait(1u);
		}

		public static void Wait(int least, int greatest)
		{
			Wait(RandomUtil.GetInt(least, greatest));
		}

		public static void Wait(int pause)
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

		public static void Wait(uint pause)
		{
			try
			{
				Simulator.Sleep(pause);
			}
			catch
			{
			}
		}

		public static bool Match(RandomizationOptions x, RandomizationOptions y)
		{
			return Match((int)x, (int)y);
		}

		public static bool Match(int x, int y)
		{
			return (x & y) > 0;
		}

		public static bool Match(uint x, uint y)
		{
			return (x & y) != 0;
		}

		public static long MinutesToTicks(int minutes)
		{
			return minutes * 37;
		}

		public static int TicksToMinutes(long ticks)
		{
			return Convert.ToInt32(ticks / 37);
		}

		public static bool Bool(string text)
		{
			if (!string.IsNullOrEmpty(text))
			{
				switch (text.ToLower())
				{
				case "enabled":
				case "true":
				case "yes":
				case "1":
					return true;
				default:
					return false;
				}
			}
			return false;
		}

		public static int Int(string text)
		{
			int result;
			int.TryParse(text, out result);
			return result;
		}

		public static uint UInt(string text)
		{
			uint result;
			uint.TryParse(text, out result);
			return result;
		}

		public static long Long(string text)
		{
			long result;
			long.TryParse(text, out result);
			return result;
		}

		public static ulong ULong(string text)
		{
			ulong result;
			ulong.TryParse(text, out result);
			return result;
		}

		public static float Float(string text)
		{
			float result;
			float.TryParse(text, out result);
			return result;
		}

		public static float GetDistanceBetween(IGameObject obj, Vector3 point)
		{
			return (obj != null) ? GetDistanceBetween(obj.Position, point) : float.PositiveInfinity;
		}

		public static float GetDistanceBetween(Vector3 a, Vector3 b)
		{
			return (a - b).Length();
		}

		public static string Localize(string key)
		{
			string text = null;
			try
			{
				if (!string.IsNullOrEmpty(key))
				{
					text = Localization.LocalizeString(key);
				}
			}
			catch
			{
				return key;
			}
			if (!string.IsNullOrEmpty(text))
			{
				return text;
			}
			return key;
		}

		public static string Localize(string key, Dictionary<string, string> replacers)
		{
			try
			{
				string text = Localize(key);
				if (!string.IsNullOrEmpty(text))
				{
					if (replacers.Count > 0)
					{
						StringBuilder stringBuilder = new StringBuilder(text);
						foreach (KeyValuePair<string, string> replacer in replacers)
						{
							if (!string.IsNullOrEmpty(replacer.Key) && replacer.Value != null)
							{
								stringBuilder.Replace(replacer.Key, replacer.Value);
							}
						}
						return stringBuilder.ToString();
					}
					return text;
				}
				return key;
			}
			catch
			{
				return key;
			}
		}

		public static void AddModule(IPassionModule module)
		{
			Modules.Load(module);
		}

		public static void Message(string message, StyledNotification.NotificationStyle style, Sim speaker, Sim target)
		{
			if (speaker != null)
			{
				if (target != null)
				{
					StyledNotification.Show(new StyledNotification.Format(message, speaker.ObjectId, target.ObjectId, style));
				}
				else
				{
					StyledNotification.Show(new StyledNotification.Format(message, speaker.ObjectId, style));
				}
			}
			else
			{
				StyledNotification.Show(new StyledNotification.Format(message, style));
			}
		}

		public static void Message(string message)
		{
			Message(message, StyledNotification.NotificationStyle.kGameMessagePositive, null, null);
		}

		public static void NegativeMessage(string message)
		{
			Message(message, StyledNotification.NotificationStyle.kGameMessageNegative, null, null);
		}

		public static void SystemMessage(string message)
		{
			Message(message, StyledNotification.NotificationStyle.kSystemMessage, null, null);
		}

		public static void SimMessage(string message, Sim speaker, Sim target)
		{
			Message(message, StyledNotification.NotificationStyle.kSimTalking, speaker, target);
		}

		public static void SimMessage(string message, Sim speaker)
		{
			Message(message, StyledNotification.NotificationStyle.kSimTalking, speaker, null);
		}

		public static void BufferLine()
		{
			BufferLine(string.Empty);
		}

		public static void BufferLine(string line)
		{
			if (string.IsNullOrEmpty(mBufferedMessageChunk))
			{
				mBufferedMessageChunk = line;
			}
			else
			{
				mBufferedMessageChunk = mBufferedMessageChunk + NewLine + line;
			}
		}

		public static void BufferMessage(string message)
		{
			BufferedMessages.Add(message);
		}

		public static void BufferMessage()
		{
			BufferMessage(mBufferedMessageChunk);
			BufferClear();
		}

		public static void SimMessage(Sim speaker, Sim target)
		{
			SimMessage(mBufferedMessageChunk, speaker, target);
			BufferClear();
		}

		public static void SimMessage(Sim sim)
		{
			SimMessage(mBufferedMessageChunk, sim, null);
			BufferClear();
		}

		public static void SystemMessage()
		{
			SystemMessage(mBufferedMessageChunk);
			BufferClear();
		}

		public static void NegativeMessage()
		{
			NegativeMessage(mBufferedMessageChunk);
			BufferClear();
		}

		public static void Message()
		{
			Message(mBufferedMessageChunk);
			BufferClear();
		}

		public static void BufferClear()
		{
			mBufferedMessageChunk = string.Empty;
		}

		public static void DumpMessages()
		{
			if (BufferedMessages.Count <= 0)
			{
				return;
			}
			foreach (string bufferedMessage in BufferedMessages)
			{
				SystemMessage(bufferedMessage);
			}
			BufferedMessages.Clear();
		}

		public static bool HasPart(Sim sim, List<ulong> IDs)
		{
			if (sim != null)
			{
				CASPart[] parts = sim.CurrentOutfit.Parts;
				for (int i = 0; i < parts.Length; i++)
				{
					CASPart cASPart = parts[i];
					if (IDs.Contains(cASPart.Key.InstanceId))
					{
						return true;
					}
				}
			}
			return false;
		}

		public static void Impregnate(Sim mom, Sim dad)
		{
			if (mom.SimDescription.Household.LotHome != null && !mom.SimDescription.Household.IsTravelHousehold && !mom.SimDescription.Household.IsServobotHousehold && !mom.SimDescription.Household.IsServiceNpcHousehold && !mom.SimDescription.Household.IsMermaidHousehold && !mom.SimDescription.Household.IsPreviousTravelerHousehold && !mom.SimDescription.Household.IsAlienHousehold && !mom.SimDescription.Household.IsSpecialHousehold && !mom.SimDescription.Household.IsFutureDescendantHousehold && !mom.SimDescription.Household.IsTouristHousehold && !mom.SimDescription.IsBonehilda && !mom.SimDescription.IsZombie && !mom.SimDescription.IsRobot && !mom.SimDescription.IsEP11Bot && !mom.SimDescription.IsFrankenstein && !mom.SimDescription.IsGhost && !mom.SimDescription.IsImaginaryFriend && !mom.SimDescription.IsMummy && !mom.SimDescription.IsPlantSim && !mom.SimDescription.IsTimeTraveler && !mom.SimDescription.IsSupernaturalForm && !mom.SimDescription.IsPregnant && (mom.SimDescription.Teen || mom.SimDescription.YoungAdult || mom.SimDescription.Adult))
			{
				Pregnancy pregnancy = new Pregnancy(mom, dad.SimDescription);
				pregnancy.PreggersAlarm = mom.AddAlarmRepeating(1f, TimeUnit.Hours, pregnancy.HourlyCallback, 1f, TimeUnit.Hours, "Hourly Pregnancy Update Alarm", AlarmType.AlwaysPersisted);
				mom.SimDescription.Pregnancy = pregnancy;
				EventTracker.SendEvent(new PregnancyEvent(EventTypeId.kGotPregnant, mom, dad, pregnancy, null));
				Audio.StartSound("sting_baby_conception");
			}
		}

		public static void ApplyRandomMoodlet(List<BuffNames> buffs)
		{
			ApplyRandomMoodlets(buffs, 1, 1);
		}

		public static void ApplyRandomMoodlets(List<BuffNames> buffs, int min, int max)
		{
			if (buffs == null || buffs.Count <= 0)
			{
				return;
			}
			if (min < 0)
			{
				min = 0;
			}
			if (max < min)
			{
				max = min;
			}
			Sim[] globalObjects = Sims3.Gameplay.Queries.GetGlobalObjects<Sim>();
			foreach (Sim sim in globalObjects)
			{
				if (Passion.IsValid(sim))
				{
					ApplyRandomMoodlets(sim, buffs, min, max);
				}
			}
		}

		public static void ApplyRandomMoodlet(Sim sim, List<BuffNames> buffs)
		{
			ApplyRandomMoodlets(sim, buffs, 1, 1);
		}

		public static void ApplyRandomMoodlets(Sim sim, List<BuffNames> buffs, int min, int max)
		{
			try
			{
				if (sim == null || buffs == null || buffs.Count <= 0)
				{
					return;
				}
				int num = 0;
				float num2 = 0f;
				float num3 = 50 / buffs.Count;
				List<BuffNames> list = new List<BuffNames>();
				if (min < 0)
				{
					min = 0;
				}
				if (max < min)
				{
					max = min;
				}
				foreach (BuffNames buff in buffs)
				{
					num2 += num3;
					if (num >= max)
					{
						break;
					}
					if (!sim.BuffManager.HasElement(buff))
					{
						if (RandomUtil.RandomChance(num2))
						{
							sim.BuffManager.AddElement(buff, Origin.None);
							num++;
						}
						else
						{
							list.Add(buff);
						}
					}
				}
				if (num >= min)
				{
					return;
				}
				foreach (BuffNames item in list)
				{
					if (num < min)
					{
						sim.BuffManager.AddElement(item, Origin.None);
						num++;
						continue;
					}
					break;
				}
			}
			catch
			{
			}
		}

		public static void CleanMoodlets(List<BuffNames> buffs)
		{
			if (buffs != null && buffs.Count > 0)
			{
				Sim[] globalObjects = Sims3.Gameplay.Queries.GetGlobalObjects<Sim>();
				foreach (Sim sim in globalObjects)
				{
					CleanMoodlets(sim, buffs);
				}
			}
		}

		public static void CleanMoodlets(Sim sim, List<BuffNames> buffs)
		{
			try
			{
				if (sim == null || sim.HasBeenDestroyed || buffs == null || buffs.Count <= 0 || !sim.BuffManager.HasAnyElement(buffs.ToArray()))
				{
					return;
				}
				foreach (BuffNames buff in buffs)
				{
					sim.BuffManager.RemoveElement(buff);
				}
			}
			catch
			{
			}
		}

		static PassionCommon()
		{
			List<string> list = new List<string>();
			list.Add("KW_Extend_Animations_For_Passion");
			list.Add("AfterDuskSims");
			list.Add("Amra72_Animations");
			list.Add("Amra72_Lesbian_Animations");
			list.Add("FallinEsper_SexAnimations");
			list.Add("ghoct_SexAnimations");
			list.Add("HDSCreations");
			list.Add("JezaAnimations");
			list.Add("KIWanimations");
			list.Add("Lady666_Animations");
			list.Add("Lady666_Crazy_Animations");
			list.Add("Lady666_Kinky_Animations");
			list.Add("L666_Animations");
			list.Add("L666_RapeBdsm_Animations");
			list.Add("MasterAnimations");
			list.Add("SFanimations");
			list.Add("SMAnimationsAutoFel");
			list.Add("SMAnimations");
			list.Add("SMAnimationsLong");
			list.Add("WTWanimations");
			list.Add("KW_Amra72_Animations");
			list.Add("KW_Lady666_Animations");
			list.Add("KW_Lady666_Kinky_Animations");
			list.Add("KW_Lady666_Crazy_Animations");
			list.Add("KW_L666_Animations");
			list.Add("KW_L666_Kinky_Animations");
			list.Add("KW_L666_RapeBdsm_Animations");
			list.Add("KW_mike24_lesbians");
			list.Add("KW_mike24_classics");
			list.Add("KinkyWorldMasteranimations");
			list.Add("KW_MaryJane_Animations");
			list.Add("KW_Galgat_animations");
			list.Add("KW_K69_Animations");
			list.Add("KW_K69_Kinky_Animations");
			list.Add("KW_K69_Classic_Animations");
			list.Add("KW_Swanny_Animations");
			list.Add("KW_illusivek_animations");
			list.Add("Passion_Sybian_Saddle");
			list.Add("KinkyAnalustAnimations");
			list.Add("KW_Clydie_Anims");
			list.Add("KW_OLLCity_Anims");
			list.Add("KinkyLucasAnimations");
			DefaultXMLFiles = list;
			List<ReactionTypes> list2 = new List<ReactionTypes>();
			list2.Add(ReactionTypes.Fascinated);
			list2.Add(ReactionTypes.Giggle);
			list2.Add(ReactionTypes.Oooh);
			list2.Add(ReactionTypes.ViewLove);
			list2.Add(ReactionTypes.PumpFist);
			list2.Add(ReactionTypes.Cheer);
			PassionReactions = list2;
			List<Type> list3 = new List<Type>();
			list3.Add(typeof(Altar));
			list3.Add(typeof(Urnstone));
			list3.Add(typeof(HotTubGrotto));
			list3.Add(typeof(HotTubBase));
			list3.Add(typeof(Bed));
			list3.Add(typeof(BedSingle));
			list3.Add(typeof(BedDouble));
			list3.Add(typeof(CarExpensive2));
			list3.Add(typeof(CarExpensive1));
			list3.Add(typeof(CarHatchback));
			list3.Add(typeof(CarUsed1));
			list3.Add(typeof(CarUsed2));
			list3.Add(typeof(CarNormal1));
			list3.Add(typeof(CarVan4door));
			list3.Add(typeof(CarPickup2door));
			list3.Add(typeof(CarSedan));
			list3.Add(typeof(CarSports));
			list3.Add(typeof(CarHighSocietyOpen));
			list3.Add(typeof(CarHighSocietyVintage));
			list3.Add(typeof(CarLuxuryExotic));
			list3.Add(typeof(CarLuxurySport));
			list3.Add(typeof(DoorSingle));
			list3.Add(typeof(DanceFloor));
			list3.Add(typeof(SwingSet));
			list3.Add(typeof(FixerCar));
			list3.Add(typeof(FixerCar.FixerCarFixed));
			list3.Add(typeof(Bicycle));
			list3.Add(typeof(MotorcycleRacing));
			list3.Add(typeof(MotorcycleChopper));
			list3.Add(typeof(BoatSpeedBoat));
			list3.Add(typeof(BoatRowBoat));
			list3.Add(typeof(BoatSpeedFishingBoat));
			list3.Add(typeof(BoatWaterScooter));
			list3.Add(typeof(AdultMagicBroom));
			list3.Add(typeof(ModerateAdultBroom));
			list3.Add(typeof(ExpensiveAdultBroom));
			list3.Add(typeof(ChairLiving));
			list3.Add(typeof(ChairDining));
			list3.Add(typeof(ChairLounge));
			list3.Add(typeof(ChairSectional));
			list3.Add(typeof(Loveseat));
			list3.Add(typeof(Couch));
			list3.Add(typeof(Counter));
			list3.Add(typeof(Floor));
			list3.Add(typeof(Rug));
			list3.Add(typeof(PoolTable));
			list3.Add(typeof(Shower));
			list3.Add(typeof(ShowerOutdoor));
			list3.Add(typeof(Sofa));
			list3.Add(typeof(TableCoffee));
			list3.Add(typeof(TableDining));
			list3.Add(typeof(TableEnd));
			list3.Add(typeof(PicnicTable));
			list3.Add(typeof(ToiletStall));
			list3.Add(typeof(Fridge));
			list3.Add(typeof(WorkoutBench));
			list3.Add(typeof(KissingBooth));
			list3.Add(typeof(Telescope));
			list3.Add(typeof(Sims3.Gameplay.Objects.Environment.Scarecrow));
			list3.Add(typeof(HauntedHouse));
			list3.Add(typeof(ScienceResearchStation));
			list3.Add(typeof(HotTubBase));
			list3.Add(typeof(Podium));
			list3.Add(typeof(MechanicalBull));
			list3.Add(typeof(Sybian));
			list3.Add(typeof(Window));
			list3.Add(typeof(Windows));
			list3.Add(typeof(JungleGymLookOutTower));
			list3.Add(typeof(Igloo));
			list3.Add(typeof(ArcadeMachineBase));
			list3.Add(typeof(DollHouse));
			list3.Add(typeof(Bathtub));
			list3.Add(typeof(Trampoline));
			list3.Add(typeof(Toilet));
			list3.Add(typeof(Bar));
			list3.Add(typeof(BarProfessional));
			list3.Add(typeof(BarCandy));
			list3.Add(typeof(BalletBarre));
			list3.Add(typeof(HayBaleSquare));
			PreferredTypes = list3;
			PassionFriendlyTraits = new TraitNames[4]
			{
				TraitNames.Flirty,
				TraitNames.HopelessRomantic,
				TraitNames.MasterOfSeduction,
				TraitNames.Inappropriate
			};
			mBufferedMessages = new List<string>();
			mBufferedMessageChunk = string.Empty;
		}
	}
}
