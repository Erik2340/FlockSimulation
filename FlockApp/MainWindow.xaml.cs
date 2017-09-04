using FlockLibrary;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace FlockApp
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly DispatcherTimer _timer = new DispatcherTimer();
        private readonly Random _random = new Random();
        private Flock flock;

        public MainWindow()
        {
            InitializeComponent();
            _timer.Tick += timer_Tick;
            _timer.Interval = TimeSpan.FromMilliseconds(20);
            _timer.Start();
            KeyDown += Canvas_KeyDown;
            flock = MakeFlock();
        }

        private void Canvas_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Down:
                    flock.Queen.setPosition(flock.Queen.Position[0], flock.Queen.Position[1] + 5);
                    break;

                case Key.Up:
                    flock.Queen.setPosition(flock.Queen.Position[0], flock.Queen.Position[1] - 5);
                    break;

                case Key.Right:
                    flock.Queen.setPosition(flock.Queen.Position[0] + 5, flock.Queen.Position[1]);
                    break;

                case Key.Left:
                    flock.Queen.setPosition(flock.Queen.Position[0] - 5, flock.Queen.Position[1]);
                    break;
            }
        }

        private Flock MakeFlock()
        {
            var flock = new Flock();
            for (var i = 0; i < 100; i++)
            {
                var bird = new Bird
                {
                    Id = "Mert"
                };
                bird.SetPosition((_random.NextDouble()+1) * 500, (_random.NextDouble()+1) * 500);
                bird.SetSpeed((_random.NextDouble()-0.5)*10, (_random.NextDouble()-0.5)*10);
                var bytes = new byte[3];
                _random.NextBytes(bytes);
                bird.Color = new FlockLibrary.Color(255,255,0);

                flock.Birds.Add(bird);
            }

            flock.Queen = new Queen();
            flock.Queen.Color = new FlockLibrary.Color(255, 0, 0); 
            flock.Queen.setPosition(50, 50);
            return flock;
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            var bytes = new byte[3];
            _random.NextBytes(bytes);
            // Canvas.Background = new SolidColorBrush(Color.FromRgb(bytes[0], bytes[1], bytes[2]));

            MoveFlock(flock);

            Canvas.Children.Clear();
            Canvas.ClipToBounds = true;
        
            foreach(var bird in flock.Birds)
            {
                DrawBird(bird);
            }

            DrawQueen(flock.Queen);

            DrawEnergy(flock);
        }

        private void DrawEnergy(Flock flock)
        {
            var energy = flock.KineticEnergy;
            var block = new TextBlock
            {
                Text = string.Format("kinetic energy = {0:g4}", energy),
                Foreground = new SolidColorBrush(System.Windows.Media.Color.FromRgb(0, 0, 0))
            };
            Canvas.SetLeft(block, 0);
            Canvas.SetTop(block, 0);
            Canvas.Children.Add(block);

        }

        private void DrawQueen(Queen queen)
        {
         
            var color = new SolidColorBrush(System.Windows.Media.Color.FromRgb(queen.Color.Red, queen.Color.Green, queen.Color.Blue));
            Ellipse el = new Ellipse();
            el.Height = 15;
            el.Width = 15;
            el.Fill = color;
            Canvas.SetTop(el, flock.Queen.Position[1]);
            Canvas.SetLeft(el, flock.Queen.Position[0]);

            Canvas.Children.Add(el);

            
         }

        private void MoveFlock(Flock flock)
        {

            if (flock.Queen.Position[0] > Canvas.ActualWidth)
            {
                flock.Queen.Position[0] = 0;
            }
            if (flock.Queen.Position[1] > Canvas.ActualHeight)
            {
                flock.Queen.Position[1] = 0;
            }
            if (flock.Queen.Position[0] < 0)
            {
                flock.Queen.Position[0] = Canvas.ActualWidth;
            }
            if (flock.Queen.Position[1] < 0)
            {
                flock.Queen.Position[1] = Canvas.ActualHeight;
            }

            foreach (var bird in flock.Birds)
            {
                bird.Position += bird.Speed;

                if (bird.Position[0] > Canvas.ActualWidth)
                {
                    bird.Position[0] = 0;
                }
                if (bird.Position[1] > Canvas.ActualHeight)
                {
                    bird.Position[1] = 0;
                }
                if (bird.Position[0] < 0)
                {
                    bird.Position[0] = Canvas.ActualWidth;
                }
                if (bird.Position[1] < 0)
                {
                    bird.Position[1] = Canvas.ActualHeight;
                }
            }

            for (var i = 0; i < flock.Birds.Count; i++)
            {
                var birdI = flock.Birds[i];
                var queen = flock.Queen;
                var deltaQ = birdI.Vectorbetween(queen);
                var distanceQ = deltaQ.Norm(2);
                if (distanceQ == 0.0) break;
                var forceQ = (-1 / distanceQ * distanceQ);
                if (distanceQ < 25) { forceQ = 10; };
                deltaQ = deltaQ.Normalize(2);
                birdI.Speed = birdI.Speed + forceQ * deltaQ;


                for (var k = 0; k < i; k++)
                {
                    if ((birdI.Vectorbetween(flock.Queen)).Norm(2)> 100) { break; }
                    var birdK = flock.Birds[k];
                    var delta = birdI.Vectorbetween(birdK);
                    var distance = delta.Norm(2);
                    if (distance == 0.0) break;
                    var force = (-0.25 / distance * distance)*0.07;
                    if (distance < 25) force = 10;
                    delta = delta.Normalize(2);
                    birdI.Speed = birdI.Speed + force * delta;
                    birdK.Speed = birdK.Speed - force * delta;
                }
                birdI.Speed[0] += (_random.NextDouble() - 0.5) * 0.1;
                birdI.Speed[1] += (_random.NextDouble() - 0.5) * 0.1;

                birdI.Speed -= birdI.Speed * 0.3;

                //birdI.Speed[0] += 0.25;

            }

        }

        private void DrawBird(Bird bird)
        {
            var color = new SolidColorBrush(System.Windows.Media.Color.FromRgb(bird.Color.Red, bird.Color.Green, bird.Color.Blue));
            Canvas.Children.Add(new Polygon
            {
                Stroke = color,
                Fill = color,
                Points =
                {
                    new Point
                    {
                        X = bird.NosePosition[0],
                        Y = bird.NosePosition[1],
                    },
                    new Point
                    {
                        X = bird.TipPosition1[0],
                        Y = bird.TipPosition1[1],
                    },
                    new Point
                    {
                        X = bird.TipPosition2[0],
                        Y = bird.TipPosition2[1],
                    }
                }
            });
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Save file", "Save");
        }

        private void StartStopButton_Click(object sender, RoutedEventArgs e)
        {
            if (_timer.IsEnabled)
            {
                _timer.Stop();
                StartStopButton.Content = "Start";
            }
            else
            {
                _timer.Start();
                StartStopButton.Content = "Stop";
            }
        }
    }
}
