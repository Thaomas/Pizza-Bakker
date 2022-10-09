using System.Windows;

namespace Pizza_Client.Views
{
    /// <summary>
    /// Interaction logic for AddIngredientView.xaml
    /// </summary>
    public partial class AddIngredientView : Window
    {
        public AddIngredientView()
        {
            InitializeComponent();
        }

        private void IngredientAmountTextBox_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            e.Handled = !uint.TryParse(e.Text + "0", out _);
        }

        private void IngredientValueTextBox_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            e.Handled = !decimal.TryParse(e.Text, out _);
        }
    }
}
