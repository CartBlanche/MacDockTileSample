using System;
using AppKit;
using CoreFoundation;
using Foundation;

namespace DockTile.NET.Plugin
{
    [Register("DockTilePlugin")]
    public class DockTilePlugin : NSDockTilePlugIn
    {
        private NSMenu nonRuntimeDockMenu;
        private const string appId = "com.CartBlanche.DockTile-NET";
        private NSObject highScoreObserver;

        public override NSMenu DockMenu()
        {
            // Lazy load our dockMenu
            if (nonRuntimeDockMenu == null)
            {
                nonRuntimeDockMenu = new NSMenu();
            }
            else
            {
                nonRuntimeDockMenu.RemoveAllItems();
            }

            if (CFPreferences.AppSynchronize(appId))
            {
                nonRuntimeDockMenu.AddItem(new NSMenuItem(string.Format("HighScore: {0}", CFPreferences.GetAppIntegerValue("HighScore", appId))));
            }
            else
            {
                nonRuntimeDockMenu.AddItem(new NSMenuItem("HighScore: None"));
            }

            return nonRuntimeDockMenu;
        }

        public override void SetDockTile(NSDockTile dockTile)
        {
            if (dockTile != null)
            {
                // Attach an observer that will update the high score in the dock tile whenever it changes
                highScoreObserver = (NSObject)NSDistributedNotificationCenter.GetDefaultCenter().AddObserver("com.apple.DockTile.NET.HighScoreChanged", null, null, (obj) => {
                    // Note that this block captures (and retains) dockTile for use later.
                    // Also note that it does not capture self, which means -dealloc may be called even while the notification is active.
                    // Although it's not clear this needs to be supported, this does eliminate a potential source of leaks.
                    UpdateScore(dockTile);
                } );

                // Make sure score is updated from the get-go as well
                UpdateScore(dockTile);
            }
            else
            {
                // Strictly speaking this may not be necessary (since the plug-in may be terminated when it's removed from the dock), but it's good practice
                NSDistributedNotificationCenter.GetDefaultCenter().RemoveObserver(highScoreObserver);
                highScoreObserver = null;
            }
        }

        public static void UpdateScore(NSDockTile dockTile)
        {
            CFPreferences.AppSynchronize(appId);

            dockTile.BadgeLabel = string.Format("{0}", CFPreferences.GetAppIntegerValue("HighScore", appId));
        }
    }

}
