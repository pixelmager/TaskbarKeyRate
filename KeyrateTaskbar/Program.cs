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

            //TODO: convert to rate
            {
                MenuItem mi = trayMenu.MenuItems.Add("5ms", MenuItemNew_Click);
                mi.Tag = new SetRepeatMs { RepeatRateMs = 5 };
            }
            {
                MenuItem mi = trayMenu.MenuItems.Add("10ms", MenuItemNew_Click);
                mi.Tag = new SetRepeatMs { RepeatRateMs = 10 };
            }
            {
                MenuItem mi = trayMenu.MenuItems.Add("15ms", MenuItemNew_Click);
                mi.Tag = new SetRepeatMs{ RepeatRateMs = 15 };
            }
            {
                MenuItem mi = trayMenu.MenuItems.Add("20ms", MenuItemNew_Click);
                mi.Tag = new SetRepeatMs { RepeatRateMs = 20 };
            }
            {
                MenuItem mi = trayMenu.MenuItems.Add("25ms", MenuItemNew_Click);
                mi.Tag = new SetRepeatMs { RepeatRateMs = 25 };
            }
            {
                MenuItem mi = trayMenu.MenuItems.Add("30ms", MenuItemNew_Click);
                mi.Tag = new SetRepeatMs { RepeatRateMs = 30 };
            }
            {
                MenuItem mi = trayMenu.MenuItems.Add("40ms", MenuItemNew_Click);
                mi.Tag = new SetRepeatMs { RepeatRateMs = 40 };
            }
            {
                MenuItem mi = trayMenu.MenuItems.Add("50ms", MenuItemNew_Click);
                mi.Tag = new SetRepeatMs { RepeatRateMs = 50 };
            }

            trayMenu.MenuItems.Add("-");

            {
                MenuItem mi = trayMenu.MenuItems.Add("Delay");
                mi.Enabled = false;
            }
            {
                MenuItem mi = trayMenu.MenuItems.Add("50ms", MenuItemNew_Click);
                mi.Tag = new SetDelayMs { DelayMs = 50 };
            }
            {
                MenuItem mi = trayMenu.MenuItems.Add("100ms", MenuItemNew_Click);
                mi.Tag = new SetDelayMs { DelayMs = 100 };
            }
            {
                MenuItem mi = trayMenu.MenuItems.Add("125ms", MenuItemNew_Click);
                mi.Tag = new SetDelayMs { DelayMs = 125 };
            }
            {
                MenuItem mi = trayMenu.MenuItems.Add("150ms", MenuItemNew_Click);
                mi.Tag = new SetDelayMs { DelayMs = 150 };
            }
            {
                MenuItem mi = trayMenu.MenuItems.Add("175ms", MenuItemNew_Click);
                mi.Tag = new SetDelayMs { DelayMs = 175 };
            }
            {
                MenuItem mi = trayMenu.MenuItems.Add("200ms", MenuItemNew_Click);
                mi.Tag = new SetDelayMs{ DelayMs = 200 };
            }
            {
                MenuItem mi = trayMenu.MenuItems.Add("250ms", MenuItemNew_Click);
                mi.Tag = new SetDelayMs { DelayMs = 250 };
            }
            {
                MenuItem mi = trayMenu.MenuItems.Add("300ms", MenuItemNew_Click);
                mi.Tag = new SetDelayMs { DelayMs = 300 };
            }
            {
                MenuItem mi = trayMenu.MenuItems.Add("400ms", MenuItemNew_Click);
                mi.Tag = new SetDelayMs { DelayMs = 400 };
            }
            {
                MenuItem mi = trayMenu.MenuItems.Add("500ms", MenuItemNew_Click);
                mi.Tag = new SetDelayMs { DelayMs = 500 };
            }

            trayMenu.MenuItems.Add("-");
            trayMenu.MenuItems.Add("Disable", DisableKeyrateOverride);
            trayMenu.MenuItems.Add("Exit", OnExit);

            //TODO: remember previous
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
