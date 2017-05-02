using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MW2_Statistics
{
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

    public class MW2Event
    {
        public string Timestamp, Type, VicGuide, VicName;//always there vars
        public string VicTeam, AttGuide, AttTeam, AttName, Weapon, MeansOfDeath, HitLocation, Message;//possible vars
        public int VicEntNum = -1, AttEntNum = -1, Damage = 0;
        //public string[] _elements = null;

        public MW2Event(string buffer)
        {
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

            if (Type == "K" || Type == "D" || Type == "J" || Type == "Q" || Type == "Weapon" || Type == "say" || Type == "sayteam")
            {
                //get Standard Attacker's/concerning person's info
                VicGuide = elements[1];
                VicEntNum = Convert.ToInt32(elements[2]);

                //get info according to the type of event
                switch (Type)
                {
                    case "K"://player killed                        -     6:12 K;01100001044e6121;7;allies;DOC.Anarchist <;0110000102e85117;5;axis;GhostPhReaK;beretta393_mp;33;MOD_PISTOL_BULLET;left_leg_upper
                    case "D"://Damage dealt                         -     6:12 D;01100001044e6121;7;allies;DOC.Anarchist <;0110000102e85117;5;axis;GhostPhReaK;beretta393_mp;37;MOD_PISTOL_BULLET;left_leg_upper
                        VicTeam = elements[3];
                        VicName = elements[4];
                        AttGuide = elements[5];
                        AttEntNum = Convert.ToInt32(elements[6]);
                        AttTeam = elements[7];
                        AttName = elements[8];
                        Weapon = elements[9];
                        Damage = Convert.ToInt32(elements[10]);
                        MeansOfDeath = elements[11];
                        HitLocation = elements[12];
                        break;
                    case "J"://Player joined                        -     5:30 J;0110000102e85117;5;GhostPhReaK
                    case "Q"://Player quit                          -     0:58 Q;01100001070f2d59;5;TheShooterYOLO
                        {
                            VicName = elements[3];
                            break;
                        }
                    case "Weapon"://Player picks up weapon          -     1:47 Weapon;0110000102e85117;11;GhostPhReaK;cheytac_thermal_mp
                        {
                            VicName = elements[3];
                            Weapon = elements[4];
                            break;
                        }
                    case "say"://player says something              -     1:04 say;011000010554f912;3;Lord Bendtner.;i'll make last nice 
                        {
                            VicName = elements[3];
                            Message = elements[4].Remove(0, 1);
                            break;
                        }
                    case "sayteam"://player says something to team  -     1:26 sayteam;0110000107a947b7;0;Mr.Outraged;p90 akimbo xD
                        {
                            VicName = elements[3];
                            Message = elements[4].Remove(0, 1);
                            break;
                        }
                }
            }
            //_elements = elements;
        }

        public string getKillFeed()
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
        }
    }
}
