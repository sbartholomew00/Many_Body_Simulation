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

using System.Windows.Media.Media3D;

namespace Many_Body_Simulation
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        SimulationManager simulationManager;
        I3dDisplay display;
        System.Timers.Timer timer;
        System.Windows.Threading.Dispatcher dispatcher;

        VisualBody? _selectedVisualBody;
        VisualBody? SelectedVisualBody
        {
            get { return _selectedVisualBody; }
            set
            {
                _selectedVisualBody = value;
                if (_selectedVisualBody != null)
                {
                    xPositionTextBox.Text = _selectedVisualBody.Position.Item1.ToString();
                    yPositionTextBox.Text = _selectedVisualBody.Position.Item2.ToString();
                    zPositionTextBox.Text = _selectedVisualBody.Position.Item3.ToString();
                    xVelocityTextBox.Text = _selectedVisualBody.Velocity.Item1.ToString();
                    yVelocityTextBox.Text = _selectedVisualBody.Velocity.Item2.ToString();
                    zVelocityTextBox.Text = _selectedVisualBody.Velocity.Item3.ToString();
                    massTextBox.Text = _selectedVisualBody.Mass.ToString();
                }
                else
                {
                    xPositionTextBox.Text = "";
                    yPositionTextBox.Text = "";
                    zPositionTextBox.Text = "";
                    xVelocityTextBox.Text = "";
                    yVelocityTextBox.Text = "";
                    zVelocityTextBox.Text = "";
                    massTextBox.Text = "";
                }
            }
        }
        Point? lastMouseLocation;

        public MainWindow()
        {
            InitializeComponent();

            simulationManager = new SimulationManager(new EulerMethod());
            display = new Viewport3dManager(viewport3d);

            timer = new();
            timer.Interval = 1000 / 60; // ~60 per second
            timer.Elapsed += Tick;
            timer.Enabled = true;

            dispatcher = Dispatcher;

            SelectedVisualBody = null;
            lastMouseLocation = null;
        }
        ~MainWindow(){
            timer.Stop();
        }

        private void Tick(object? sender, EventArgs e)
        {
            dispatcher.BeginInvoke(new Action(() =>
            {
                simulationManager.Update();
                display.Update();
                if ((string)pauseButton.Content == ("Pause"))
                    SelectedVisualBody = SelectedVisualBody; // Updates textboxes while unpaused
            }
                ));
        }

        private void LoadButton_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog();
            openFileDialog.DefaultExt = ".json";
            openFileDialog.Filter = "Json files (*.json)|*.json|All files (*.*)|*.*";
            if (openFileDialog.ShowDialog() == true)
            {
                simulationManager.LoadState(System.IO.File.ReadAllText(openFileDialog.FileName));
            }

            List<VisualBody> bodyList = simulationManager.GetBodies();
            display.setVisuals(bodyList);
            // TODO: stop accessing viewport in this file.
        }
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.SaveFileDialog saveFileDialog = new Microsoft.Win32.SaveFileDialog();
            saveFileDialog.DefaultExt = ".json";
            saveFileDialog.Filter = "Json files (*.json)|*.json|All files (*.*)|*.*";
            if (saveFileDialog.ShowDialog() == true)
            {
                string jsonString = simulationManager.SaveState();
                using (System.IO.StreamWriter file = new(saveFileDialog.FileName))
                {
                    file.Write(jsonString);
                }
            }
        }
        private void PauseButton_Click(object sender, RoutedEventArgs e)
        {
            if((string)pauseButton.Content == ("Unpause"))
            {
                pauseButton.Content = "Pause";
                simulationManager.SetPaused(false);
            }
            else
            {
                pauseButton.Content = "Unpause";
                simulationManager.SetPaused(true);
            }
        }
        private void AddBodyButton_Click(object sender, RoutedEventArgs e)
        {
            VisualBody visual = new(new Tuple<double, double, double>(0.0, 0.0, 0.0),
                new Tuple<double, double, double>(0.0, 0.0, 0.0),
                1.0);
            viewport3d.AddBody(visual);
            simulationManager.AddBody(visual);
            SelectedVisualBody = visual;
        }
        private void UpButton_Click(object sender, RoutedEventArgs e)
        {
            display.SetCameraMovement(I3dDisplay.Direction.Up);
        }
        private void LeftButton_Click(object sender, RoutedEventArgs e)
        {
            display.SetCameraMovement(I3dDisplay.Direction.Left);
        }
        private void RightButton_Click(object sender, RoutedEventArgs e)
        {
            display.SetCameraMovement(I3dDisplay.Direction.Right);
        }
        private void DownButton_Click(object sender, RoutedEventArgs e)
        {
            display.SetCameraMovement(I3dDisplay.Direction.Down);
        }
        private void ZoomInButton_Click(object sender, RoutedEventArgs e)
        {
            display.SetCameraMovement(I3dDisplay.Direction.In);
        }
        private void ZoomOutButton_Click(object sender, RoutedEventArgs e)
        {
            display.SetCameraMovement(I3dDisplay.Direction.Out);
        }
        private void MovementButtonRelease(object sender, RoutedEventArgs e)
        {
            display.SetCameraMovement(I3dDisplay.Direction.None);
        }
        private void SimulationSpeed_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void Name_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void XPosition_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (SelectedVisualBody != null)
                {
                    try
                    {
                        SelectedVisualBody.Position = new(Convert.ToDouble(xPositionTextBox.Text), // TODO: Check for NAN, inf, and any other bad values for all numeric inputs
                            SelectedVisualBody.Position.Item2,
                            SelectedVisualBody.Position.Item3);
                    }
                    catch { }
                }
            }
        }
        private void YPosition_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (SelectedVisualBody != null)
                {
                    try
                    {
                        SelectedVisualBody.Position = new(SelectedVisualBody.Position.Item1,
                            Convert.ToDouble(yPositionTextBox.Text),
                            SelectedVisualBody.Position.Item3);
                    }
                    catch { }
                }
            }
        }
        private void ZPosition_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (SelectedVisualBody != null)
                {
                    try
                    {
                        SelectedVisualBody.Position = new(SelectedVisualBody.Position.Item1,
                            SelectedVisualBody.Position.Item2,
                            Convert.ToDouble(zPositionTextBox.Text));
                    }
                    catch { }
                }
            }
        }
        private void XVelocity_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (SelectedVisualBody != null)
                {
                    try
                    {
                        SelectedVisualBody.Velocity = new(Convert.ToDouble(xVelocityTextBox.Text),
                            SelectedVisualBody.Velocity.Item2,
                            SelectedVisualBody.Velocity.Item3);
                    }
                    catch { }
                }
            }
        }
        private void YVelocity_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (SelectedVisualBody != null)
                {
                    try
                    {
                        SelectedVisualBody.Velocity = new(SelectedVisualBody.Velocity.Item1,
                            Convert.ToDouble(yVelocityTextBox.Text),
                            SelectedVisualBody.Velocity.Item3);
                    }
                    catch { }
                }
            }
        }
        private void ZVelocity_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (SelectedVisualBody != null)
                {
                    try
                    {
                        SelectedVisualBody.Velocity = new(SelectedVisualBody.Velocity.Item1,
                            SelectedVisualBody.Velocity.Item2,
                            Convert.ToDouble(zVelocityTextBox.Text));
                    }
                    catch { }
                }
            }
        }
        private void Mass_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (SelectedVisualBody != null)
                {
                    try
                    {
                        SelectedVisualBody.Mass = Convert.ToDouble(massTextBox.Text);
                    }
                    catch { }
                }
            }
        }
        private void Radius_KeyDown(object sender, KeyEventArgs e)
        {

        }
        private void Red_KeyDown(object sender, KeyEventArgs e)
        {

        }
        private void Green_KeyDown(object sender, KeyEventArgs e)
        {

        }
        private void Blue_KeyDown(object sender, KeyEventArgs e)
        {

        }

        /// <summary>
        /// Won't be called when the background is clicked on (unless very close to something).
        /// </summary>
        private void Viewport_MouseDown(object sender, MouseButtonEventArgs e)
        {
            HitTestResult result = VisualTreeHelper.HitTest(viewport3d, e.GetPosition(viewport3d));
            if(result != null)
            {
                SelectedVisualBody = result.VisualHit as VisualBody;
            }
        }

        private void BackgroundMouseDown(object sender, MouseButtonEventArgs e) // Remove?
        {
            ;
        }
        private void BackgroundMouseUp(object sender, MouseButtonEventArgs e) // Remove?
        {
            ;
        }
        private void ViewportMouseMove(object sender, MouseEventArgs e)
        {
            if(e.LeftButton == MouseButtonState.Pressed)
            {
                if (lastMouseLocation != null)
                {
                    Vector change = (Vector)(e.GetPosition(viewport3d) - lastMouseLocation);
                    display.RotateCamera(change);
                }
                lastMouseLocation = e.GetPosition(viewport3d);
            }
            else
            {
                lastMouseLocation = null;
            }
        }
    }
}
