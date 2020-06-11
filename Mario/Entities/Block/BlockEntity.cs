using Mario.Entities.Commands;
using Mario.Entities.Mario;
using Mario.Input;
using Mario.Levels;
using Microsoft.Xna.Framework;
using Mario.Entities.mobs;
using Microsoft.Xna.Framework.Graphics;
using static Mario.Input.GameEvent;

namespace Mario.Entities.Block
{
    public class BlockEntity : Entity, IEventObserver
    {
        private ItemType? storedItem;
        private string storedLevel;

        public StateList<BlockState, States> BlockStates { get; set; }
        public override bool SolidTo(IEntity other, Point dir) => BlockStates.State.SolidTo(other, dir);

        public override Sprite SpriteOverride => BlockStates.State.Sprite;
        public ItemType? StoredItem
        {
            get => storedItem;
            set
            {
                storedItem = value;
                if(StoredItemCount == 0) StoredItemCount = 1;
                if(storedItem.HasValue) BlockStates.Handle(b => b.HandleItemAdd(value.Value));
            }
        }
        public string StoredLevel
        {
            get => storedLevel;
            set
            {
                storedLevel = value;
            }
        }
        public int StoredItemCount { get; set; } = 0;
        public BlockEntity(Level l, Point position) : base(l, position)
        {
            Level.Manager.AddObserver(this, new GameEvent[]{
                QUESTION_BLOCK_HIT,
                USED_BLOCK_HIT,
                BRICK_BLOCK_HIT,
                HIDDEN_BLOCK_HIT,
            });
            BoundingBox = new Bounds(this, Color.Blue, new Vector2(0, 0), new Vector2(16, 16));
            Solid = true;
        }
        public void DrawBase(SpriteBatch batch)
        {
            base.Draw(batch);
        }
        public override void Draw(SpriteBatch batch)
        {
            BlockStates.State.HandleDraw(batch);
        }

        public static readonly (Vector2 dir, int repeat) BumpMovement = (new Vector2(0, -2), 5);
        public void RevealItem(MarioContext activator)
        {
            if(StoredItemCount == 0) return;
            Bump();
            if (StoredItem == ItemType.MUSHROOM
                && activator.PowerUpStates.CurrentState != PowerUpEnum.NORMAL)
            {
                StoredItem = ItemType.FIRE_FLOWER;
            }
            IEntity item = Level.Spawn(typeof(ItemEntity), Position, StoredItem.Value);
            if (StoredItem.Value != ItemType.BLOCK_COIN)
            {
                item.Commands += new Command[]{
                    new Move(0, -1).Repeat(16),
                    new MethodCall<ItemEntity>((i) => Level.MusicPlayer.PlaySoundEffect("powerup_appears"))
                };
            }
            else
            {
                item.Position += new Vector2(0, -18);
            }
            StoredItemCount--;
        }
        public void Bump()
        {
            foreach (var entity in Level.Collider.GridAt(Position - new Vector2(0, 16)).ToArray())
            {
                if(entity is MonsterEntity monster) monster.Kill(DeathType.Fall);
                else if(entity is Coin coin) coin.OnCollected();
            }
            Level.MusicPlayer.PlaySoundEffect("bump");
            // foreach(var m in Block.CollidingWith.Where(e => e is MonsterEntity)) (m as MonsterEntity).Kill(DeathType.Fall);
            if (Commands.IsNext<Move>())
            {
                Move m = Commands.GetNext<Move>();
                if (m.Direction == BumpMovement.dir)
                {
                    Commands += new[] {
                                    new Move(BumpMovement.dir).Repeat(BumpMovement.repeat - (m.Condition as Repeat).Count + 1),
                                    new Move(BumpMovement.dir * -1).Repeat(BumpMovement.repeat)
                                };
                    (m.Condition as Repeat).Count = 0;
                }
            }
            else
            {
                Commands += new[] {
                                new Move(BumpMovement.dir).Repeat(BumpMovement.repeat),
                                new Move(BumpMovement.dir * -1).Repeat(BumpMovement.repeat)
                            };
            }
        }

        public void Shatter()
        {
            foreach (var entity in Level.Collider.GridAt(Position - new Vector2(0, 16)).ToArray())
            {
                if (!(entity is FatBill))
                {
                    if (entity is MonsterEntity monster) monster.Kill(DeathType.Fall);
                    else if (entity is Coin coin) coin.OnCollected();
                }
            }
            foreach (int i in new[] { -2, -1, 1, 2 })
            {
                BlockEntity b = (BlockEntity)(Level.Spawn(typeof(BlockEntity), Position + new Point(i, 0), States.BabyBrick));
                b.Velocity = new Vector2(i / 2f, -2);
            }
            Level.MusicPlayer.PlaySoundEffect("break");
            BoundingBox.Active = false;
            Destroy();
        }
        public override void Update()
        {
            BlockStates.Update();
        }
        public override void Destroy()
        {
            Level.Manager.RemoveObserver(this);
            base.Destroy();
        }

        public void OnEventTriggered(GameEvent e)
        {
            States? s = null;
            switch (e)
            {
                case QUESTION_BLOCK_HIT:
                    s = States.Question;
                    break;
                case USED_BLOCK_HIT:
                    s = States.Used;
                    break;
                case BRICK_BLOCK_HIT:
                    s = States.Brick;
                    break;
                case HIDDEN_BLOCK_HIT:
                    s = States.Hidden;
                    break;
            }
            if (s == BlockStates.CurrentState) BlockStates.HandleEvent(e);
        }

        public override void OnCollision(IEntity other, Point direction)
        {
            BlockStates.HandleCollision(other, direction);
            base.OnCollision(other, direction);
        }
    }
}
