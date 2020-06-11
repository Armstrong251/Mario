using Mario.Entities;
using Mario.Entities.Block;
using Mario.Entities.Mario;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Mario.Levels
{

    static class LevelReader
    {
        static readonly Color RED = new Color(255, 0, 0);
        static readonly Color ORANGE = new Color(255, 135, 0);
        static readonly Color YELLOW = new Color(255, 255, 0);
        static readonly Color BROWN = new Color(200, 135, 75);
        static readonly Color CYAN = new Color(0, 255, 255);
        static readonly Color GREEN = new Color(0, 255, 0);
        static readonly Color DARKGREEN = new Color(0, 180, 0);
        static readonly Color GREENYELLOW = new Color(135, 255, 0);
        static readonly Color DARKGREENYELLOW = new Color(100, 180, 0);
        static readonly Color GREENWHITE = new Color(156, 180, 126);
        static readonly Color REDWHITE = new Color(255, 128, 128);
        static readonly Color DARKGREENH = new Color(0, 200, 0);
        static readonly Color DARKGREENHE = new Color(0, 160, 0);
        static readonly Color DARKDARKGREEN = new Color(0, 80, 0);
        static readonly Color BLACK = new Color(0, 0, 0);
        static readonly Color DARKGREY = new Color(16, 16, 16);
        static readonly Color GREY = new Color(64, 64, 64);
        static readonly Color LIGHTGREY = new Color(128, 128, 128);
        static readonly Color PURPLE = new Color(255, 0, 255);
        static readonly Color PINK = new Color(255, 0, 128);
        static readonly Color OFFWHITE = new Color(240, 214, 214);
        static readonly Color BLURPLE = new Color(97, 94, 221);
        static readonly Color TURQ = new Color(135, 255, 206);
        static readonly Color GREENTURQ = new Color(135, 255, 150);
        static readonly Color SATURPURP = new Color(135, 0, 150);
        static readonly Color DARKRED = new Color(135, 0, 0);
        static readonly Color VERYDARKRED = new Color(90, 0, 0);
        static readonly Color TAN = new Color(255, 206, 198);
        static readonly Color LAVENDER = new Color(135, 115, 150);
        static readonly Color YELLOWORANGE = new Color(255, 190, 55);
        //Will probably have a parser class which these will be put into later...Just reminding myself that there need to be at least that many
        //Blocks to parse through.
        public static Enum ColorTo(Type type, Color color)
        {
            Enum e = null;
            if (type == typeof(BlockEntity))
            {
                e = ColorToBlock(color);
            }
            else if (type == typeof(MarioContext))
            {
                e = ColorToMario();
            }
            else if (type == typeof(MonsterEntity))
            {
                e = ColorToMonster(color);
            }
            else if (type == typeof(ItemEntity))
            {
                e = ColorToItem(color);
            }
            if(e == null)
            {
                Console.Write("");
            }
            return e;
        }
        static readonly Dictionary<Color, Enum> blockDict = new Dictionary<Color, Enum>(){
                [ORANGE] = States.Stair,
                [RED] = States.Floor,
                [YELLOW] = States.Question,
                [BROWN] = States.Brick,
                [CYAN] = States.Hidden,
                [BLACK] = States.Used,
                [GREEN] = States.PipeVert,
                [DARKGREEN] = States.PipeTop,
                [GREENYELLOW] = States.Flagpole,
                [DARKGREENYELLOW] = States.FlagCap,
                [DARKGREENH] = States.PipeHoriz,
                [DARKGREENHE] = States.PipeSide,
                [GREENWHITE] = States.Flag,
                [DARKDARKGREEN] = States.PipeJoint,
                [PURPLE] = States.Castle,
                [LIGHTGREY] = States.BulletLauncher,
                [DARKGREY] = States.BanzaiLauncher,
                [GREY] = States.BigBrick,
                [PINK] = States.Firebar,
                [BLURPLE] = States.Platform,
                [TAN] = States.SmallBridge,
                [TURQ] = States.LavaTop,
                [GREENTURQ] = States.LavaBottom,
                [SATURPURP] = States.Axe,
                [DARKRED] = States.BossPlatformFull,
                [VERYDARKRED] = States.BossPlatformSmall,
                [LAVENDER] = States.Toad,
                [YELLOWORANGE] = States.Dirt,
        };
        public static Enum ColorToBlock(Color color)
        {
            //Debug.WriteLine("" + color.ToString());
            return blockDict[color];
        }

        static readonly Dictionary<Color, Enum> itemDict = new Dictionary<Color, Enum>(){
            [ORANGE] = ItemType.FIRE_FLOWER,
            [RED] = ItemType.MUSHROOM,
            [GREEN] = ItemType.ONEUP,
            [CYAN] = ItemType.STAR,
        };
        public static Enum ColorToItem(Color color)
        {
            if(itemDict.ContainsKey(color)) return itemDict[color];
            else if (color.R == 255 && color.G == 255 && color.B < 200)
            {
                return ItemType.WORLD_COIN;
            }
            return null;
        }

        public static Enum ColorToMario()//Color color)
        {
            return ActionEnum.SelfState;
        }

        static readonly Dictionary<Color, Enum> monsterDict = new Dictionary<Color, Enum>() {
            [BROWN] = Mobs.goomba,
            [GREEN] = Mobs.greenKoopa,
            [GREENWHITE] = Mobs.GreenFlyingKoopa,
            [RED] = Mobs.redKoopa,
            [REDWHITE] = Mobs.RedFlyingKoopa,
            [PINK] = Mobs.PiranhaPlant,
            [DARKGREY] = Mobs.FatBill,
            [OFFWHITE] = Mobs.BooHiddn,
            [DARKGREEN]= Mobs.Bowser,
            [DARKGREENH] = Mobs.AIBowser,
            [GREENYELLOW] = Mobs.HammerBro,
            [CYAN] = Mobs.Lakitu,
        };
        public static Enum ColorToMonster(Color color)
        {
            return monsterDict[color];
        }
    }
}
