#pragma once
#ifndef BATTLE_RAGES_ON
#include "BattleCharacter.h"
#include "Character.h"
#include <map>
#define BATTLE_RAGES_ON 0

static map<string, Character*> list_of_characters;

//manages the battle sequence
void battleSystem(Character*, Character*);

//returns true if character 1 goes first
//reeturns false if character 2 goes first
bool whoGoesNext(BattleCharacter*, BattleCharacter*);

//returns positive if the second character's hp is 0
//returns negative if the first character's hp is 0
int checkHP(BattleCharacter*, BattleCharacter*);


#endif
