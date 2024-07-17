using System;
using System.Collections.Generic;
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

namespace MatchGame
{
    using System.Windows.Media.Animation;
    using System.Windows.Threading;
    public partial class MainWindow : Window
    {
        DispatcherTimer timer = new DispatcherTimer();
        int tenthsOfSeconds;
        int matchesFound;
        public MainWindow()
        {
            InitializeComponent();

            timer.Interval = TimeSpan.FromSeconds(.1);
            timer.Tick += Timer_Tick;
            SetUpGame();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            tenthsOfSeconds++;
            timeTextBlock.Text = (tenthsOfSeconds / 10F).ToString("0.0s");
            if(matchesFound == 8)
            {
                timer.Stop();
                timeTextBlock.Text = timeTextBlock.Text + " - Play again?";
            }
        }

        private void SetUpGame()
        {
            
            List<string> animalEmoji = new List<string>()
            {
                "🐯", "🐯",
                "🦝", "🦝",
                "🐒", "🐒",
                "🦐", "🦐",
                "🦭", "🦭",
                "🐸", "🐸",
                "🐼", "🐼",
                "🦩", "🦩",
            };
            Random random = new Random();

            foreach (TextBlock textBlock in mainGrid.Children.OfType<TextBlock>())
            {
                if(textBlock.Name != "timeTextBlock")
                {
                    int index = random.Next(animalEmoji.Count);
                    string nextEmoji = animalEmoji[index];
                    textBlock.Text = nextEmoji;
                    animalEmoji.RemoveAt(index);
                    textBlock.Visibility = Visibility.Visible;
                }
                
            }
            timer.Start();
            tenthsOfSeconds = 0;
            matchesFound = 0;
        }

        TextBlock lastTextBlockClicked;
        bool findingMatch = false;
        private void TextBlock_MouseDown(object sender, MouseButtonEventArgs e)
        {
           
            TextBlock clickedAnimal = sender as TextBlock;
            if(findingMatch == false)
            {
                clickedAnimal.Visibility = Visibility.Hidden;
                lastTextBlockClicked = clickedAnimal;
                findingMatch = true;
            }
            else if(clickedAnimal.Text == lastTextBlockClicked.Text)
            {
                matchesFound++;
                clickedAnimal.Visibility = Visibility.Hidden;
                findingMatch = false;
            }
            else
            {
                lastTextBlockClicked.Visibility = Visibility.Visible;
                findingMatch = false;
            }

        }

        private void timeTextBlock_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if(matchesFound == 8)
            {
                SetUpGame();
            }
        }

        private void TextBlock_MouseEnter(object sender, MouseEventArgs e)
        {
            var icon = sender as TextBlock;

            animateColor(icon, Colors.Black, Colors.Orange);
        }

        private void TextBlock_MouseLeave(object sender, MouseEventArgs e)
        {
            var icon = sender as TextBlock;
            animateColor(icon, Colors.Orange, Colors.Black);
        }

        private void animateColor(TextBlock icon, Color firstColor, Color secondColor)
        {
            ColorAnimation blackToWhite = new ColorAnimation(firstColor, secondColor, TimeSpan.FromSeconds(0.2));

            SolidColorBrush scb = new SolidColorBrush(firstColor);
            scb.BeginAnimation(SolidColorBrush.ColorProperty, blackToWhite);

            TextEffect tfe = new TextEffect();
            tfe.Foreground = scb;
            tfe.PositionStart = 0;
            tfe.PositionCount = int.MaxValue;

            icon.TextEffects = new TextEffectCollection();
            icon.TextEffects.Add(tfe);
        }
    }
}
