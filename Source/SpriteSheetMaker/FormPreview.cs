using System.Drawing;
using System.Windows.Forms;

namespace SpriteSheetMaker
{
    public sealed partial class FormPreview : Form
    {
        public FormPreview(Image image)
        {
            InitializeComponent();
            pictureBox1.Image = image;
        }

        private void FormPreview_KeyUp(object sender, KeyEventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }
    }
}