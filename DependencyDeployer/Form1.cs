using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DependencyDeployer
{

    public partial class Form1 : Form
    {
        private string myGID;
        private string myAID;
        private string myV;
        private string myD;



        public Form1()
        {
            InitializeComponent();
            myGID = "";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
        }

        private void Form1_Load(object sender, EventArgs e)
        {


            string[] lineOfContents = File.ReadAllLines("repositories.txt");
            foreach (var line in lineOfContents)
            {
                string[] tokens = line.Split(',');
                ComboboxItem mitem = new ComboboxItem
                {
                    Text = tokens[0],
                    Value = tokens[1]
                };
                comboBox1.Items.Add(mitem);
            }
            string[] lineOfContents2 = File.ReadAllLines("groups.txt");
            foreach (var line in lineOfContents2)
            {
                string[] tokens = line.Split(',');
                ComboboxItem mitem = new ComboboxItem
                {
                    Text = tokens[0],
                    Value = tokens[1]
                };
                comboBox2.Items.Add(mitem);
            }
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            label5.Text = openFileDialog1.FileName;
            string fileName = openFileDialog1.SafeFileName;
            string removed = fileName.Remove(fileName.Length - 4, 4);
            string[] authorsList = removed.Split("-");
            if (authorsList.Length == 2)
            {
                textBox2.Text = authorsList[0];
                textBox3.Text = authorsList[1];
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (
                openFileDialog1.FileName.Length > 0
                && textBox2.Text.Length > 0
                && textBox3.Text.Length > 0
                && comboBox1.SelectedIndex > -1
                && comboBox2.SelectedIndex > -1
               )
            {
                string GID = "";
                //calculating groupID
                GID = (comboBox1.SelectedItem as ComboboxItem).Value.ToString() + (comboBox2.SelectedItem as ComboboxItem).Value.ToString();

                myGID = GID;
                myAID = textBox2.Text;
                myV = textBox3.Text;
                string strCmdText = "/K mvn deploy:deploy-file -DgroupId=";
                strCmdText += GID;
                strCmdText += " -DartifactId=";
                strCmdText += textBox2.Text;
                strCmdText += " -Dversion=";
                strCmdText += textBox3.Text;
                strCmdText += " -Dpackaging=jar  -Dfile=\"";
                strCmdText += openFileDialog1.FileName;
                strCmdText += "\" -DrepositoryId=";
                strCmdText += comboBox1.SelectedItem.ToString();
                string[] url = File.ReadAllLines("url.txt");
                strCmdText += " -Durl=\"" + url[0] + "";
                strCmdText += comboBox1.SelectedItem.ToString();
                strCmdText += "\" -DgeneratePom=true";
                richTextBox1.Text = strCmdText;
                if (MessageBox.Show("Are you sure", "hint", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    System.Diagnostics.Process.Start("CMD.exe", strCmdText);
                    linkLabel1.Visible = true;
                    myD = comboBox1.SelectedItem.ToString();
                    /*comboBox1.SelectedIndex = -1;
                    comboBox2.SelectedIndex = -1;*/
                    textBox2.Text = "";
                    textBox3.Text = "";
                }
                else
                { }


                /*System.Diagnostics.Process process = new System.Diagnostics.Process();
                System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
                startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                startInfo.FileName = "cmd.exe";
                startInfo.Arguments = strCmdText;
                process.StartInfo = startInfo;
                process.Start();*/
            }
            else
            {
                MessageBox.Show("یک چیزی مشکل داره تمامی موارد ورودی را مجدد چک کنید و دوباره امتحان کنید.");
            }

        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("explorer", "http://192.168.100.243:8082/ui/repos/tree/General/"
                + myD
                + "/" + myGID.Replace('.', '/') + "/" + myAID + "/" + myV + "/");
        }
    }
}
