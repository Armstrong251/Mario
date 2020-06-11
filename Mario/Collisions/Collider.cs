using Mario.Entities;
using Mario.Entities.Commands;
using Mario.Levels;
using Microsoft.Xna.Framework;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics;
using Utils;
using Mario.Entities.Mario;
using Mario.Entities.Block;
using System.Drawing;
using Point = Microsoft.Xna.Framework.Point;
using System.Collections.Specialized;
using Mario.Entities.mobs;

namespace Mario.Collisions
{
    class Collider
    {
        private const int GRIDSIZE = 16;
        private HashSet<IEntity> moving;
        private Dictionary<(IEntity, IEntity), float> collisions;
        private List<(float, (IEntity, IEntity, Point))> timeSortedCollisions;
        private Dictionary<IEntity, RectangleF> stored;
        //private Level level;
        private List<IEntity>[,] grid;

        public Collider(Level level)
        {
            //this.level = level;
            moving = new HashSet<IEntity>();
            stored = new Dictionary<IEntity, RectangleF>();
            collisions = new Dictionary<(IEntity, IEntity), float>();
            timeSortedCollisions = new List<(float, (IEntity, IEntity, Point))>();
            grid = new List<IEntity>[(level.Width / GRIDSIZE), (level.Height / GRIDSIZE)];
            for (int i = 0; i < grid.GetLength(0); i++)
            {
                for (int j = 0; j < grid.GetLength(1); j++)
                {
                    grid[i, j] = new List<IEntity>();
                }
            }
        }

        public void Update()
        {
            collisions.Clear();
            timeSortedCollisions.Clear();
            Collide();
        }

        private static float Interp(float begin, float end, float mid)
        {
            if (end == begin) return mid - begin;
            return (mid - begin) / (end - begin);
        }
        private static (float, Point) Interp2(Vector2 begin, Vector2 end, Vector2 mid)
        {
            float xterp = Interp(begin.X, end.X, mid.X);
            float yterp = Interp(begin.Y, end.Y, mid.Y);
            Vector2 dist = end - begin;
            float interp = -1;
            Point direction = new Point(0, 0);
            if (xterp >= 0 && xterp <= 1 && xterp >= yterp)
            {
                if (xterp == yterp)
                {
                    if (dist.X != 0 && dist.Y != 0)
                    {
                        direction = new Point(Math.Sign(dist.X), Math.Sign(dist.Y));
                    }
                    else
                    {
                        return (-1, Point.Zero);
                    }
                }
                else
                {

                    direction = new Point(Math.Sign(dist.X), 0);
                }
                interp = xterp;
            }
            if (yterp >= 0 && yterp <= 1 && yterp > xterp)
            {
                direction = new Point(0, Math.Sign(dist.Y));
                interp = yterp;
            }
            return (interp, direction);
        }
        //mid must be between begin and end
        private static (float, Point) Interp4(RectangleF begin, RectangleF end, RectangleF mid)
        {
            if (begin.IntersectsWith(mid)) return (0, Point.Zero);
            Vector2 velocity = end.Location.ToVec2() - begin.Location.ToVec2();
            (float, Point) interp;
            if (velocity.X < 0 && velocity.Y < 0)
            {
                interp = Interp2(begin.TL(), end.TL(), mid.BR());
            }
            else if (velocity.X >= 0 && velocity.Y < 0)
            {
                interp = Interp2(begin.TR(), end.TR(), mid.BL());
            }
            else if (velocity.X < 0 && velocity.Y >= 0)
            {
                interp = Interp2(begin.BL(), end.BL(), mid.TR());
            }
            else //if x >= 0 and y >= 0
            {
                interp = Interp2(begin.BR(), end.BR(), mid.TL());
            }
            return interp;
        }
        public static (float, Point) InterpEntity(IEntity one, IEntity two)
        {
            return Interp4(one.BoundingBox.PrevRect, one.BoundingBox.Rectangle.OffsetConst(two.PrevPosition - two.Position), two.BoundingBox.PrevRect);
        }

        public void Collide()
        {
            foreach (var one in moving.ToArray())
            {
                AddAllCollidingWith(one);
            }
            var tc = new TupleCompare();
            timeSortedCollisions.Sort(tc);
            while(timeSortedCollisions.Count > 0)
            {
                var pair = timeSortedCollisions[0];
                timeSortedCollisions.RemoveAt(0);
                (float interp, (IEntity one, IEntity two, Point dir)) = pair;
                if(dir.X != 0 && dir.Y != 0)
                {
                    int i = 0;
                    for(; i < timeSortedCollisions.Count 
                                && timeSortedCollisions[i].Item1 == pair.Item1
                                && (timeSortedCollisions[i].Item2.Item3.X == 0 || timeSortedCollisions[i].Item2.Item3.Y == 0); i++);
                    if(i > 0) 
                    {
                        timeSortedCollisions.Insert(i, pair);
                        continue;
                    }
                }
                CheckCollision(one, two, interp, dir, tc);
                CheckCollision(two, one, interp, dir.Mult(-1), tc);
            }
            moving.Clear();
        }
        public void CheckCollision(IEntity one, IEntity two, float interp, Point dir, TupleCompare tc)
        {
            bool moved = DoCollision(one, two, interp, dir);
            if (moved)
            {
                foreach (var c in timeSortedCollisions.Where(i => i.Item1 >= interp && (i.Item2.Item1 == one || i.Item2.Item2 == one)).ToArray())
                {
                    timeSortedCollisions.Remove(c);
                    var t = c;
                    (t.Item1, t.Item2.Item3) = InterpEntity(t.Item2.Item1, t.Item2.Item2);
                    if (t.Item1 >= 0 && t.Item1 <= 1 && t.Item2.Item1.BoundingBox.Intersects(t.Item2.Item2.BoundingBox))
                    {
                        ReAdd(t, tc);
                    }
                    else if (c.Item1 == interp && (c.Item2.Item3.X == 0 || c.Item2.Item3.Y == 0))
                    {
                        ReAdd(c, tc);
                    }
                }
            }
        }
        public void ReAdd((float, (IEntity, IEntity, Point)) item, TupleCompare tc)
        {
            int location = timeSortedCollisions.BinarySearch(item, tc);
            timeSortedCollisions.Insert(location < 0 ? ~location : location, item);
        }
        private void AddAllCollidingWith(IEntity one)
        {

            //RectangleF prev = one.BoundingBox.PrevRect, rect = one.BoundingBox.Rectangle;
            ForEachInArea(one.BoundingBox.SweptRect,
                g =>
                {
                    foreach (var two in g.ToArray())
                    {
                        (IEntity, IEntity) ot = (one, two), to = (two, one);
                        if (two != one && two.BoundingBox.Active
                            && !collisions.ContainsKey(to) && !collisions.ContainsKey(ot)
                            && two.BoundingBox.Intersects(one.BoundingBox))
                        {
                            (float, Point) interp = InterpEntity(one, two);
                            if (interp.Item1 >= 0 && interp.Item1 <= 1)
                            {
                                timeSortedCollisions.Add((interp.Item1, (one, two, interp.Item2)));
                            }
                            collisions.Add(ot, interp.Item1);
                            collisions.Add(to, interp.Item1);
                        }
                    }
                }
            );
        }
        public static void CollisionBase(IEntity one, IEntity two, Point dir)
        {
            one.OnCollision(two, dir);
            if (!one.Commands.IsNext<CollisionCue>()) one.Commands += new CollisionCue().Repeat(CollisionCue.LENGTH);
            else (one.Commands.GetNext<CollisionCue>().Condition as Repeat).Count = CollisionCue.LENGTH;
        }
        private static bool DoCollision(IEntity one, IEntity two, float interp, Point dir)
        {
            CollisionBase(one, two, dir);
            if (!one.ForceMove
                    && one.BoundingBox.Active && two.BoundingBox.Active
                    && Entity.SolidTo(one, two, dir))
            {
                Vector2 direction = one.Position - one.PrevPosition;
                Vector2 newPos = one.PrevPosition + direction * interp;
                if (dir.X != 0 && dir.Y != 0)
                {
                    //only stop in one direction
                    dir = (((direction + direction.Flip()).Sign() - (direction.Flip() - direction).Sign()) * direction.Sign() * direction.Flip().Sign()).Mult(0.5f).Flip();
                }
                Point scale = new Point(1) - dir.Mult(dir);
                //extra movement in the direction the collision didn't happen
                Vector2 offset = (one.Position - newPos) * scale.ToVector2();
                one.PrevPosition = newPos;
                one.Position = newPos + offset;
                one.Velocity += dir.ToVector2() * PointExt.Min(one.Velocity * dir.Mult(-1), Vector2.Zero);
                if (two.ForceMove && one.BoundingBox.Intersects(two.BoundingBox) && !(one is BlockEntity))
                {
                    VectorPoint twoDir = two.Position - two.PrevPosition;
                    (interp, dir) = Interp4(one.BoundingBox.Rectangle.OffsetConst(twoDir), one.BoundingBox.Rectangle, two.BoundingBox.Rectangle);
                    if (interp >= 0 && interp <= 1)
                    {
                        one.Position += twoDir * (1 - interp);
                        one.Velocity += twoDir;
                    }
                }
                return true;
            }
            else
            {
                return false;
            }
        }
        // public static Point Raycast(Map map, Vector2 position, Vector2 direction, float maxSq)
        // {
        //     Point tile = Point.FloorToInt(position);
        //     Point dtile = new Point((int)Mathf.Sign(direction.x), (int)Mathf.Sign(direction.y));
        //     Vector2 dt = (tile + ((dtile + new Vector2(1, 1)) / 2) - position) / direction;
        //     Vector2 ddt = dtile / direction;
        //     float t = 0;
        //     while (map.GetLoc(tile).type == Node.Type.WALKABLE)
        //     {
        //         Point mask = (dt.x < dt.y) ? new Point(1, 0) : new Point(0, 1);
        //         float dtTmp = Mathf.Min(dt.x, dt.y);
        //         tile += mask * dtile;
        //         t += dtTmp;
        //         dt += ddt * mask - new Vector2(dtTmp, dtTmp);
        //         if ((tile - position).sqrMagnitude > maxSq)
        //         {
        //             tile = new Point(-10, -10);
        //         }
        //     }
        //     return tile;
        // }

        private bool InBounds(int x, int y)
        {
            return x >= 0 && y >= 0
                && x < grid.GetLength(0) && y < grid.GetLength(1);
        }
        public void UpdateLocation(IEntity e)
        {
            Remove(e);
            if (InBounds((int)e.Position.X / GRIDSIZE, (int)e.Position.Y / GRIDSIZE))
            {
                if (e.BoundingBox.Active)
                {
                    Add(e);
                    if (e.Position != e.PrevPosition) moving.Add(e);
                }
            }
            else
            {
                e.Destroy();
            }
        }
        public void Add(IEntity e)
        {
            ForEachInArea(e.BoundingBox.SweptRect, list => { if (!list.Contains(e)) list.Add(e); });
            stored[e] = e.BoundingBox.SweptRect;
        }
        public void Remove(IEntity e)
        {
            if (!stored.ContainsKey(e)) return;
            Remove(e, stored[e]);
            stored.Remove(e);
        }
        private void Remove(IEntity e, RectangleF r)
        {
            ForEachInArea(r, list => list.Remove(e));
        }
        public List<IEntity> GridAt(Point p)
        {
            if(!InBounds(p.X / GRIDSIZE, p.Y / GRIDSIZE)) return null;
            return grid[p.X / GRIDSIZE, p.Y / GRIDSIZE];
        }
        // public void Test()
        // {
        //     for (int i = 0; i < grid.GetLength(0); i++)
        //     {
        //         for (int j = 0; j < grid.GetLength(1); j++)
        //         {
        //             foreach (var e in grid[i, j].ToArray())
        //             {
        //                 if (!e.BoundingBox.Rectangle.Intersects(new Rectangle(i * 16, j * 16, 16, 16)))
        //                 {
        //                     Console.WriteLine("oops" + e.GetType());
        //                     grid[i, j].Remove(e);
        //                 }
        //             }
        //         }
        //     }
        // }
        public void ForEachInArea(RectangleF r, Action<List<IEntity>> a)
        {
            for (int i = (int)r.Left / GRIDSIZE; i < Math.Ceiling(r.Right / GRIDSIZE); i++)
            {
                for (int j = (int)r.Top / GRIDSIZE; j < Math.Ceiling(r.Bottom/ GRIDSIZE); j++)
                {
                    if (InBounds(i, j)) a.Invoke(grid[i, j]);
                }
            }
        }
    }
    class TupleCompare : IComparer<(float, (IEntity, IEntity, Point))>
    {
        public int Compare((float, (IEntity, IEntity, Point)) x, (float, (IEntity, IEntity, Point)) y)
        {
            return x.Item1.CompareTo(y.Item1);
        }
    }
}
