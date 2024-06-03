#pragma warning disable IDE1006
using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace SpriteSheetMaker
{
    public sealed partial class FormMain : Form
    {
        public FormMain()
        {
            InitializeComponent();
        }

        private void exitButton_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void aboutToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            MessageBox.Show("SpriteSheetMaker 1.0\nBy Brain Bluetooth\nhanjisung686@gmail.com", "SpriteSheetMaker Info");
        }

        private void addButton_Click(object sender, EventArgs e)
        {
            var result = openFileDialog_Image.ShowDialog();
            if (!result.HasFlag(DialogResult.Yes) && !result.HasFlag(DialogResult.OK))
                return;
            var files = openFileDialog_Image.FileNames;
            foreach (var file in files)
                imageList.Items.Add(new FileInfo(file));

            AfterChangeList();
        }
        private void deleteButton_Click(object sender, EventArgs e)
        {
            int index = imageList.SelectedIndex;
            if (index == -1)
                return;
            imageList.Items.RemoveAt(index);
            index = Math.Min(index, imageList.Items.Count - 1);
            if (index != -1)
                imageList.SelectedIndex = index;

            AfterChangeList();
        }
        private void clearButton_Click(object sender, EventArgs e)
        {
            imageList.Items.Clear();

            AfterChangeList();
        }
        private void AfterChangeList()
        {
            imageGroup.Text = $"Images ({imageList.Items.Count})";
            numericUpDown1_ValueChanged(null, null);
        }

        private void Flip(int index1, int index2)
        {
            var list = imageList.Items;
            var obj1 = list[index1];
            var obj2 = list[index2];
            list[index1] = obj2;
            list[index2] = obj1;
        }
        private void flipButton_Click(object sender, EventArgs e)
        {
            int n = imageList.Items.Count;
            int k = n >> 1;
            --n;
            for (int i = 0; i < k; i++)
                Flip(i, n - i);
        }
        private void moveUpButton_Click(object sender, EventArgs e)
        {
            var index = imageList.SelectedIndex;
            if (index < 1)
                return;
            Flip(index, index - 1);
            imageList.SelectedIndex = index - 1;
        }
        private void moveDownButton_Click(object sender, EventArgs e)
        {
            var index = imageList.SelectedIndex;
            if (index == -1 || index == imageList.Items.Count - 1)
                return;
            Flip(index, index + 1);
            imageList.SelectedIndex = index + 1;
        }

        private void orderByAscButton_Click(object sender, EventArgs e)
        {
            var list = imageList.Items;
            int len = list.Count;
            FileInfo[] arr = new FileInfo[len];
            for (int i = 0; i < len; i++)
                arr[i] = list[i] as FileInfo;
            Array.Sort(arr);
            for (int i = 0; i < len; i++)
                list[i] = arr[i];
        }
        private void orderByDescButton_Click(object sender, EventArgs e)
        {
            var list = imageList.Items;
            int len = list.Count;
            FileInfo[] arr = new FileInfo[len];
            for (int i = 0; i < len; i++)
                arr[i] = list[i] as FileInfo;
            Array.Sort(arr, (FileInfo a, FileInfo b) => b.CompareTo(a));
            for (int i = 0; i < len; i++)
                list[i] = arr[i];
        }

        private void mergeButton_Click(object sender, EventArgs e)
        {
            var list = imageList.Items;
            if (list.Count == 0)
            {
                Debug.LogError("No image to merge!");
                return;
            }

            var prevImg = previewBox.Image;
            if (prevImg != null)
            {
                prevImg.Dispose();
                previewBox.Image = null;
            }

            int nCols = (int)numericUpDown1.Value;
            int nRows = (int)numericUpDown2.Value;

            int singleW, singleH;
            using (var tmp = new Bitmap((list[0] as FileInfo).path))
            {
                singleW = tmp.Width;
                singleH = tmp.Height;
            }

            Bitmap bmp = new Bitmap(nCols * singleW, nRows * singleH);
            previewBox.Image = bmp;

            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.Clear(Color.Transparent);
                int i = 0;
                for (int row = 0; row < nRows; row++)
                    for (int col = 0; col < nCols; col++)
                    {
                        var info = list[i++] as FileInfo;
                        var path = info.path;
                        Bitmap src;
                        try
                        {
                            src = new Bitmap(path);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.ToString());
                            continue;
                        }

                        if (src.Width != singleW || src.Height != singleH)
                            Debug.LogWarning($"Size of image({info.name}) is different with others. It leads to an incorrect sprite sheet!");

                        try
                        {
                            g.DrawImageUnscaled(src, singleW * col, singleH * row);
                        }
                        finally
                        {
                            src.Dispose();
                        }
                    }
            }

            AfterMerge();
        }
        private void AfterMerge()
        {
            var img = previewBox.Image;
            label3.Text = $"Width: {img.Width}{Environment.NewLine}Height: {img.Height}";
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            var image = previewBox.Image;
            if (image == null)
                return;

            var response = saveFileDialog_Image.ShowDialog();
            if (!response.HasFlag(DialogResult.OK) && !response.HasFlag(DialogResult.Yes))
                return;

            string path = saveFileDialog_Image.FileName;
            image.Save(path);
        }

        private static int DivCeil(int a, int b)
        {
            int div = Math.DivRem(a, b, out int rem);
            if (rem != 0)
                div++;
            return div;
        }
        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            int count = imageList.Items.Count;
            numericUpDown2.Value = Math.Max(1, DivCeil(count, (int)numericUpDown1.Value));
        }
        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            int count = imageList.Items.Count;
            numericUpDown1.Value = Math.Max(1, DivCeil(count, (int)numericUpDown2.Value));
        }

        private void previewGluedImageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var image = previewBox.Image;
            if (image == null)
            {
                Debug.LogError("No merged image!");
                return;
            }
            FormPreview form = new FormPreview(image);
            form.ShowDialog(this);
        }

        private void newProjectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Debug.LogException(new NotImplementedException());
        }

        private void loadProjectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Debug.LogException(new NotImplementedException());
        }

        private void saveProjectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Debug.LogException(new NotImplementedException());
        }
    }
}