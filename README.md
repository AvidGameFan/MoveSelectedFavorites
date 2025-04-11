# Favorites
## Copy favorite images
Tag your favorite images in Easy Diffusion, and use this utility to copy/move them to another folder.  This is helpful when you have working folders with many images that have slight variations, and you only want certain variations or even just the final copy.  How you organize images is up to you, but if you generate many intermediate images attempting to create the perfect one, you likely have folders with many unneeded images, and it's harder to sort through them later.  If you find it easier to recognize which images you want to keep while still within the Easy Diffusion interface, then this utility and plugin will be useful.

## How to use
### Tagging
First, install the favorites plugin (favorites.plugin.js) to the EasyDiffusion\plugins\ui folder.  Tag images that you want to keep.  (Look for Favorites as you hover over the generated image.)  Be sure that your Easy Diffusion Settings has "auto-save" turned on, so that your images are actually saved to folders.  (The favorites plugin doesn't save the images, just the seeds to find them later.)

When you're done tagging, hit the save (floppy) icon on the Favorites line.  This saves a list of the seeds to a favorites___.txt file in your download folder (or whereever you specify).

### Copying
Run MoveSelectedFavorites.exe.

Select the "favorites___.txt" file (probably saved to your downloads folder) that corresponds with the source folder (the one with all of the generated images).  Select the source folder, if it is not auto-filled properly.  Select the destination folder.  Hit Copy!

#### More Info
Note that you can also drag-and-drop your "favorites___.txt" file into the window. 

When the favorites file is selected or dropped, it will attempt to auto-detect the source folder by matching timestamps.  Occasionally, it does not automatically recognize the appropriate folder, and you have to manually select.  The default for the search is the Easy Diffusion default within the user\Easy Diffusion UI folder, however, if you use another location, you can put the new Easy Diffusion UI folder in the "copy from" field, and it'll use that to auto-search for a matching folder.

There's an option to add a suffix to the folder after it's processed.  This will let you know what the folder has been worked upon.  After processing all of your favorites text files, whatever is left will need to be examined manually.

Currently, it "copies" files, not "move", so that the original folders are not modified (except for optional renaming).

## License
Free to use by users of Easy Diffusion. No guarantee of accuracy or suitability is given.
