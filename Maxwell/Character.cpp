#include "stdafx.h"
#include "Character.h"
#include <iomanip>

map<string, Move> Character::list_of_moves;

istream& operator>>(istream &input, Character &chara)
{
	throw runtime_error("please do not use this, i don't wanna deal with this");
}

ostream& operator<<(ostream &output, const Character &chara)
{
	int i = 0;

	output << "Name of Character: " << chara.name << endl;
	output << "\tVIT: " << setw(3) << chara.statistics.vitality;
	output << "\tSTR: " << setw(3) << chara.statistics.strength;
	output << "\tSPR: " << setw(3) << chara.statistics.spirit;
	output << "\tEND: " << setw(3) << chara.statistics.endurance;
	output << "\tMND: " << setw(3) << chara.statistics.mind;
	output << "\tAGL: " << setw(3) << chara.statistics.agility << endl;

	output << "\tNON: " << setw(3) << chara.elementals.non;
	output << "\tFIRE: " << setw(3) << chara.elementals.fire;
	output << "\tAIR: " << setw(3) << chara.elementals.air;
	output << "\tWATER: " << setw(3) << chara.elementals.water;
	output << "\tEARTH: " << setw(3) << chara.elementals.earth << endl;

	output << "\tHP: " << setw(4) << chara.getHP();
	output << "\tSpeed: " << setw(4) << chara.getSpeed() << "s";
	output << "\tAtk Spd: " << setw(4) << chara.getAtkSpd() << "s";
	output << "\tCast Spd: " << setw(4) << chara.getCstSpd() << "s" << endl;
	output << "\tPhys Atk: " << setw(4) << chara.getPAtk();
	output << "\tMag Atk: " << setw(4) << chara.getMAtk();
	output << "\tPhys Def: " << setw(4) << chara.getPDef();
	output << "\tMag Def: " << setw(4) << chara.getMDef() << endl;

	chara.printCharMoves();

	return output;
}

bool Character::checkParameters() const
{
	if (name == "")
	{
		throw runtime_error("Characters must have a name");
		return false;
	}
	else if (statistics.vitality > 100 || statistics.strength > 100 || statistics.spirit > 100
		|| statistics.endurance > 100 || statistics.mind > 100 || statistics.agility > 100
		|| statistics.vitality < 0 || statistics.strength < 0 || statistics.spirit < 0
		|| statistics.endurance < 0 || statistics.mind < 0 || statistics.agility < 0)
	{
		throw runtime_error("Between 0 and 100");
		return false;
	}
	else if (elementals.non < 1 || elementals.fire < 1 || elementals.water < 1 ||
		elementals.air < 1 || elementals.earth < 1 || elementals.non > 200 || elementals.fire > 200
		|| elementals.water > 200 || elementals.air > 200 || elementals.earth > 200)
	{
		throw runtime_error("Between 1 and 200");
		return false;
	}
	return true;
}

Character::Character(char n[], int vit, int str, int spr, int end, int mnd, int agl,
	unsigned int non, unsigned int fire, unsigned int air, unsigned int water,
	unsigned int earth)
	: statistics({vit, str, spr, end, mnd, agl}), elementals({non, fire, air, water, earth})
{
	strcpy_s(name, 32, n);
	checkParameters();
	num_attacks = 0;
}

Character::Character(char n[], Stats stats, Resistances ele)
	: statistics(stats), elementals(ele)
{
	strcpy_s(name, 32, n);
	checkParameters();
	num_attacks = 0;
}

Character::Character(const Character& other)
	: statistics(other.statistics), elementals(other.elementals)
{
	strcpy_s(name, other.name);
	checkParameters();
	for (int i = 0; i < other.num_attacks; i++)
	{
		strcpy_s(attacks[i], other.attacks[i]);
	}
	num_attacks = other.num_attacks;
}

const string Character::getName() const
{
	return name;
}

int Character::getVit() const
{
	return statistics.vitality;
}

int Character::getStr() const
{
	return statistics.strength;
}

int Character::getSpr() const
{
	return statistics.spirit;
}

int Character::getEnd() const
{
	return statistics.endurance;
}
int Character::getMnd() const
{
	return statistics.mind;
}

int Character::getAgl() const
{
	return statistics.agility;
}

unsigned int Character::getNon() const
{
	return elementals.non;
}

unsigned int Character::getFire() const
{
	return elementals.fire;
}

unsigned int Character::getWater() const
{
	return elementals.water;
}

unsigned int Character::getAir() const
{
	return elementals.air;
}

unsigned int Character::getEarth() const
{
	return elementals.earth;
}

int Character::getHP() const
{
	return (int) round(statistics.vitality * 24.99);
}

int Character::getPAtk() const
{
	return (int) round(statistics.strength * 2.55);
}

int Character::getMAtk() const
{
	return (int) round(statistics.spirit * 2.55);
}

int Character::getPDef() const
{
	return (int) round(statistics.endurance * 2.55);
}

int Character::getMDef() const
{
	return (int) round(statistics.mind * 2.55);
}

float Character::getSpeed() const
{
	return round((26 - (statistics.agility / 4)) * 10) / 10;
}

float Character::getAtkSpd() const
{
	return round(((statistics.endurance * 0.3 + statistics.agility * 0.7) / 10 - 4) * 10) / 10;
}

float Character::getCstSpd() const
{
	return round(((statistics.mind * 0.7 + statistics.agility * 0.3) / 10 - 4) * 10) / 10;
}

Character& Character::addExistingMove(string name)
{
	if (num_attacks >= NUMBER_OF_ATTACKS)
	{
		throw runtime_error("Too many moves");
	}
	if (!this->hasMove(name))
	{
		strcpy_s(attacks[num_attacks], 32, name.c_str());
		num_attacks++;
	}
	return *this;
}

Character& Character::addMove(char name[], float power, float time, unsigned int acc,
	unsigned int crit, unsigned int ele, unsigned int type)
{
	Move new_move;
	strcpy_s(new_move.name, 32, name);
	new_move.power = power;
	new_move.time = time;
	new_move.accuracy = acc;
	new_move.critical = crit;
	new_move.element = ele;
	new_move.type = type;
	
	if (!Character::checkMove(new_move))
	{
		throw runtime_error("Not a valid move");
	}
	else if (num_attacks >= NUMBER_OF_ATTACKS)
	{
		throw runtime_error("Too many moves");
	}
	(Character::list_of_moves)[name] = new_move;
	return addExistingMove(name);
}

Move Character::getMove(string name)
{
	if (Character::list_of_moves.find(name) != Character::list_of_moves.end())
	{
		return Character::list_of_moves[name];
	}
	return Move{ "", 0.0, 0.0, 0, 0, 0, 0 };
}

void Character::printMoves()
{
	cout << "List of moves:" << endl << right;
	cout << setw(32) << "NAME" << setw(6) << "POW" << setw(6) << "ACC" << setw(6) << "CRIT"
		<< setw(6) << "TIME" << setw(12) << "ELEMENT" << setw(8) << "TYPE" << endl;
	map<string, Move>::iterator it = Character::list_of_moves.begin();
	while (it != list_of_moves.end())
	{
		cout << it->second << endl;
		it++;
	}
}

bool Character::checkMove(const Move &attack)
{
	if (attack.name == "")
	{
		return false;
	}
	if (attack.accuracy < 0 || attack.accuracy > 150 || attack.critical < 0 || attack.critical > 100 ||
		attack.element < 0 || attack.element > 4 || attack.power < 0.0 || attack.power - 10.0 > 0.0 ||
		attack.time - 5.0 < 0.0 || attack.time - 60.0 > 0.0 || attack.type < 0 || attack.type > 1)
	{
		return false;
	}
	return true;
}

bool Character::containsMove(string name)
{
	if (Character::list_of_moves.find(name) != Character::list_of_moves.end())
	{
		return true;
	}
	return false;
}

const Character& Character::printCharMoves() const
{
	cout << "List of moves:" << endl << right;
	cout << setw(32) << "NAME" << setw(6) << "POW" << setw(6) << "ACC" << setw(6) << "CRIT"
		<< setw(6) << "TIME" << setw(12) << "ELEMENT" << setw(8) << "TYPE" << endl;
	for (unsigned int i = 0; i < num_attacks; i++)
	{
		if (attacks[i] != "")
		{
			cout << Character::getMove(attacks[i]) << endl;
		}
	}
	return *this;
}

int Character::getNumAttacks() const
{
	return num_attacks;
}

const char* Character::getMoveName(int index) const
{
	return attacks[index];
}

bool Character::hasMove(string name) const
{
	for (int i = 0; i < num_attacks; i++)
	{
		if (attacks[i] == name)
		{
			return true;
		}
	}
	return false;
}

bool Character::operator==(const Character &other) const
{
	bool truth = (this->name == other.name)
		&& (this->statistics.vitality == other.statistics.vitality)
		&& (this->statistics.strength == other.statistics.strength)
		&& (this->statistics.spirit == other.statistics.spirit)
		&& (this->statistics.endurance == other.statistics.endurance)
		&& (this->statistics.mind == other.statistics.mind)
		&& (this->statistics.agility == other.statistics.agility)
		&& (this->elementals.non == other.elementals.non)
		&& (this->elementals.fire == other.elementals.fire)
		&& (this->elementals.air == other.elementals.air)
		&& (this->elementals.water == other.elementals.water)
		&& (this->elementals.earth == other.elementals.earth)
		&& (this->num_attacks == other.num_attacks);
	for (int i = 0; i < num_attacks && truth; i++)
	{
		truth = truth && (this->attacks[i] == other.attacks[i]);
	}
	return truth;
}

bool Character::operator!=(const Character &other) const
{
	return !(*this == other);
}

Character::~Character()
{
	
}

ostream& operator<<(ostream &output, const Move &move)
{
	output << setw(32) << move.name << setw(6) << move.power << setw(6) << move.accuracy
		<< setw(6) << move.critical << setw(6) << move.time;

	switch (move.element)
	{
	case Non:
		cout << setw(12) << "None";
		break;
	case Fire:
		cout << setw(12) << "Fire";
		break;
	case Air:
		cout << setw(12) << "Air";
		break;
	case Earth:
		cout << setw(12) << "Earth";
		break;
	case Water:
		cout << setw(12) << "Water";
		break;
	default:
		throw invalid_argument("Move element is not a valid element");
		break;
	}

	switch (move.type)
	{
	case Physical:
		cout << setw(8) << "Tech";
		break;
	case Magical:
		cout << setw(8) << "Magic";
		break;
	default:
		throw invalid_argument("Move type is not a valid type");
		break;
	}
	return output;
}

bool operator==(const Move &one, const Move &two)
{
	return (one.accuracy == two.accuracy) && (one.critical == two.critical)
		&& (one.element == two.element) && (one.name == two.name) &&
		(one.power == two.power) && (one.time == two.time) && (one.type == two.type);
}

bool operator==(const Move &first, const string &second)
{
	string name(first.name);
	return (name == second);
}

bool operator==(const string &first, const Move &second)
{
	string name(second.name);
	return (name == first);
}