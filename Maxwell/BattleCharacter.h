#pragma once
#ifndef BATTLE_READY_WARRIORS
#define BATTLE_READY_WARRIORS 1

#include "Character.h"

typedef struct battle
{
	int hp;
	int p_atk;
	int m_atk;
	int p_def;
	int m_def;
	float time_to_next_turn;
	float atk_spd;
	float cst_spd;
} Battle_Stats;

class BattleCharacter
{
	friend ostream& operator<< (ostream&, const BattleCharacter&);
public:
	BattleCharacter(Character *chara);
	int calculateDamage(BattleCharacter *attacker, const string move_name);
	float getTimeToNextTurn() const;
	int getCurrentHP() const;
	int getMaxHP() const;
	string getName() const;
	void printMoves() const;
	void reset();
	void resetHP();
	void reduceTime(float time);
	float delayTime(const string move_name);
	void forfeit();
	~BattleCharacter();
private:
	const Character *basis;
	Battle_Stats stats;

	const Move getMove(string move_name) const;
	void changeHP(int diff); //positive lowers HP, negative raises HP
	float randomizeStartTime(float speed);
};

#endif