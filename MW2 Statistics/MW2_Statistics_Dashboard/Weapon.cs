using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MW2_Statistics_Dashboard
{
    public class Weapon
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string CleanName { get; set; }
        private string mWeaponImage;
        public string WeaponImage { get { return mWeaponImage + ".png"; } }
        private string mAttachmentImage1;
        public string AttachmentImage1 { get { return mAttachmentImage1 + ".png"; } }
        private string mAttachmentImage2;
        public string AttachmentImage2 { get { return mAttachmentImage2 + ".png"; } }



        public Weapon(int id, string name)
        {
            Id = id;
            Name = name;
            GetCleanName(name);
        }

        public override string ToString()
        {
            return CleanName;
        }

        private void GetCleanName(string name)
        {
            MW2Weapon[] weaponsWithAttachments = 
            {
                // AR
                new MW2Weapon { TechnicalName = "m4", CleanName = "M4A1", ImageName = "m4" },
                new MW2Weapon { TechnicalName = "famas", CleanName = "FAMAS", ImageName = "famas" },
                new MW2Weapon { TechnicalName = "scar", CleanName = "SCAR-H", ImageName = "scar" },
                new MW2Weapon { TechnicalName = "tavor", CleanName = "TAR-21", ImageName = "tavor" },
                new MW2Weapon { TechnicalName = "fal", CleanName = "FAL", ImageName = "fal" },
                new MW2Weapon { TechnicalName = "m16", CleanName = "M16A4", ImageName = "m16" },
                new MW2Weapon { TechnicalName = "masada", CleanName = "ACR", ImageName = "masada" },
                new MW2Weapon { TechnicalName = "f2000", CleanName = "F2000", ImageName = "f2000" },
                new MW2Weapon { TechnicalName = "ak47", CleanName = "AK-47", ImageName = "ak47" },

                // SMG
                new MW2Weapon { TechnicalName = "mp5k", CleanName = "MP5K", ImageName = "mp5k" },
                new MW2Weapon { TechnicalName = "ump45", CleanName = "UMP45", ImageName = "ump45" },
                new MW2Weapon { TechnicalName = "kriss", CleanName = "Vector", ImageName = "kriss" },
                new MW2Weapon { TechnicalName = "p90", CleanName = "P90", ImageName = "p90" },
                new MW2Weapon { TechnicalName = "uzi", CleanName = "Mini-Uzi", ImageName = "uzi" },

                // LMG
                new MW2Weapon { TechnicalName = "sa80", CleanName = "L86 LSW", ImageName = "sa80" },
                new MW2Weapon { TechnicalName = "rpd", CleanName = "RPD", ImageName = "rpd" },
                new MW2Weapon { TechnicalName = "mg4", CleanName = "MG4", ImageName = "mg4" },
                new MW2Weapon { TechnicalName = "aug", CleanName = "AUG HBAR", ImageName = "aug" },
                new MW2Weapon { TechnicalName = "m240", CleanName = "M240", ImageName = "m240" },

                // Sinper Rifles
                new MW2Weapon { TechnicalName = "barrett", CleanName = "Barrett .50cal", ImageName = "barrett" },
                new MW2Weapon { TechnicalName = "cheytac", CleanName = "Intervention", ImageName = "cheytac" },
                new MW2Weapon { TechnicalName = "wa2000", CleanName = "WA2000", ImageName = "wa2000" },
                new MW2Weapon { TechnicalName = "m21", CleanName = "M21 EBR", ImageName = "m21" },

                // Machine Pistols
                new MW2Weapon { TechnicalName = "pp2000", CleanName = "PP2000", ImageName = "pp2000" },
                new MW2Weapon { TechnicalName = "glock", CleanName = "G18", ImageName = "glock" },
                new MW2Weapon { TechnicalName = "beretta393", CleanName = "M93 Raffica", ImageName = "beretta393" },
                new MW2Weapon { TechnicalName = "tmp", CleanName = "TMP", ImageName = "tmp" },

                // Shotguns
                new MW2Weapon { TechnicalName = "spas12", CleanName = "SPAS-12", ImageName = "spas12" },
                new MW2Weapon { TechnicalName = "aa12", CleanName = "AA-12", ImageName = "aa12" },
                new MW2Weapon { TechnicalName = "striker", CleanName = "Striker", ImageName = "striker" },
                new MW2Weapon { TechnicalName = "ranger", CleanName = "Ranger", ImageName = "ranger" },
                new MW2Weapon { TechnicalName = "m1014", CleanName = "M1014", ImageName = "m1014" },
                new MW2Weapon { TechnicalName = "model1887", CleanName = "Model 1887", ImageName = "model1887" },

                // Handguns
                new MW2Weapon { TechnicalName = "usp", CleanName = "USP .45", ImageName = "usp" },
                new MW2Weapon { TechnicalName = "coltanaconda", CleanName = ".44 Magnum", ImageName = "coltanaconda" },
                new MW2Weapon { TechnicalName = "beretta", CleanName = "M9", ImageName = "beretta" },
                new MW2Weapon { TechnicalName = "deserteagle", CleanName = "Desert Eagle", ImageName = "deserteagle" },

                
            };

            MW2Weapon[] WeaponsWithoutAttachments =
            {
                // Riot Shield
                new MW2Weapon { TechnicalName = "riotshield", CleanName = "RIOT SHIELD", ImageName = "riotshield" },

                // Launcher
                new MW2Weapon { TechnicalName = "at4", CleanName = "AT4-HS", ImageName = "at4" },
                new MW2Weapon { TechnicalName = "m79", CleanName = "Thumper", ImageName = "m79" },
                new MW2Weapon { TechnicalName = "stinger", CleanName = "Stringer", ImageName = "stinger" },
                new MW2Weapon { TechnicalName = "javelin", CleanName = "Javelin", ImageName = "javelin" },
                new MW2Weapon { TechnicalName = "rpg", CleanName = "RPG-7", ImageName = "rpg" },

                // Equipment
                new MW2Weapon { TechnicalName = "frag_grenade", CleanName = "Frag", ImageName = "frag_grenade" },
                new MW2Weapon { TechnicalName = "semtex", CleanName = "Semtex", ImageName = "semtex" },
                new MW2Weapon { TechnicalName = "throwingknife", CleanName = "Throwing Knife", ImageName = "throwingknife" },
                new MW2Weapon { TechnicalName = "claymore", CleanName = "Claymore", ImageName = "claymore" },
                new MW2Weapon { TechnicalName = "c4", CleanName = "C4", ImageName = "c4" },
                new MW2Weapon { TechnicalName = "flash_grenade", CleanName = "Flash Grenade", ImageName = "flash_grenade" },
                new MW2Weapon { TechnicalName = "concussion_grenade", CleanName = "Stun Grenade", ImageName = "concussion_grenade" },
                new MW2Weapon { TechnicalName = "smoke_grenade", CleanName = "Smoke Grenade", ImageName = "smoke_grenade" },

                // Killstreaks
                new MW2Weapon { TechnicalName = "sentry", CleanName = "Sentry Gun", ImageName = "sentry" },
                new MW2Weapon { TechnicalName = "remotemissile_projectile", CleanName = "Predator Missile", ImageName = "remotemissile_projectile" },
                new MW2Weapon { TechnicalName = "artillery", CleanName = "Precision Airstrike", ImageName = "artillery" },
                new MW2Weapon { TechnicalName = "harrier_20mm", CleanName = "Harrier Strike", ImageName = "harrier_20mm" },
                new MW2Weapon { TechnicalName = "cobra_20mm", CleanName = "Attack Helicopter", ImageName = "cobra_20mm" },
                new MW2Weapon { TechnicalName = "pavelow_minigun", CleanName = "Pave Low", ImageName = "pavelow_minigun" },
                new MW2Weapon { TechnicalName = "stealth_bomb", CleanName = "Stealth Bomber", ImageName = "stealth_bomb" },
                new MW2Weapon { TechnicalName = "cobra_player_minigun", CleanName = "Chopper Gunner", ImageName = "cobra_player_minigun" },
                new MW2Weapon { TechnicalName = "ac130_25mm", CleanName = "AC130", ImageName = "ac130" },
                new MW2Weapon { TechnicalName = "ac130_40mm", CleanName = "AC130", ImageName = "ac130" },
                new MW2Weapon { TechnicalName = "ac130_105mm", CleanName = "AC130", ImageName = "ac130" },
                new MW2Weapon { TechnicalName = "nuke", CleanName = "Tactical Nuke", ImageName = "nuke" },

                // Environment
                new MW2Weapon { TechnicalName = "barrel", CleanName = "Barrel", ImageName = "barrel" },
                new MW2Weapon { TechnicalName = "destructible_car", CleanName = "Car", ImageName = "destructible_car" },
                new MW2Weapon { TechnicalName = "turret_minigun", CleanName = "Mounted Minigun", ImageName = "turret_minigun" },
            };

            MW2Weapon[] weaponAttachments =
            {
                new MW2Weapon { TechnicalName = "gl", CleanName = "Grenade Launcher", ImageName = "gl" },
                new MW2Weapon { TechnicalName = "reflex", CleanName = "Red Dot Sight", ImageName = "reflex" },
                new MW2Weapon { TechnicalName = "silencer", CleanName = "Silencer", ImageName = "silencer" },
                new MW2Weapon { TechnicalName = "acog", CleanName = "ACOG Scope", ImageName = "acog" },
                new MW2Weapon { TechnicalName = "fmj", CleanName = "FMJ", ImageName = "fmj" },
                new MW2Weapon { TechnicalName = "shotgun", CleanName = "Shotgun", ImageName = "shotgun" },
                new MW2Weapon { TechnicalName = "eotech", CleanName = "Holographic Sight", ImageName = "eotech" },
                new MW2Weapon { TechnicalName = "heartbeat", CleanName = "Heartbeat Sensor", ImageName = "heartbeat" },
                new MW2Weapon { TechnicalName = "thermal", CleanName = "Thermal", ImageName = "thermal" },
                new MW2Weapon { TechnicalName = "xmags", CleanName = "Extended Mags", ImageName = "xmags" },
                new MW2Weapon { TechnicalName = "rof", CleanName = "Rapid Fire", ImageName = "rof" },
                new MW2Weapon { TechnicalName = "akimbo", CleanName = "Akimbo", ImageName = "akimbo" },
                new MW2Weapon { TechnicalName = "grip", CleanName = "Grip", ImageName = "grip" },
                new MW2Weapon { TechnicalName = "tactical", CleanName = "Tactical Knife", ImageName = "tactical" },
            };

            string cleanName = string.Empty;
            if(name.Substring(0,3) == "gl_")            // Underbarrel grenade launcher used
            {
                cleanName += "Grenade Launcher attached to ";
                mWeaponImage = "gl";
                foreach(var item in weaponsWithAttachments)
                {
                    if (name.Contains(item.TechnicalName))
                    {
                        cleanName += item.CleanName;
                        mAttachmentImage1 = item.ImageName;
                        break;
                    }
                }
            }
            else if(name.Contains("shotgun_attach"))    // Underbarrel shotgun used
            {
                cleanName += "Shotgun attached to ";
                mWeaponImage = "shotgun";
                foreach (var item in weaponsWithAttachments)
                {
                    if (name.Contains(item.TechnicalName))
                    {
                        cleanName += item.CleanName;
                        mAttachmentImage1 = item.ImageName;
                        break;
                    }
                }
            }
            else
            {
                foreach (var item in weaponsWithAttachments)
                {
                    if(name.Contains(item.TechnicalName))
                    {
                        cleanName += item.CleanName;
                        mWeaponImage = item.ImageName;
                        int attachmentCount = name.Split('_').Length - 2;   // -2 Because we throw away the <weaponName> and the "_mp"
                        if(attachmentCount > 0)
                        {
                            cleanName += " w/ ";
                            int attachmentsAddedToName = 0;
                            foreach (var att in weaponAttachments)
                            {
                                if(name.Contains("_" + att.TechnicalName + "_"))
                                {
                                    cleanName += att.CleanName;

                                    if (attachmentsAddedToName == 0)
                                        mAttachmentImage1 = att.ImageName;
                                    else
                                        mAttachmentImage2 = att.ImageName;

                                    attachmentsAddedToName++;
                                    if (attachmentsAddedToName >= attachmentCount)
                                        break;
                                    else
                                        cleanName += " & ";
                                }
                            }
                        }
                    }
                }
                if(cleanName == string.Empty)
                {
                    foreach (var item in WeaponsWithoutAttachments)
                    {
                        if(name.Contains(item.TechnicalName))
                        {
                            cleanName += item.CleanName;
                            mWeaponImage = item.ImageName;
                            break;
                        }
                    }
                }
                if (cleanName == string.Empty)
                    cleanName = name;
            }

            CleanName = cleanName;
        }
    }
}
