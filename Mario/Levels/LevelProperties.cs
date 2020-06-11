using Mario.Entities;
using Mario.Entities.Block;
using Mario.Entities.mobs;
using Microsoft.Xna.Framework;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Mario.Levels
{
    public class LevelProperties
    {
        private JObject[] objects;
        public LevelProperties(string fileName)
        {
            string text = System.IO.File.ReadAllText(@fileName);
            string[] words = text.Split(new char[] {'$'}, StringSplitOptions.RemoveEmptyEntries);
            objects = words.Select(s => JObject.Parse(s)).ToArray();
        }
        static Dictionary<string, string> loadTypes = new Dictionary<string, string>()
        {
            ["Level"] = "Level"
        };
        // Themes are loaded at loadType = "Level", anything not in the dictionary is loaded at loadType = "Map"
        public void LoadProperties(Level level, string loadType)
        {
            foreach(var json in objects)
            {
                string type = (string)json["Type"];
                if(loadType == (loadTypes.ContainsKey(type)? loadTypes[type] : "Map"))
                {
                    switch (type)
                    {
                        case "Level":
                            level.LevelName = (string)json["Name"];
                            level.NextLevel = (string)json["NextLevel"];
                            level.Theme = Theme.GetTheme((string)json["Theme"]);
                            if (level.Theme.Name == "Space")
                                level.Gravity = .15f;
                            break;
                        case "Castle":
                            BlockEntity castle = (BlockEntity)(level.Map.blockGrid[(int)json["X"], (int)json["Y"]]);
                            castle.BoundingBox.Active = !(bool)(json["AtBeginning"] ?? false);
                            break;
                        case "Theme":
                            level.Theme = Theme.GetTheme((string)json["Theme"]);
                            break;
                        case "Pipe":
                            PipeState pipe = (PipeState)(level.Map.blockGrid[(int)json["X"], (int)json["Y"]].BlockStates.State);
                            pipe.StoredLevel = (string)json["Destination"];
                            pipe.Destination = new Point((int)json["DestX"], (int)json["DestY"]);
                            pipe.Return = (bool)(json["Return"] ?? true);
                            break;
                        case "Firebar":
                            Firebar bar = (Firebar)(level.Map.blockGrid[(int)json["X"], (int)json["Y"]].BlockStates.State);
                            bar.Size = (int)json["Size"];
                            bar.Clockwise = (bool)json["Clockwise"];
                            break;
                        case "Launcher":
                            BulletLauncher launcher = (BulletLauncher)(level.Map.blockGrid[(int)json["X"], (int)json["Y"]].BlockStates.State);
                            launcher.BulletType = (Mobs)Enum.Parse(typeof(Mobs), (string)json["BulletType"]);
                            break;
                        case "BLauncher":
                            BanzaiLauncher launcher1 = (BanzaiLauncher)(level.Map.blockGrid[(int)json["X"], (int)json["Y"]].BlockStates.State);
                            launcher1.BulletType = (Mobs)Enum.Parse(typeof(Mobs), (string)json["BulletType"]);
                            launcher1.flipped = ((int)json["Flip"]) == 1;
                            break;
                    }
                }
            }
        }
    }
}
