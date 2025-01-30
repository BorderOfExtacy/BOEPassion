using Sims3.Gameplay.Actors;
using Sims3.Gameplay.ActorSystems;

namespace S3_Passion
{
	public class Libido : CustomBuff
	{
		public Libido(BuffData info)
			: base(info)
		{
		}

		public static bool Replace(BuffManager bm, BuffNames newBuff, Origin origin)
		{
			if (bm.Actor != null && Names.Libido.Contains(newBuff))
			{
				foreach (BuffNames item in Names.Libido)
				{
					bm.RemoveElement(item);
				}
				bm.AddElement(newBuff, origin);
				return true;
			}
			return false;
		}

		public static bool Urgency(Sim sim)
		{
			if (sim != null)
			{
				return Urgency(sim.BuffManager);
			}
			return false;
		}

		public static bool Urgency(BuffManager bm)
		{
			if (bm != null)
			{
				BuffNames name = BuffNames.Undefined;
				BuffNames buffNames = BuffNames.Undefined;
				foreach (BuffNames item in Names.Libido)
				{
					if (bm.HasElement(item))
					{
						name = item;
						break;
					}
				}
				buffNames = Urgency(name);
				return Replace(bm, buffNames, Origin.None);
			}
			return false;
		}

		public static bool Urgency(BuffManager bm, BuffInstance bi)
		{
			BuffNames newBuff = Urgency(bi);
			return Replace(bm, newBuff, Origin.None);
		}

		public static BuffNames Urgency(BuffInstance instance)
		{
			BuffNames buffGuid = (BuffNames)instance.BuffGuid;
			return Urgency(buffGuid);
		}

		public static BuffNames Urgency(BuffNames name)
		{
			switch (name)
			{
			case (BuffNames)16113274642161440716uL:
				return (BuffNames)10626820509964641423uL;
			case (BuffNames)16341761339885008577uL:
				return (BuffNames)16512105510841219241uL;
			case (BuffNames)10626820509964641423uL:
			case (BuffNames)16512105510841219241uL:
				return (BuffNames)8185339104921261200uL;
			case (BuffNames)8185339104921261200uL:
				return (BuffNames)7244019685987188093uL;
			case (BuffNames)7244019685987188093uL:
				return (BuffNames)13910300093031145699uL;
			case (BuffNames)13910300093031145699uL:
				return (BuffNames)12415407305475427397uL;
			case (BuffNames)12415407305475427397uL:
			case (BuffNames)1358929223039794148uL:
				return (BuffNames)1358929223039794148uL;
			default:
				return (BuffNames)8185339104921261200uL;
			}
		}

		public static bool Satisfaction(Sim sim)
		{
			if (sim != null)
			{
				BuffManager buffManager = sim.BuffManager;
				BuffNames name = BuffNames.Undefined;
				BuffNames buffNames = BuffNames.Undefined;
				foreach (BuffNames item in Names.Libido)
				{
					if (buffManager.HasElement(item))
					{
						name = item;
						break;
					}
				}
				buffNames = Satisfaction(name);
				return Replace(buffManager, buffNames, Origin.FromBeingNaked);
			}
			return false;
		}

		public static bool PartialSatisfaction(Sim sim)
		{
			if (sim != null)
			{
				BuffManager buffManager = sim.BuffManager;
				if (buffManager.HasAnyElement((BuffNames)1358929223039794148uL, (BuffNames)12415407305475427397uL, (BuffNames)13910300093031145699uL, (BuffNames)7244019685987188093uL, (BuffNames)8185339104921261200uL))
				{
					return Replace(buffManager, (BuffNames)8185339104921261200uL, Origin.FromBeingNaked);
				}
				if (!buffManager.HasAnyElement(Names.Libido.ToArray()))
				{
					buffManager.AddElement((BuffNames)8185339104921261200uL, Origin.FromBeingNaked);
					return true;
				}
			}
			return false;
		}

		public static bool WatchUrgency(Sim sim)
		{
			if (sim != null)
			{
				BuffManager buffManager = sim.BuffManager;
				if (buffManager.HasElement((BuffNames)12415407305475427397uL))
				{
					return Replace(buffManager, (BuffNames)1358929223039794148uL, Origin.None);
				}
				if (buffManager.HasElement((BuffNames)13910300093031145699uL))
				{
					return Replace(buffManager, (BuffNames)12415407305475427397uL, Origin.None);
				}
				if (buffManager.HasElement((BuffNames)7244019685987188093uL))
				{
					return Replace(buffManager, (BuffNames)13910300093031145699uL, Origin.None);
				}
				if (buffManager.HasElement((BuffNames)8185339104921261200uL))
				{
					return Replace(buffManager, (BuffNames)7244019685987188093uL, Origin.None);
				}
				if (!buffManager.HasAnyElement(Names.Libido.ToArray()))
				{
					buffManager.AddElement((BuffNames)8185339104921261200uL, Origin.None);
					return true;
				}
			}
			return false;
		}

		public static BuffNames Satisfaction(BuffInstance instance)
		{
			BuffNames buffGuid = (BuffNames)instance.BuffGuid;
			return Satisfaction(buffGuid);
		}

		public static BuffNames Satisfaction(BuffNames name)
		{
			switch (name)
			{
			case (BuffNames)16113274642161440716uL:
			case (BuffNames)16341761339885008577uL:
				return (BuffNames)16113274642161440716uL;
			case (BuffNames)10626820509964641423uL:
				return (BuffNames)16341761339885008577uL;
			case (BuffNames)16512105510841219241uL:
				return (BuffNames)10626820509964641423uL;
			case (BuffNames)8185339104921261200uL:
				return (BuffNames)16512105510841219241uL;
			default:
				return (BuffNames)8185339104921261200uL;
			}
		}

		public override void OnTimeout(BuffManager bm, BuffInstance bi, OnTimeoutReasons reason)
		{
			Urgency(bm, bi);
		}
	}
}
