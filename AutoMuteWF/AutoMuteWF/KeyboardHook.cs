using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
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
        private const int WM_KEYUP = 0x0101;

        private static LowLevelKeyboardProc _proc = HookCallback;
        private static IntPtr _hookId = IntPtr.Zero;

        public bool started { get; private set; }

        public bool Enabled = true;

        public delegate void MuteHook(bool muted);
        public MuteHook muteHook { get; set; }

        private Timer _keyCheckTimer;
        private static long _lastKeypressAt;

        public KeyboardHook(MuteHook muteHook)
        {
            started = false;
            this.muteHook = muteHook;
        }

        public void EnableHook()
        {
            _hookId = SetHook(_proc);
            started = true;
            _keyCheckTimer = new Timer(.05);
            _keyCheckTimer.Elapsed += KeyCheckTimer_Elapsed;
            _keyCheckTimer.Start();
        }

        private void KeyCheckTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (DateTime.Now.Ticks - _lastKeypressAt > 500000)
            {
                if (MuteThing.Mute && DateTime.Now.Ticks - _lastKeypressAt > 2500000)
                {
                    MuteThing.Mute = false;
                    muteHook(false);
                }
            } else if (!MuteThing.Mute && Enabled)
            {
                MuteThing.Mute = true;
                muteHook(true);
            }
        }

        public void DisableHook()
        {
            _keyCheckTimer.Stop();
            UnhookWindowsHookEx(_hookId);
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
            if (nCode >= 0 && (wParam == (IntPtr)WM_KEYDOWN || wParam == (IntPtr)WM_KEYUP))
            {
                _lastKeypressAt = DateTime.Now.Ticks;
            }
            return CallNextHookEx(_hookId, nCode, wParam, lParam);
        }
    }
}
