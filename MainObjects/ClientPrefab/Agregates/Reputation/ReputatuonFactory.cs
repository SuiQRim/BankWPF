using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace BankObjects.ClientPrefab.Agregates.Reputation
{
    internal static class ReputatuonFactory
    {
        public static ClientReputation GetReputationUsingLVL(int statusLVL)
        {
            return statusLVL switch
            {
                1 => new Terribly(),
                2 => new Medium(),
                3 => new Good(),
                4 => new Perfect(),
                _ => new NotSelectedReputation()
            };

        }
    }
}