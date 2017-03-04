using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace SnipTool
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            noteIcon.Icon = this.Icon;
            noteIcon.MouseClick += noteIcon_MouseClick;
            this.WindowState = FormWindowState.Minimized;
            noteIcon.ContextMenuStrip = contextMenuStrip1;
            this.ShowInTaskbar = false; 
        }

     


        /*
namespace Snip
{
    public partial class CatchForm: Form
    {

        Button bCatch = new Button() {Text="Snip"};
        RichTextBox richtextbox1 = new RichTextBox();

        public CatchForm()
        {
            this.Controls.Add(bCatch);
            this.Controls.Add(richtextbox1);
            richtextbox1.Size = this.Size;
            bCatch.Click +=bCatch_Click;
            bCatch.Location = new Point(this.Width / 2 - bCatch.Width / 2, this.Height / 2 - bCatch.Height / 2);
        }
        private void bCatch_Click(object sender, EventArgs e)
        {
                this.Hide();//隐藏当前窗体
                Thread.Sleep(500);//让线程睡眠一段时间，窗体消失需要一点时间
                Catch Catch = new Catch();
                Bitmap CatchBmp = new Bitmap(Screen.AllScreens[0].Bounds.Width, Screen.AllScreens[0].Bounds.Height);//新建一个和屏幕大小相同的图片         
                Graphics g = Graphics.FromImage(CatchBmp);
                g.CopyFromScreen(new Point(0, 0), new Point(0, 0), new Size(Screen.AllScreens[0].Bounds.Width, Screen.AllScreens[0].Bounds.Height));//保存全屏图片
                Catch.BackgroundImage = CatchBmp;//将Catch窗体的背景设为全屏时的图片
                if (Catch.ShowDialog() == DialogResult.OK)
                {
                    //如果Catch窗体结束,就将剪贴板中的图片放到信息发送框中
                    IDataObject iData = Clipboard.GetDataObject();
                    DataFormats.Format myFormat = DataFormats.GetFormat(DataFormats.Bitmap);
                    if (iData.GetDataPresent(DataFormats.Bitmap))
                    {
                        richtextbox1.Paste(myFormat);
                        //Clipboard.Clear();//清除剪贴板中的对象
                    }
                    bCatch.Hide();
                    this.Show();//重新显示窗体
                }
            }

        private void CatchForm_Load(object sender, EventArgs e)
        {

        }


        }
    }
         * 
         */

        /*
         
namespace Snip
{
    public partial class Catch : Form
    {
        public Catch()
        {
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.WindowState = FormWindowState.Maximized;
            this.MouseDoubleClick += Catch_MouseDoubleClick;
            this.MouseUp += Catch_MouseUp;
            this.MouseDown += Catch_MouseDown;
            this.MouseMove += Catch_MouseMove;
            this.MouseClick += Catch_MouseClick;
            this.Load +=Catch_Load;
        }

        #region 用户变量
        private Point DownPoint = Point.Empty;//记录鼠标按下坐标，用来确定绘图起点
        private bool CatchFinished = false;//用来表示是否截图完成
        private bool CatchStart = false;//表示截图开始
        private Bitmap originBmp;//用来保存原始图像
        private Rectangle CatchRect;//用来保存截图的矩形

        #endregion

        //窗体初始化操作
        private void Catch_Load(object sender, EventArgs e)
        {
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint, true);
            this.UpdateStyles();
            //以上两句是为了设置控件样式为双缓冲，这可以有效减少图片闪烁的问题，关于这个大家可以自己去搜索下
            originBmp = new Bitmap(this.BackgroundImage);//BackgroundImage为全屏图片，我们另用变量来保存全屏图片
        }

        //鼠标右键点击结束截图
        private void Catch_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        //鼠标左键按下时动作
        private void Catch_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (!CatchStart)
                {//如果捕捉没有开始
                    CatchStart = true;
                    DownPoint = new Point(e.X, e.Y);//保存鼠标按下坐标
                }
            }
        }

        private void Catch_MouseMove(object sender, MouseEventArgs e)
        {
            if (CatchStart)
            {//如果捕捉开始
                Bitmap destBmp = new Bitmap(originBmp, originBmp.Size);
                //Bitmap destBmp = (Bitmap)originBmp.Clone();//新建一个图片对象，并让它与原始图片相同
                Point newPoint = new Point(DownPoint.X, DownPoint.Y);//获取鼠标的坐标
                Graphics g = Graphics.FromImage(destBmp);//在刚才新建的图片上新建一个画板
                Pen p = new Pen(Color.Blue, 1);
                int width = Math.Abs(e.X - DownPoint.X), height = Math.Abs(e.Y - DownPoint.Y);//获取矩形的长和宽
                if (e.X < DownPoint.X)
                {
                    newPoint.X = e.X;
                }
                if (e.Y < DownPoint.Y)
                {
                    newPoint.Y = e.Y;
                }
                CatchRect = new Rectangle(newPoint, new Size(width, height));//保存矩形
                g.DrawRectangle(p, CatchRect);//将矩形画在这个画板上
                g.Dispose();//释放目前的这个画板
                p.Dispose();
                Graphics g1 = this.CreateGraphics();//重新新建一个Graphics类
                //如果之前那个画板不释放，而直接g=this.CreateGraphics()这样的话无法释放掉第一次创建的g,因为只是把地址转到新的g了．如同string一样
                g1 = this.CreateGraphics();//在整个全屏窗体上新建画板
                g1.DrawImage(destBmp, new Point(0, 0));//将刚才所画的图片画到这个窗体上
                //这个也可以属于二次缓冲技术，如果直接将矩形画在窗体上，会造成图片抖动并且会有无数个矩形．
                g1.Dispose();
                destBmp.Dispose();//要及时释放，不然内存将会被大量消耗

            }
        }

        private void Catch_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (CatchStart)
                {
                    CatchStart = false;
                    CatchFinished = true;

                }
            }
        }

        //鼠标双击事件，如果鼠标位于矩形内，则将矩形内的图片保存到剪贴板中
        private void Catch_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && CatchFinished)
            {
                if (CatchRect.Contains(new Point(e.X, e.Y)))
                {
                    Bitmap CatchedBmp = new Bitmap(CatchRect.Width, CatchRect.Height);//新建一个于矩形等大的空白图片
                    Graphics g = Graphics.FromImage(CatchedBmp);
                    g.DrawImage(originBmp, new Rectangle(0, 0, CatchRect.Width, CatchRect.Height), CatchRect, GraphicsUnit.Pixel);
                    //把orginBmp中的指定部分按照指定大小画在画板上
                    Clipboard.SetImage(CatchedBmp);//将图片保存到剪贴板
                    g.Dispose();
                    CatchFinished = false;
                    this.BackgroundImage = originBmp;
                    CatchedBmp.Dispose();
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
            }
        }
    }
}
 
         */
        [DllImport("user32.dll")]
        private static extern int RegisterHotKey(IntPtr hwnd, int id, uint fsModifiers, Keys vk);
        [DllImport("user32.dll")]
        private static extern int UnregisterHotKey(IntPtr hwnd, int id);
        int snipID = 666; //热键ID
        private const int WM_HOTKEY = 0x312; //窗口消息-热键
        private const int WM_CREATE = 0x1; //窗口消息-创建
        private const int WM_DESTROY = 0x2; //窗口消息-销毁
        private const int MOD_ALT = 0x1; //ALT
        private const int MOD_CONTROL = 0x2; //CTRL
        private const int MOD_SHIFT = 0x4; //SHIFT
        private const int VK_SPACE = 0x20; //SPACE

        NotifyIcon noteIcon = new NotifyIcon();
       
        
        private void RegKey(IntPtr hwnd, int hotKey_id, uint fsModifiers, Keys vk)
        {
            bool result;
            if (RegisterHotKey(hwnd, hotKey_id, fsModifiers, vk) == 0)
            {
                result = false;
            }
            else
            {
                result = true;
            }
            if (!result)
            {
                if (MessageBox.Show("注册热键失败！\n请检查程序是否已在运行\n请关闭程序", "是否关闭程序", MessageBoxButtons.OK) == DialogResult.OK)
                    Application.Exit();
            }
        }

        private void UnRegKey(IntPtr hwnd, int hotKey_id)
        {
            UnregisterHotKey(hwnd, hotKey_id);
        }

        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);
            switch (m.Msg)
            {
                case WM_HOTKEY: //窗口消息-热键
                    switch (m.WParam.ToInt32())
                    {
                        case 666: //热键ID
                            startCatch();
                            break;
                        default:
                            break;
                    }
                    break;
                case WM_CREATE: //窗口消息-创建
                    RegKey(Handle, snipID, MOD_ALT | MOD_CONTROL, Keys.S); //注册热键
                    break;
                case WM_DESTROY: //窗口消息-销毁
                    UnRegKey(Handle, snipID); //销毁热键
                    noteIcon.Visible = false;
                    break;
                default:
                    break;
            }
        }

        private void startCatch()
        {
            CatchForm Catch = new CatchForm();
            Catch.TopMost = true;
            Catch.ShowDialog();
            Catch.Dispose();
            

        }


        private void Form1_SizeChanged(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                this.Hide();        //可以要，也可以不要，取决于是否隐藏主窗体
                this.noteIcon.Visible = true;
            }
        }

        private void noteIcon_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                contextMenuStrip1.Show();
            }
        }

        private void 关闭ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void 帮助ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string helpstr = 
        @"
        SnipTool v1.0.1 All Right Reserved
        **************************************************************
        ***通过Ctrl+Alt+S进入截屏模式                    
        ***拖拽鼠标以框选截屏范围                          
        ***按Ctrl+C或双击矩形框复制至剪切板                 
        ***按Ctrl+S保存为本地图片文件 
        ***按Esc退出截屏                 
        **************************************************************
        Ps:请持续关注本产品，图片编辑和其他更多功能以搬上行程......
        你的支持就是我最大的动力哦(*^_^*)";

            MessageBox.Show(helpstr,"Help");
        }

        

   

        
    }
}
