using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace AutoMuteWF
{
    class KeyboardHook
    {
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr SetWindowsHookEx(int idHook,
        LowLevelKeyboardProc lpfn, IntPtr hMod, uint dwThreadId);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool UnhookWindowsHookEx(IntPtr hhk);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode,
            IntPtr wParam, IntPtr lParam);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr GetModuleHandle(string lpModuleName);

        private const int WH_KEYBOARD_LL = 13;
        private const int WM_KEYDOWN = 0x0100;
        private static LowLevelKeyboardProc _proc = HookCallback;
        private static IntPtr _hookID = IntPtr.Zero;

        public bool started { get; private set; }

        public delegate void MuteHook(bool muted);
        public MuteHook muteHook { get; set; }
        private Timer keyCheckTimer;
        private static long lastKeypressAt;

        public KeyboardHook(MuteHook muteHook)
        {
            started = false;
            this.muteHook = muteHook;
        }

        public void enableHook()
        {
            _hookID = SetHook(_proc);
            started = true;
            keyCheckTimer = new Timer(.05);
            keyCheckTimer.Elapsed += KeyCheckTimer_Elapsed;
            keyCheckTimer.Start();
        }

        private void KeyCheckTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (DateTime.Now.Ticks - lastKeypressAt > 500000)
            {
                if (MuteThing.Mute == true && DateTime.Now.Ticks - lastKeypressAt > 2500000)
                {
                    MuteThing.Mute = false;
                    muteHook(false);
                }
            } else
            {
                if (MuteThing.Mute == false)
                {
                    MuteThing.Mute = true;
                    muteHook(true);
                }
            }
        }

        public void disableHook()
        {
            keyCheckTimer.Stop();
            UnhookWindowsHookEx(_hookID);
            started = false;
        }

        private static IntPtr SetHook(LowLevelKeyboardProc proc)
        {
            using (Process curProcess = Process.GetCurrentProcess())
            using (ProcessModule curModule = curProcess.MainModule)
            {
                return SetWindowsHookEx(WH_KEYBOARD_LL, proc,
                    GetModuleHandle(curModule.ModuleName), 0);
            }
        }

        private delegate IntPtr LowLevelKeyboardProc(
            int nCode, IntPtr wParam, IntPtr lParam);

        private static IntPtr HookCallback(
            int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode >= 0 && wParam == (IntPtr)WM_KEYDOWN)
            {
                lastKeypressAt = DateTime.Now.Ticks;
            }
            return CallNextHookEx(_hookID, nCode, wParam, lParam);
        }
    }
}
