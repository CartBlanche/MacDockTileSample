# DockTile Sample

As Apple has removed any tutorials on how to write **NSDockTilePlugin**s so I Googled for one. This is an updated version of [Janetzko Helmut's](https://github.com/HelmutJ) Objective C [DockTile](https://github.com/HelmutJ/CocoaSampleCode/tree/master/DockTile) sample.

His version is many years out of date, so this version updates the code to work in Xcode 10 and later.

## Original Sample's ReadMe

DockTile application demonstrates the use of NSDockTile, and more importantly, the NSDockTilePlugIn protocol introduced in 10.6.

The example includes two pieces: The DockTile app, and the plug-in, DockTilePlugIn. Building the target for the app will build and package both together.

You can think of DockTile as a terribly simple game. Your score goes up by 1 just by launching the app. So keep on launching the app over and over to reach new high scores.
 
The high score is shown in the dock tile, and the app window, where you can also reset it.  The plug-in is useful as a way to show the score even when the application is not running.
 
The whole application source code is implemented in the DockTileAppDelegate class, which is the delegate of the application. On applicationDidFinishLaunching: it updates the high score. The only other thing it does is to implement resetHighScore: to set it back to 0.
 
The dock tile plug-in shows off your high score even when the app is not running. The plug-in simply reads the high score from the defaults domain of the app, displays it as a badge on the dock tile, then updates it on receipt of a distributed notification that indicates when the high score changed.

Some notes and things to watch out for:

   * The project has two targets; the DockTileApp target also builds the DockTilePlugIn target, and includes it in the app package. 
   * The plug-in goes inside the Contents/PlugIns folder in the app package. Just the file name goes in the app's Info.plist, as the value of NSDockTilePlugIn.  
   * Remember to build the plug-in 32 and 64 bits.
   * The plug-in's principal class should be listed in the plug-in's Info.plist.
   * As of 10.6, SystemUIServer (the server which hosts the plug-ins) does not always notice when the plug-ins are updated. So during development, you may want to restart it after updating the plug-in.  Most thorough steps are: Remove your app from the dock. Wait 3-5 seconds. killall Dock.  killall SystemUIServer.
   * If your app does not draw into the dock tile, the plug-in's updates will be in effect even when the app is running.  So you can actually have your dock tile plug-in do all dock tile updating if you wish.  In the DockTile example the app actually shows the score itself while it's running.
   * Dock tile plug-ins should remember they are "guests" inside SystemUIServer, and take care not to destabilize or hog the process, or do anything that would block the main thread, such as networking, messaging, etc.  The DockTile example itself uses two techniques to hear about the high score status: Registers for a distributed notification, and access the high score using CFPreferences out of the app's preferences domain.  For instance the latter could have also been achieved using the NSUserDefaults addSuiteNamed: API, but that would have changed the global preferences domain list for the SystemUIServer — not a good idea.

## Development tips

* The current version (1.2) was created in Xcode 10.1. It may have trouble opening in older Xcode versions. If it does, try taking the project file of version 1.0 and then add the new files (icon2.icns, SharedDockItem.m/.h) again, and also turn on ARC again, or re-add the missing `release` and `[super dealloc]` calls.

* While macOS 11 (Big Sur) appears to properly reload the DockTilePlugin after it's been rebuilt (and as long as it's not set to "Keep in Dock"), older macOS versions (including 10.13) do not automatically reload the plugin. There, issue `killall Dock ; killall SystemUIServer` in Terminal after building a new version and before testing it.

## What the original sample showed

Each time the app is opened a counter increments and the app's DockTile is updated with the new value.

## Enhancement over original Sample

* Added DockMenu support
   * When you right click the app's dock tile, while it is running, the popped up menu will display the current counter
   * After setting the app to **Keep in Dock**, then shutting it down, when you right click the app's dock tile, now no longer running, the popped up menu will display the current counter
* (v1.2) Added support for changing the Dock icon, with considering a different icon for macOS Big Sur (and later) by default. The icon can be changed via the dock tile's menu. This work was done by [Thomas Tempelmann](https://github.com/tempelmann/MacDockTileSample)

## How to add multiple Dock Icons to choose from

See the `Info.plist` (`DockTile.ObjC-Info.plist`) file for the main app. There's a `DockIconsToChooseFrom` key that lists pairs of titles and file names. The titles will appear in the dock menu, whereas the file names identify the icon files that are to be located inside the app's Resources folder. The titles are also localizable through the usual ".strings" files.

## Future

I hope to update this repo with example of how to do DockTile plugins in other languages.
1st should be a Xamarin/.NET sample.

If you would like to port this sample to another language and would like to add it to this repo, please fork the repo as Pull Requests are always welcome!! :D

## License

The DockTile project is under the [Microsoft Public License](https://opensource.org/licenses/MS-PL) except for a few portions of the code.  See the [LICENSE.txt](LICENSE.txt) file for more details.
