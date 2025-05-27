using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace lab1
{
    internal class Correction
    {
        private readonly RichTextBox _richTextBox;
        private readonly Stack<string> _undoStack = new();
        private readonly Stack<string> _redoStack = new();

        public Correction(RichTextBox richTextBox)
        {
            _richTextBox = richTextBox;
            _richTextBox.TextChanged += (s, e) => TrackChanges();
        }

        private void TrackChanges()
        {
            if (_undoStack.Count == 0 || _undoStack.Peek() != _richTextBox.Text)
            {
                _undoStack.Push(_richTextBox.Text);
                _redoStack.Clear();
            }
        }

        public void Undo()
        {
            if (_undoStack.Count > 1)
            {
                _redoStack.Push(_undoStack.Pop());
                _richTextBox.Text = _undoStack.Peek();
                _richTextBox.SelectionStart = _richTextBox.Text.Length;
            }
        }

        public void Redo()
        {
            if (_redoStack.Count > 0)
            {
                string redoText = _redoStack.Pop();
                _undoStack.Push(redoText);
                _richTextBox.Text = redoText;
                _richTextBox.SelectionStart = _richTextBox.Text.Length;
            }
        }

        public void Cut()
        {
            _richTextBox.Cut();
        }

        public void Copy()
        {
            _richTextBox.Copy();
        }

        public void Paste()
        {
            _richTextBox.Paste();
        }

        public void Delete()
        {
            _richTextBox.SelectedText = "";
        }

        public void SelectAll()
        {
            _richTextBox.SelectAll();
        }
    }
}