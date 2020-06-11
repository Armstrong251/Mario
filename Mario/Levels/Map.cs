using System;
using System.Collections.Generic;
using System.IO;
using Mario.Entities;
using Mario.Entities.Block;
using Mario.Entities.Mario;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Mario.Levels
{
    public class Map
    {
        private Level Level;
        public int Width { get; private set; }
        public int Height { get; private set; }
        public BlockEntity[,] blockGrid;
        private int minDone, maxDone;
        private Dictionary<Type, Layer> layers;
        //private string levelFolder;
        //private MarioGame mg;
        public Map(Level level, string levelFolder)//, MarioGame mg)
        {
            //this.levelFolder = levelFolder;
            Level = level;
            //this.mg = mg;
            layers = new Dictionary<Type, Layer>();
            LoadMap(typeof(BlockEntity), levelFolder, "BlockMap");
            LoadMap(typeof(MarioContext), levelFolder, "SpawnMap");
            LoadMap(typeof(MonsterEntity), levelFolder, "EnemyMap");
            LoadMap(typeof(ItemEntity), levelFolder, "CPMap");
            LoadMap(typeof(Checkpoint), levelFolder, "CheckpointMap");
            Width = layers[typeof(BlockEntity)].Width;
            Height = layers[typeof(BlockEntity)].Height;
            blockGrid = new BlockEntity[Width, Height];
            minDone = maxDone = (int)Level.RespawnPosition.X / 16;
        }
        public void InitMap(LevelProperties lp)
        {
            Spawn(typeof(BlockEntity), 0, Width);
            Spawn(typeof(MarioContext), 0, Width);
            Spawn(typeof(Checkpoint), 0, Width);
            lp.LoadProperties(Level, "Map");
        }
        public void SpawnDynamic(int min, int max)
        {
            if(min < 0) min = 0;
            if(max > Width) max = Width;
            if (min < minDone)
            {
                Spawn(typeof(MonsterEntity), min, MathHelper.Min(minDone, max));
                Spawn(typeof(ItemEntity), min, MathHelper.Min(minDone, max));
            }
            if (max > maxDone)
            {
                Spawn(typeof(MonsterEntity), MathHelper.Max(min, maxDone), max);
                Spawn(typeof(ItemEntity), MathHelper.Max(min, maxDone), max);
            }
            minDone = MathHelper.Min(min, minDone);
            maxDone = MathHelper.Max(max, maxDone);
        }
        public void Spawn(Type type, int min, int max)
        {
            Layer l = layers[type];
            if(l == null) return;
            for (int x = min; x < max; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    if (l[x, y] != Color.White && l[x, y].A > 0)
                    {
                        if (type == typeof(ItemEntity) && blockGrid[x, y] != null)
                        {
                            var item = (ItemType)LevelReader.ColorTo(type, l[x, y]);
                            if(item == ItemType.WORLD_COIN) 
                            {
                                blockGrid[x, y].StoredItemCount = (200 - l[x, y].B) / 20;
                                blockGrid[x, y].StoredItem = ItemType.BLOCK_COIN;
                            }
                            else
                            {
                                blockGrid[x, y].StoredItem = item;
                            }
                        }
                        else if (type == typeof(MarioContext))
                        {
                            if(Level.RespawnPosition == Vector2.Zero) Level.RespawnPosition = new Vector2(x * 16, y * 16);
                        }
                        else
                        {
                            IEntity entity = Level.Spawn(type, new Point(x * 16, y * 16), LevelReader.ColorTo(type, l[x, y]));
                            if (type == typeof(BlockEntity)) blockGrid[x, y] = (BlockEntity)entity;
                        }
                    }
                }
            }
        }
        public void LoadMap(Type type, string folder, string file)
        {
            string path = Level.Content.RootDirectory + "/Levels/" + folder + "/" + file + ".png";
            if(File.Exists(path))
            {
                FileStream fs = new FileStream(path, FileMode.Open);
                var texture = Texture2D.FromStream(Level.Game.GraphicsDevice, fs);
                layers[type] = new Layer(texture);
                fs.Close();
            }
            else
            {
                layers[type] = null;
            }
        }
    }
}