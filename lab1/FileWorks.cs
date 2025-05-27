using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;

namespace lab1
{
    internal class FileWorks
    {
        private readonly Compiler _mainForm;
        private string? _currentFilePath;
        private bool _isModified = false;

        public FileWorks(Compiler mainForm)
        {
            _mainForm = mainForm;
        }

        private void UpdateWindowTitle()
        {
            string fileName = string.IsNullOrEmpty(_currentFilePath) ? "Новый файл" : Path.GetFileName(_currentFilePath);
            string modifiedMark = _isModified ? "*" : "";
            _mainForm.Text = $"Текстовый редактор - {fileName}{modifiedMark}";
        }

        public void CreateNewFile()
        {
            if (CheckUnsavedChanges()) return;

            _mainForm.Editor.Clear();
            _currentFilePath = null;
            _isModified = false;
            UpdateWindowTitle();
        }

        public void OpenFile()
        {
            if (CheckUnsavedChanges()) return;

            using OpenFileDialog openDialog = new OpenFileDialog { Filter = "Текстовые файлы (*.txt)|*.txt|Все файлы (*.*)|*.*" };
            if (openDialog.ShowDialog() == DialogResult.OK)
            {
                _currentFilePath = openDialog.FileName;
                _mainForm.Editor.Text = File.ReadAllText(_currentFilePath);
                _isModified = false;
                UpdateWindowTitle();
            }
        }

        public void SaveFile()
        {
            if (string.IsNullOrEmpty(_currentFilePath))
            {
                SaveAsFile();
            }
            else
            {
                File.WriteAllText(_currentFilePath, _mainForm.Editor.Text);
                _isModified = false;
                UpdateWindowTitle();
            }
        }

        public void SaveAsFile()
        {
            using SaveFileDialog saveDialog = new SaveFileDialog { Filter = "Текстовые файлы (*.txt)|*.txt|Все файлы (*.*)|*.*" };
            if (saveDialog.ShowDialog() == DialogResult.OK)
            {
                _currentFilePath = saveDialog.FileName;
                File.WriteAllText(_currentFilePath, _mainForm.Editor.Text);
                _isModified = false;
                UpdateWindowTitle();
            }
        }

        public void Exit()
        {
            _mainForm.Close();
        }

        public bool CheckUnsavedChanges()
        {
            if (!_isModified) return false;

            DialogResult result = MessageBox.Show("Сохранить изменения перед выходом?", "Несохраненные изменения", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);

            if (result == DialogResult.Yes)
            {
                SaveFile();
                return false;
            }

            return result == DialogResult.Cancel;
        }

        public void MarkAsModified()
        {
            _isModified = true;
            UpdateWindowTitle();
        }
    }
}
