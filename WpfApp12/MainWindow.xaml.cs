using System.Windows;
using System.Windows.Media;
using System.Windows.Documents;
using System.Windows.Controls;

namespace WpfApp12
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            InitializeFontFamilyComboBox();
            InitializeFontSizeComboBox();
        }

        private void ToggleBold(object sender, RoutedEventArgs e)
        {
            ApplyToSelectedText(TextBlock.FontWeightProperty, FontWeights.Bold);
        }

        private void ToggleItalic(object sender, RoutedEventArgs e)
        {
            ApplyToSelectedText(TextBlock.FontStyleProperty, FontStyles.Italic);
        }

        private void ToggleUnderline(object sender, RoutedEventArgs e)
        {
            TextDecorationCollection decorations = new TextDecorationCollection();
            decorations.Add(TextDecorations.Underline[0]);
            ApplyToSelectedText(Inline.TextDecorationsProperty, decorations);
        }

        private void ChangeFontColor(object sender, RoutedEventArgs e)
        {
            var dialog = new System.Windows.Forms.ColorDialog();
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                Color color = Color.FromArgb(dialog.Color.A, dialog.Color.R, dialog.Color.G, dialog.Color.B);
                ApplyToSelectedText(TextElement.ForegroundProperty, new SolidColorBrush(color));
            }
        }

        private void InitializeFontFamilyComboBox()
        {
            FontFamilyComboBox.ItemsSource = Fonts.SystemFontFamilies;
            FontFamilyComboBox.SelectedIndex = 0;
        }

        private void ChangeFontFamily(object sender, SelectionChangedEventArgs e)
        {
            FontFamily selectedFontFamily = (FontFamily)FontFamilyComboBox.SelectedItem;
            ApplyToSelectedText(TextElement.FontFamilyProperty, selectedFontFamily);
        }

        private void InitializeFontSizeComboBox()
        {
            for (double fontSize = 8; fontSize <= 72; fontSize += 2)
            {
                FontSizeComboBox.Items.Add(fontSize);
            }
            FontSizeComboBox.SelectedIndex = 5; // Default font size
        }

        private void ChangeFontSize(object sender, SelectionChangedEventArgs e)
        {
            double selectedFontSize = (double)FontSizeComboBox.SelectedItem;
            ApplyToSelectedText(TextElement.FontSizeProperty, selectedFontSize);
        }

        private void ApplyToSelectedText(DependencyProperty property, object value)
        {
            TextPointer selectionStart = richTextBox.Selection.Start;
            TextPointer selectionEnd = richTextBox.Selection.End;

            if (selectionStart != null && selectionEnd != null)
            {
                TextRange textRange = new TextRange(selectionStart, selectionEnd);
                if (textRange != null)
                {
                    if (textRange.GetPropertyValue(TextElement.FontFamilyProperty) != DependencyProperty.UnsetValue)
                    {
                        textRange.ApplyPropertyValue(property, value);
                    }
                    else
                    {
                        TextPointer originalPosition = richTextBox.CaretPosition;
                        richTextBox.CaretPosition = selectionStart.GetInsertionPosition(LogicalDirection.Backward);
                        textRange = new TextRange(richTextBox.CaretPosition, selectionEnd);
                        textRange.ApplyPropertyValue(property, value);
                        richTextBox.CaretPosition = originalPosition;
                    }
                }
            }
        }
    }
}