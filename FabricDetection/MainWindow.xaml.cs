using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Shapes;

namespace FabricDetection
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        MyFile myFile;
        ObservableCollection<FabricInfo> FabricInfos = new ObservableCollection<FabricInfo>();  // 存储瑕疵列表
        ObservableCollection<ListItem> KindItems = new ObservableCollection<ListItem>();    // 存储瑕疵种类
        Dictionary<string, int> Kind2Num = new Dictionary<string, int>();   // 存储当前瑕疵种类的个数用于给瑕疵编号
        public MainWindow()
        {
            InitializeComponent();
            this.SizeChanged += new System.Windows.SizeChangedEventHandler(MainWindow_Resize);  // 绑定窗口大小变化事件
            myFile = new MyFile() { File_BasePath = "" };
            // 前后端数据绑定
            this.CurFilePath.SetBinding(TextBox.TextProperty, new Binding("File_BasePath") { Source =  myFile}) ;
            this.fileName.SetBinding(TextBox.TextProperty, new Binding("File_Name") { Source = myFile});
            this.Img.SetBinding(Image.SourceProperty, new Binding("Image_Path") { Source = myFile });
            this.Gene_num.SetBinding(TextBox.TextProperty, new Binding("Genaration_num") {Source = myFile ,Mode = BindingMode.TwoWay });
            this.Img.MouseMove += Img_MouseMove;

            KindItems.Add(new ListItem() { Kind = "擦洞" });
            KindItems.Add(new ListItem() { Kind = "回边" });
            KindItems.Add(new ListItem() { Kind = "解洞" });
            KindItems.Add(new ListItem() { Kind = "跳花" });
            KindItems.Add(new ListItem() { Kind = "织入" });
            KindItems.Add(new ListItem() { Kind = "毛粒" });
            KindItems.Add(new ListItem() { Kind = "愣断" });
            KindItems.Add(new ListItem() { Kind = "毛洞" });
            KindItems.Add(new ListItem() { Kind = "破洞" });
            KindItems.Add(new ListItem() { Kind = "耳朵" });
            KindItems.Add(new ListItem() { Kind = "黄渍" });
            KindItems.Add(new ListItem() { Kind = "破边" });
            KindItems.Add(new ListItem() { Kind = "污渍" });
            KindItems.Add(new ListItem() { Kind = "线印" });

            FabricList.ItemsSource = FabricInfos;
        }
        /// <summary>
        /// 限制只能输入数字
        /// </summary>
        /// <param name="e"></param>
        public void limitnumber(object sender, TextCompositionEventArgs e)
        {
            Regex re = new Regex("[^0-9]+");
            e.Handled = re.IsMatch(e.Text);
        }
        /// <summary>
        /// 窗口大小变化事件的回调函数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainWindow_Resize(object sender, System.EventArgs e)
        {
            // 窗口大小变化后先清空所有矩形再重新绘制
            Clear_Rect();   
            Draw_Rect();
        }


    }

    /// <summary>
    /// 保存文件信息
    /// </summary>
    public class MyFile : INotifyPropertyChanged
    {
        private string file_basepath;
        private string file_name;
        private string image_path;
        private int cur_pos = 0;
        private string genaration_num;
        public int Len { get; set; }
        public int Cur_Pos { get { return cur_pos; } }
        public void add_pos() { cur_pos++; }
        public void sub_pos() { cur_pos--; }
        public String File_BasePath { set { // 如果base路径发生变化则通知前端界面做出相应改变
                file_basepath = value;
                OnPropertyChanged("file_basepath"); // 通知前端界面改变
                this.files.Clear(); // 清除目前已保存的文件夹中的文件名
                cur_pos = 0;
                if (file_basepath != "")
                {
                    read_file();    // 将当前文件夹中的所有图像文件读入
                }
                Len = files.Count();    // 计算总图片数量
            } 
            get { return file_basepath; } }

        /// <summary>
        /// 读取当前文件夹中的所有图片并保存起来
        /// </summary>
        private void read_file()
        {
            DirectoryInfo folder = new DirectoryInfo(file_basepath);
            var fs = folder.GetFiles();
            Regex re = new Regex("^*.(jpg|png|bmp|gif|jpeg)$"); // 判断是否为图像
            foreach(FileInfo file in fs)
            {
                if(re.IsMatch(file.Name))
                    files.Add(file.Name);
            }
        }

        public String File_Name
        {
            set
            {
                file_name = value;
                OnPropertyChanged("file_name"); // 通知前端界面做出改变
            }
            get
            {
                return file_name;
            }
        }
        public String Image_Path
        {
            set
            {
                image_path = value;
                OnPropertyChanged("image_path");    // 通知前端界面做出改变
            }
            get
            {
                return image_path;
            }
        }

        public String Genaration_num
        {
            get
            {
                return genaration_num;
            }
            set
            {
                genaration_num = value;
                OnPropertyChanged("genaration_num");    // 通知前端界面做出改变
            }
        }


        public string get_curFile()
        {
            return Len==0?"":this.files[cur_pos]; // 返回当前文件的文件名
        }

        protected void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));  // 通知前端界面改变
            }
        }
        private List<String> files = new List<string>();    // 保存当前文件所有文件名

        public event PropertyChangedEventHandler PropertyChanged;  
    }

    public class FabricInfo : INotifyPropertyChanged
    {
        private bool isChecked;
        public string KindName { get; set; }
        public bool IsChecked { get {
                return this.isChecked;
            } set{
                isChecked = value;
                if (PropertyChanged != null)//有改变  
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("IsChecked"));//对Name进行监听  
                }
            } }
        public Rectangle CurRect { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
    }

}
