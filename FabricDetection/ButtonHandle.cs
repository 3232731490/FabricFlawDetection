using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Windows;
using OpenCvSharp;
using System.Xml;
using System.IO;
using System.Windows.Controls;
using System.Windows.Media;

namespace FabricDetection
{
    public partial class MainWindow : System.Windows.Window
    {
        Button prebtn = null;
        /// <summary>
        /// 退出系统
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("是否确认退出？", "提示", MessageBoxButton.YesNo,MessageBoxImage.Question,MessageBoxResult.Yes) == MessageBoxResult.Yes)
            {
                Close();
            }
        }

        /// <summary>
        /// 选择文件目录
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FindFile_Click(object sender, RoutedEventArgs e)
        {
            CommonOpenFileDialog dialog = new CommonOpenFileDialog();
            dialog.IsFolderPicker = true;//设置为选择文件夹
            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)   // 判断用户是否点击的确认
            {
                this.myFile.File_BasePath = dialog.FileName;    // 更新当前文件的base路径
                this.myFile.File_Name = this.myFile.get_curFile();  // 更新当前文件名
                this.myFile.Image_Path = this.myFile.File_BasePath + "\\" + this.myFile.File_Name;  // 由base路径和文件名组合成一个完整路径
                read_XML();     // 读取当前样本信息
            }
        }

        /// <summary>
        /// 上一个图片
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pre_Click(object sender, RoutedEventArgs e)
        {
            if(this.myFile.Len == 0)
            {
                MessageBox.Show("当前文件夹中不含有图片文件");
                return;
            }
            if (this.myFile.Cur_Pos > 0)    // 判断目前是否是第一张图片
            {
                // 选取上一个文件
                this.myFile.sub_pos();
                this.myFile.File_Name = this.myFile.get_curFile();
                this.myFile.Image_Path = this.myFile.File_BasePath + "\\" + this.myFile.File_Name;
                foreach (MyRect rect in rects)
                {
                    ImageParent.Children.Remove(rect.Rec);  // 将当前画板上的区域全部删除
                }
                read_XML(); // 读取XML文件
            }
            else
            {
                MessageBox.Show("当前已经为第一张样本图");
            }
        }

        /// <summary>
        /// 下一个图片
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void next_Click(object sender, RoutedEventArgs e)
        {
            if (this.myFile.Len == 0)
            {
                MessageBox.Show("当前文件夹中不含有图片文件");
                return;
            }
            if (this.myFile.Cur_Pos < this.myFile.Len-1)    // 判断目前是否为最后一张图片
            {
                // 选取下一张
                this.myFile.add_pos();
                this.myFile.File_Name = this.myFile.get_curFile();
                this.myFile.Image_Path = this.myFile.File_BasePath + "\\" + this.myFile.File_Name;
                foreach (MyRect rect in rects)
                {
                    ImageParent.Children.Remove(rect.Rec);  // 将当前画板上的区域全部删除
                }
                read_XML(); // 读取XML文件
            }
            else
            {
                MessageBox.Show("当前已经为最后一张样本图");
            }
        }

        /// <summary>
        /// 随机生成图片
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Generation_Click(object sender, RoutedEventArgs e)
        {
            // 特殊情况判断
            if (myFile.Image_Path == null || myFile.Image_Path == "") {
                MessageBox.Show("当前还未选择模板样本，无法生成随机样本，请先选择一张图片...");
                return;
            };
            Mat src = Cv2.ImRead(myFile.Image_Path,ImreadModes.Color);
            if (src.Empty())
            {
                MessageBox.Show("图片打开失败");
                return;
            }
            if (this.Gene_num.Text.Length == 0)
            {
                MessageBox.Show("请先输入生成样本数量");
                return;
            }
            // 随机生成一个uuid
            string id = System.Guid.NewGuid().ToString("N");
            int generation = int.Parse(this.Gene_num.Text);
            Random rd = new Random();
            for(int i = 1; i <= generation; i++)
            {
                string filename = myFile.File_BasePath+"\\"+id.Substring(0,8) + "_" + i.ToString()+".jpg";  // 通过base路径和文件名组合完整路径
                Mat temp = Random_genaration(src,rd.Next(6)+1);     // 将当前样本图传入随机打乱像素点以达到生成样本图的目的
                Cv2.ImWrite(filename, temp);    // 将生成的图片写入磁盘
            }
            MessageBox.Show("样本已生成完毕...");
        }

        /// <summary>
        /// 随机打乱样本像素
        /// </summary>
        /// <param name="src"></param>
        /// <param name="c"></param>
        /// <returns></returns>
        private Mat Random_genaration(Mat src , int c)
        {
            Mat newmat = src.Clone();
            int w = newmat.Width;
            int h = newmat.Height;
            int mind = w > h ? h : w;
            Random rd = new Random();
            int minw = mind > 1000 ? 100 : (mind > 500 ? 50 : (mind > 100) ? 10 : 3);   // 通过当前图片大小选取合适的像素框大小
            for(int i = 0; i < c; i++)  // 打乱c次
            {
                int srcx = rd.Next(w - minw);
                int srcy = rd.Next(h - minw);
                int dstx = rd.Next(w - minw);
                int dsty = rd.Next(h - minw);
                for(int j = 0; j < minw; j++)
                {
                    for(int k = 0; k < minw; k++)
                    {
                        // 交换像素值
                        Vec3b pix = newmat.At<Vec3b>(srcy +j, srcx + k);
                        Vec3b temp = newmat.At<Vec3b>(dsty + j, dstx + k);
                        newmat.Set(srcy + j, srcx + k,temp);
                        newmat.Set(dsty + j, dstx + k, pix);
                    }
                }
            }
            return newmat;
        }

        /// <summary>
        /// 框选瑕疵
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SelectFlaw_Click(object sender, RoutedEventArgs e)
        {
            if(prebtn != null)
            {
                prebtn.Background = Brushes.YellowGreen;
            }
            prebtn = sender as Button;
            prebtn.Background = Brushes.DarkGreen;
            painting = true;
            deleting = false;
        }

        /// <summary>
        /// 完成框选
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Completed_Click(object sender, RoutedEventArgs e)
        {
            if (prebtn != null)
            {
                prebtn.Background = Brushes.YellowGreen;
            }
            prebtn = sender as Button;
            prebtn.Background = Brushes.DarkGreen;
            painting = false;
            deleting = false;
            moving = false;
        }

        /// <summary>
        /// 删除瑕疵区域
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DeleteDraw_Click(object sender, RoutedEventArgs e)
        {
            if (prebtn != null)
            {
                prebtn.Background = Brushes.YellowGreen;
            }
            prebtn = sender as Button;
            prebtn.Background = Brushes.DarkGreen;
            MessageBox.Show("请框选您要删除的瑕疵框");
            this.painting = false;
            this.deleting = true;
            moving = false;
        }

        /// <summary>
        /// 保存样本信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void saveXML_Click(object sender, RoutedEventArgs e)
        {
            if (prebtn != null)
            {
                prebtn.Background = Brushes.YellowGreen;
            }
            prebtn = sender as Button;
            prebtn.Background = Brushes.DarkGreen;
            if (myFile.Len == 0)
            {
                MessageBox.Show("当前未选中样本");
                return;
            }
            XmlDocument doc = new XmlDocument();    // 获取操作XML的doc元素

            XmlDeclaration xmldecl = doc.CreateXmlDeclaration("1.0", "utf-8", null);    // 设置xml文件头部信息
            XmlElement root = doc.DocumentElement;
            doc.InsertBefore(xmldecl, root);    

            XmlElement ele = doc.CreateElement("Flaw"); 
            doc.AppendChild(ele);

            XmlElement SName = doc.CreateElement("SampleName");
            SName.InnerText = myFile.File_Name;
            ele.AppendChild(SName);

            XmlElement Sclass = doc.CreateElement("CLASS");
            Sclass.InnerText = ComboBox1.Text;
            ele.AppendChild(Sclass);
            int count = 1;
            XmlElement SPosition = doc.CreateElement("POSITION");
            // 将当前图片的瑕疵坐标全部存入XML文件
            foreach (MyRect rect in rects)
            {
                XmlElement SPosition1 = doc.CreateElement("POSITION"+count.ToString());
                XmlElement SPositionX = doc.CreateElement("POSITIONX");
                XmlElement SPositionY = doc.CreateElement("POSITIONY");
                XmlElement SWidth = doc.CreateElement("WIDTH");
                XmlElement SHeight = doc.CreateElement("HEIGHT");
                SPositionX.InnerText= ((int)rect.Start.X).ToString();
                SPositionY.InnerText = ((int)rect.Start.Y).ToString();
                SWidth.InnerText = ((int)rect.Width).ToString();
                SHeight.InnerText = ((int)rect.Height).ToString();
                SPosition1.AppendChild(SPositionX);
                SPosition1.AppendChild(SPositionY);
                SPosition1.AppendChild(SWidth);
                SPosition1.AppendChild(SHeight);
                XmlElement WindowWidth = doc.CreateElement("WINDOWWIDTH");
                XmlElement WindowHeight = doc.CreateElement("WINDOWHEIGHT");
                WindowWidth.InnerText = rect.WindowWidth.ToString();
                WindowHeight.InnerText = rect.WindowHeight.ToString();
                SPosition1.AppendChild(WindowWidth);
                SPosition1.AppendChild(WindowHeight);
                SPosition.AppendChild(SPosition1);
                count++;
            }
            ele.AppendChild(SPosition);

            // 通过base路径组合完整路径
            string base_path = this.myFile.File_BasePath + "\\XML";
            var names = this.myFile.File_Name.Split(".");
            string temp_path = "";
            for (int i= 0 ; i < names.Length-1; i++)
            {
                temp_path += names[i];
                if (i < names.Length - 2)
                    temp_path += "_";
            }
            string img_path = base_path + "\\" + temp_path + ".xml";
            if (!Directory.Exists(base_path))
            {
                Directory.CreateDirectory(base_path);
            }
            doc.Save(img_path); // 写入磁盘
            MessageBox.Show("当前样本信息已保存至当前样本目录的XML文件夹下");
            painting = false;
            deleting = false;
            moving = false;
        }

        /// <summary>
        /// 当切换图片时，读取XML文件中的信息
        /// </summary>
        private void read_XML()
        {
            rects.Clear();  // 先清空原有瑕疵信息
            if (this.myFile.Len == 0)
                return;
            var names = this.myFile.File_Name.Split(".");
            string temp_path = "";
            for (int i = 0; i < names.Length - 1; i++)
            {
                temp_path += names[i];
                if (i < names.Length - 2)
                    temp_path += "_";
            }
            string base_path = this.myFile.File_BasePath+"\\XML";
            string img_path = base_path+"\\"+temp_path+".xml";
            if (!Directory.Exists(base_path))
            {
                this.ComboBox1.SelectedIndex = 1;
                Directory.CreateDirectory(base_path);
                return;
            }
            if (!File.Exists(img_path))
            {
                this.ComboBox1.SelectedIndex = 1;
                return;
            }
            XmlDocument doc = new XmlDocument();
            doc.Load(img_path);
            XmlNode xn = doc.SelectSingleNode("Flaw");

            XmlNode Sclass = xn.SelectSingleNode("CLASS");
            this.ComboBox1.SelectedValue = Sclass.InnerText;

            XmlNode SPosition = xn.SelectSingleNode("POSITION");
            XmlNodeList Postions = SPosition.ChildNodes;
            
            foreach (XmlNode posion in Postions)
            {
                XmlNode POSITIONX = posion.SelectSingleNode("POSITIONX");
                XmlNode POSITIONY = posion.SelectSingleNode("POSITIONY");
                XmlNode WIDTH = posion.SelectSingleNode("WIDTH");
                XmlNode HEIGHT = posion.SelectSingleNode("HEIGHT");
                MyRect r = new MyRect();
                r.Start =new System.Windows.Point( double.Parse(POSITIONX.InnerText),double.Parse(POSITIONY.InnerText));
                r.Width = double.Parse(WIDTH.InnerText);
                r.Height = double.Parse(HEIGHT.InnerText);
                XmlNode WindowWidth = posion.SelectSingleNode("WINDOWWIDTH");
                XmlNode WindowHeight = posion.SelectSingleNode("WINDOWHEIGHT");
                r.WindowHeight = double.Parse(WindowHeight.InnerText);
                r.WindowWidth = double.Parse(WindowWidth.InnerText);
                rects.Add(r);
            }
            
            Draw_Rect();    // 绘制瑕疵区域
        }
    }
}
