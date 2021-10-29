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
        private bool draging = false;
        List<MyRect> rects = new List<MyRect>();      // 存储绘制的所有矩形
        MyRect rect = null; // 当前正在绘制的矩形
        Rectangle currect;
        Point DragBegin;
        Point DragEnd;
        Rectangle preHighLightRect = null;
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
                    addRectEvents(currect);
                    rect.WindowWidth = this.textBlock.ActualWidth;
                    rect.WindowHeight = this.textBlock.ActualHeight;
                    rect.Rec = currect;
                    MyDialog myDialog = new MyDialog(rect,KindItems,Kind2Num);
                    myDialog.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                    myDialog.Owner = this;
                    if (myDialog.ShowDialog() == true)
                    {
                        if(rect.Kind == null)
                        {
                            MessageBox.Show("您选择类别太快了，我还没看清……");
                            return;
                        }
                        rects.Add(rect);
                        FabricInfos.Add(new FabricInfo() { IsChecked = true, KindName = rect.Kind, CurRect=currect });
                        ImageParent.Children.Add(currect);
                    };
                    // 将矩形加入画布中
                    currect = null;
                }
                else  // 如果是在删除瑕疵区域
                {
                    Point end = e.GetPosition(this.ImageParent);    // 记录当前鼠标坐标
                    if (MessageBox.Show("确定清除所选瑕疵？(●__●)", "操作提示", MessageBoxButton.OKCancel, MessageBoxImage.Question) == MessageBoxResult.OK)
                    {
                        for (int i = 0; i < rects.Count; i++) // 遍历所有瑕疵区域
                        {
                            double ratingw = this.textBlock.ActualWidth / rects[i].WindowWidth;
                            double ratingh = this.textBlock.ActualHeight / rects[i].WindowHeight;
                            if (rect.Start.X <= rects[i].Start.X * ratingw && rect.Start.Y <= rects[i].Start.Y * ratingh && end.X >= rects[i].Start.X * ratingw + rects[i].Width * ratingw && end.Y >= rects[i].Start.Y * ratingh + rects[i].Height * ratingh)  // 将当前完全框住的瑕疵区域删除
                            {
                                ImageParent.Children.Remove(rects[i].Rec);
                                Remove_Fabric(rects[i].Rec);    // 删除瑕疵列表中的元素
                                rects.Remove(rects[i]);
                                i--;
                            }
                            else if (end.X <= rects[i].Start.X * ratingw && end.Y <= rects[i].Start.Y * ratingh && rect.Start.X >= rects[i].Start.X * ratingw + rects[i].Width * ratingw && rect.Start.Y >= rects[i].Start.Y * ratingh + rects[i].Height * ratingh)
                            {
                                ImageParent.Children.Remove(rects[i].Rec);
                                Remove_Fabric(rects[i].Rec);    // 删除瑕疵列表中的元素
                                rects.Remove(rects[i]);
                                i--;
                            }
                        }
                    }
                }
                rect = null;
            }
        }

        /// <summary>
        /// 开始拖动矩形
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RECT_MouseLeftButtonDown(object sender, MouseEventArgs e)
        {
            DragBegin = e.GetPosition(ImageParent);
            draging = true;
        }

        /// <summary>
        /// 鼠标拖动事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RECT_MouseMove(object sender, MouseEventArgs e)
        {
            if (!draging) return;
            Rectangle rectangle = sender as Rectangle;
            MyRect curRect = Find_Rect(rectangle);
            DragEnd = e.GetPosition(ImageParent);
            double deltx = DragEnd.X - DragBegin.X;
            double delty = DragEnd.Y - DragBegin.Y;
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                if (curRect.Start.X + deltx < 0 ) {
                    curRect.Start = new Point(curRect.Start.X + deltx+2, curRect.Start.Y + delty);
                    draging = false;
                    return;
                }else if (curRect.Start.X + deltx + curRect.Width > textBlock.ActualWidth)
                {
                    curRect.Start = new Point(curRect.Start.X + deltx -2, curRect.Start.Y + delty);
                    draging = false;
                    return;
                }else if(curRect.Start.Y + delty < 0)
                {
                    curRect.Start = new Point(curRect.Start.X + deltx , curRect.Start.Y + delty+2);
                    draging = false;
                    return;
                }else if(curRect.Start.Y + delty + curRect.Height > textBlock.ActualWidth)
                {
                    curRect.Start = new Point(curRect.Start.X + deltx, curRect.Start.Y + delty-2);
                    draging = false;
                    return;
                }
                Canvas.SetLeft(rectangle, curRect.Start.X + deltx);
                Canvas.SetTop(rectangle, curRect.Start.Y + delty);
                Canvas.SetRight(rectangle, Double.NaN);
                Canvas.SetBottom(rectangle, Double.NaN);
            }
            else
            {
                curRect.Start = new Point(curRect.Start.X + deltx, curRect.Start.Y + delty);
                draging = false;
            }
        }
        /// <summary>
        /// 鼠标进入矩形事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RECT_MouseEnter(object sender, MouseEventArgs e)
        {
            Rectangle rectangle = sender as Rectangle;
            rectangle.Fill = new SolidColorBrush(Color.FromArgb(124, 91, 201, 153));
            MyRect curt = Find_Rect(rectangle);
            rectangle.ToolTip = "左键点击并按住拖动本矩形 & 瑕疵种类：" + curt.Kind;
        }

        /// <summary>
        /// 鼠标离开矩形事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RECT_MouseLeave(object sender, MouseEventArgs e)
        {
            Rectangle rectangle = sender as Rectangle;
            rectangle.Fill = new SolidColorBrush(Color.FromArgb(0,0,0,0));
            draging = false;
        }

        /// <summary>
        /// 给鼠标添加事件
        /// </summary>
        /// <param name="cur"></param>
        public void addRectEvents(Rectangle cur)
        {
            cur.MouseLeftButtonDown += RECT_MouseLeftButtonDown;  // 给矩形添加鼠标按下事件
            cur.MouseEnter += RECT_MouseEnter;  // 给矩形添加鼠标进入事件
            cur.MouseLeave += RECT_MouseLeave;  // 给矩形添加鼠标离开事件
            cur.MouseMove += RECT_MouseMove;    // 给矩形添加鼠标移动事件
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
                double curWindowHeight = this.textBlock.ActualHeight;
                double curWindowWidth = this.textBlock.ActualWidth;
                currect.Width = rec.Width * curWindowWidth / rec.WindowWidth;
                currect.Height = rec.Height * curWindowHeight / rec.WindowHeight;
                Canvas.SetLeft(currect, rec.Start.X * curWindowWidth / rec.WindowWidth);
                Canvas.SetTop(currect, rec.Start.Y * curWindowHeight / rec.WindowHeight);
                Canvas.SetRight(currect, Double.NaN);
                Canvas.SetBottom(currect, Double.NaN);
                FabricInfos.Add(new FabricInfo() { IsChecked = true, KindName = rec.Kind, CurRect = currect});
                ImageParent.Children.Add(currect);
                addRectEvents(currect);
                rec.Rec = currect;
            }
            IsAll.IsChecked = true;
        }


        /// <summary>
        /// 删除瑕疵列表中的元素
        /// </summary>
        /// <param name=""></param>
        private void Remove_Fabric(Rectangle f)
        {
            foreach (FabricInfo i in FabricInfos)
            {
                if (i.CurRect == f)
                {
                    FabricInfos.Remove(i);
                    return;
                }
            }
        }

        /// <summary>
        /// 根据选择框绘制瑕疵区域
        /// </summary>
        private void FromInfoDrawRect()
        {
            foreach(FabricInfo fabricInfo in FabricInfos)
            {
                if (fabricInfo.IsChecked)
                {
                    ImageParent.Children.Add(fabricInfo.CurRect);
                }
            }
        }

        /// <summary>
        /// 清除瑕疵区域，用于重新绘制
        /// </summary>
        private void FromInfoClearRect()
        {
            foreach (MyRect rect in rects)
            {
                ImageParent.Children.Remove(rect.Rec);  // 将当前画板上的区域全部删除
            }
        }

        /// <summary>
        /// 清空瑕疵区域
        /// </summary>
        private void Clear_Rect()
        {
            FabricInfos.Clear();    // 清除原瑕疵列表
            Kind2Num.Clear();   // 清除编号
            foreach (MyRect rect in rects)
            {
                ImageParent.Children.Remove(rect.Rec);  // 将当前画板上的区域全部删除
            }
        }

        /// <summary>
        /// 查找特定的矩形
        /// </summary>
        /// <param name="curRect"></param>
        /// <returns></returns>
        private MyRect Find_Rect( Rectangle curRect)
        {
            foreach(MyRect r in rects)
            {
                if (curRect == r.Rec)
                {
                    return r;
                }
            }
            return null;
        }

        /// <summary>
        /// 鼠标进入右边瑕疵列表的瑕疵对选中瑕疵高亮显示
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FabricItem_MouseEnter(object sender, MouseEventArgs e)
        {
            TextBlock textBlock = sender as TextBlock;
            string name = textBlock.Text;   // 瑕疵名-唯一标识
            if (preHighLightRect != null)
            {
                preHighLightRect.Fill = new SolidColorBrush(Color.FromArgb(0, 0, 0, 0));
            }
            HighLightRect(name);    // 高亮显示
        }

        /// <summary>
        /// 给特定瑕疵高亮显示
        /// </summary>
        /// <param name="name"></param>
        private void HighLightRect(string name)
        {
            foreach(FabricInfo fabric in FabricInfos)
            {
                if (fabric.IsChecked && fabric.KindName.Equals(name))
                {
                    preHighLightRect = fabric.CurRect;
                    fabric.CurRect.Fill = new SolidColorBrush(Color.FromArgb(124, 102, 186, 214));
                }
            }
        }


        private void FabricItem_MouseLeave(object sender, MouseEventArgs e)
        {
            if(preHighLightRect!=null)  preHighLightRect.Fill = new SolidColorBrush(Color.FromArgb(0, 0, 0, 0)); 
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
        private string kind;    // 瑕疵种类
        private Rectangle rec;
        public Point Start { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }
        public Rectangle Rec { get; set; }
        public double WindowHeight { get; set; }
        public double WindowWidth { get; set; }
        public string Kind { get; set; }
        public override string ToString()
        {
            return Start + "  Width: " + Width.ToString() + " Height = " + Height.ToString();
        }
    }

}
