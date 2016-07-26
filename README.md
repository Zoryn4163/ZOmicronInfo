# ZOmicronInfo

**REQUIRES .NET FRAMEWORK 4.5.2 - GET IT HERE: https://www.microsoft.com/en-us/download/details.aspx?id=42643**

A simple program made in the .NET Framwork with C# to pull information about any Pokemon in the fan-made games Pokemon Zeta and Pokemon Omicron.

Due to how the program handles the data, it can easily be adapted to any game made using Pokemon Essentials.

- Currently, the program features a simple GUI that, when opened directly without any parameters, will initialize a JSON blob containing all Pokemon data in the game currently. 
- The GUI will then require the user to choose a Pokemon from the 'Tabs' drop-down on the top of the window, and will display all information regarding the selected Pokemon.
- The display of the selected Pokemon is multi-tab-based, so (almost) any number of Pokemon's data can be switched to with ease.

![alt text](https://i.gyazo.com/7d1520ae6bebb89a60270ff016a26e8a.png "Version 1.3 GUI")

**ABOUT PICTURES IN GUI**

- Since V1.3, the GUI has support for images of the selected Pokemon at the bottom. 
- The program supports *.png, *.jpg, and *.gif files.
- The program will look for them in the folder ./Graphics/Battlers/
- The simplest way to make the images show is to drop this program *next to Game.exe for Zeta/Omicron*

**This is not required and the program will work fine without any images - they are purely aesthetic.**

**CONSOLE INFORMATION (AKA NO GUI)**

- If anyone prefers a console-implementation, simply launch the program with any* argument, or the ones listed below. (* Some arguments may not work, but most will.)

- Colours can be disabled by launching the program with the argument **"-nc"**.
- This will simply disable colouring, and all text outputs will be the default gray.

- If someone wants to use an external program alongside mine, a request for a pokemon can be made and piped out using any framework's output redirection.
- To do this, the program would be started with the first argument being the name or ID that would normally be entered followed by "-p".
- This data will then be output as a JSON blob for anyone to do as they wish with. All of the pokemon data is already in a JSON that is shipped with the release as well.

*Argument Examples and Effects*

 - ZOmicronInfo.exe -nc | All text colour is the default gray.

 - ZOmicronInfo.exe eevee -p | Pipes out all data on "EEVEE" as a JSON blob. Program exits immediately after data is output.

 - ZOmicronInfo.exe 133 -p | Same effect as the above query for "EEVEE".

**OTHER INFORMATION**

- Note: The source contains the file "pokemon.raw" - this is the "pokemon.dat" file that comes directly from Pokemon Zeta. It should be accurate for Omicron as well.
- When the program runs, it will first check for this file. If it exists, it will convert it to JSON every time the program is launched.

**I will not aid anyone in converting Zeta/Omicron files into raw text/json blobs/etc.**