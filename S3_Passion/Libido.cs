using System.Collections.Generic;
using Sims3.Gameplay.Actors;
using Sims3.Gameplay.ActorSystems;

namespace Passion.S3_Passion
{
    public class Libido : CustomBuff
    {
        public Libido(BuffData info)
            : base(info)
        {
        }

        private static class BuffNameConstants
        {
            public const BuffNames Libido100 = (BuffNames)2922253427052633003uL;
            public const BuffNames Libido90 = (BuffNames)13147589483235469726uL;
            public const BuffNames Libido80 = (BuffNames)14041574305464178967uL;
            public const BuffNames Libido70 = (BuffNames)16251613925768384549uL;
            public const BuffNames Libido60 = (BuffNames)2917472750494117670uL;
            public const BuffNames Libido50 = (BuffNames)8200297330989383022uL;
            public const BuffNames Libido40 = (BuffNames)8198323707617122614uL;
            public const BuffNames Libido30 = (BuffNames)3097843141287298166uL;
            public const BuffNames Libido20 = (BuffNames)2922268820215428064uL;
            public const BuffNames Libido10 = (BuffNames)2913570583726353694uL;
            public const BuffNames Libido0 = (BuffNames)2248271455579464240uL;

            public const BuffNames PartialSatisfaction1 = (BuffNames)1358929223039794148uL;
            public const BuffNames PartialSatisfaction2 = (BuffNames)12415407305475427397uL;
            public const BuffNames PartialSatisfaction3 = (BuffNames)13910300093031145699uL;
            public const BuffNames PartialSatisfaction4 = (BuffNames)7244019685987188093uL;
            public const BuffNames PartialSatisfaction5 = (BuffNames)8185339104921261200uL;
        }

        private static readonly Dictionary<BuffNames, BuffNames> UrgencyMap = new Dictionary<BuffNames, BuffNames>();

        static Libido()
        {
            UrgencyMap.Add(BuffNameConstants.Libido100, BuffNameConstants.Libido90);
            UrgencyMap.Add(BuffNameConstants.Libido90, BuffNameConstants.Libido80);
            UrgencyMap.Add(BuffNameConstants.Libido80, BuffNameConstants.Libido70);
            UrgencyMap.Add(BuffNameConstants.Libido70, BuffNameConstants.Libido60);
            UrgencyMap.Add(BuffNameConstants.Libido60, BuffNameConstants.Libido50);
            UrgencyMap.Add(BuffNameConstants.Libido50, BuffNameConstants.Libido40);
            UrgencyMap.Add(BuffNameConstants.Libido40, BuffNameConstants.Libido30);
            UrgencyMap.Add(BuffNameConstants.Libido30, BuffNameConstants.Libido20);
            UrgencyMap.Add(BuffNameConstants.Libido20, BuffNameConstants.Libido10);
            UrgencyMap.Add(BuffNameConstants.Libido10, BuffNameConstants.Libido0);
            UrgencyMap.Add(BuffNameConstants.Libido0, BuffNameConstants.Libido0);
        }

        private static bool Replace(BuffManager bm, BuffNames newBuff, Origin origin)
        {
            if (bm.Actor == null || !Names.Libido.Contains(newBuff)) return false;
            foreach (BuffNames item in Names.Libido)
            {
                bm.RemoveElement(item);
            }

            bm.AddElement(newBuff, origin);
            return true;
        }

        public static bool Urgency(Sim sim)
        {
            return sim != null && Urgency(sim.BuffManager);
        }

        private static bool Urgency(BuffManager bm)
        {
            if (bm == null) return false;
            BuffNames name = GetCurrentLibidoBuff(bm);
            BuffNames value;
            BuffNames nextBuff = UrgencyMap.TryGetValue(name, out value) ? value : BuffNameConstants.Libido0;
            return Replace(bm, nextBuff, Origin.None);
        }

        private static bool Urgency(BuffManager bm, BuffInstance bi)
        {
            BuffNames newBuff = Urgency(bi);
            return Replace(bm, newBuff, Origin.None);
        }

        private static BuffNames Urgency(BuffInstance instance)
        {
            BuffNames buffGuid = (BuffNames)instance.BuffGuid;
            BuffNames value;
            return UrgencyMap.TryGetValue(buffGuid, out value) ? value : BuffNameConstants.Libido0;
        }

        public static bool Satisfaction(Sim sim)
        {
            if (sim == null) return false;
            BuffManager buffManager = sim.BuffManager;
            BuffNames currentBuff = GetCurrentLibidoBuff(buffManager);
            BuffNames value;
            BuffNames nextBuff = UrgencyMap.TryGetValue(currentBuff, out value)
                ? value
                : BuffNameConstants.Libido0;
            return Replace(buffManager, nextBuff, Origin.FromBeingNaked);
        }

        public static bool PartialSatisfaction(Sim sim)
        {
            if (sim == null) return false;
            BuffManager buffManager = sim.BuffManager;
            BuffNames[] partialSatisfactionBuffs =
            {
                BuffNameConstants.PartialSatisfaction1,
                BuffNameConstants.PartialSatisfaction2,
                BuffNameConstants.PartialSatisfaction3,
                BuffNameConstants.PartialSatisfaction4,
                BuffNameConstants.PartialSatisfaction5
            };

            if (buffManager.HasAnyElement(partialSatisfactionBuffs))
            {
                return Replace(buffManager, BuffNameConstants.PartialSatisfaction5, Origin.FromBeingNaked);
            }

            if (buffManager.HasAnyElement(Names.Libido.ToArray())) return false;
            buffManager.AddElement(BuffNameConstants.PartialSatisfaction5, Origin.FromBeingNaked);
            return true;
        }

        public static bool IncreaseUrgency(Sim sim)
        {
            if (sim == null) return false;
            BuffManager buffManager = sim.BuffManager;
            BuffNames[] libidoBuffs =
            {
                BuffNameConstants.Libido100,
                BuffNameConstants.Libido90,
                BuffNameConstants.Libido80,
                BuffNameConstants.Libido70,
                BuffNameConstants.Libido60,
                BuffNameConstants.Libido50,
                BuffNameConstants.Libido40,
                BuffNameConstants.Libido30,
                BuffNameConstants.Libido20,
                BuffNameConstants.Libido10,
                BuffNameConstants.Libido0
            };

            foreach (BuffNames buff in libidoBuffs)
            {
                if (buffManager.HasElement(buff))
                {
                    BuffNames value;
                    BuffNames nextBuff = UrgencyMap.TryGetValue(buff, out value) ? value : BuffNameConstants.Libido0;
                    return Replace(buffManager, nextBuff, Origin.None);
                }
            }

            if (buffManager.HasAnyElement(Names.Libido.ToArray())) return false;
            buffManager.AddElement(BuffNameConstants.PartialSatisfaction5, Origin.None);
            return true;
        }

        private static BuffNames GetCurrentLibidoBuff(BuffManager buffManager)
        {
            foreach (BuffNames item in Names.Libido)
            {
                if (buffManager.HasElement(item))
                {
                    return item;
                }
            }

            return BuffNameConstants.Libido0;
        }

        public override void OnTimeout(BuffManager bm, BuffInstance bi, OnTimeoutReasons reason)
        {
            Urgency(bm, bi);
        }
    }
}