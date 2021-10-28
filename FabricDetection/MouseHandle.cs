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
        private bool moving = false;
        List<MyRect> rects = new List<MyRect>();      // 存储绘制的所有矩形
        MyRect rect = null; // 当前正在绘制的矩形
        Rectangle currect;

        /// <summary>
        /// 监听鼠标左键按下
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Img_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (!painting && !deleting) return;  // 没有绘制或者删除则返回
            rect = new MyRect() { Start = e.GetPosition(this.ImageParent) };    // 初始化左上角顶点
            moving = true;
        }

        /// <summary>
        /// 监听鼠标移动  不知道什么原因，加了这个鼠标抬起事件就不灵了
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Img_MouseMove(object sender, MouseEventArgs e)
        {
            if (!moving) return;  // 没有绘制或者删除则返回
            if (rect == null) return;       // 没有按下鼠标返回
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                Point end = e.GetPosition(this.ImageParent);    // 当前鼠标坐标
                // 获取鼠标画的矩形的宽高
                rect.Width = end.X - rect.Start.X;
                rect.Height = end.Y - rect.Start.Y;
                // 特殊情况处理
                if (rect.Width < 0)
                {
                    rect.Width *= -1;
                }
                if (rect.Height < 0)
                {
                    rect.Height *= -1;
                }
                if (painting)
                {
                    // 以下代码在画布上绘制矩形
                    currect = new Rectangle();
                    ImageParent.Children.Remove(rect.Rec);  // 将当前画板上的删除
                    currect.Stroke = new SolidColorBrush(Colors.Red);
                    currect.Width = rect.Width;
                    currect.Height = rect.Height;
                    Canvas.SetLeft(currect, rect.Start.X > end.X ? end.X : rect.Start.X);
                    Canvas.SetTop(currect, rect.Start.Y > end.Y ? end.Y : rect.Start.Y);
                    Canvas.SetRight(currect, Double.NaN);
                    Canvas.SetBottom(currect, Double.NaN);
                    ImageParent.Children.Add(currect);
                    rect.Rec = currect;
                }
                else if (deleting)
                {
                    // 以下代码在画布上绘制矩形
                    currect = new Rectangle();
                    ImageParent.Children.Remove(rect.Rec);  // 将当前画板上的区域全部删除
                    currect.Stroke = new SolidColorBrush(Colors.Gray);
                    currect.Width = rect.Width;
                    currect.Height = rect.Height;
                    Canvas.SetLeft(currect, rect.Start.X > end.X ? end.X : rect.Start.X);
                    Canvas.SetTop(currect, rect.Start.Y > end.Y ? end.Y : rect.Start.Y);
                    Canvas.SetRight(currect, Double.NaN);
                    Canvas.SetBottom(currect, Double.NaN);
                    ImageParent.Children.Add(currect);
                    rect.Rec = currect;
                }
            }
            else
            {
                moving = false;
                ImageParent.Children.Remove(currect);
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
                    
                    rect.WindowWidth = this.textBlock.ActualWidth;
                    rect.WindowHeight = this.textBlock.ActualHeight;
                    rect.Rec = currect;
                    // 将矩形加入画布中
                    currect = null;
                    rects.Add(rect);
                }
                else  // 如果是在删除瑕疵区域
                {
                    Point end = e.GetPosition(this.ImageParent);    // 记录当前鼠标坐标
                    for (int i = 0; i < rects.Count; i++) // 遍历所有瑕疵区域
                    {
                        double ratingw = this.textBlock.ActualWidth / rects[i].WindowWidth;
                        double ratingh = this.textBlock.ActualHeight / rects[i].WindowHeight;
                        if (rect.Start.X <= rects[i].Start.X * ratingw && rect.Start.Y <= rects[i].Start.Y * ratingh && end.X >= rects[i].Start.X * ratingw + rects[i].Width * ratingw && end.Y >= rects[i].Start.Y * ratingh + rects[i].Height * ratingh)  // 将当前完全框住的瑕疵区域删除
                        {
                            ImageParent.Children.Remove(rects[i].Rec);
                            rects.Remove(rects[i]);
                            i--;
                        }
                        else if (end.X <= rects[i].Start.X * ratingw && end.Y <= rects[i].Start.Y * ratingh && rect.Start.X >= rects[i].Start.X * ratingw + rects[i].Width * ratingw && rect.Start.Y >= rects[i].Start.Y * ratingh + rects[i].Height * ratingh)
                        {
                            ImageParent.Children.Remove(rects[i].Rec);
                            rects.Remove(rects[i]);
                            i--;
                        }
                    }
                }
                rect = null;
            }
        }

        /// <summary>
        /// 监听鼠标抬起
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
       /* private void Img_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            moving = false;
            ImageParent.Children.Remove(currect);
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
                rect.WindowWidth = this.textBlock.ActualWidth;
                rect.WindowHeight = this.textBlock.ActualHeight;
                rect.Rec = currect;
                // 将矩形加入画布中
                rects.Add(rect);
            }
            else  // 如果是在删除瑕疵区域
            {
                Point end = e.GetPosition(this.ImageParent);    // 记录当前鼠标坐标
                for (int i = 0; i < rects.Count; i++) // 遍历所有瑕疵区域
                {
                    double ratingw = this.textBlock.ActualWidth / rects[i].WindowWidth;
                    double ratingh = this.textBlock.ActualHeight / rects[i].WindowHeight;
                    if (rect.Start.X <= rects[i].Start.X * ratingw && rect.Start.Y <= rects[i].Start.Y * ratingh && end.X >= rects[i].Start.X * ratingw + rects[i].Width * ratingw && end.Y >= rects[i].Start.Y * ratingh + rects[i].Height * ratingh)  // 将当前完全框住的瑕疵区域删除
                    {
                        ImageParent.Children.Remove(rects[i].Rec);
                        rects.Remove(rects[i]);
                        i--;
                    }
                    else if (end.X <= rects[i].Start.X * ratingw && end.Y <= rects[i].Start.Y * ratingh && rect.Start.X >= rects[i].Start.X * ratingw + rects[i].Width * ratingw && rect.Start.Y >= rects[i].Start.Y * ratingh + rects[i].Height * ratingh)
                    {
                        ImageParent.Children.Remove(rects[i].Rec);
                        rects.Remove(rects[i]);
                        i--;
                    }
                }
            }
            rect = null;
        }*/

        /// <summary>
        /// 根据瑕疵区域集合绘制瑕疵
        /// </summary>
        private void Draw_Rect()
        {
            foreach (MyRect rec in rects)
            {
                currect = new Rectangle();
                currect.Stroke = new SolidColorBrush(Colors.Red);
                double curWindowHeight = this.textBlock.ActualHeight;
                double curWindowWidth = this.textBlock.ActualWidth;
                currect.Width = rec.Width * curWindowWidth / rec.WindowWidth;
                currect.Height = rec.Height * curWindowHeight / rec.WindowHeight;
                Canvas.SetLeft(currect, rec.Start.X * curWindowWidth / rec.WindowWidth);
                Canvas.SetTop(currect, rec.Start.Y * curWindowHeight / rec.WindowHeight);
                Canvas.SetRight(currect, Double.NaN);
                Canvas.SetBottom(currect, Double.NaN);
                ImageParent.Children.Add(currect);
                rec.Rec = currect;
            }
        }

        /// <summary>
        /// 清空瑕疵区域
        /// </summary>
        private void Clear_Rect()
        {
            foreach (MyRect rect in rects)
            {
                ImageParent.Children.Remove(rect.Rec);  // 将当前画板上的区域全部删除
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
        private double windowWidth;
        private double windowHeight;
        private Rectangle rec;
        public Point Start { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }
        public Rectangle Rec { get; set; }
        public double WindowHeight { get; set; }
        public double WindowWidth { get; set; }
        public override string ToString()
        {
            return Start + "  Width: " + Width.ToString() + " Height = " + Height.ToString();
        }
    }
}
