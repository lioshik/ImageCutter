using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Management;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ImageCutter
{
    public partial class FormStart : Form
    {
        public Form1 form;
        public FormStart()
        {
            this.MaximizeBox = false;
            form = new Form1(this) { Visible = false };
            InitializeComponent();
            checkComputer();
            labelShowInfo.Text = "";
        }

        private async void checkComputer()
        {
            String req = "https://boiling-forest-14211.herokuapp.com/checkcomputer?name=" + getComputerName();
            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync(req);
            String result = contentToString(response.Content);
            if (result == "ok")
            {
                form.Visible = true;
                this.Visible = false;
            }
        }

        String getComputerName()
        {
            String v1 = "", v2 = "";
            ManagementObjectSearcher MOS = new ManagementObjectSearcher("Select * From Win32_BaseBoard");
            foreach (ManagementObject getserial in MOS.Get())
            {
                v1 = getserial["SerialNumber"].ToString();
            }

            MOS = new ManagementObjectSearcher("Select * From Win32_processor");
            foreach (ManagementObject getPID in MOS.Get())
            {
                v2 = getPID["ProcessorID"].ToString();
            }
            return v1 + v2;
        }

        private async void buttonActivate_Click_1(object sender, EventArgs e)
        {
            labelShowInfo.Text = "Проверка может занять около 30 секнуд";
            Cursor.Current = Cursors.WaitCursor;
            String key = textBoxKeyInput.Text;
            String computer = getComputerName();
            String req = "https://boiling-forest-14211.herokuapp.com/activate?key=" + key + "&computer=" + computer;
            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync(req);
            String result = contentToString(response.Content);
            if (result == "ok")
            {
                form.Visible = true;
                this.Visible = false;
            } else
            {
                labelShowInfo.Text = "";
                MessageBox.Show("Проверьте правильность ключа", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private string contentToString(HttpContent httpContent)
        {
            var readAsStringAsync = httpContent.ReadAsStringAsync();
            return readAsStringAsync.Result;
        }
    }
}
