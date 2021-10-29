using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace FabricDetection
{
    /// <summary>
    /// MyDialog.xaml 的交互逻辑
    /// </summary>
    public partial class MyDialog : Window
    {
        MyRect rect1;
        ObservableCollection<ListItem> _KindItems;  // 所有种类
        Dictionary<string, int> Kind2Num ;   // 存储当前瑕疵种类的个数用于给瑕疵编号
        public MyDialog(MyRect rect, ObservableCollection<ListItem> KindItems, Dictionary<string, int> _Kind2Num)
        {
            rect1 = rect;
            InitializeComponent();
            _KindItems = KindItems;
            icTodoList.ItemsSource = _KindItems;
            Kind2Num = _Kind2Num;
        }

        private void Commit(object sender, RoutedEventArgs e)
        {
            //业务逻辑。。。。
            if (!KindName.Text.Equals(""))
            {
                bool flag = true;
                // 判断用户输入种类是否在列表中出现过，没有出现则新加入
                foreach(ListItem listItem in _KindItems)
                {
                    if (listItem.Kind.Equals(KindName.Text))
                    {
                        flag = false;
                    }
                }
                if (flag) _KindItems.Add(new ListItem() { Kind = KindName.Text });  // 增加新种类在列表中，方便用户再次选择
                int num;
                if (Kind2Num.TryGetValue(KindName.Text,out num))
                {
                    rect1.Kind = KindName.Text + num.ToString();
                    Kind2Num.Remove(KindName.Text);
                    Kind2Num.Add(KindName.Text, num + 1);
                }
                else
                {
                    Kind2Num.Add(KindName.Text, 1);
                    rect1.Kind = KindName.Text + "1";
                }
            }
            DialogResult = true;
            Close();
        }

        private void Cancel(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void TextBlock_MouseDown(object sender, MouseButtonEventArgs e)
        {
            TextBlock text = sender as TextBlock;
            int num;
            if (Kind2Num.TryGetValue(text.Text, out num))
            {
                rect1.Kind = text.Text + num.ToString();
                Kind2Num.Remove(text.Text);
                Kind2Num.Add(text.Text, num + 1);
            }
            else
            {
                Kind2Num.Add(text.Text, 2);
                rect1.Kind = text.Text + "1";
            }
        }
    }
    public class ListItem
    {
        public string Kind { get; set; }
    }
}
