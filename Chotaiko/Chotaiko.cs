using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Chotaiko.Chart;
using Chotaiko.Game;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;

namespace Chotaiko
{
    class Chotaiko : GameWindow
    {
        public Chotaiko() : base(800, 600, GraphicsMode.Default, "Chotaiko")
        {
            VSync = VSyncMode.On;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            GL.ClearColor(0.1f, 0.2f, 0.5f, 0.0f);
            GL.Enable(EnableCap.DepthTest);
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            GL.Viewport(ClientRectangle.X, ClientRectangle.Y, ClientRectangle.Width, ClientRectangle.Height);

            Matrix4 projection = Matrix4.CreatePerspectiveFieldOfView((float)Math.PI / 4, Width / (float)Height, 1.0f, 64.0f);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadMatrix(ref projection);
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);

            ChotaikoGlobalContext.UpdateGameContext();
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            GL.Begin(PrimitiveType.Polygon);

            ChotaikoGlobalContext.DrawGameContext();

            GL.End();

            SwapBuffers();
        }

        static void ChotaikoOnKeyDown(object sender, KeyboardKeyEventArgs e)
        {
            if (!e.IsRepeat)
            {
                ChotaikoKey l = new ChotaikoKey();
                ChotaikoGlobalContext.OnPress(l);
            }
        }

        [STAThread]
        static void Main()
        {
            ChotaikoChart TestChart = new ChotaikoChart(new System.IO.StreamReader(@"../../TestChart.ctc"));
            ChotaikoGameContext TestGameContext = new ChotaikoGameContext(TestChart);
            ChotaikoGlobalContext.SetGameContext(TestGameContext);
            // The 'using' idiom guarantees proper resource cleanup.
            // We request 30 UpdateFrame events per second, and unlimited
            // RenderFrame events (as fast as the computer can handle).
            using (Chotaiko game = new Chotaiko())
            {
                game.KeyDown += ChotaikoOnKeyDown;
                game.Run(30.0);
            }
        }
    }
}
