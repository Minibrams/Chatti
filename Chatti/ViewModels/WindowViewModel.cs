using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Chatti.ViewModels
{
    /// <summary>
    /// Basic View Model that is used for containing values for margins, spacings, and such. 
    /// The main window and others are heavily customized, so keep as many settings in here as possible
    /// </summary>
    class WindowViewModel : BaseViewModel
    {
        #region Private members
        private Window _window;
        private int _resizeBorderThickness = 6;
        private int _outerMarginSize       = 10;
        private int _outerMarginThickness  = 10;
        private int _windowRadius          = 10;
        #endregion

        #region Public Properties
        /// <summary>
        /// The number of pixels within which the user can resize the window at the borders
        /// </summary>
        public Thickness ResizeBorderThickness => new Thickness(_resizeBorderThickness + _outerMarginSize);

        /// <summary>
        /// The margin of the outer window - we use this for the surrounding dropshadow. 
        /// If the window is maximized, this is 0. 
        /// </summary>
        public int OuterMarginSize
        {
            get
            {
                return _window.WindowState == WindowState.Maximized ? 0 : _outerMarginSize;
            }
            set
            {
                _outerMarginSize = value;
            }
        }

        /// <summary>
        /// Radius of the window itself
        /// </summary>
        public int WindowRadius
        {
            get
            {
                return _window.WindowState == WindowState.Maximized ? 0 : _windowRadius;
            }
            set
            {
                _windowRadius = value;
            }
        }

        /// <summary>
        /// The border radius of the window corners. When the window is maximized, this value is 0.
        /// </summary>
        public CornerRadius WindowCornerRadius => new CornerRadius(_windowRadius);

        public Thickness OuterMarginThickness => new Thickness(_outerMarginThickness);

        /// <summary>
        /// The height of the title bar/caption of the window
        /// </summary>
        public int TitleHeight { get; set; } = 42;
        public GridLength TitleHeightGridLength => new GridLength(TitleHeight + _resizeBorderThickness );

        public double WindowMinimumHeight { get; set; } = 200;
        public double WindowMinimumWidth { get; set; } = 200;
        #endregion

        #region Commands
        public ICommand MinimizeCommand { get; set; }
        public ICommand MaximizeCommand { get; set; }
        public ICommand CloseCommand { get; set; }
        public ICommand MenuCommand { get; set; }

        #endregion

        // For old school debugging
        [DllImport("kernel32.dll")]
        private static extern bool AllocConsole();

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="window">The window that this view model should control</param>
        public WindowViewModel(Window window)
        {
            //AllocConsole();
            _window = window;

            // Find out when the window resizes
            _window.StateChanged += (sender, args) =>
            {
                // Let whoever is using the viewmodel know that the margin and thickness values should be adjusted
                OnPropertyChanged(nameof(ResizeBorderThickness));
                OnPropertyChanged(nameof(OuterMarginSize));
                OnPropertyChanged(nameof(OuterMarginThickness));
                OnPropertyChanged(nameof(WindowRadius));
                OnPropertyChanged(nameof(WindowCornerRadius));
            };

            // Commands 
            MinimizeCommand = new RelayCommand( x => 
            {
                _window.WindowState = WindowState.Minimized;
            }, x => true);

            MaximizeCommand = new RelayCommand(x =>
            {
                _window.WindowState ^= WindowState.Maximized;
            }, x => true);

            CloseCommand = new RelayCommand(x =>
            {
                _window.Close();
            }, x => true);

            MenuCommand = new RelayCommand(x => 
            {
                SystemCommands.ShowSystemMenu(_window, _window.PointFromScreen(Mouse.GetPosition(_window)));

            });
        }

        #region Helpers

        #endregion
    }
}
