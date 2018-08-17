using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace HappyValentinesDay
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        MediaPlayer mp = new MediaPlayer();
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
                Close();
        }

        void Start(Canvas panel)
        {
            Random random = new Random();
            Task.Factory.StartNew(new Action(() =>
            {
                for (int j = 0; j < 25; j++)
                {
                    Thread.Sleep(j * 100);
                    Dispatcher.Invoke(new Action(() =>
                    {
                        int snowCount = random.Next(0, 10);
                        for (int i = 0; i < snowCount; i++)
                        {
                            int width = random.Next(10, 40);
                            Petal pack = new Petal();
                            pack.Width = width;
                            pack.Height = width;
                            pack.RenderTransform = new RotateTransform();

                            int left = random.Next(0, (int)panel.ActualWidth);
                            Canvas.SetLeft(pack, left);
                            panel.Children.Add(pack);
                            int seconds = random.Next(20, 30);
                            pack.BeginAnimation(OpacityProperty, new DoubleAnimation(0, 1, TimeSpan.FromSeconds(1)));
                            DoubleAnimationUsingPath doubleAnimation = new DoubleAnimationUsingPath()        //下降动画
                            {
                                Duration = new Duration(new TimeSpan(0, 0, seconds)),
                                RepeatBehavior = RepeatBehavior.Forever,
                                PathGeometry = new PathGeometry(new List<PathFigure>() { new PathFigure(new Point(left, 0), new List<PathSegment>() { new LineSegment(new Point(left, panel.ActualHeight), false) }, false) }),
                                Source = PathAnimationSource.Y
                            };
                            pack.BeginAnimation(Canvas.TopProperty, doubleAnimation);
                            DoubleAnimation doubleAnimation1 = new DoubleAnimation(360, new Duration(new TimeSpan(0, 0, 10)))              //旋转动画
                            {
                                RepeatBehavior = RepeatBehavior.Forever,
                            };
                            pack.RenderTransform.BeginAnimation(RotateTransform.AngleProperty, doubleAnimation1);
                        }
                    }));
                }
            }));
        }
        private async void StartTextAsync(String[] data) {
            for(int i = 0; i < data.Length; i++) { 
                Tb.BeginAnimation(OpacityProperty, new DoubleAnimation(0, 1, TimeSpan.FromSeconds(1.2)));
                Tb.Text = data[i];
                if (i != data.Length-1){
                    await Task.Delay(3000);
                    Tb.BeginAnimation(OpacityProperty, new DoubleAnimation(1, 0, TimeSpan.FromSeconds(1.5)));
                    await Task.Delay(1500);
                }
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            mp.Open(new Uri("http://ws.stream.qqmusic.qq.com/C100000gVgYF0knVAk.m4a?fromtag=46"));
            mp.Play();
            mp.MediaEnded += delegate { mp.Position = TimeSpan.FromSeconds(0); mp.Play(); };
            Start(PetalBackground);
            String data = "我一直在等待你的出现^" +
    "谢谢你选择了我^" +
    "此生不换^" +
    "执子之手，与子偕老^" +
    "携手到永远……";
            string path = AppDomain.CurrentDomain.BaseDirectory + "/Data.txt";
            if (File.Exists(path))
                data = File.ReadAllText(path);
            else File.WriteAllText(path, data);
            StartTextAsync(data.Split('^'));
        }
    }
}
