using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Xml;
using System.IO;
using SLGetDataFromWCFRIA.Web;
using System.Windows.Controls.Primitives;
using SLVisualTreeHelper;
using SAC.DQYCT.SLRoom;
using System.Windows.Browser;

namespace SAC.DQYCT
{
    public partial class NXWZMainPage : UserControl
    {
        System.Windows.Threading.DispatcherTimer _MessageControler;
        public NXWZMainPage()
        {
            InitializeComponent();

            SLWCFRIAClient client = ServerManager.GetPox();
            //bool registerResult = WebRequest.RegisterPrefix("http://", WebRequestCreator.ClientHttp);
            //bool httpsResult = WebRequest.RegisterPrefix("https://", WebRequestCreator.ClientHttp);
            //调用GetData方法并加载事件

            client.GetDataAsync("2");
            client.GetDataCompleted += new EventHandler<GetDataCompletedEventArgs>(client_GetDataCompleted);
            Globals VTHelper = new Globals();
            List<TextBox> textblock = VTHelper.GetChildObjects<TextBox>(this.LayoutRoot, "");
            
            _MessageControler = new System.Windows.Threading.DispatcherTimer();
            _MessageControler.Interval = new TimeSpan(0, 0, 20);
            _MessageControler.Tick += new EventHandler(timer_Tick); 
            _MessageControler.Start();
        }
        private void timer_Tick(object sender, EventArgs e)
        {

            SLWCFRIAClient client = ServerManager.GetPox();
            //bool registerResult = WebRequest.RegisterPrefix("http://", WebRequestCreator.ClientHttp);
            //bool httpsResult = WebRequest.RegisterPrefix("https://", WebRequestCreator.ClientHttp);
            //调用GetData方法并加载事件

            client.GetDataAsync("2");
            client.GetDataCompleted += new EventHandler<GetDataCompletedEventArgs>(client_GetDataCompleted);
            Globals VTHelper = new Globals();
            List<TextBox> textblock = VTHelper.GetChildObjects<TextBox>(this.LayoutRoot, "");
        }
        void client_GetDataCompleted(object sender, GetDataCompletedEventArgs e)
        {
            using (XmlReader xReader = XmlReader.Create(new StringReader(e.Result)))
            {
                for (int i = 0; i < e.Result.Split(';').Length - 1; i++)
                {
                    if (e.Result.Split(';')[i].Split(',')[0].IndexOf("textBox") > -1)
                    {
                        TextBlock tt = ((TextBlock)this.FindName(e.Result.Split(';')[i].Split(',')[0]));
                        tt.Text = e.Result.Split(';')[i].Split(',')[1];
                    }
                }
            }

        }
    }
}