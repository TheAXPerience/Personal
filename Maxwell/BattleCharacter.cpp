#include "stdafx.h"
#include "BattleCharacter.h"
#include <iomanip>
#include <map>
#include <cstdlib>

ostream& operator<< (ostream &output, const BattleCharacter &bc)
{
	output << "Name of Character: " << bc.basis->getName() << endl;
	output << "\tHP: " << setw(4) << bc.getCurrentHP() << "/" << setw(4) << bc.getMaxHP();
	output << "\tTime: " << setw(4) << bc.getTimeToNextTurn() << endl;
	output << "\tPhy Atk: " << setw(4) << bc.stats.p_atk;
	output << "\tPhy Def: " << setw(4) << bc.stats.p_def;
	output << "\tAtk Spd: " << setw(4) << bc.stats.atk_spd << "s" << endl;
	output << "\tMag Atk: " << setw(4) << bc.stats.m_atk;
	output << "\tMag Def: " << setw(4) << bc.stats.m_def;
	output << "\tCast Spd: " << setw(4) << bc.stats.cst_spd << "s" << endl;
	output << "\tNON: " << setw(3) << bc.basis->getNon();
	output << "\tFIRE: " << setw(3) << bc.basis->getFire();
	output << "\tAIR: " << setw(3) << bc.basis->getAir();
	output << "\tWATER: " << setw(3) << bc.basis->getWater();
	output << "\tEARTH: " << setw(3) << bc.basis->getEarth() << endl;

	return output;
}

BattleCharacter::BattleCharacter(Character *chara)
	: basis(chara)
{
	reset();
}

int BattleCharacter::calculateDamage(BattleCharacter *attacker, const string move_name)
{
	int damage = 0;
	const Move move = attacker->getMove(move_name);
	if (!Character::checkMove(move))
	{
		throw runtime_error("Not a valid move");
	}

	switch (move.type)
	{
	case Physical:
		damage = (attacker->stats.p_atk * move.power) - this->stats.p_def;
		break;
	case Magical:
		damage = (attacker->stats.m_atk * move.power) - this->stats.m_def;
		break;
	default:
		// not a valid move type
		throw runtime_error("Not a valid move");
		break;
	}

	switch (move.element)
	{
	case Non:
		damage = round(1.0 * damage * (201.0 - basis->getNon()) / 100.0);
		break;
	case Fire:
		damage = round(1.0 * damage * (201.0 - basis->getFire()) / 100.0);
		break;
	case Water:
		damage = round(1.0 * damage * (201.0 - basis->getWater()) / 100.0);
		break;
	case Air:
		damage = round(1.0 * damage * (201.0 - basis->getAir()) / 100.0);
		break;
	case Earth:
		damage = round(1.0 * damage * (201.0 - basis->getEarth()) / 100.0);
		break;
	default:
		// not a valid element
		throw runtime_error("Not a valid move");
		break;
	}

	// check hit rate
	int random = rand() % 100;
	if (random >= move.accuracy)
	{
		// missed
		cout << "Attack missed!" << endl;
		return 0;
	}

	// check critical hit rate
	random = rand() % 100;
	if (random < move.critical)
	{
		// critical hit
		cout << "Critical Hit!" << endl;
		damage *= 2;
	}
	this->changeHP(damage);
	return damage;
}

float BattleCharacter::getTimeToNextTurn() const
{
	return stats.time_to_next_turn;
}

int BattleCharacter::getCurrentHP() const
{
	return stats.hp;
}

int BattleCharacter::getMaxHP() const
{
	return basis->getHP();
}

string BattleCharacter::getName() const
{
	return basis->getName();
}

void BattleCharacter::printMoves() const
{
	basis->printCharMoves();
}

void BattleCharacter::reset()
{
	resetHP();
	stats.p_atk = basis->getPAtk();
	stats.p_def = basis->getPDef();
	stats.m_atk = basis->getMAtk();
	stats.m_def = basis->getMDef();
	stats.time_to_next_turn = randomizeStartTime(basis->getSpeed());
	stats.atk_spd = basis->getAtkSpd();
	stats.cst_spd = basis->getCstSpd();
}

void BattleCharacter::resetHP()
{
	stats.hp = basis->getHP();
}

void BattleCharacter::reduceTime(float time)
{
	this->stats.time_to_next_turn = round((this->stats.time_to_next_turn - time) * 10) / 10;
}

float BattleCharacter::delayTime(const string move_name)
{
	Move move = this->getMove(move_name);
	float delayed_time = move.time;
	switch (move.type)
	{
	case Physical:
		delayed_time -= stats.atk_spd;
		break;
	case Magical:
		delayed_time -= stats.cst_spd;
		break;
	default:
		throw runtime_error("Not a valid move");
		break;
	}
	delayed_time = (delayed_time < 1.0) ? 1.0 : delayed_time;
	this->stats.time_to_next_turn = round((this->stats.time_to_next_turn + delayed_time) * 10) / 10;
	return delayed_time;
}

void BattleCharacter::forfeit()
{
	stats.hp = 0;
}

const Move BattleCharacter::getMove(string move_name) const
{
	// returns a blank Move if it cannot be found
	// no moves should have "" as a name

	if (Character::list_of_moves.find(move_name) != Character::list_of_moves.end()
		&& basis->hasMove(move_name))
	{
		return Character::list_of_moves[move_name];
	}
	throw runtime_error("Move cannot be found");
	return Move{ "", 0.0, 0.0, 0, 0, 0, 0 };
}

void BattleCharacter::changeHP(int difference)
{
	stats.hp -= difference;

	if (stats.hp > basis->getHP())
	{
		resetHP();
	}
	else if (stats.hp < 0)
	{
		stats.hp = 0;
	}
}

float BattleCharacter::randomizeStartTime(float speed)
{
	float diff = (float)(rand() % 101) - 50.0;
	return round((speed * 10.0) + diff) / 10;
}

BattleCharacter::~BattleCharacter()
{

}
