using System.Threading;
using System.Windows;
using System.Windows.Forms;

namespace NicoTrola
{
    /// <summary>
    /// Lógica de interacción para Messages.xaml
    /// </summary>
    public partial class Messages : Window
    {
        public Messages(string message)
        {
            InitializeComponent();
            Width= Screen.PrimaryScreen.Bounds.Width;
            messageTB.Text = message;
        }

        public void Sleep()
        {
            Thread.Sleep(2000);
            Close();
        }
        public void Sleep(int mili)
        {
            Thread.Sleep(mili);
            Close();
        }
    }
}
