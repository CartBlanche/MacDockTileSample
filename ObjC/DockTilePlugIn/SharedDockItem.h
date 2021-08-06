//
//  SharedDockItem.h
//  FAFv2
//
//  Created by Thomas Tempelmann on 05.08.21.
//  Copyright © 2021 Thomas Tempelmann. All rights reserved.
//

#import <Cocoa/Cocoa.h>

// The name of the default icon the app, as set in the Info.plist (`CFBundleIconFile`)
#define DEFAULT_ICON_NAME @"icon.icns"

// The Info.plist key that lists the available icons the user can choose from
#define InfoKeyDockIconsToChooseFrom @"DockIconsToChooseFrom"

// The userdefaults key specifying the dock's icon file
#define PrefsKeyDockIcon @"DockIcon"

// The userdefaults key specifying the highscore that'll appear in the dock tile's badge
#define PrefsKeyHighScore @"HighScore"

// The message we send to ask the DockTile plugin to refresh its icon and/or badge
static NSString *DockUpdateNotificationKey = @"com.apple.DockTile.RefreshDock";

@interface SharedDockItem : NSObject

// A function that creates the dock's custom menu,
// used by both the main app and by the plugin when the app is not running
+ (NSMenu*) dockMenuForPrefs:(NSUserDefaults*)prefs bundle:(NSBundle*)bundle;

@end
