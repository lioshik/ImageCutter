using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.Globalization;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.IO;
using System.Net;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;

namespace ImageCutter
{
    public partial class Form1 : Form
    {
        FolderBrowserDialog dialogOpenSaveDest;
        String[] files;
        public FormStart formStart;
        String fileWaterMark = null;
        Bitmap waterMarkPreviewImage;

        public Form1(FormStart form)
        {
            this.MaximizeBox = false;
            this.formStart = form;
            InitializeComponent();
            dialogOpenSaveDest = new FolderBrowserDialog();
            buttonBrowseFolder2.Enabled = false;
            buttonBrowseFolder2.Visible = false;
            textBoxFolder2.Visible = false;
            textBoxFolder2.Enabled = false;
            label2.Visible = false;
            label2.Enabled = false;
            panelChooseImages.Location = new Point(3, 30);
        }

        private void DisableControls(Control con)
        {
            foreach (Control c in con.Controls)
            {
                DisableControls(c);
            }
            con.Enabled = false;
        }
        private void EnableControls(Control con)
        {
            foreach (Control c in con.Controls)
            {
                EnableControls(c);
            }
            con.Enabled = true;
        }
        private void initImagesListFromFolder()
        {
            String crpath = textBoxFolder1.Text;
            if (crpath.Length == 0)
            {
                return;
            }
            if (checkBox1.Checked)
            {
                files = Directory.GetFiles(crpath, "*", SearchOption.AllDirectories);
            } else
            {
                files = Directory.GetFiles(crpath);
            }
        }

        private void showError(String s)
        {
            string caption = "Ошибка";
            string message = s;
            MessageBoxButtons buttons = MessageBoxButtons.OK;
            var iconType = MessageBoxIcon.Error;
            MessageBox.Show(message, caption, buttons, iconType);
        }
        
        private string getBackupPath(String path, ref bool retCode)
        {
            retCode = false;
            int l = textBoxBackup.Text.Length;
            if (l == 0)
            {
                return "Папка для сохранения не выбрана";
            }
            String subFolder = "\\";
            if (radioButtonChooseF.Checked)
            {
                l = textBoxFolder1.Text.Length;
                if (l < path.Length)
                {
                    subFolder = "\\" + path.Substring(l, path.Length - l);
                }
                FileInfo f = new FileInfo(textBoxBackup.Text + subFolder);
                Directory.CreateDirectory(f.Directory.FullName);
                retCode = true;
                return textBoxBackup.Text + subFolder;
            } else
            {
                FileInfo f = new FileInfo(path);
                subFolder = "\\" + f.Name;
                Directory.CreateDirectory(new FileInfo(textBoxBackup.Text + subFolder).Directory.FullName);
                retCode = true;
                return textBoxBackup.Text + subFolder;
            }
            
        }

        private String getSavePath(String path, ref bool resCode)
        {
            resCode = false;
            String res;
            if (!checkBox2.Checked)
            {
                res = path;
            } else
            {
                String subFolder = "\\";
                if (radioButtonChooseF.Checked)
                {
                    int l = textBoxFolder1.Text.Length;
                    if (l == 0)
                    {
                        return "Папка для сохранения не выбрана";
                    }
                    if (l < path.Length)
                    {
                        subFolder = "\\" + path.Substring(l, path.Length - l);
                    }
                } else
                {
                    FileInfo fname = new FileInfo(path);
                    subFolder = "\\" + fname.Name;
                }
                FileInfo f = new FileInfo(textBoxFolder2.Text + subFolder);
                Directory.CreateDirectory(f.Directory.FullName);
                res = textBoxFolder2.Text + subFolder;
            }
            if (!(radioButtonFormatDefault.Checked))
            {
                if (res.LastIndexOf('.') != -1)
                {
                    res = res.Remove(res.LastIndexOf('.'));
                }
                if (radioButtonFormatJpg.Checked)
                {
                    res += ".jpg";
                } else
                {
                    res += ".png";
                }
            }
            resCode = true;
            return res;
        }
        
        private Bitmap cropImage(Bitmap image)
        {
            int l = -1, r = -1, t = -1, b = -1;

            // l
            for (int i = 0; i < image.Width; i++)
            {
                for (int j = 0; j < image.Height; j++)
                {
                    Color clr = image.GetPixel(i, j);
                    if (!(clr.R >= 248 && clr.G >= 248 && clr.B >= 248))
                    {
                        l = i;
                        break;
                    }
                }
                if (l != -1) break;
            }

            // r
            for (int i = image.Width - 1; i >= 0; i--)
            {
                for (int j = 0; j < image.Height; j++)
                {
                    Color clr = image.GetPixel(i, j);
                    if (!(clr.R >= 248 && clr.G >= 248 && clr.B >= 248))
                    {
                        r = i;
                        break;
                    }
                }
                if (r != -1) break;
            }

            // t
            for (int i = image.Height - 1; i >= 0; i--)
            {
                for (int j = 0; j < image.Width; j++)
                {
                    Color clr = image.GetPixel(j, i);
                    if (!(clr.R >= 248 && clr.G >= 248 && clr.B >= 248))
                    {
                        t = i;
                        break;
                    }
                }
                if (t != -1) break;
            }

            // b
            for (int i = 0; i < image.Height; i++)
            {
                for (int j = 0; j < image.Width; j++)
                {
                    Color clr = image.GetPixel(j, i);
                    if (!(clr.R >= 248 && clr.G >= 248 && clr.B >= 248))
                    {
                        b = i;
                        break;
                    }
                }
                if (b != -1) break;
            }
            Rectangle cropRect = new Rectangle(l, b, r - l + 1, t - b + 1);
            Bitmap res = image.Clone(cropRect, image.PixelFormat);
            return res;
        }

        private void buttonBrowseFolder1_Click(object sender, EventArgs e)
        {
            if (dialogOpenSaveDest.ShowDialog() == DialogResult.OK)
            {
                textBoxFolder1.Text = dialogOpenSaveDest.SelectedPath;
                initImagesListFromFolder();
            }
        }
        private void buttonBrowseFolder2_Click(object sender, EventArgs e)
        {
            if (dialogOpenSaveDest.ShowDialog() == DialogResult.OK)
            {
                textBoxFolder2.Text = dialogOpenSaveDest.SelectedPath;
            }
        }
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            initImagesListFromFolder();
        }

        private void buttonLaunch_Click(object sender, EventArgs e)
        {
            if (files == null || (files != null && files.Length == 0))
            {
                showError("Не выбрано ни одного изображения");
                return;
            }
            if (!checkBox2.Checked && ! checkBoxBackup.Checked)
            {
                string caption = "Предупреждение";
                string message = "Обработанные изображения навсегда заменят старые, продолжить?";
                MessageBoxButtons buttons = MessageBoxButtons.YesNo;
                var iconType = MessageBoxIcon.Information;
                var res = MessageBox.Show(message, caption, buttons, iconType);
                if (res != DialogResult.OK && res != DialogResult.Yes)
                {
                    return;
                }
            }
            DisableControls(tabPage1);
            DisableControls(tabPage2);
            DisableControls(tabPage4);
            DisableControls(tabPage3);
            buttonLaunch.Visible = false;
            progressBar1.Visible = true;
            var t = Task.Run(
                        () =>
            {
                int target = files.Length * 2;
                progressBar1.BeginInvoke(new Action(() => { progressBar1.Value = 0; }));
                progressBar1.BeginInvoke(new Action(() => { progressBar1.Maximum = files.Length * 7; }));
                progressBar1.BeginInvoke(new Action(() => { progressBar1.Minimum = 0; }));
                int cnt = 0;
                int mxW = 0, mxH = 0;
                foreach (String file in files)
                {
                    try
                    {
                        FileStream fs = new FileStream(file, FileMode.Open);
                        Bitmap res = (Bitmap)Image.FromStream(fs);
                        res = cropImage(res);
                        mxH = Math.Max(mxH, res.Height);
                        fs.Close();
                        res.Dispose();
                    }
                    catch (Exception error)
                    {

                    }
                    progressBar1.BeginInvoke(new Action(() => { progressBar1.PerformStep(); }));
                }
                foreach (String file in files)
                {
                    try
                    {
                        FileStream fs = new FileStream(file, FileMode.Open);
                        Bitmap res = (Bitmap)Image.FromStream(fs);
                        res = cropImage(res);
                        double k = mxH / (double)res.Height;
                        mxW = (int)Math.Max(mxW, res.Width * k);
                        fs.Close();
                        res.Dispose();
                    }
                    catch (Exception error)
                    {

                    }
                    progressBar1.BeginInvoke(new Action(() => { progressBar1.PerformStep(); }));
                }
                foreach (String file in files)
                {
                    bool resCode = false;
                    try
                    {
                        target += 5;
                        cnt++;
                        if (checkBoxBackup.Checked)
                        {
                            String backupFile = getBackupPath(file, ref resCode);
                            if (!resCode)
                            {
                                buttonLaunch.BeginInvoke(new Action(() => { buttonLaunch.Visible = true; }));
                                progressBar1.BeginInvoke(new Action(() => { progressBar1.Visible = false; }));
                                showError(backupFile);
                                return;
                            }
                            File.Copy(file, backupFile, true);
                        }
                        FileStream fs = new FileStream(file, FileMode.Open);
                        Bitmap res = (Bitmap)Image.FromStream(fs);
                        res.SetResolution(1500, 1500);
                        bool f = true;
                        if (checkBoxChangeFields.Checked)
                        {
                            Bitmap croped = cropImage(res);
                            if (croped.Width == res.Width && croped.Height == res.Height)
                            {
                                f = false;
                            }
                        }
                        if (checkBoxCutFields.Checked && f) res = cropImage(res);
                        progressBar1.BeginInvoke(new Action(() => { progressBar1.PerformStep(); }));
                        if (checkBoxAlignHeight.Checked && f) res = alignHeight(res, mxW, mxH);
                        progressBar1.BeginInvoke(new Action(() => { progressBar1.PerformStep(); }));
                        if (checkBoxAlighResolution.Checked && f) res = toResolution(res, (int)numericUpDownHeight.Value, (int)numericUpDownWidth.Value);
                        progressBar1.BeginInvoke(new Action(() => { progressBar1.PerformStep(); }));
                        if (checkBox3.Checked) res = addWaterMark(res);
                        progressBar1.BeginInvoke(new Action(() => { progressBar1.PerformStep(); }));
                        Image tmp = superImpose(new Bitmap(res.Width, res.Height), res, 0, 0);
                        fs.Close();
                        String savePath = getSavePath(file, ref resCode);
                        if (!resCode)
                        {
                        buttonLaunch.BeginInvoke(new Action(() => { buttonLaunch.Visible = true; }));
                        progressBar1.BeginInvoke(new Action(() => { progressBar1.Visible = false; }));
                        this.Invoke(new Action(() => { showError(savePath); }));
                        this.Invoke(new Action(() => { EnableControls(tabPage1); }));
                        this.Invoke(new Action(() => { EnableControls(tabPage2); }));
                        this.Invoke(new Action(() => { EnableControls(tabPage3); }));
                        this.Invoke(new Action(() => { EnableControls(tabPage4); }));
                           
                            return;
                        }
                        if (savePath.EndsWith("jpg"))
                        {

                            tmp.Save(savePath, ImageFormat.Jpeg);
                        }
                        else
                        {
                            tmp.Save(savePath, ImageFormat.Png);
                        }
                        progressBar1.BeginInvoke(new Action(() => { progressBar1.PerformStep(); }));
                    }
                    catch (Exception error)
                    {
                        cnt--;
                    }
                    while (progressBar1.Value < target)
                    {
                        progressBar1.BeginInvoke(new Action(() => { progressBar1.PerformStep(); }));
                    }
                }
                buttonLaunch.BeginInvoke(new Action(() => { buttonLaunch.Visible = true; }));
                progressBar1.BeginInvoke(new Action(() => { progressBar1.Visible = false; }));
                this.Invoke(new Action(() => { EnableControls(tabPage1); }));
                this.Invoke(new Action(() => { EnableControls(tabPage2); }));
                this.Invoke(new Action(() => { EnableControls(tabPage3); }));
                this.Invoke(new Action(() => { EnableControls(tabPage4); }));
                string caption = "Редактирование фотографий завершено";
                string message = "Обработано фотографий: " + cnt.ToString();
                MessageBoxButtons buttons = MessageBoxButtons.OK;
                var iconType = MessageBoxIcon.Information;
                MessageBox.Show(message, caption, buttons, iconType);
            });
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked)
            {
                buttonBrowseFolder2.Enabled = true;
                buttonBrowseFolder2.Visible = true;
                textBoxFolder2.Visible = true;
                textBoxFolder2.Enabled = true;
                label2.Visible = true;
                label2.Enabled = true;
            }
            else
            {
                buttonBrowseFolder2.Enabled = false;
                buttonBrowseFolder2.Visible = false;
                textBoxFolder2.Visible = false;
                textBoxFolder2.Enabled = false;
                label2.Visible = false;
                label2.Enabled = false;
            }
        }

        public Bitmap addWaterMark(Bitmap largeImage)
        {
            if (fileWaterMark == null) return largeImage;
            double k = 0;
            trackBarSize.Invoke(new Action(() => { k = trackBarSize.Value / 100.0f; }));
            int targetW = (int)(largeImage.Width * k);
            int targetH = (int)(largeImage.Height * k);
            Bitmap waterMark = new Bitmap(fileWaterMark);
            waterMark.SetResolution(1000, 1000);
            waterMark = toResolution(waterMark, targetH, targetW, true);
            float o = 0;
            trackBarOpacity.Invoke(new Action(() => { o = trackBarOpacity.Value; }));
            waterMark = SetImageOpacity(waterMark, o / 100.0f);
            int x, y;
            if (TR.Checked || TM.Checked || TL.Checked)
            {
                y = 0;
            } else if (MR.Checked || MM.Checked || ML.Checked)
            {
                y = (largeImage.Height - waterMark.Height) / 2;
            } else
            {
                y = largeImage.Height - waterMark.Height;
            }
            if (TL.Checked || ML.Checked || BL.Checked)
            {
                x = 0;
            }
            else if (MM.Checked || TM.Checked || BM.Checked)
            {
                x = (largeImage.Width - waterMark.Width) / 2;
            }
            else
            {
                x = largeImage.Width - waterMark.Width;
            }
            return superImpose(largeImage, waterMark, x, y);
        }

        public Bitmap superImpose(Bitmap largeBmp, Bitmap smallBmp, int x, int y, bool transp = true)
        {
            largeBmp.SetResolution(3000, 3000);
            smallBmp.SetResolution(3000, 3000);
            Graphics g = Graphics.FromImage(largeBmp);
            g.CompositingMode = CompositingMode.SourceOver;
            //if (transp) smallBmp.MakeTransparent();
            g.DrawImage(smallBmp, new Point(x, y));
            return largeBmp;
        }

        public Bitmap alignHeight(Bitmap image, int mxW, int mxH)
        {
            double k = mxH / (double)image.Height;
            image = resizeImage(image, (int)(image.Width * k), (int)(image.Height * k));
            return toResolution(image, image.Height, (int)(mxW));
        }

        public Bitmap toResolution(Bitmap image, int height, int width, bool transp = false)
        {
            Bitmap res = new Bitmap(width, height);
            if (!transp)
            {
                for (int i = 0; i < width; i++)
                {
                    for (int j = 0; j < height; j++)
                    {
                        res.SetPixel(i, j, Color.White);
                    }
                }
            }
            double k;
            if (image.Height < height)
            {
                k = ((double)height / image.Height);
                image = resizeImage(image, (int)(image.Width * k), (int)(image.Height * k));
            }
            if (image.Width < width)
            {
                k = (double)width / image.Width;
                image = resizeImage(image, (int)(image.Width * k), (int)(image.Height * k));
            }
            if (image.Height > height)
            {
                k = (double)height / image.Height;
                image = resizeImage(image, (int)(image.Width * k), (int)(image.Height * k));
            }
            if (image.Width > width)
            {
                k = (double)width / image.Width;
                image = resizeImage(image, (int)(image.Width * k), (int)(image.Height * k));
            }
            int x = (width - image.Width) / 2;
            int y = (height - image.Height) / 2;
            return superImpose(res, image, x, y, false);
        }

        public static Bitmap resizeImage(Image image, int width, int height)
        {
            var destRect = new Rectangle(0, 0, width, height);
            var destImage = new Bitmap(width, height);

            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceOver;

                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }
            return destImage;
        }

        public Bitmap SetImageOpacity(Bitmap image, float opacity)
        {
            //create a Bitmap the size of the image provided  
            Bitmap bmp = new Bitmap(image.Width, image.Height);

            //create a graphics object from the image  
            using (Graphics gfx = Graphics.FromImage(bmp))
            {

                //create a color matrix object  
                ColorMatrix matrix = new ColorMatrix();

                //set the opacity  
                matrix.Matrix33 = opacity;

                //create image attributes  
                ImageAttributes attributes = new ImageAttributes();

                //set the color(opacity) of the image  
                attributes.SetColorMatrix(matrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);

                //now draw the image  
                gfx.DrawImage(image, new Rectangle(0, 0, bmp.Width, bmp.Height), 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, attributes);
            }
            return bmp;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            formStart.Close();
        }

        private void checkBoxAlighResolution_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxAlighResolution.Checked)
            {
                panelNumbers.Visible = true;
            } else
            {
                panelNumbers.Visible = false;
            }
        }

        private void radioButtonChoodeF_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonChooseF.Checked)
            {
                panelChooseFolder.Visible = true;
                panelChooseImages.Visible = false;
            } else
            {
                panelChooseFolder.Visible = false;
                panelChooseImages.Visible = true;
            }
            textBoxFolder1.Text = "";
            labelImageCount.Text = "Выбрано изображений: 0";
            files = new String[] { };
        }

        private void buttonMultiChoose_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Multiselect = true;
            dialog.Filter = "Images (*.BMP;*.JPG;*.GIF,*.PNG)|*.BMP;*.JPG;*.GIF;*.PNG";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                files = dialog.FileNames;
                labelImageCount.Text = "Выбрано изображений: " + files.Length;
            }
        }

        private void updatePreview()
        {
            if (fileWaterMark != null)
            {
                Bitmap waterMark = new Bitmap(fileWaterMark);
                //waterMark = resizeImage(waterMark, waterMark.Width * 15, waterMark.Height * 15);
                waterMark = toResolution(waterMark, pictureBoxWaterMa.Height, pictureBoxWaterMa.Width, false);
                waterMarkPreviewImage = waterMark;
                pictureBoxWaterMa.Image = SetImageOpacity(waterMarkPreviewImage, Int32.Parse(textBoxOpacity.Text) / 100.0f);
            }
        }

        private void buttonChooseWater_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "Images (*.BMP;*.JPG;*.GIF,*.PNG)|*.BMP;*.JPG;*.GIF;*.PNG";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                fileWaterMark = dialog.FileName;
                updatePreview();
            }
        }

        private void trackBarOpacity_Scroll(object sender, EventArgs e)
        {
            textBoxOpacity.Text = trackBarOpacity.Value.ToString();
            if (waterMarkPreviewImage != null)
                pictureBoxWaterMa.Image = SetImageOpacity(waterMarkPreviewImage, Int32.Parse(textBoxOpacity.Text) / 100.0f);
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            textBoxSize.Text = trackBarSize.Value.ToString();
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox3.Checked)
            {
                panelWaterMark.Visible = true;
            } else
            {
                panelWaterMark.Visible = false;
            }
        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxBackup.Checked)
            {
                panelBackup.Visible = true;
            } else
            {
                panelBackup.Visible = false;
            }
        }

        private void buttonBackupDialog_Click(object sender, EventArgs e)
        {
            if (dialogOpenSaveDest.ShowDialog() == DialogResult.OK)
            {
                textBoxBackup.Text = dialogOpenSaveDest.SelectedPath;
            }
        }
    }
}
