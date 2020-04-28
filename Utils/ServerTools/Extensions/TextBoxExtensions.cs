using System;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using CsStat.SystemFacade.Extensions;
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
                    color = Color.WhiteSmoke;
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
                case LineTypes.CtWin:
                    color = Color.Aqua;
                    break;
                case LineTypes.TerroristWin:
                    color = Color.DarkOrange;
                    break;
                default:
                    color = Color.WhiteSmoke;
                    break;
            }

            textBox.SelectionStart = textBox.TextLength;
            textBox.SelectionLength = 0;
            textBox.SelectionColor = color;
            textBox.AppendText(DateTime.Now.ToString(CultureInfo.InvariantCulture) + " " + text + Environment.NewLine);
            textBox.SelectionColor = textBox.ForeColor;
        }

        public static string LastLine(this RichTextBox textBox)
        {
            return textBox.Text.Split('\n').Last(x => x.IsNotEmpty());
        }
    }
}