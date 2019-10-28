using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media.Animation;

namespace ProjectPart1
{
    public partial class MainWindow : Window
    {    
        public MainWindow()
        {
            InitializeComponent();
        }

        System.Windows.Threading.DispatcherTimer dispatcherTimer = new System.Windows.Threading.DispatcherTimer();

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            // make game element visible
            ((Button)sender).Visibility = Visibility.Hidden;
            TypingBox.Visibility = Visibility.Visible;
            TimerBlock.Visibility = Visibility.Visible;
            LifeCount.Visibility = Visibility.Visible;

            // initialise the content
            TimerBlock.Text = GameController.Instance.Counter + "s";
            LifeCount.Text = "Life: " + GameController.Instance.Life;
            TypingBox.Focus();

            // assign the delegate to call in each tick
            dispatcherTimer.Tick += dispatcherTimer_Tick;
            dispatcherTimer.Interval = TimeSpan.FromSeconds(1);
            dispatcherTimer.Start();

            // start to throw the first word
            GameController.Instance.DropWord(WordCanvas);
        }

        // Every seconde past, trigger this call
        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            GameController.Instance.Counter--;
            TimerBlock.Text = GameController.Instance.Counter + "s";
            LifeCount.Text = "Life: " + GameController.Instance.Life;
            if (GameController.Instance.Counter == 0) {
                StopTimer();
                GameController.Instance.EndGame();
            }
        }

        private void StopTimer()
        {
            dispatcherTimer.Tick -= dispatcherTimer_Tick;
            dispatcherTimer.Stop();
        }

        private void TypingBlock_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
            {
                for (int i = 0; i < WordCanvas.Children.Count; i++)     // search for the word typed
                {
                    TextBlock word = (TextBlock)WordCanvas.Children[i];
                    if (TypingBox.Text == word.Text)
                    {
                        WordCanvas.Children.RemoveAt(i);                // removed the typed word
                        TypingBox.Text = "";                            // clear the typing box for the player
                        GameController.Instance.AddScore();             // add the score
                    }
                }
                GameController.Instance.DropWord(WordCanvas);           // whenever the player hit space bar, gen a word
                e.Handled = true;
            }
        }

        private class GameController
        {
            private int counter = 60; // for counting 1 minute
            public int Counter {
                get { return counter; }
                set { if (value >= 0) counter = value; }
            }

            private int life = 5;  // 5 chance
            public int Life
            {
                get { return life; }
                set { if (value >= 0) life = value; }
            }

            private int score = 0;
            public int Score {
                get { return score; }
                set { if (value > score) score = value; }
            }

            Random rnd = new Random();

            // Signleton
            static private GameController instance;
            static public GameController Instance
            {
                get
                {
                    if (instance == null)
                        instance = new GameController();
                    return instance;
                }
            }                        public void DropWord(Canvas obj) {
                GenWord word = new GenWord();
                TextBlock wordBlock = new TextBlock
                {
                    Width = 220,
                    FontSize = 24,
                    TextAlignment = TextAlignment.Center,
                    Text = word.getWord()
                };
                obj.Children.Add(wordBlock);
                Canvas.SetTop(wordBlock, rnd.Next(200));                        // randomly place the text block
                Canvas.SetLeft(wordBlock, rnd.Next(400));

                DoubleAnimation WordAnimation = new DoubleAnimation();          // create animation for falling words
                WordAnimation.From = Canvas.GetTop(wordBlock);
                WordAnimation.To = 405;
                WordAnimation.Duration = new Duration(TimeSpan.FromSeconds(10));

                Storyboard.SetTarget(WordAnimation, wordBlock);
                Storyboard.SetTargetProperty(WordAnimation, new PropertyPath(Canvas.TopProperty));
                Storyboard story = new Storyboard();

                story.Completed += GameController.Instance.Scold;
                story.Completed += (o, s) => { obj.Children.Remove(wordBlock);};
                story.Children.Add(WordAnimation);
                story.Begin();
            }            public void AddScore() {
                this.Score++;
            }            public void Scold(object sender, EventArgs e) {
                Life--;
                if (Life == 0)
                {
                    GameController.Instance.EndGame();
                }
            }

            public void EndGame() {
                if (Life != 0)
                    MessageBox.Show(this.Score + " words typed with " + this.Life + " chance left. You survived !!");
                else MessageBox.Show(this.Score + " words typed only. You are fired !!");
                System.Windows.Application.Current.Shutdown();
            }
        }

        private class GenWord {
            private string[] words = new string[]{"if","private","public","return","System","static","protected","get","set","else","break","continue",
                                                  "this","new","int","string","double","List","Dictionary","true","false","null","delegate","instance",
                                                  "for","while","bool","object","namespace","override","virtual","abstract","Generic","try","catch",
                                                  "foreach","Exception","ToString","Console.WriteLine","Collections","ContainsKey","TryGetValue",
                                                  "KeyValuePair","KeyCollection","ValueCollection" };

            Random rnd = new Random();
            public string getWord() {
                return words[rnd.Next(1, words.Length)];
            }
        }
    }
}
