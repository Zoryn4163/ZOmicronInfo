# ZOmicronInfo

**REQUIRES .NET FRAMEWORK 4.5.2 - GET IT HERE: https://www.microsoft.com/en-us/download/details.aspx?id=42643 **

A simple program made in the .NET Framwork with C# to pull information about any Pokemon in the fan-made games Pokemon Zeta and Pokemon Omicron.

Due to how the program handles the data, it can easily be adapted to any game made using Pokemon Essentials.

Currently, the program features a simple console that, when opened directly without any parameters, will initialize a JSON blob containing all Pokemon data in the game currently. 
The console will then prompt for user input, and expects either the **internal name** of a Pokemon, or it's **national dex ID**.

Colours can be disabled by launching the program with the argument **"-nc"**.
This will simply disable colouring, and all text outputs will be the default gray.

If someone wants to use an external program alongside mine, a request for a pokemon can be made and piped out using any framework's output redirection.
To do this, the program would be started with the first argument being the name or ID that would normally be entered followed by "-p".
This data will then be output as a JSON blob for anyone to do as they wish with. All of the pokemon data is already in a JSON that is shipped with the release as well.

Note: The source contains the file "pokemon.raw" - this is the "pokemon.dat" file that comes directly from Pokemon Zeta. It should be accurate for Omicron as well.
When the program runs, it will first check for this file. If it exists, it will convert it to JSON every time the program is launched.

**I will not aid anyone in converting Zeta/Omicron files into raw text/json blobs/etc.**

**Argument Examples and Effects**

ZOmicronInfo.exe -nc | All text colour is the default gray.

ZOmicronInfo.exe eevee -p | Pipes out all data on "EEVEE" as a JSON blob. Program exits immediately after data is output.

ZOmicronInfo.exe 133 -p | Same effect as the above query for "EEVEE".