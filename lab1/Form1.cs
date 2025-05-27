using System;
using System.Linq;
using System.Windows.Forms;

namespace lab1
{
    public partial class Compiler : Form
    {
        private readonly FileWorks _fileWorks;
        private readonly Correction _correction;
        private readonly Reference _reference;

        private const string _aboutPath = "lab1.Resources.About.html";
        private const string _helpPath = "lab1.Resources.Help.html";

        public Compiler()
        {
            InitializeComponent();
            _fileWorks = new FileWorks(this);
            _correction = new Correction(richTextBox1);
            _reference = new Reference(_helpPath, _aboutPath);
            richTextBox1.TextChanged += richTextBox1_TextChanged;
            this.FormClosing += Compiler_FormClosing;
            this.MinimumSize = new Size(600, 400);
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
            => _fileWorks.MarkAsModified();

        public RichTextBox Editor => richTextBox1;

        private void richTextBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            _fileWorks.CreateNewFile();
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            _fileWorks.OpenFile();
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            _fileWorks.SaveFile();
        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            _correction.Undo();
        }

        private void toolStripButton5_Click(object sender, EventArgs e)
        {
            _correction.Redo();
        }

        private void toolStripButton6_Click(object sender, EventArgs e)
        {
            _correction.Copy();
        }

        private void toolStripButton7_Click(object sender, EventArgs e)
        {
            _correction.Cut();
        }

        private void toolStripButton8_Click(object sender, EventArgs e)
        {
            _correction.Paste();
        }

        private void RunParser()
        {
            var input = richTextBox1.Text;
            List<Token> poliz;
            Parser parser;

            richTextBox2.Clear();

            try
            {
                parser = new Parser(input);
                poliz = parser.Parse();
            }
            catch (ParserException ex)
            {
                if (ex.Message.StartsWith("������������ ������"))
                {
                    // ����������� ������
                    richTextBox2.AppendText($"����������� ������: {ex.Message}");
                }
                else
                {
                    // �������������� ������
                    richTextBox2.AppendText($"�������������� ������:\n{ex.Message}");
                }
                return;
            }

            // 2) ����� �����
            richTextBox2.AppendText("�����:\n");
            richTextBox2.AppendText(string.Join(" ", poliz.Select(t => t.Lexeme)));

            // 3) ���������� ����� � ��������� ������� �� ����
            try
            {
                var result = parser.EvaluatePoliz(poliz);
                richTextBox2.AppendText($"\n\n���������:\n{result}");
            }
            catch (ParserException ex)
            {
                richTextBox2.AppendText($"\n\n������: {ex.Message}"); // ������� �� ����
            }
        }

        private void toolStripButton9_Click(object sender, EventArgs e) => RunParser();

        private void Compiler_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (_fileWorks.CheckUnsavedChanges())
                e.Cancel = true;
        }



        private void toolStripButton10_Click(object sender, EventArgs e)
        {
            _reference.ShowHelp();
        }

        private void toolStripButton11_Click(object sender, EventArgs e)
        {
            _reference.ShowAbout();
        }

        private void �������ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _fileWorks.CreateNewFile();
        }

        private void �������ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _fileWorks.OpenFile();
        }

        private void ���������ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _fileWorks.SaveFile();
        }

        private void ������������ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _fileWorks.SaveAsFile();
        }

        private void �����ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _fileWorks.Exit();
        }

        private void ��������ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _correction.Undo();
        }

        private void ���������ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _correction.Redo();
        }

        private void ��������ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _correction.Cut();
        }

        private void ����������ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _correction.Copy();
        }

        private void ��������ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _correction.Paste();
        }

        private void �������ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _correction.Delete();
        }

        private void �����������ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _correction.SelectAll();
        }

        private void ������������ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _reference.ShowHelp();
        }

        private void ����������ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _reference.ShowAbout();
        }

        public void ChangeFontSize(float newSize)
        {
            if (newSize < 8 || newSize > 72) return;

            Editor.Font = new Font(Editor.Font.FontFamily, newSize, Editor.Font.Style);
        }

        private void toolStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void ����ToolStripMenuItem_Click(object sender, EventArgs e) => RunParser();

        private void ����������������ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _reference.ShowTask();
        }

        private void ����������ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _reference.ShowGrammary();
        }

        private void �����������������������ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _reference.ShowClassGrammary();
        }

        private void ������������ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _reference.ShowMAnalysis();
        }

        private void �������������������������������ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _reference.ShowDiagnosticErr();
        }

        private void ��������������ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _reference.ShowTest();
        }

        private void ����������������ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _reference.ShowListLibrary();
        }

        private void ��������������������ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _reference.ShowCode();
        }
    }
}