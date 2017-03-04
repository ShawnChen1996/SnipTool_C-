using System;
using System.Drawing;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Linq;
using System.Text;
using System.IO;

namespace SnipTool
{
    class CatchForm: Form
    {
        public CatchForm()
        {
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.WindowState = FormWindowState.Maximized;
            this.MouseDown += CatchForm_MouseDown;
            this.MouseUp += CatchForm_MouseUp;
            this.MouseMove += CatchForm_MouseMove;
            this.Load += CatchForm_Load;
            
            this.ShowInTaskbar = false;
            this.PreviewKeyDown += CatchForm_PreviewKeyDown;
            this.MouseDoubleClick += CatchForm_MouseDoubleClick;

            //取电脑全屏
            
            int sWidth = Screen.AllScreens[0].Bounds.Width ;
            int sHeight = Screen.AllScreens[0].Bounds.Height ;
            originBmp = new Bitmap(sWidth, sHeight);
            Graphics g = Graphics.FromImage(originBmp);
            g.CopyFromScreen(new Point(0, 0), new Point(0, 0), new Size(sWidth, sHeight));
            g.Dispose();
            bkGround = (Bitmap)originBmp.Clone();
            Graphics g1 = Graphics.FromImage(bkGround);
            g1.DrawRectangle(new Pen(Color.Orange, 3), new Rectangle(0, 0, originBmp.Width, originBmp.Height));
            g1.Dispose();
            //保存为用户变量
            this.BackgroundImage = bkGround;
        }

  

   

        void CatchForm_Load(object sender, EventArgs e)
        {
            //设置控件样式为双缓冲，这可以有效减少图片闪烁的问题
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer |
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.UserPaint, true);

            this.UpdateStyles();

        }

        void CatchForm_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            
            if (Rec.Contains(e.Location))
            {
                if (catchedBmp != null)
                {
                    Clipboard.SetImage(catchedBmp);
                    MyDispose();
                }

            }
        }

        private void MyDispose ()
        {
            if (catchedBmp != null)
            {
                catchedBmp.Dispose();
            }
            originBmp.Dispose();
            bkGround.Dispose();
            base.Dispose();
        }

        void CatchForm_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                MyDispose();
            }
            if (e.KeyCode == Keys.C && e.Modifiers== Keys.Control)
            {
                if(catchedBmp != null)
                {
                    Clipboard.SetImage(catchedBmp);
                    MyDispose();
                }
                    
                    //DrawFuncs funcs = new DrawFuncs();
                    //funcs.Location = new Point(Rec.X + Rec.Width - 200, Rec.Bottom + 30);
                    //funcs.Show();
                    //funcs.BringToFront()
            }
            if (e.KeyCode == Keys.S && e.Modifiers == Keys.Control)
            {
                if(catchedBmp != null)
                {
                    SaveFileDialog saveFile = new SaveFileDialog();
                    saveFile.Filter = "jpg文件|*.jpg|png图片|*.png|gif图片|*.gif";
                    string path = string.Empty;
                    if (saveFile.ShowDialog() == DialogResult.OK)
                    {
                        path = saveFile.FileName;
                        catchedBmp.Save(path);
                    }
                    saveFile.Dispose();
                    MyDispose();
                }

            }
        }

        #region 用户变量
                Rectangle Rec;
                private Point DownPoint = Point.Empty;
                private bool Catching = false;
                private Bitmap originBmp; 
                Bitmap bkGround;
                private bool mouseMoved = false;
                Bitmap catchedBmp = null;
        #endregion
        
        void CatchForm_MouseMove(object sender, MouseEventArgs e)
        {
            if (Catching)
            {
                Point newPoint = new Point(e.X,e.Y);
                int width = Math.Abs(newPoint.X - DownPoint.X);
                int height = Math.Abs(newPoint.Y - DownPoint.Y);
                if (width != 0 && height != 0)
                {
                    mouseMoved = true;
                    Bitmap curBmp = new Bitmap(bkGround, bkGround.Size);
                    if (newPoint.X - DownPoint.X > 0)
                    {
                        newPoint.X = DownPoint.X;

                    }
                    if (newPoint.Y - DownPoint.Y > 0)
                    {
                        newPoint.Y = DownPoint.Y;
                    }

                    Rec = new Rectangle(newPoint, new Size(width, height));

                    Graphics g = Graphics.FromImage(curBmp);
                    g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighSpeed;
                    g.DrawRectangle(new Pen(Color.Blue, 2), Rec);
                    g.Dispose();

                    Graphics g1 = this.CreateGraphics();
                    g1.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighSpeed;
                    g1.DrawImage(curBmp, new Point(0, 0));
                    curBmp.Dispose();
                    g1.Dispose();

                }
                    
                
                
            }
        }

        void CatchForm_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Catching = false;

                if (mouseMoved)
                {
                    try
                    {
                        if (catchedBmp != null)
                        {
                            catchedBmp.Dispose();
                            catchedBmp = null;
                        }    
                        catchedBmp = new Bitmap(Rec.Width, Rec.Height);
                        Graphics g = Graphics.FromImage(catchedBmp);
                        g.DrawImage(originBmp, new Rectangle(new Point(0, 0), new Size(Rec.Width, Rec.Height)), Rec, GraphicsUnit.Pixel);
                        g.Dispose();
                        
                    }
                    catch (ArgumentException) { }
                }
                else
                {
                    try
                    {
                        Graphics g1 = this.CreateGraphics();
                        g1.DrawImage(bkGround, new Point(0, 0));
                        g1.Dispose();   
                    }
                    catch (ObjectDisposedException) { }
                       
                }
            }
        }

        void CatchForm_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                DownPoint = e.Location;
                Catching = true;
                mouseMoved = false;
            }
        }

        

       
    }
}
