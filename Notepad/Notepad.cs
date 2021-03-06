﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Notepad
{
    public partial class Notepad : Form
    {
        private string fileName = null;
        private bool isUnsaved = false;
        private bool ignoreTextChangeEvent = false;

        public Notepad()
        {
            InitializeComponent();
            UpdateTitle();
        }

        private void UpdateTitle()
        {
            string file;
            if (string.IsNullOrEmpty(fileName))
            {
                file = "Unnamed";
            }
            else
            {
                file = Path.GetFileName(fileName);
            }

            if (isUnsaved)
            {
                Text = file + "* - Notepad";
            }
            else
            {
                Text = file + " - Notepad";
            }
            
        }

        private void SaveFile()
        {
            if (string.IsNullOrEmpty(fileName))
            {
                if (saveFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    fileName = saveFileDialog.FileName;
                }
                else
                {
                    return;
                }
            }

            File.WriteAllText(fileName, textBox.Text);
            isUnsaved = false;
            UpdateTitle();
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var eventArgs = new FormClosingEventArgs(CloseReason.None, false);
            Notepad_FormClosing(null, eventArgs);

            if (eventArgs.Cancel)
            {
                return;
            }

            textBox.Text = string.Empty;
            fileName = null;
            isUnsaved = false;
            UpdateTitle();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var eventArgs = new FormClosingEventArgs(CloseReason.None, false);
            Notepad_FormClosing(null, eventArgs);

            if (eventArgs.Cancel)
            {
                return;
            }

            if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                ignoreTextChangeEvent = true;
                textBox.Text = File.ReadAllText(openFileDialog.FileName);
                fileName = openFileDialog.FileName;
                isUnsaved = false;
                UpdateTitle();
            }
            
        }

        private void quitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFile();
        }

        private void textBox_TextChanged(object sender, EventArgs e)
        {
            if (ignoreTextChangeEvent)
            {
                ignoreTextChangeEvent = false;
                return;
            }

            isUnsaved = true;
            UpdateTitle();
        }

        private void Notepad_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (isUnsaved)
            {
                var res = MessageBox.Show(this, "Would you like to save changes?", "Notepad", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Information);

                if (res == System.Windows.Forms.DialogResult.Yes)
                {
                    SaveFile();
                }
                else if (res == System.Windows.Forms.DialogResult.No)
                {
                    // Do Nothing
                }
                else if (res == System.Windows.Forms.DialogResult.Cancel)
                {
                    e.Cancel = true;
                }
            }
        }
    }
}
