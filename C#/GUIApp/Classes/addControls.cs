using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GUIApp
{
    static public class addControls
    {

        static public Label createLabel(string Text, int x, int y)
        {
            Label label = new Label();
            label.Text = Text;
            label.AutoSize = true;
            label.Location = new Point(x, y);

            return label;
        }

        static public PictureBox createPB(Bitmap Image, int x, int y, int width, int heigth)
        {
            PictureBox pictureBox = new PictureBox();
            pictureBox.Image = Image;
            pictureBox.Size = new Size(width, heigth);
            pictureBox.Location = new Point(x, y);
            pictureBox.SizeMode = PictureBoxSizeMode.Zoom;

            return pictureBox;
        }

        static public Button createButton(string Text, int x, int y, int width, int height)
        {
            Button button = new Button();
            button.Text = Text;
            button.Size = new Size(width, height);
            button.Location = new Point(x, y);

            return button;
        }

    }
}
