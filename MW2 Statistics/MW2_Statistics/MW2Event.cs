using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MW2_Statistics
{
    #region NEW
    /*public class MW2Event
    {
        string mBuffer;

        string Type
        {
            get
            {
                return 
            }
            set
            {

            }
        }

        public MW2Event(string buffer)
        {
            mBuffer = buffer;
        }
    }*/
    #endregion
    #region OLD
    /*public enum MW2EventType
    {
        Kill,
        Death,
        Join,
        Quit,
        WeaponPickUp,
        Message,
        TeamMessage
    }*/

    public struct Player
    {
        public UInt64 SteamId;
        public int EntityNumber;
        public string Team;
        public string Name;
    }

    public class MW2Event
    {
        public Player Victim, Attacker;

        public string Timestamp, Type/*, VicName*/;//always there vars
        public string /*VicTeam, AttTeam, AttName,*/ Weapon, MeansOfDeath, HitLocation, Message;//possible vars
        public int /*VicEntNum = -1, AttEntNum = -1,*/ Damage = 0;
        //public UInt64 VicSteamId, AttSteamId;
        //public string[] _elements = null;

        public MW2Event(string buffer)
        {
            //buffer = buffer.ToLower();
            string[] elements = buffer.Split(';');

            // Get Timestamp and Type
            string temp = elements[0].Trim();
            int spaceIndex = -1;
            for (int i = 0; i < temp.Length; i++)
            {
                if (temp[i] == ' ')
                {
                    spaceIndex = i;
                    break;
                }
            }
            Timestamp = temp.Substring(0, spaceIndex);
            Type = temp.Substring(spaceIndex + 1, Convert.ToInt16(temp.Length) - (spaceIndex + 1));
            Type = Type.ToLower();

            if (Type == "k" || Type == "d" || Type == "j" || Type == "q" || Type == "weapon" || Type == "say" || Type == "sayteam")
            {
                //get Standard Attacker's/concerning person's info
                //VicSteamId = Convert.ToUInt64(elements[1]);
                //VicEntNum = Convert.ToInt32(elements[2]);
                Victim.SteamId = ConvertStringToSteamId(elements[1]);
                Victim.EntityNumber = Convert.ToInt32(elements[2]);

                //get info according to the type of event
                switch (Type)
                {
                    case "k"://player killed                        -     6:12 K;01100001044e6121;7;allies;DOC.Anarchist <;0110000102e85117;5;axis;GhostPhReaK;beretta393_mp;33;MOD_PISTOL_BULLET;left_leg_upper
                    case "d"://Damage dealt                         -     6:12 D;01100001044e6121;7;allies;DOC.Anarchist <;0110000102e85117;5;axis;GhostPhReaK;beretta393_mp;37;MOD_PISTOL_BULLET;left_leg_upper
                        Victim.Team = elements[3];
                        Victim.Name = elements[4];
                        Attacker.EntityNumber = Convert.ToInt32(elements[6]);
                        if(Attacker.EntityNumber != -1)                             // If attackerEntityNumber is -1, player hurt himself
                            Attacker.SteamId = ConvertStringToSteamId(elements[5]);
                        Attacker.Team = elements[7];
                        Attacker.Name = elements[8];
                        Weapon = elements[9];
                        Damage = Convert.ToInt32(elements[10]);
                        MeansOfDeath = elements[11];
                        HitLocation = elements[12];
                        break;
                    case "j"://Player joined                        -     5:30 J;0110000102e85117;5;GhostPhReaK
                    case "q"://Player quit                          -     0:58 Q;01100001070f2d59;5;TheShooterYOLO
                        {
                            Victim.Name = elements[3];
                            break;
                        }
                    case "weapon"://Player picks up weapon          -     1:47 Weapon;0110000102e85117;11;GhostPhReaK;cheytac_thermal_mp
                        {
                            Victim.Name = elements[3];
                            Weapon = elements[4];
                            break;
                        }
                    case "say"://player says something              -     1:04 say;011000010554f912;3;Lord Bendtner.;i'll make last nice 
                        {
                            Victim.Name = elements[3];
                            Message = elements[4].Remove(0, 1);
                            break;
                        }
                    case "sayteam"://player says something to team  -     1:26 sayteam;0110000107a947b7;0;Mr.Outraged;p90 akimbo xD
                        {
                            Victim.Name = elements[3];
                            Message = elements[4].Remove(0, 1);
                            break;
                        }
                }
            }
            //_elements = elements;
        }

        private UInt64 ConvertStringToSteamId(string hex)
        {
            return BitConverter.ToUInt64(StringToByteArray(hex).Reverse().ToArray(), 0);
        }

        public byte[] StringToByteArray(string hex)
        {
            return Enumerable.Range(0, hex.Length)
                             .Where(x => x % 2 == 0)
                             .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
                             .ToArray();
        }

        /*public string getKillFeed()
        {
            string killfeed = "- - - - -";
            switch (Type)
            {
                case "K"://player killed                        -     6:12 K;01100001044e6121;7;allies;DOC.Anarchist <;0110000102e85117;5;axis;GhostPhReaK;beretta393_mp;33;MOD_PISTOL_BULLET;left_leg_upper
                    killfeed = "(" + AttTeam + ")" + AttName + " [" + Weapon + "]> (" + VicTeam + ")" + VicName;
                    break;
                case "J"://Player joined                        -     5:30 J;0110000102e85117;5;GhostPhReaK
                    killfeed = VicName + " joined the game.";
                    break;
                case "Q"://Player quit                          -     0:58 Q;01100001070f2d59;5;TheShooterYOLO
                    killfeed = VicName + " left the game.";
                    break;
                case "say"://player says something              -     1:04 say;011000010554f912;3;Lord Bendtner.;i'll make last nice
                case "sayteam"://player says something to team  -     1:26 sayteam;0110000107a947b7;0;Mr.Outraged;p90 akimbo xD
                    killfeed = VicName + ": " + Message;
                    break;
                case "Weapon"://Player picks up weapon          -     1:47 Weapon;0110000102e85117;11;GhostPhReaK;cheytac_thermal_mp
                case "D"://Damage dealt                         -     6:12 D;01100001044e6121;7;allies;DOC.Anarchist <;0110000102e85117;5;axis;GhostPhReaK;beretta393_mp;37;MOD_PISTOL_BULLET;left_leg_upper
                    break;
            }
            return killfeed;
        }*/
    }
    #endregion
}
