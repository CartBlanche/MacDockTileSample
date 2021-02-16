using System;
using AppKit;
using Foundation;

namespace DockTile.NET
{
    [Register("AppDelegate")]
    public partial class AppDelegate : NSApplicationDelegate
    {
        private nint HighScore => NSUserDefaults.StandardUserDefaults.IntForKey(HighScoreKey);

        private const string HighScoreKey = "HighScore";
        private NSMenu runtimeDockMenu;

        public AppDelegate()
        {
        }

        public override void DidFinishLaunching(NSNotification notification)
        {
            SetHighScore(HighScore + 1);
        }

        private void SetHighScore(nint newScore)
        {
            NSUserDefaults defaults = NSUserDefaults.StandardUserDefaults;

            // We just save the value out, we don't keep a copy of the high score in the app.
            defaults.SetInt(newScore, HighScoreKey);

            // Save the value out to defaults now. We often don't explicit synchronize, since it's best to let the system take care of it automatically.
            // However, in this case since we're asking the plug-in to update the score, synchronizing before the notification ensures that the plug-in sees the latest value.
            // Always make sure the value is updated and synchronized before sending out the distributed notification to other processes.
            if (!defaults.Synchronize())
            {
                Console.WriteLine("Synchronize Failed");
                return;
            }

            // And post a notification so the plug-in sees the change.
            NSDistributedNotificationCenter.GetDefaultCenter().PostNotificationName("com.apple.DockTile.NET.HighScoreChanged", null);

            // Now update the dock tile. Note that a more general way to do this would be to observe the highScore property,
            // but we're just keeping things short and sweet here, trying to demo how to write a plug-in. 
            NSApplication.SharedApplication.DockTile.BadgeLabel = string.Format("{0}", newScore);
        }

        public override void WillTerminate(NSNotification notification)
        {
            // Insert code here to tear down your application
        }

        // This gets called at Runtime when you right click the DockTile
        public override NSMenu ApplicationDockMenu(NSApplication sender)
        {
            if (runtimeDockMenu == null)
            {
                runtimeDockMenu = new NSMenu();
            }
            else
            {
                runtimeDockMenu.RemoveAllItems();
            }

            runtimeDockMenu.AddItem(new NSMenuItem(string.Format("{0}", HighScore)));

            return runtimeDockMenu;
        }

        //Reset the high score. Simple...
        partial void ResetHighScore(NSObject sender)
        {
            SetHighScore(0);
        }
    }
}
