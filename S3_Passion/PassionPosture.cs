using Sims3.Gameplay.Abstracts;
using Sims3.Gameplay.Actors;
using PassionCore = Passion.S3_Passion.Core.Passion;

namespace Passion.S3_Passion
{
	public static class PassionPosture
	{
		public class Normal : Sim.StandingPosture
		{
			public override string Name
			{
				get
				{
					return PassionCore.Settings.Label;
				}
			}

			public Normal(Sim actor)
				: base(actor)
			{
			}

			public override bool AllowsNormalSocials()
			{
				return false;
			}

			public override bool AllowsCallOver()
			{
				return false;
			}

			public override bool AllowsRouting()
			{
				return false;
			}
		}

		public static void Set(Sim actor)
		{
			Set(actor, null);
		}

		public static void Set(Sim actor, GameObject obj)
		{
			if (actor != null)
			{
				actor.Posture = new Normal(actor);
			}
		}

		public static void Revert(Sim actor)
		{
			if (actor != null)
			{
				actor.Posture = null;
			}
		}
	}
}
