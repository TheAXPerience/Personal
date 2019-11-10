# RPG Battle System

One of my most favorite genres of games is RPGs. However in a standard RPG, there are many things you have to consider: how to traverse the overworld, how to interact with things in the overworld, dialogue, and the battle system. Let's discuss thinking about the battle system of RPGs in the context of 1v1. This basic system is used in games like Pokemon, and can be extended into larger and larger fights.

## Parts of the Battle System

A regular turn-based RPG will have many different actions available to a particular character per turn. Some of the standard ones, at least for the player, include:
- Attacks (moves, spells, artes, etc.)
- Defend
- Special Actions (like "Steal")
- Items
- Escape

Some of these things, like Defend and Escape, can be hard-coded into the battle system since they are relatively simple mechanics-wise. However, the implementation of attacks, special actions, and items will be more particular.

For attacks and special actions, they can be repesented the same way; a special action can be a type of attack, and an attack can be considered a special action. These should probably not be hard-coded into the system and be represented in a data file. For items, it depends on the character's current inventory and the items themselves may also be stored in a data file.

### Attacks

Attacks have multiple parts that can differ between games. For example, a game like Fire Emblem would have a straightforward attack system, with most being calculated by the system with the only real differences being character stats and items. However in the recent ones, there are actual Combat Arts that fall into this category. Many attacks rely on a character's stats for damage calculation, but the attack itself should not have to rely on the characters to be represented in data form.

The basic parts of an attack include:
- Damage (however it will be implemented; damage multiplier, damage bonus, etc.)
- Accuracy (dependent on game... Pokemon has a straight-up accuracy, Fire Emblem would have an accuracy bonus, etc.)
- Critical (same as accuracy)
- Effect and effect percentage
- Range (in 1v1, either self, enemy, or both)
- Game-specific parameters (types in Pokemon, etc.)
- Name and Description (could be stored separately according to some sort of ID)
- Animation (definitely stored separately)

Many of these things can be represented in a couple bytes. Particularly: damage, accuracy, and critical.
The other things would have to be represented as some sort of mapping from physical byte to the effect/type/etc.
For effect and range, they can vary from having only a few to having hundreds depending on the game.
For things like types or elements, those can be just a single byte since it is not good game design to have 256+ different elements in your game. Like seriously, what is the point of that? Might as well just have no elements anymore.
Effect percentage would also be a byte, and refers to the percentage of an effect happening.

### Characters

The most important part of the battle system is the characters that act in them. Standardly, a character would have the following:
- Equipment
- Static statistics (level, max HP, base strength, etc.)
- Dynamic statistics (change in stats like current HP, strength boosts, etc.)
- List of actions (can be segmented into categories like items, attacks, etc.)
- Model and Costume (stored separately, just have to keep track of current state)
- Name (of course)
- Extra characteristics (Pokemon has natures, abilities, gender, etc.)

For storage, the character's dynamic statistics do not need to be stored, but rather they are needed only during combat.
If something were to carry over to the next battle, then it would be needed to be stored, such as current HP.

Equipment would probably be a mapping of a couple bytes to the item being referred to.
Static statistics can be represented in a couple bytes as well, and can be kept track of easily in the physical character.
The name would need a character limit and take an array of characters to save.

The list of actions would be different depending on the game. Perhaps it is the same for all characters, or it is something like Pokemon where there is a set quantity.
In the former case, a bitmap could be good enough to be like "is this action available for this character?".
In the latter case, enough bytes need to be used per move to identify the action in particular.
