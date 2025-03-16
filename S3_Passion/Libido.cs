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

	// um. i think this is like. the list of the libibo buffs and what the 'next stage' is?
	// scratch that. 'case' is the stage, 'return' is what it decreases into... i think?
		public static BuffNames Urgency(BuffNames name)
		{
			switch (name)
			{
			// 100 to 90
			case (BuffNames)2922253427052633003uL:
				return (BuffNames)13147589483235469726uL;
			// 90 to 80
			case (BuffNames)13147589483235469726uL:
				return (BuffNames)14041574305464178967uL;
			// 80 to 70
			case (BuffNames)14041574305464178967uL:
				return (BuffNames)16251613925768384549uL;
			// 70 to 60
			case (BuffNames)16251613925768384549uL:
				return (BuffNames)2917472750494117670uL;
			// 60 to 50
			case (BuffNames)2917472750494117670uL:
				return (BuffNames)8200297330989383022uL;
			// 50 to 40
			case (BuffNames)8200297330989383022uL:
				return (BuffNames)8198323707617122614uL;
			// 40 to 30
			case (BuffNames)8198323707617122614uL:
				return (BuffNames)3097843141287298166uL;
			// 30 to 20
			case (BuffNames)3097843141287298166uL:
				return (BuffNames)2922268820215428064uL;
			// 20 to 10
			case (BuffNames)2922268820215428064uL:
				return (BuffNames)2913570583726353694uL;
			// 10 to 0
			case (BuffNames)2913570583726353694uL:
				return (BuffNames)2248271455579464240uL;
			// 0 to 0
			case (BuffNames)2248271455579464240uL:
				return (BuffNames)2248271455579464240uL;
			default:
				return (BuffNames)2248271455579464240uL; // default, 0% libido
			}
		}

		//TODO: refactor all the satisfaction stuff?


	// this seems to trigger the libido increase from watching stuff. im changing watchurgency to increaseurgency to make it a lil more broad
	// starts at highest left and ifelses down to the lowest
	// if this causes everything to explode? oops
		public static bool IncreaseUrgency(Sim sim)
		{
			if (sim != null)
			{
				BuffManager buffManager = sim.BuffManager;
				// 100 to 100
				if (buffManager.HasElement((BuffNames)2922253427052633003uL))
				{
					return Replace(buffManager, (BuffNames)2922253427052633003uL, Origin.None);
				}
				// 90 to 100
				if (buffManager.HasElement((BuffNames)13147589483235469726uL))
				{
					return Replace(buffManager, (BuffNames)2922253427052633003uL, Origin.None);
				}
				//80 to 90
				if (buffManager.HasElement((BuffNames)14041574305464178967uL))
				{
					return Replace(buffManager, (BuffNames)13147589483235469726uL, Origin.None);
				}
				//70 to 80
				if (buffManager.HasElement((BuffNames)16251613925768384549uL))
				{
					return Replace(buffManager, (BuffNames)14041574305464178967uL, Origin.None);
				}
				//60 to 70
				if (buffManager.HasElement((BuffNames)2917472750494117670uL))
				{
					return Replace(buffManager, (BuffNames)16251613925768384549uL, Origin.None);
				}
				//50 to 60
				if (buffManager.HasElement((BuffNames)8200297330989383022uL))
				{
					return Replace(buffManager, (BuffNames)2917472750494117670uL, Origin.None);
				}
				//40 to 50
				if (buffManager.HasElement((BuffNames)8198323707617122614uL))
				{
					return Replace(buffManager, (BuffNames)8200297330989383022uL, Origin.None);
				}
				//30 to 40
				if (buffManager.HasElement((BuffNames)3097843141287298166uL))
				{
					return Replace(buffManager, (BuffNames)8198323707617122614uL, Origin.None);
				}
				//20 to 30
				if (buffManager.HasElement((BuffNames)2922268820215428064uL))
				{
					return Replace(buffManager, (BuffNames)3097843141287298166uL, Origin.None);
				}
				//10 to 20
				if (buffManager.HasElement((BuffNames)2913570583726353694uL))
				{
					return Replace(buffManager, (BuffNames)2922268820215428064uL, Origin.None);
				}
				//0 to 10
				if (buffManager.HasElement((BuffNames)2248271455579464240uL))
				{
					return Replace(buffManager, (BuffNames)2913570583726353694uL, Origin.None);
				}
				if (!buffManager.HasAnyElement(Names.Libido.ToArray()))
				{
					buffManager.AddElement((BuffNames)8185339104921261200uL, Origin.None);
					return true;
				}
			}
			return false;
		}

		// satisfaction used to be here but i nuked it lmao



		public override void OnTimeout(BuffManager bm, BuffInstance bi, OnTimeoutReasons reason)
		{
			Urgency(bm, bi);
		}
	}
}
