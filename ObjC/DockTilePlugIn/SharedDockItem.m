//
//  SharedDockItem.m
//

#import "SharedDockItem.h"

static NSMenu *myDockMenu = nil;
static NSUserDefaults *mainPrefs = nil;

@implementation SharedDockItem

+ (NSMenu*) dockMenuForPrefs:(NSUserDefaults*)prefs bundle:(NSBundle*)bundle
{
	/*
	 * This method performs two things:
	 * 1. Add the current high score as a disabled menu item
	 * 2. Add a list of icon names, from which the user can then choose one.
	 * Note that the DockTile menu does not appear to support submenus - a submenu's contents will be flattened out to the top level.
	 */

	[prefs synchronize];

	// Create or empty the our menu
	if (myDockMenu == nil) {
		myDockMenu = [[NSMenu alloc] init];
	} else {
		[myDockMenu removeAllItems];
	}
	
	//
	// Add the HighScore to the menu
	//
	NSInteger highScore = [prefs integerForKey:PrefsKeyHighScore];
	NSString *highScoreAsString = [NSString stringWithFormat:@"High Score: %ld", (long)highScore];
	NSMenuItem *highScoreMenu = [[NSMenuItem alloc] initWithTitle:highScoreAsString action:NULL keyEquivalent:@""];
	[myDockMenu addItem: highScoreMenu];

	//
	// Add the icon choices to the menu
	//
	NSString *iconName = [prefs objectForKey:PrefsKeyDockIcon];
	NSArray *iconChoices = bundle.infoDictionary[InfoKeyDockIconsToChooseFrom];
	NSMenuItem *subMenu = [myDockMenu addItemWithTitle:@"Choose Dock Icon" action:nil keyEquivalent:@""];
	subMenu.enabled = YES;	// while we use a submenu here, it won't show as a sub menu, unfortunately.
	for (NSDictionary *d in iconChoices) {
		NSString *key = d[@"title"];
		NSString *name = d[@"file name"];
		NSString *title = NSLocalizedString(key, @"DockIcon title");	// In case you'd want to localize the titles
		NSMenuItem *menuItem = [subMenu.menu addItemWithTitle:title action:@selector(chooseIcon:) keyEquivalent:@""];
		menuItem.target = self;
		menuItem.representedObject = name;
		if ([name isEqualToString:iconName]) {
			menuItem.state = NSControlStateValueOn;	// checkmark the currently chosen icon
		}
		menuItem.enabled = YES;
	}
	mainPrefs = prefs;	// needed in `chooseIcon:`, though we could have also put it into a dictionary assigned to `menuItem.representedObject`

	return myDockMenu;
}

+ (void)chooseIcon:(NSMenuItem*)menuItem
{
	[mainPrefs setObject:menuItem.representedObject forKey:PrefsKeyDockIcon];
	[NSDistributedNotificationCenter.defaultCenter postNotificationName:DockUpdateNotificationKey object:nil];
}

@end
