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
using System.Windows.Threading;

namespace EvilDeleteSystem32
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private DispatcherTimer timer;
        private double progressStep;
        private const double totalDurationInSeconds = 60;//7200; // 2 hours
        private double totalProgressSteps;

        public MainWindow()
        {
            InitializeComponent();

            // Set up the progress increment based on the total duration
            totalProgressSteps = totalDurationInSeconds; // One step per second
            progressStep = 100.0 / totalProgressSteps; // Calculate percentage increment per tick

            // Initialize and start the timer
            timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(1) // Update every second
            };
            timer.Tick += Timer_Tick;
            timer.Start();

            this.StateChanged += MainWindow_StateChanged;
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            LoadingProgressBar.Value += progressStep;

            // Update loading text
            LoadingText.Text = $"Deleting C:\\Windows\\System32... {Math.Min((int)LoadingProgressBar.Value, 100)}%";

            // When progress completes, show the popup
            if (LoadingProgressBar.Value >= 100)
            {
                timer.Stop();
                CompletionPopup.IsOpen = true; // Show the popup
            }
        }

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            // Prevent closing by canceling the event
            e.Cancel = true;
            MessageBox.Show("This Window should not be closed.", "System Stability Warning", MessageBoxButton.OK, MessageBoxImage.Warning);

        }

        private void MainWindow_StateChanged(object sender, EventArgs e)
        {
            // Prevent minimizing the window
            if (this.WindowState == WindowState.Minimized)
            {
                MessageBox.Show("This Window should not be minimized.", "System Stability Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                this.WindowState = WindowState.Normal; // Reset state to Normal
            }
        }

    }


}