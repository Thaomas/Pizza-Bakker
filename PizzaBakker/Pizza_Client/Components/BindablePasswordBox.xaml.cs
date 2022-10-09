using System.Windows;
using System.Windows.Controls;

namespace Pizza_Client.Components
{
    /// <summary>
    /// Interaction logic for BindablePasswordBox.xaml
    /// </summary>
    public partial class BindablePasswordBox : UserControl
    {
        private bool _isPasswordChanging;

        public static readonly DependencyProperty PasswordProperty = DependencyProperty.Register
            (
            "Password",
            typeof(string),
            typeof(BindablePasswordBox),
            new FrameworkPropertyMetadata
            (
                string.Empty,
                FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                PasswordPropertyChanged,
                null,
                false,
                System.Windows.Data.UpdateSourceTrigger.PropertyChanged
                ));

        private static void PasswordPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is BindablePasswordBox passwordBox)
            {
                passwordBox.UpdatePassword();
            }
        }

        private void UpdatePassword()
        {
            if (!_isPasswordChanging) { passwordBox.Password = Password; }
        }

        public string Password
        {
            get { return (string)GetValue(PasswordProperty); }
            set { SetValue(PasswordProperty, value); }
        }

        public BindablePasswordBox()
        {
            InitializeComponent();
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            _isPasswordChanging = true;
            Password = passwordBox.Password;
            _isPasswordChanging = false;
        }
    }
}
