using System.Diagnostics;
using System.Windows.Controls;

namespace Employee_Client.Views
{
    /// <summary>
    /// Interaction logic for HomepageView.xaml
    /// </summary>
    public partial class WarehouseView : UserControl
    {
        public WarehouseView()
        {
            InitializeComponent();
        }

        private void IngredientPriceTextBox_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {

            e.Handled = !decimal.TryParse(e.Text + "0", out _);
        }
        private void IngredientAmountTextBox_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            e.Handled = !uint.TryParse(e.Text, out _);
        }
    }
}
