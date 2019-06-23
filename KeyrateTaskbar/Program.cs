using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace KeyRateTaskbar
{
    public class SysTrayApp : Form
    {
        [DllImport("KEYRATECPP.dll", CallingConvention = CallingConvention.Cdecl)] static extern void SetKeyRate(int repeatrate_ms, int delay_ms);
        //[DllImport("KEYRATECPP.dll")] static extern int GetRepeatRateMs();
        //[DllImport("KEYRATECPP.dll")] static extern int GetDelayMs();

        // ==========================

        int DefaultRepeatRateMs = 15;
        int DefaultDelayMs = 175;

        int CurrentRepeatRateMs = -1;
        int CurrentDelayMs = -1;

        private void MenuItemNew_Click(Object sender, System.EventArgs e)
        {
            MenuItem mi = sender as MenuItem;
            if ( mi.Tag is SetDelayMs )
            {
                CurrentDelayMs = ((SetDelayMs)mi.Tag).DelayMs;
            }
            if (mi.Tag is SetRepeatMs)
            {
                CurrentRepeatRateMs = ((SetRepeatMs)mi.Tag).RepeatRateMs;
            }

            Apply();
        }

        private void DisableKeyrateOverride(Object sender, System.EventArgs e)
        {
            CurrentRepeatRateMs = -1;
            CurrentDelayMs = -1;
            Apply();
        }

        void Apply()
        {
            if ( CurrentRepeatRateMs > 0 && CurrentDelayMs < 0 )
            {
                CurrentDelayMs = DefaultDelayMs;
            }
            if ( CurrentDelayMs > 0 && CurrentRepeatRateMs < 0 )
            {
                CurrentRepeatRateMs = DefaultRepeatRateMs;
            }

            foreach (MenuItem m in trayMenu.MenuItems)
            {
                if (m.Tag is SetDelayMs)
                {
                    m.Checked = ((SetDelayMs)m.Tag).DelayMs == CurrentDelayMs;
                }
                if (m.Tag is SetRepeatMs)
                {
                    m.Checked = ((SetRepeatMs)m.Tag).RepeatRateMs == CurrentRepeatRateMs;
                }
            }

            SetKeyRate(CurrentRepeatRateMs, CurrentDelayMs);
        }

        [STAThread]
        public static void Main()
        {
            Application.Run(new SysTrayApp());
        }

        private NotifyIcon trayIcon;
        private ContextMenu trayMenu;

        struct SetRepeatMs
        {
            public int RepeatRateMs;
        }
        struct SetDelayMs
        {
            public int DelayMs;
        }

        public SysTrayApp()
        {
            // Create a simple tray menu with only one item.
            trayMenu = new ContextMenu();

            {
                MenuItem mi = trayMenu.MenuItems.Add("RepeatRate");
                mi.Enabled = false;
            }

            //int[] rate_ms = new int[]{ 1, 5, 10, 15, 20, 30, 40, 50, 100 }; //note: for testing
            int[] rate_ms = new int[] { 15, 30, 45, 60, 75 }; //note: after testing, increments in 15ms

            for ( int i=0,n=rate_ms.Length; i<n; ++i )
            {
                MenuItem mi = trayMenu.MenuItems.Add( rate_ms[i].ToString() + "ms (" + Math.Floor(1000.0f/(float)rate_ms[i]).ToString() + "Hz)", MenuItemNew_Click);
                mi.Tag = new SetRepeatMs { RepeatRateMs = rate_ms[i] };
            }

            trayMenu.MenuItems.Add("-");
            {
                MenuItem mi = trayMenu.MenuItems.Add("Delay");
                mi.Enabled = false;
            }

            int[] delay_ms = new int[]{ 50, 100, 125, 150, 175, 200, 250, 300, 400, 500 };

            for (int i = 0, n = delay_ms.Length; i < n; ++i)
            {
                MenuItem mi = trayMenu.MenuItems.Add(delay_ms[i].ToString() + "ms", MenuItemNew_Click);
                mi.Tag = new SetDelayMs { DelayMs = delay_ms[i] };
            }

            trayMenu.MenuItems.Add("-");
            trayMenu.MenuItems.Add("Disable", DisableKeyrateOverride);
            trayMenu.MenuItems.Add("Exit", OnExit);

            //TODO: query for previous at startup
            //TODO: presets

            trayIcon = new NotifyIcon();
            trayIcon.Text = "KeyRate Control";
            trayIcon.Icon = new Icon(SystemIcons.Information, 40, 40);

            // Add menu to tray icon and show it.
            trayIcon.ContextMenu = trayMenu;
            trayIcon.Visible = true;
        }

        protected override void OnLoad(EventArgs e)
        {
            Visible = false; // Hide form window.
            ShowInTaskbar = false; // Remove from taskbar.

            base.OnLoad(e);
        }

        private void OnExit(object sender, EventArgs e)
        {
            Application.Exit();
        }

        protected override void Dispose(bool isDisposing)
        {
            if (isDisposing)
            {
                // Release the icon resource.
                trayIcon.Dispose();
            }

            base.Dispose(isDisposing);
        }
    }
}
