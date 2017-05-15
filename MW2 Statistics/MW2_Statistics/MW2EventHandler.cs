using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MW2_Statistics
{
    public static class MW2EventHandler
    {
        private static int mMatchId = -1;    // Holds the id of the current match

        public static void HandleMW2Event(MW2Event e)
        {
            //if (mMatchId == -1 && e.Type != "initgame")     // If we start become host after hostmigration
            //    return;

            switch (e.Type)
            {
                case "initgame":
                    if (e.Timestamp == "0:00")    // New match began
                    {
                        mMatchId = DataBase.RegisterNewMatch();
                    }
                    break;
                case "exitlevel: executed":         // Match had finished
                    DataBase.EndMatch(mMatchId);
                    mMatchId = -1;
                    break;
                case "j":
                case "q":
                case "say":
                case "sayteam":
                    UpdatePlayer(e.Victim);         // With these types there's only a victim, no attacker
                    break;
                case "weapon":
                    UpdatePlayer(e.Victim);
                    UpdateWWeapon(e.Weapon);
                    break;
                case "k":
                case "d":
                    long vicPlayerId = UpdatePlayer(e.Victim);
                    long attPlayerId = UpdatePlayer(e.Attacker);
                    int wepId = UpdateWWeapon(e.Weapon);

                    if (attPlayerId == -1)              // Player damaged himself
                        attPlayerId = vicPlayerId;
                    DataBase.RegisterHit(vicPlayerId, attPlayerId, mMatchId, wepId, e.Damage, e.HitLocation, e.MeansOfDeath, e.Type == "k" ? true : false);
                    break;
            }
        }

        private static long UpdatePlayer(Player p)
        {
            if (p.EntityNumber == -1)                       // If entityNumber is -1, the player was not defined (eg Player took falling damage)
                return -1;

            long playerId = GetPlayerId(p.SteamId);
            AddAlias(playerId, p.Name);
            DataBase.UpdatePlayerLastSeen(playerId);
            return playerId;
        }

        private static int UpdateWWeapon(string weaponName)
        {
            int result;
            result = DataBase.GetWeaponId(weaponName);
            if(result == -1)
            {
                DataBase.AddWeapon(weaponName);
                result = DataBase.GetWeaponId(weaponName);
            }
            return result;
        }

        private static long GetPlayerId(UInt64 steamId)
        {
            long result;
            result = DataBase.GetPlayerIdBySteamId(steamId);
            if(result == -1)
            {
                DataBase.AddPlayer(steamId);
                result = DataBase.GetPlayerIdBySteamId(steamId);
            }
            return result;
        }

        private static void AddAlias(long playerId, string playerName)
        {
            if(!DataBase.PlayerAliasExists(playerName, playerId, mMatchId))
            {
                DataBase.AddPlayerAlias(playerName, playerId, mMatchId);
            }
        }
    }
}
