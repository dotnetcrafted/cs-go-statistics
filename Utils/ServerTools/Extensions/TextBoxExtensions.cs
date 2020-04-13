using System;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using ServerTools.Enums;

namespace ServerTools.Extensions
{
    public static class TextBoxExtensions
    {
        public static void WriteLine(this RichTextBox textBox, string text, LineTypes type = LineTypes.Text)
        {
            Color color;
            switch (type)
            {
                case LineTypes.Text:
                    color = Color.White;
                    break;
                case LineTypes.Warning:
                    color = Color.Orange;
                    break;
                case LineTypes.Error:
                    color = Color.Red;
                    break;
                case LineTypes.Success:
                    color = Color.Green;
                    break;
                default:
                    color = Color.White;
                    break;
            }

            textBox.SelectionStart = textBox.TextLength;
            textBox.SelectionLength = 0;
            textBox.SelectionColor = color;
            textBox.AppendText(DateTime.Now.ToString(CultureInfo.InvariantCulture) + " " + text + Environment.NewLine);
            textBox.SelectionColor = textBox.ForeColor;
        }
    }
}