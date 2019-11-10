# Thinking about Visual Novels

A wise man once told me that if I don't actually make projects, I should at least write up my thoughts about them.
At least this way, I don't feel pressured to do everything but still allow myself to think creatively andat least ponder how I might go about doing something like this.

Thinking about a lot of games, there is a story to be told through text.
This text involves both some sort of narration and dialogue with characters in the game world.
Visual novels, especially dating sims, can be considered the basis of the system needed to interact in such a manner.

## Analysis

The system for visual novels need to keep track of certain parameters.
Some of them include stats, such as Hatoful Boyfriend with its vitality, knowledge, and charm.
Some also may involve you keeping track of flags that alter dialogue, either in the same conversation or later on down the road.
Some flags can be range from "have you seen this conversation?" to "you told Erika that her cat reminded you of the Dead Sea and she hates you for it".
Only specific flags would have to be marked down, but it's all dependent on the game itself.
These flags can be represented in an `HashMap` that links a certain event's name to a `bool`.
Rather, this can just be a `HashSet` where the flag only exists if it has been seen.

The aforementioned stats can be represented in a `HashMap` that links the stat's name to the current value.

### Representing Scenes

Different scenes would require different scripts that would be represented either in a different file or a different place in the same file.
We assume the former, since the latter would involve having to keep track of precise positioning of scenes in a single file.

Each script would have to contain enough information for the system to determine the scenery, characters, and actions available.
The scenery can be written as a name specific to the system, like "beach" for a beach scene or "city" for a scene in the city.
In some games, this is unneccessary as dialogue happens in a layer above the gameplay, but for visual novels, the scenery will be in constant change
The characters are simple and can be indenitified with either a character id or their name, depending on the specifics of the game
(for example, John Cena and John Marsh and...).
Either way, the scenery and characters can be simple to represent in text form.

The real challenge is thinking about the actions.
Actions not only require the player to make a choice, but for the system to recognize the choice and act accordingly.
This means there needs to be a trigger for the action, as well as a way to determine the action.
There may be some visual novels that involve typing something, but we assume the actions themselves are pre-determined.
Then, these actions we assume will be text, so we would allow the player to choose between a number of text options.
These text options must be included in the same package as the action initiation.
Then, the system needs to allow the player to choose an action, probably with a series of buttons of some sort.
These buttons then should mark a temporary flag that states a player's last choice.

This last choice then impacts how the rest of the script will go.
The script will now have to account for what the player has said and done before.
In the short term, if the effect is immediate, the temporary flag will be enough.
In the long term, some sort of global flag will have to be set.
Either way, the script must now indicate if a certain piece of text must happen if a certain flag is placed.
It must also do the same for things such as a scene change, future actions, etc.

Finally, we need a way for scripts to indicate  the scene that comes afterwards.
Perhaps we have some sort of marker indicating a scene change, and pass on the name of the next script file.
Perhaps the system itself will know what scene to go to next.
Sometimes it depends on flags and, in some cases, the player's own choice on what they want to do.

Some scripts could then be used for a chain of scenes that are directly related to each other.
These scenes then would have something between them that allows the player to go to another script depending on choice.
Then, there would be a sort of "main script" that allows the player to make a choice between scenes.
This main script doesn't need be a file, but hardcoded into the game itself, as it acts as a ub between all scenes.
Then, all scene files can also be hardcoded into the game, or the file names can be stored in its own file.
The indexer file will be read at the start of execution and arranged, perhaps in a `HashMap`, to allow the system to easily access the scripts.

### Graphics

There are not too many things to keep track of in terms of graphics. Only the following need to be kept track of:
- The background
- Character portraits
- Text box
- Buttons (if not part of the text box)

First off, we question the size we want to define the game. I'm thinking 1080p (1920 x 1080 pixels) as standard.
However, what happens if the screen is too large or too small?
Should we allow resizing or not? This might have to be a question for another time, as the major problem with resizing is having to scale everything with each other dependent on the window size.

The background itself will have to be changeable, which may be simple; just change the image on a background tile of some sort.
The exact details might differ between GUI systems, but for now we assume that changing an image on a background is implemented.

Character portraits are down to whether we want to show the portraits all at once (in scenes with multiple characters talking) or one at a time.
For now we assume one at a time, as showing all at once, or multiple at once, means we have to consider positioning the images along with possible overlapping.
The character portraits themselves cannot be just any picture.
They need an invisible background so it doesn't clash with the current background image.

The text box is something that might be difficult to consider.
I do not believe that GUi systems have lots of customization for their text boxes, and some games might want to use a custom text box.
This text box could be stored in the program as an image file, and we would overlap text on top to simulate an actual text box.
The text would have to be properly aligned, however, to make sure they don't collide with the borders of the custom text box.
Buttons may be similar in nature to text boxes, where a custom button is used instead of the GUI system's button.
However, is it possible for a button to exist such that it can be pressed while being invisible?
Or would we have to program the image itself to be clickable?

### Saving
As with many games, visual novels have a system to save. What do we need to save?
How large should the save file be? Can it be just any size or should it remain constant?
If it remains constant, then the game itself should remain constant.
This way, we ensure there are no updates to the game that can break the status of the player's current save.

However if we were to grow the game itself, would we not want a growing save file?
But then how do we ensure the save file is viable regardless of the version of the game?
Perhaps we have different functions that read save files differently.
These different functions correspond to different versions of the game, and the file itself could have the version number as the first byte.
Then, the file could then be read by the system to continue from where the player last left off.
There are probably better was to do this, but this is the first thought that comes to mind.

As for what needs to be save, it depends on the game. In our case, we consider the following:
- What scenes have been seen?
- What is the character's current progress?
- What is the character's current flags and stats?
- Information like names

Many of these things can be saved as bits that say whether a scene has been scene, a flag has been set, or something.
Stats are a different matter; they may have to be stored as a number of bytes.
For numerical stats, we determine a place in the save file to be that stat.
For strings like names, we probably want to allocate an array of unicode characters with a null-terminator character at the end.
Why unicode? Why not? ASCII can work, but unicode is more diverse. Since, you know, 2 byte characters vs. 1.
