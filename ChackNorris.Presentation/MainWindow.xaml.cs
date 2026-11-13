using ChackNorris.Application.Services;
using ChackNorris.Domain.Models;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ChackNorris.Presentation
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //private readonly string _connectionString = "Server=.\\SQLEXPRESS;Database=ChuckNorrisDb;Trusted_Connection=True;Encrypt=True;TrustServerCertificate=True;";
        private readonly JokeService _jokeService;
        private Joke? _currentJoke;
        public MainWindow()
        {
            InitializeComponent();

                 string _connectionString =
            "Server=.\\SQLEXPRESS;Database=ChuckNorrisDb;Trusted_Connection=True;Encrypt=True;TrustServerCertificate=True;";

            _jokeService = new JokeService(_connectionString);
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //jokesListBox.ItemsSource = await _jokeService.GetAllJokes();
            await RefreshListBox();

            Joke joke = await _jokeService.GetRandomJokeAsync();
            jokeTextBlock.Text = joke.Content ?? "Joke niet gevonden";

            if (jokeTextBlock.Text != null)
                saveButton.IsEnabled = true;
        }
        private async Task RefreshListBox()
        {
            //jokesListBox.ItemsSource = null;
            jokesListBox.ItemsSource = await _jokeService.GetAllJokesAsync();
        }
        private void JokesListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (jokesListBox.SelectedItem is Joke selectedJoke)
            {
                selectedJokeTextBlock.Text = selectedJoke.Content;
                DisplayAverageScore(selectedJoke.Score);
            }
            else
            {
                selectedJokeTextBlock.Text = "Select a joke to view details";
                DisplayAverageScore(0);
            }
        }


        private void DisplayAverageScore(double score)
        {
            averageScorePanel.Children.Clear();

            int fullStars = (int)score;
            bool hasHalfStar = (score - fullStars) >= 0.5;

            for (int i = 1; i <= 5; i++)
            {
                var starText = new TextBlock
                {
                    Text = i <= fullStars ? "★" : (i == fullStars + 1 && hasHalfStar ? "⯨" : "☆"),
                    FontSize = 24,
                    Width = 40,
                    Height = 40,
                    TextAlignment = TextAlignment.Center,
                    Margin = new Thickness(2),
                    Foreground = Brushes.Gold
                };
                averageScorePanel.Children.Add(starText);
            }

            var scoreText = new TextBlock
            {
                Text = $" ({score:F1})",
                FontSize = 14,
                VerticalAlignment = VerticalAlignment.Center,
                Margin = new Thickness(5, 0, 0, 0)
            };
            averageScorePanel.Children.Add(scoreText);
        }

        private async void StarButton_Click(object sender, RoutedEventArgs e)
        {  
            if (sender is Button button && int.TryParse(button.Tag.ToString(), out int score))
            {
                //TODO:
                if (jokesListBox.SelectedItem is not Joke selectedJoke)
                {
                    MessageBox.Show("Selecteer eerst je favorite joke van lijst", 
                        "Fout",
                        MessageBoxButton.OK, 
                        MessageBoxImage.Exclamation);
                }
                else
                {
                    UpdateReviewStars(score);

                    Review newReview = new Review { JokeId = selectedJoke.Id, Score = score };
                    await _jokeService.AddReviewAsync(newReview);
                }                

                //Reset buttons to ☆☆☆☆☆:
                UpdateReviewStars(0);
            }
        }

        private void StarButton_MouseMove(object sender, MouseEventArgs e)
        {
            if (sender is Button button && int.TryParse(button.Tag.ToString(), out int score))
            {
                UpdateReviewStars(score);
            }
        }

        private void UpdateReviewStars(int score)
        {
            var starButtons = new[] { star1Button, star2Button, star3Button, star4Button, star5Button };

            for (int i = 0; i < starButtons.Length; i++)
            {
                starButtons[i].Content = i < score ? "★" : "☆";
            }
        }

        private void StarButton_MouseLeave(object sender, MouseEventArgs e)
        {
            //Reset buttons to ☆☆☆☆☆:
            UpdateReviewStars(0);
        }

        private async void refreshButton_Click(object sender, RoutedEventArgs e)
        {
            _currentJoke = await _jokeService.GetRandomJokeAsync();
            jokeTextBlock.Text = _currentJoke.Content ?? "Joke niet gevonden";
        }

        private async void saveButton_Click(object sender, RoutedEventArgs e)
        {
            string joke = jokeTextBlock.Text;
            try
            {
                Joke newJoke = new Joke { Content = joke, Score = 0 };
                await _jokeService.AddJokeAsync(newJoke);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            await RefreshListBox();
        }

        private void jokesListBox_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            if (jokesListBox.SelectedItem is not Joke selectedJoke)
            {
                MessageBox.Show("Selecteer eerst je favorite joke!");
            }

        }
    }
}