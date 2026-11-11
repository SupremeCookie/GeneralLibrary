Wishlist for changes to the general library:

- Take Coastal Cargo's Generic Interaction logic
-- Include a sample scene/prefab where there's an interaction backplane available
--- The sample scene/prefab will live in an examples sub-assembly
-- Also take a look at dynamically switching between cameras, this is a big one that I need to make work
-- And add a bunch of base prefabs for UI elements and world items like I have for Coastal Cargo
--- Would be good to recolor the interaction stuff based on height? that way I can more easily see what lays on top of what

- Take a look at the TODOs surrounding the Coastal Cargo SteamWrapper
-- See what can be taken to the base Steam assembly
-- What we can take to the General Dummy Data assembly
-- Add example prefabs and scripts, that way we can easily take the example script and adapt to our needs
--- Examples live in their own sub-assemblies, take a look at how other packages do this

- Dummy Data Assembly
-- like the url for dkat, naming conventions for the build script, etc.
-- stuff that everything needs access to
-- and then we can add examples for stuff like "SteamStats" and its helper methods, and other such things

- In general examples of handy classes
-- Application Manager
-- Gameplay Manager
-- Take a look at other handy classes

- Rebuild build window
-- UIElements (uitoolkit) instead of ongui
-- When making a build, automatically name the folder within the versioned folder
--- So I just make a folder, like: "V_0.6.0_Demo" and then based on whether I'm building for linux, or webgl, or windows, the system will make an appropriate folder within the folder. 
--- Also make sure to add "_Steam" to the internal folder if we're building for steam

