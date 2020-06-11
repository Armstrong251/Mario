using System;
using Mario.Levels;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Mario.Entities.mobs;
using System.Collections.Generic;
using System.Linq;

namespace Mario.Entities
{
    public enum Mobs
    {
        goomba,
        redKoopa,
        greenKoopa,
        RedFlyingKoopa,
        GreenFlyingKoopa,
        PiranhaPlant,
        BulletBill,
        FatBill,
        SuperBill,
        FakeBill,
        TurboBill,
        Spiny,
        SpinyEgg,
        Lakitu,
        LakituThrow,
        BooHiddn,
        BooChase,
        Bowser,
        AIBowser,
        HammerBro,
    }
    class MonsterFactory : Factory
    {
        enum States
        {
            Normal,
            Squash
        }
        public MonsterFactory(Level level) : base(level) { }
        static Dictionary<BowserEnum, Func<Sprite, Bowser, BowserState>> bowserStates = new Dictionary<BowserEnum, Func<Sprite, Bowser, BowserState>>()
        {
            [BowserEnum.Walking] = (s, k) => new BowserWalkingState(s, k),
            [BowserEnum.Breathing] = (s, k) => new BowserBreathingState(s, k),
            [BowserEnum.Dashing] = (s, k) => new BowserDashingState(s, k),
            [BowserEnum.Jumping] = (s, k) => new BowserJumpingState(s, k),
        };

        static Dictionary<BroEnum, Func<Sprite, HammerBro, BroState>> broStates = new Dictionary<BroEnum, Func<Sprite, HammerBro, BroState>>()
        {
            [BroEnum.WALKING] = (s, b) => new BroWalkingState(s, b),
            [BroEnum.JUMPING] = (s, b) => new BroJumpingState(s, b),
            [BroEnum.THROWING] = (s, b) => new BroThrowingState(s, b),
        };
        static Dictionary<KoopaEnum, Func<Sprite, Koopa, KoopaState>> koopaStates = new Dictionary<KoopaEnum, Func<Sprite, Koopa, KoopaState>>()
        {
            [KoopaEnum.FLYING] = (s, k) => new KoopaFlyingState(s, k),
            [KoopaEnum.WALKING] = (s, k) => new KoopaWalkingState(s, k),
            [KoopaEnum.IN_SHELL] = (s, k) => new KoopaShellState(s, k),
            [KoopaEnum.IN_SHELL_MOVING] = (s, k) => new KoopaShellMovingState(s, k),
            [KoopaEnum.IN_SHELL_TRANSITION] = (s, k) => new KoopaShellTransitionState(s, k),
        };
        public override Entity Create(Point position, Enum id)
        {
            switch ((Mobs)id)
            {
                case Mobs.goomba:
                    return new Goomba(Level, position, CreateSprite((Mobs)id), CreateSquashSprite((Mobs)id));
                case Mobs.SpinyEgg:
                case Mobs.Spiny:
                    return new Spiny(Level, position, CreateSprite(Mobs.SpinyEgg), CreateSprite(Mobs.Spiny));
                case Mobs.LakituThrow:
                case Mobs.Lakitu:
                    return new Lakitu(Level, position, CreateSprite(Mobs.Lakitu), CreateSprite(Mobs.LakituThrow));
                case Mobs.redKoopa:
                case Mobs.greenKoopa:
                case Mobs.GreenFlyingKoopa:
                case Mobs.RedFlyingKoopa:
                    Koopa k = Koopa.CreateKoopa(Level, position, (Mobs)id);
                    k.States = new StateList<KoopaState, KoopaEnum>(
                            ((Mobs)id == Mobs.GreenFlyingKoopa || (Mobs)id == Mobs.RedFlyingKoopa) ?
                                KoopaEnum.FLYING : KoopaEnum.WALKING,
                            e => koopaStates[e](CreateSprite(id, e), k));
                    return k;
                case Mobs.HammerBro:
                    HammerBro h = new HammerBro(Level, position);
                    h.BroStates = new StateList<BroState, BroEnum>(BroEnum.WALKING,
                        e => broStates[e](CreateSprite(id, e), h));
                    return h;
                case Mobs.PiranhaPlant:
                    return new PiranhaPlant(Level, position, CreateSprite((Mobs)id));
                case Mobs.BulletBill:
                    return new BulletBill(Level, position, CreateSprite((Mobs)id));
                case Mobs.SuperBill:
                    return new SuperBill(Level, position, CreateSprite((Mobs)id));
                case Mobs.FatBill:
                    return new FatBill(Level, position, CreateSprite((Mobs)id));
                case Mobs.FakeBill:
                    return new FakeBill(Level, position, CreateSprite((Mobs)id));
                case Mobs.TurboBill:
                    return new TurboBill(Level, position, CreateSprite((Mobs)id));
                case Mobs.BooHiddn:
                    return new Boo(Level, position, CreateSprite(Mobs.BooHiddn), CreateSprite(Mobs.BooChase));
                case Mobs.Bowser:
                    BowserWeak bowserWeak = new BowserWeak(Level, position);
                    bowserWeak.States = new StateList<BowserState, BowserEnum>(BowserEnum.Walking, e => bowserStates[e](CreateSprite(id, e), bowserWeak));
                    return bowserWeak;
                case Mobs.AIBowser:
                    AIBowser bowser = new AIBowser(Level, position);
                    bowser.States = new StateList<BowserState, BowserEnum>(BowserEnum.Walking, e => bowserStates[e](CreateSprite(id, e), bowser));
                    return bowser;
            }
            return null;
        }

        public override Sprite CreateSprite(Enum id, Enum state)
        {
            return CreateSprite((Mobs)id, state);
        }
        Sprite CreateSquashSprite(Mobs id)
        {
            Point size;
            Point[] animation;
            string spritesheet;
            bool themeOffset = true;
            switch (id)
            {
                case Mobs.goomba:
                    size = new Point(16);
                    animation = new[] { new Point(32, 16) };
                    spritesheet = "Sprites/Enimies2";
                    break;
                default:
                    return NullSprite.Instance;
            }
            if (themeOffset) animation = animation.Select(p => p + offsets[Theme]).ToArray();
            return new Sprite(Content.Load<Texture2D>(spritesheet), size, animation);
        }
        static Dictionary<string, Point> offsets = new Dictionary<string, Point>()
        {
            ["Plains"] = new Point(0, 0),
            ["Cave"] = new Point(0, 32),
            ["Castle"] = new Point(0, 96),
            ["DarkPlains"] = new Point(0, 0),
            ["Space"] = new Point(0, 128),
        };
        Sprite CreateSprite(Mobs ID)
        {
            return CreateSprite(ID, null);
        }

        //Just like before this is where the complexity goes.
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        Sprite CreateSprite(Mobs ID, Enum state)
        {
            Point size;
            Point[] animation;
            string spritesheet = "Sprites/Enimies2";
            bool themeOffset = true;
            switch (ID)
            {
                case Mobs.goomba:
                    size = new Point(16, 16);
                    animation = new Point[] { new Point(0, 16), new Point(16, 16) };
                    break;
                case Mobs.GreenFlyingKoopa:
                case Mobs.greenKoopa:
                        switch ((KoopaEnum)state)
                        {

                            case KoopaEnum.FLYING:
                                size = new Point(16, 24);
                                animation = new Point[] { new Point(128, 8), new Point(144, 8) };
                                break;
                            case KoopaEnum.WALKING:
                                size = new Point(16, 24);
                                animation = new Point[] { new Point(96, 8), new Point(112, 8) };
                                break;
                            case KoopaEnum.IN_SHELL:
                            case KoopaEnum.IN_SHELL_MOVING:
                                size = new Point(16, 16);
                                animation = new Point[] { new Point(160, 16) };
                                break;
                            case KoopaEnum.IN_SHELL_TRANSITION:
                                size = new Point(16, 16);
                                animation = new Point[] { new Point(176, 16) };
                                break;
                            default:
                                return NullSprite.Instance;
                        }
                    break;
                case Mobs.RedFlyingKoopa:
                case Mobs.redKoopa:
                    if (offsets[Theme].Y != 128)
                    {
                        switch ((KoopaEnum)state)
                        {
                            case KoopaEnum.FLYING:
                                size = new Point(16, 24);
                                animation = new Point[] { new Point(128, 72), new Point(144, 72) };
                                break;
                            case KoopaEnum.WALKING:
                                size = new Point(16, 24);
                                animation = new Point[] { new Point(96, 72), new Point(112, 72) };
                                break;
                            case KoopaEnum.IN_SHELL:
                            case KoopaEnum.IN_SHELL_MOVING:
                                size = new Point(16, 16);
                                animation = new Point[] { new Point(160, 80) };
                                break;
                            case KoopaEnum.IN_SHELL_TRANSITION:
                                size = new Point(16, 16);
                                animation = new Point[] { new Point(176, 80) };
                                break;
                            default:
                                return NullSprite.Instance;
                        }
                    }
                    else
                    {
                        switch ((KoopaEnum)state)
                        {
                            case KoopaEnum.FLYING:
                                size = new Point(16, 24);
                                animation = new Point[] { new Point(128, 168), new Point(144, 168) };
                                break;
                            case KoopaEnum.WALKING:
                                size = new Point(16, 24);
                                animation = new Point[] { new Point(96, 168), new Point(112, 168) };
                                break;
                            case KoopaEnum.IN_SHELL:
                            case KoopaEnum.IN_SHELL_MOVING:
                                size = new Point(16, 16);
                                animation = new Point[] { new Point(160, 176) };
                                break;
                            case KoopaEnum.IN_SHELL_TRANSITION:
                                size = new Point(16, 16);
                                animation = new Point[] { new Point(176, 176) };
                                break;
                            default:
                                return NullSprite.Instance;
                        }
                    }
                    themeOffset = false;
                    break;
                case Mobs.HammerBro:
                    switch ((BroEnum)state)
                    {
                        case BroEnum.THROWING:
                            animation = new[] { new Point(352, 8), new Point(368, 8) };
                            break;
                        case BroEnum.JUMPING:
                            animation = new[] { new Point(336, 8) };
                            break;
                        case BroEnum.WALKING:
                            animation = new[] { new Point(320, 8), new Point(336, 8) };
                            break;
                        default:
                            return NullSprite.Instance;
                    }
                    size = new Point(16, 24);
                    break;
                case Mobs.PiranhaPlant:
                    size = new Point(16, 24);
                    animation = new Point[] { new Point(191, 8), new Point(208, 8) };
                    break;
                case Mobs.BulletBill:
                    size = new Point(16, 16);
                    animation = new Point[] { new Point(560, 16) };
                    break;
                case Mobs.SuperBill:
                case Mobs.FakeBill:
                    size = new Point(16, 16);
                    animation = new Point[] { new Point(576, 16) };
                    themeOffset = false;
                    break;
                case Mobs.FatBill:
                    size = new Point(64, 64);
                    animation = new Point[] { new Point(0, 0) };
                    spritesheet = "Sprites/BanzaiBillL";
                    themeOffset = false;
                    break;
                case Mobs.TurboBill:
                    size = new Point(16, 16);
                    animation = new Point[] { new Point(576, 48) };
                    themeOffset = false;
                    break;
                case Mobs.SpinyEgg:
                    size = new Point(16, 16);
                    animation = new Point[] { new Point(448, 80), new Point(464, 80) };
                    themeOffset = false;
                    break;
                case Mobs.Spiny:
                    size = new Point(16, 16);
                    animation = new Point[] { new Point(480, 80), new Point(496, 80) };
                    themeOffset = false;
                    break;
                case Mobs.Lakitu:
                    size = new Point(16, 24);
                    animation = new Point[] { new Point(416, 8) };
                    themeOffset = false;
                    break;
                case Mobs.LakituThrow:
                    size = new Point(16, 24);
                    animation = new Point[] { new Point(432, 8) };
                    themeOffset = false;
                    break;
                case Mobs.BooChase:
                    spritesheet = ("Sprites/BooChase");
                    size = new Point(16, 16);
                    animation = new Point[] { new Point(0, 0) };
                    themeOffset = false;
                    break;
                case Mobs.BooHiddn:
                    spritesheet = ("Sprites/BooHide");
                    size = new Point(16, 16);
                    animation = new Point[] { new Point(0, 0) };
                    themeOffset = false;
                    break;
                case Mobs.Bowser:
                    size = new Point(32, 32);
                    themeOffset = false;
                    switch ((BowserEnum)state)
                    {
                        case BowserEnum.Breathing:
                            animation = new Point[] { new Point(655, 0) };
                            break;
                        case BowserEnum.Walking:
                            animation = new Point[] { new Point(720, 0), new Point(752, 0) };
                            break;
                        case BowserEnum.Dashing:
                            animation = new Point[] { new Point(720, 0), new Point(752, 0) };
                            break;
                        default:
                            animation = new Point[] { new Point(720, 0) };
                            break;
                    }
                    break;
                case Mobs.AIBowser:
                    size = new Point(32, 32);
                    themeOffset = false;
                    switch ((BowserEnum)state)
                    {
                        case BowserEnum.Breathing:
                            animation = new Point[] { new Point(655, 33) };
                            break;
                        case BowserEnum.Walking:
                            animation = new Point[] { new Point(720, 33), new Point(752, 33) };
                            break;
                        case BowserEnum.Dashing:
                            animation = new Point[] { new Point(720, 33), new Point(752, 33) };
                            break;
                        default:
                            animation = new Point[] { new Point(720, 33) };
                            break;
                    }
                    break;
                default:
                    return NullSprite.Instance;
            }
            if (themeOffset) animation = animation.Select(p => p + offsets[Theme]).ToArray();
            return new Sprite(Content.Load<Texture2D>(spritesheet), size, animation);
        }
    }
}