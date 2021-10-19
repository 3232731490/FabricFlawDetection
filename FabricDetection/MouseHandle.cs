using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Controls;

namespace FabricDetection
{
    public partial class MainWindow : Window
    {
        private bool painting = false;  // 标识是否开始绘制
        private bool deleting = false;  // 标识是否在删除
        List<MyRect> rects = new List<MyRect>();      // 存储绘制的所有矩形
        MyRect rect = null; // 当前正在绘制的矩形
        Rectangle currect;
        double startX_Del, startY_Del;

        /// <summary>
        /// 监听鼠标左键按下
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Img_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (!painting && !deleting) return;  // 没有绘制或者删除则返回
            if(painting)    // 如果是在绘制瑕疵区域
                rect = new MyRect() { Start = e.GetPosition(this.ImageParent) };    // 初始化瑕疵区域左上角顶点
            else  // 如果是在删除瑕疵区域
            {
                startX_Del = e.GetPosition(ImageParent).X;  // 初始化删除瑕疵区域左上角坐标
                startY_Del = e.GetPosition(ImageParent).Y;
            }
        }

        /// <summary>
        /// 监听鼠标抬起
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Img_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (!painting && !deleting) return; // 没有绘制或者删除则返回
            if (painting)   // 如果是在绘制瑕疵区域
            {
                Point end = e.GetPosition(this.ImageParent);    // 当前鼠标坐标
                // 获取鼠标画的矩形的宽高
                rect.Width = end.X - rect.Start.X;
                rect.Height = end.Y - rect.Start.Y;
                // 特殊情况处理
                if (rect.Width < 0)
                {
                    rect.Start = new Point(rect.Start.X + rect.Width, rect.Start.Y);
                    rect.Width *= -1;
                }
                if (rect.Height < 0)
                {
                    rect.Start = new Point(rect.Start.X, rect.Start.Y + rect.Height);
                    rect.Height *= -1;
                }
                // 以下代码在画布上绘制矩形
                currect = new Rectangle();
                currect.Stroke = new SolidColorBrush(Colors.Red);
                currect.Width = rect.Width;
                currect.Height = rect.Height;

                Canvas.SetLeft(currect, rect.Start.X);
                Canvas.SetTop(currect, rect.Start.Y);
                Canvas.SetRight(currect, Double.NaN);
                Canvas.SetBottom(currect, Double.NaN);
                ImageParent.Children.Add(currect);
                rect.Rec = currect;
                // 将矩形加入画布中
                rects.Add(rect);
            }
            else  // 如果是在删除瑕疵区域
            {
                Point end = e.GetPosition(this.ImageParent);    // 记录当前鼠标坐标
                for(int i=0;i<rects.Count;i++) // 遍历所有瑕疵区域
                {
                    if (startX_Del <= rects[i].Start.X && startY_Del <= rects[i].Start.Y && end.X >= rects[i].Start.X + rects[i].Width && end.Y >= rects[i].Start.Y + rects[i].Height)  // 将当前完全框住的瑕疵区域删除
                    {
                        ImageParent.Children.Remove(rects[i].Rec);
                        rects.Remove(rects[i]);
                    }
                }
                this.deleting = false;
            }
        }
        /// <summary>
        /// 根据瑕疵区域集合绘制瑕疵
        /// </summary>
        private void Draw_Rect()
        {
            foreach (MyRect rec in rects)
            {
                currect = new Rectangle();
                currect.Stroke = new SolidColorBrush(Colors.Red);
                currect.Width = rec.Width;
                currect.Height = rec.Height;
                Canvas.SetLeft(currect, rec.Start.X);
                Canvas.SetTop(currect, rec.Start.Y);
                Canvas.SetRight(currect, Double.NaN);
                Canvas.SetBottom(currect, Double.NaN);
                ImageParent.Children.Add(currect);
                rec.Rec = currect;
            }
        }
    }


    /// <summary>
    /// 保存瑕疵区域的信息
    /// </summary>
    public class MyRect
    {
        private Point start;
        private double width;
        private double height;
        private Rectangle rec;
        public Point Start { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }

        public Rectangle Rec { get; set; }
        public override string ToString()
        {
            return Start + "  Width: " + Width.ToString() + " Height = " + Height.ToString();
        }
    }
}
