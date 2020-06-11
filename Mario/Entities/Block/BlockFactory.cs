using Mario.Levels;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Mario.Entities.Block
{
    class BlockFactory : Factory
    {
        public BlockFactory(Level level) : base(level) { }
        public override Entity Create(Point position, Enum e)
        {
            BlockEntity block = new BlockEntity(Level, position);
            block.BlockStates = new StateList<BlockState, States>((States)e, state => CreateState(state, block));
            return block;
        }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        BlockState CreateState(States state, BlockEntity b)
        {
            Sprite s = CreateSprite(state);
            switch (state)
            {
                case States.Used: return new UsedState(b, s);
                case States.Brick: return new BrickState(b, s);
                case States.ItemBrick: return new ItemBrickState(b, s);
                case States.Question: return new QuestionState(b, s);
                case States.Hidden: return new HiddenState(b, s);
                case States.Stair: return new StairState(b, s);
                case States.BigBrick:
                case States.SmallBridge:
                case States.Floor: return new FloorState(b, s);
                case States.BabyBrick: return new BabyBrick(b, s);
                case States.PipeTop: return new PipeTopState(b, s);
                case States.PipeVert: return new PipeVertState(b, s);
                case States.PipeHoriz: return new PipeHorizState(b, s);
                case States.PipeSide: return new PipeSideState(b, s);
                case States.Flagpole:
                case States.FlagCap: return new FlagBlock(b, s);
                case States.Flag: return new Flag(b, s);
                case States.PipeJoint: return new PipeJointState(b, s);
                case States.Castle: return new Castle(b, s);
                case States.Firebar: return new Firebar(b, s);
                case States.BulletLauncher: return new BulletLauncher(b,s);
                case States.BanzaiLauncher: return new BanzaiLauncher(b, s);
                case States.Platform: return new PlatformState(b, s);
                case States.LavaTop: return new LavaTopState(b, s);
                case States.LavaBottom: return new LavaBottomState(b, s);
                case States.Axe: return new AxeState(b, s, this);
                case States.BossPlatformFull: return new BossPlatformFull(b, s, this);
                case States.BossPlatformSmall: return new BossPlatformSmall(b, s);
                case States.Toad: return new Toad(b, s);
                case States.Dirt: return new DirtState(b, s);
            }
            return null;
        }

        public override Sprite CreateSprite(Enum id, Enum state) { return CreateSprite((States)id); }

        //Rectangle is the area of the spritesheet and offset is the offset for that area
        static (Rectangle, Dictionary<string, Point>)[] offsets =
        {
            (new Rectangle(0, 0, 432, 128),
            new Dictionary<string, Point>()
            {
                ["Plains"] = new Point(0, 0),
                ["Cave"] = new Point(0, 32),
                ["Castle"] = new Point(0, 64),
                ["DarkPlains"] = new Point(0, 0),
                ["Space"] = new Point(0, 0),
            }),
            (new Rectangle(0, 128, 432, 192),
            new Dictionary<string, Point>()
            {
                ["Plains"] = new Point(0, 0),
                ["Cave"] = new Point(0, 32),
                ["Castle"] = new Point(0, 128),
                ["DarkPlains"] = new Point(0, 96),
                ["Space"] = new Point(0, 96),
            }),
            (new Rectangle(0, 320, 432, 128),
            new Dictionary<string, Point>()
            {
                ["Plains"] = new Point(0, 0),
                ["Cave"] = new Point(0, 32),
                ["Castle"] = new Point(0, 64),
                ["DarkPlains"] = new Point(0, 0),
                ["Space"] = new Point(0, 64),
            }),
        };
        public static Point[] GetThemeOffset(string theme, Point[] animation)
        {
            var offset = offsets.First(rd => rd.Item1.Contains(animation[0])).Item2;
            return animation.Select(p => p + offset[theme]).ToArray();
        }

        //Suppress because we can't really reduce the complexity here. This is where it all goes.
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        Sprite CreateSprite(States ID)
        {
            bool themeOffset = true;
            Point size = new Point(16);
            Point[] animation;
            string spritesheet = "Sprites/PipesandBlocks";
            switch (ID)
            {
                case States.Used:
                case States.Firebar:
                    animation = new Point[] { new Point(48, 0) };
                    break;
                case States.Brick:
                case States.ItemBrick:
                    animation = new Point[] { new Point(16, 0) };
                    break;
                case States.Floor:
                    animation = new Point[] { new Point(0, 0) };
                    break;
                case States.Stair:
                    animation = new Point[] { new Point(0, 16) };
                    break;
                case States.Question:
                    animation = new Point[] { new Point(0, 0), new Point(16, 0), new Point(32, 0), new Point(16, 0) };
                    spritesheet = "Sprites/QuestionBlock";
                    break;
                case States.BabyBrick:
                    size = new Point(9, 5);
                    animation = new Point[] { new Point(16, 7) };
                    break;
                case States.BigBrick:
                    animation = new [] { new Point(32, 16) };
                    break;
                case States.SmallBridge:
                    animation = new [] { new Point(48, 16) };
                    break;
                case States.PipeTop:
                    size = new Point(32, 16);
                    animation = new Point[] { new Point(0, 128) };
                    break;
                case States.PipeVert:
                    size = new Point(32, 16);
                    animation = new Point[] { new Point(0, 144) };
                    break;
                case States.PipeHoriz:
                    size = new Point(16, 32);
                    animation = new Point[] { new Point(48, 128) };
                    break;
                case States.PipeSide:
                    size = new Point(16, 32);
                    animation = new Point[] { new Point(32, 128) };
                    break;
                case States.PipeJoint:
                    size = new Point(32, 32);
                    animation = new Point[] { new Point(48, 128) };
                    break;
                case States.Flagpole:
                    animation = new Point[] { new Point(256, 144) };
                    break;
                case States.FlagCap:
                    animation = new Point[] { new Point(256, 128) };
                    break;
                case States.Flag:
                    animation = new Point[] { new Point(416, 128) };
                    break;
                case States.Castle:
                    size = new Point(80, 128);
                    animation = new Point[] { new Point(0, 0) };
                    themeOffset = false;
                    spritesheet = "Sprites/Castle";
                    break;
                case States.BulletLauncher:
                    size = new Point(16, 32);
                    animation = new Point[] { new Point(144, 0) };
                    break;
                case States.BanzaiLauncher:
                    size = new Point(32, 64);
                    animation = new Point[] { new Point(176, 320) };
                    themeOffset = false;
                    break;
                case States.Platform:
                    size = new Point(16, 7);
                    animation = new Point[] { new Point(0, 1) };
                    themeOffset = false;
                    spritesheet = "Sprites/MovingPlatformSmall";
                    break;
                case States.LavaTop:
                    size = new Point(16, 16);
                    animation = new Point[] { new Point(48, 320) };
                    break;
                case States.LavaBottom:
                    size = new Point(16, 16);
                    animation = new Point[] { new Point(48, 336) };
                    break;
                case States.Axe:
                    size = new Point(16, 16);
                    animation = new Point[] { new Point(384, 0), new Point(400, 0), new Point(416, 0) };
                    themeOffset = false;
                    break;
                case States.BossPlatformFull:
                case States.BossPlatformSmall:
                    size = new Point(16, 16);
                    animation = new Point[] { new Point(64, 384) };
                    themeOffset = false;
                    break;
                case States.Toad:
                    size = new Point(16, 28);
                    animation = new Point[] { new Point(0, 230) };
                    spritesheet = "Sprites/AdditionalItems";
                    themeOffset = false;
                    break;
                case States.Dirt:
                    size = new Point(16, 16);
                    animation = new Point[] { new Point(16, 80) };
                    break;
                case States.Hidden:
                default:
                    return NullSprite.Instance;
            }
            if (themeOffset) 
            {
                animation = GetThemeOffset(Theme, animation);
            }
            return new Sprite(Content.Load<Texture2D>(spritesheet), size, animation);

        }
    }
}