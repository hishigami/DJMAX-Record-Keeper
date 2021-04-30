# DJMAX Record Keeper
A C# WPF app that archives past Respect V scores entered by the user and tabulates them.
This can kinda be used for PS4 Respect as well, just don't expect Only On to stay for long...

Song and pattern information sourced from [DJMAX Random Selector.](https://github.com/wowvv0w/DJMAX_Random_Selector)
I've fed AllTrackData.csv into a Python script that converted the csv's data into JSON, then editted that output to both
translate Korean titles into their English translations as they appear in Respect and to mark (the lack of) charts for each song.

Currently a big work in progress.
In addition to adding actual help into the program and improving its aesthetics, the program needs to enforce song selection to exactly match Respect's songlist.
This would also include other feature implications such as dynamically disabling difficulties that don't exist for a specific song and mode combination.

The .NET Core Runtime must be installed to run this app.
