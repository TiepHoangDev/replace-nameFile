using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Rename_Javascript_download
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            lvFolder.View = View.List;
            CheckForIllegalCrossThreadCalls = false;
        }

        private void txtForder_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                txtForder.Text = folderBrowserDialog1.SelectedPath;
                showFile(txtForder.Text);
            }
        }

        private void showFile(string path)
        {
            lvFolder.Clear();
            var allFile = Directory.GetFiles(path);
            if (allFile.Length == 0)
            {
                MessageBox.Show("Folder ko có file");
                return;
            }
            foreach (var item in allFile)
            {
                if (item.Contains(txtOldName.Text))
                {
                    lvFolder.Items.Add(item);
                }
            }
        }

        private void btnRun_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtOldName.Text))
            {
                MessageBox.Show("Nhập kí tự cũ cần thay");
                return;
            }
            if (MessageBox.Show("Sure? đổi tên?", "Sure", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning) != System.Windows.Forms.DialogResult.Yes) return;
            progressBar1.Value = 0;
            progressBar1.Maximum = lvFolder.Items.Count - 1;
            progressBar1.Step = 1;
            backgroundWorker1.RunWorkerAsync();
        }

        void showRun(int value)
        {
            lblRun.Text = "oke " + value + "/" + lvFolder.Items.Count;
        }

        private void btnReload_Click(object sender, EventArgs e)
        {
            showFile(txtForder.Text);
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                for (int i = 0; i < lvFolder.Items.Count; i++)
            {
                var item = lvFolder.Items[i];
                try
                {
                    File.Move(item.Text, item.Text.Replace(txtOldName.Text, txtNewName.Text));
                }
                catch (IOException)
                {
                    File.Delete(item.Text);
                }
                backgroundWorker1.ReportProgress(i);
                showRun(i + 1);
            }
            showFile(txtForder.Text);
            }
            catch (Exception exx)
            {
                MessageBox.Show(exx.Message);
            }
        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBar1.Value = e.ProgressPercentage;
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            MessageBox.Show("Done");
        }
    }
}
