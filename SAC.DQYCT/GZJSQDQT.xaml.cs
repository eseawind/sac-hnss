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

namespace SAC.DQYCT
{
    public partial class GZJSQDQT : UserControl
    {
        System.Windows.Threading.DispatcherTimer _MessageControler; 
        string a = "3";
        public GZJSQDQT()
        {

            InitializeComponent(); 
            //tabControl1.SelectionChanged += firstTabControl_SelectionChanged;
            SLWCFRIAClient client = ServerManager.GetPox();
            //bool registerResult = WebRequest.RegisterPrefix("http://", WebRequestCreator.ClientHttp);
            //bool httpsResult = WebRequest.RegisterPrefix("https://", WebRequestCreator.ClientHttp);
            //调用GetData方法并加载事件
            client.GetDataAsync(a);
            client.GetDataCompleted += new EventHandler<GetDataCompletedEventArgs>(client_GetDataCompleted);
            Globals VTHelper = new Globals();
            List<TextBox> textblock = VTHelper.GetChildObjects<TextBox>(this.LayoutRoot, "");
            _MessageControler = new System.Windows.Threading.DispatcherTimer();
            _MessageControler.Interval = new TimeSpan(0, 0, 5);
            _MessageControler.Tick += new EventHandler(timer_Tick);
            _MessageControler.Start();
        }
        //private void firstTabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{
        //    tabControl1.SelectionChanged -= firstTabControl_SelectionChanged;
        //    tabControl1.SelectedIndex = -1;
        //    tabControl1.SelectionChanged += tabControl_SelectionChanged;
        //}

        //private void tabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{
        //    int aa = this.tabControl1.SelectedIndex;
        //}
        private void timer_Tick(object sender, EventArgs e)
        {

            SLWCFRIAClient client = ServerManager.GetPox();
            //bool registerResult = WebRequest.RegisterPrefix("http://", WebRequestCreator.ClientHttp);
            //bool httpsResult = WebRequest.RegisterPrefix("https://", WebRequestCreator.ClientHttp);
            //调用GetData方法并加载事件

            client.GetDataAsync(a);
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
                        try
                        {
                            TextBlock tt = ((TextBlock)this.FindName(e.Result.Split(';')[i].Split(',')[0]));
                            tt.Text = e.Result.Split(';')[i].Split(',')[1];
                        }
                        catch
                        {

                        }
                    }
                }
            }

        }

        //private void tabControl1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{
        //    if (this.tabControl1 != null)
        //    {
        //        switch (this.tabControl1.SelectedIndex)
        //        {
        //            case 0:
        //                { //加载第1个选项卡内容
        //                    Grid gItem = ((TabItem)this.tabControl1.SelectedItem).Content as Grid;
        //                    if (gItem != null && gItem.Children.Count < 1) // 为啥判断子成员个数，因为来回切换可能造成数据重新加载，入力内容丢失 
        //                    {
        //                        //gItem.Children.Add(new TextBox());
        //                        //添加子选项卡内容，这里用UserControl代替
        //                        gItem.Children.Add(new UserControl());
        //                    }
        //                    break;
        //                }
        //            case 1:
        //                { //加载第二个选项卡内容
        //                    break;
        //                }
        //            default:
        //                {
        //                    break;
        //                }
        //        }
        //    }
        //}
    }
}
