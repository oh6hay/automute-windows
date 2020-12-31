using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AutoMuteWF
{
    public partial class Form1 : Form
    {
        private KeyboardHook keyboardHook;
        

        public Form1()
        {
            InitializeComponent();
            keyboardHook = new KeyboardHook(muteHook);
            keyboardHook.enableHook();
            this.muteStatus.Text = "hooked";
        }

        private void muteHook(bool muted)
        {
            this.muteStatus.Text = muted ? "MUTED" : "unmuted";
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            Console.WriteLine("closing");
            keyboardHook.disableHook();
        }
    }
}
