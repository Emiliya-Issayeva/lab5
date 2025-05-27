using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Windows.Forms;
using System.Reflection;

namespace lab1
{
    internal class Reference
    {
        private readonly string _helpPath;
        private readonly string _aboutPath;

        public Reference(string helpPath, string aboutPath)
        {
            _helpPath = helpPath;
            _aboutPath = aboutPath;
        }

        public void ShowHelp() => ShowHtmlFile(_helpPath, "Справка");
        public void ShowAbout() => ShowHtmlFile(_aboutPath, "О программе");

        // Методы для новых пунктов меню
        public void ShowTask() => ShowHtmlFile("lab1.Resources.Task.html", "Постановка задачи");
        public void ShowGrammary() => ShowHtmlFile("lab1.Resources.Grammary.html", "Грамматика");
        public void ShowClassGrammary() => ShowHtmlFile("lab1.Resources.CGrammary.html", "Классификация грамматики");
        public void ShowMAnalysis() => ShowHtmlFile("lab1.Resources.MAnalysis.html", "Метод анализа");
        public void ShowDiagnosticErr() => ShowHtmlFile("lab1.Resources.DiagnosticErr.html", "Диагностика и нейтрализация ошибок");
        public void ShowTest() => ShowHtmlFile("lab1.Resources.Test.html", "Тестовый пример");
        public void ShowListLibrary() => ShowHtmlFile("lab1.Resources.ListLibrary.html", "Список литературы");
        public void ShowCode() => ShowHtmlFile("lab1.Resources.Code.html", "Исходный код программы");

        private void ShowHtmlFile(string resourcePath, string title)
        {
            try
            {
                using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourcePath))
                {
                    if (stream == null)
                    {
                        MessageBox.Show($"Не найден ресурс: {resourcePath}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    var tempPath = Path.Combine(Path.GetTempPath(), Path.GetFileName(resourcePath));
                    using (var fileStream = File.Create(tempPath))
                    {
                        stream.CopyTo(fileStream);
                    }

                    var form = new Form
                    {
                        Text = title,
                        Size = new Size(800, 600),
                        StartPosition = FormStartPosition.CenterScreen
                    };

                    var webBrowser = new WebBrowser
                    {
                        Dock = DockStyle.Fill,
                        Url = new Uri(tempPath)
                    };

                    form.Controls.Add(webBrowser);
                    form.Icon = SystemIcons.Application;
                    form.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка отображения файла: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}