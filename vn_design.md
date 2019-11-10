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
The characters are simple and can be indeitified with either a character id or their name, depending on the specifics of the game
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
