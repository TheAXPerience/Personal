#pragma once

#ifndef NUMBER_OF_ATTACKS
#define NUMBER_OF_ATTACKS 10

#include <iostream>
#include <map>
#include <string>
using namespace std;

enum Elements {Non = 0, Fire, Air, Water, Earth};
enum AttackType {Physical = 0, Magical};

typedef struct statistics
{
	int vitality : 8;
	int strength : 8;
	int spirit : 8;
	int endurance : 8;
	int mind : 8;
	int agility : 8;
} Stats;

typedef struct move
{
	char name[32];
	float power;
	float time;
	unsigned int accuracy : 8;
	unsigned int critical : 7;
	unsigned int element : 3;
	unsigned int type : 1;
} Move;

typedef struct elemental
{
	unsigned int non : 8;
	unsigned int fire : 8;
	unsigned int air : 8;
	unsigned int water : 8;
	unsigned int earth : 8;
} Resistances;

class Character
{
	friend std::istream& operator>>(std::istream &, Character &);
	friend std::ostream& operator<<(std::ostream &, const Character &);
public:
	static map<string, Move> list_of_moves;

	explicit Character(char n[], int vit = 0, int str = 0, int spr = 0, int end = 0,
		int mnd = 0, int agl = 0, unsigned int non = 100, unsigned int fire = 100,
		unsigned int air = 100, unsigned int water = 100, unsigned int earth = 100);
	Character(char n[], Stats stats, Resistances ele);
	Character(const Character& other);

	const string getName() const;
	int getVit() const;
	int getStr() const;
	int getSpr() const;
	int getEnd() const;
	int getMnd() const;
	int getAgl() const;

	unsigned int getNon() const;
	unsigned int getFire() const;
	unsigned int getWater() const;
	unsigned int getAir() const;
	unsigned int getEarth() const;

	int getHP() const;
	int getPAtk() const;
	int getMAtk() const;
	int getPDef() const;
	int getMDef() const;
	float getSpeed() const;
	float getAtkSpd() const;
	float getCstSpd() const;

	Character& addExistingMove(string name);
	Character& addMove(char name[], float power, float time, unsigned int acc,
		unsigned int crit, unsigned int ele, unsigned int type);

	static Move getMove(string name);
	static void printMoves();
	static bool checkMove(const Move &);
	static bool containsMove(string name);

	const Character& printCharMoves() const;
	int getNumAttacks() const;
	const char* getMoveName(int) const;
	bool hasMove(string) const;

	bool operator==(const Character &) const;
	bool operator!=(const Character &) const;

	~Character();
private:
	char name[32];
	char attacks[NUMBER_OF_ATTACKS][32];
	const Stats statistics;
	const Resistances elementals;
	int num_attacks;

	bool checkParameters() const;
};

ostream& operator<<(ostream &output, const Move &move);
bool operator==(const Move &one, const Move &two);
bool operator==(const Move &first, const string &second);
bool operator==(const string &first, const Move &second);

#endif