using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;


using Mario.Entities;
using Microsoft.Xna.Framework.Graphics;

namespace Mario
{
    public class BackgroundLayer : IDisposable
    {

        private List<Sprite> sprites;

        private List<Point> positions;
        
        private Camera camera;

        private Point position;

        private RenderTarget2D canvas;

        private GraphicsDevice graphicsDevice;

        private Rectangle layerSize;

        private Vector2 parallax;

        private bool rendered;

        public BackgroundLayer(GraphicsDevice graphicsDevice, Camera camera, Point position, Point tileSize, Point layerSize, Vector2 parallax)
        {

            sprites = new List<Sprite>();
            positions = new List<Point>();

            this.camera = camera;

            this.position = position;

            this.graphicsDevice = graphicsDevice;

            canvas = new RenderTarget2D(this.graphicsDevice, tileSize.X, tileSize.Y);

            this.layerSize = new Rectangle(Point.Zero, layerSize);

            this.parallax = parallax;

            rendered = false;
            
        }

        public void AddSprite(Sprite sprite, Point canvasPosition)
        {

            sprites.Add(sprite);
            positions.Add(canvasPosition);
            if (rendered) { 
                this.Render();
            }
        }

        public void Render()
        {
            SpriteBatch batch = new SpriteBatch(graphicsDevice);
            graphicsDevice.SetRenderTarget(canvas);
            graphicsDevice.Clear(Color.Transparent);

            batch.Begin();
            for (int i = 0; i < sprites.Count; i++)
            {
                sprites[i].Draw(batch, positions[i]);
            }
            batch.End();

            graphicsDevice.SetRenderTarget(null);

            rendered = true;
        }

        public void AddSprite(Sprite sprite, int x, int y)
        {
            this.AddSprite(sprite, new Point(x, y));
        }


        public void Draw(SpriteBatch spriteBatch)
        {
            if (!rendered)
            {
                this.Render();
            }
            spriteBatch.Begin(samplerState: SamplerState.PointWrap, transformMatrix: camera.GetViewMatrix(parallax));
            spriteBatch.Draw(canvas, position.ToVector2(), layerSize, Color.White);
            spriteBatch.End();
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    canvas.Dispose();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~BackgroundLayer()
        // {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion


    }
}
