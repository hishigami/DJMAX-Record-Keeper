# DJMAX Record Keeper
A C# WPF app that archives past Respect V scores entered by the user and tabulates them.
This can be used for PS4 Respect as well if you don't mind the differing rank thresholds and V-exclusive licenses. ~~RIP Only On~~

Song and pattern information sourced from [DJMAX Random Selector.](https://github.com/wowvv0w/DJMAX_Random_Selector)
I've fed AllTrackData.csv into a Python script that converted the csv's data into JSON, then editted that output to both
translate Korean titles into their English translations as they appear in Respect and to mark (the lack of) charts for each song.

Currently a work in progress.
In addition to improving the program's aesthetics, other features such as dynamically disabling non-existent difficulties for a specific song and mode combination,
(properly) remembering saved folders instead of defaulting to base game's, and singling out DLC songs in Respect's folder are in planning.

The .NET Core Runtime must be installed to run this app.
