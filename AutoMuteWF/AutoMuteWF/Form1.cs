using System;
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
            keyboardHook.EnableHook();
            muteStatus.Text = "hooked";
        }

        private void muteHook(bool muted)
        {
            if (muteStatus.InvokeRequired)
            {
                muteStatus.Invoke(new Action(() =>
                            muteStatus.Text = muted ? "MUTED" : "unmuted"
                ));
            }
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            Console.WriteLine("closing");
            keyboardHook.DisableHook();
        }

        private void checkBoxToggled(object sender, EventArgs e)
        {
            keyboardHook.Enabled = ToggleBox.Checked;

            if (!keyboardHook.Enabled)
            {
                MuteThing.Mute = false;
            }
        }
    }
}
