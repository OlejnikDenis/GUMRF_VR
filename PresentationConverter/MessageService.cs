using System.Windows.Forms;

namespace PresentationConverter
{
    internal class MessageService
    {
        private ToolStripLabel _toolStripLabel;

        public MessageService(ToolStripLabel toolStripLabel)
        {
            _toolStripLabel = toolStripLabel;
        }

        public void SentMessage(string message)
        {
            _toolStripLabel.Text = message;
        }

    }
}
