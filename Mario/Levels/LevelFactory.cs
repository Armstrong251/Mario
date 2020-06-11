using System;
using System.Collections.Generic;
using Mario.Entities.Mario;
using Microsoft.Xna.Framework;

namespace Mario.Levels
{
    public static class LevelFactory
    {
        delegate Level LevelCtor(MarioGame game, string levelFolder, Vector2 respawnPosition, PowerUpEnum powerUp);

        static Dictionary<string, LevelCtor> creators = new Dictionary<string, LevelCtor>()
        {
            ["Level2/Beginning"] = (a, b, c, d) => new Level2Beginning(a, b, c, d),
            ["Level11/Beginning"] = (a, b, c, d) => new Level2Beginning(a, b, c, d),
        };
        public static Level LoadLevel(MarioGame game, string levelFolder, Vector2 respawnPosition, PowerUpEnum powerUp)
        {
            if(creators.ContainsKey(levelFolder)) return creators[levelFolder](game, levelFolder, respawnPosition, powerUp);
            return new Level(game, levelFolder, respawnPosition, powerUp);
        }
    }
}