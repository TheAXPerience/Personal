#include "stdafx.h"
#include "BattleSystem.h"
#include <string>

void playerChoice(BattleCharacter *actor, BattleCharacter *receiver);

void battleSystem(Character *first, Character *second)
{
	string confirm = "0";
	int battleState = 0;

	BattleCharacter *one = new BattleCharacter(first);
	BattleCharacter *two = new BattleCharacter(second);

	while (confirm == "0")
	{
		try
		{
			while ((battleState = checkHP(one, two)) == 0)
			{
				//battle system
				if (whoGoesNext(one, two) == true)
				{
					float reduced_time = one->getTimeToNextTurn();
					one->reduceTime(reduced_time);
					two->reduceTime(reduced_time);
					playerChoice(one, two);
				}
				else
				{
					float reduced_time = two->getTimeToNextTurn();
					one->reduceTime(reduced_time);
					two->reduceTime(reduced_time);
					playerChoice(two, one);
				}
			}
			if (battleState > 0)
			{
				cout << "The winner is: " << one->getName() << "!" << endl;
				cout << "HP left: " << one->getCurrentHP() << "/" << one->getMaxHP() << endl;
			}
			else if (battleState < 0)
			{
				cout << "The winner is: " << two->getName() << "!" << endl;
				cout << "HP left: " << two->getCurrentHP() << "/" << two->getMaxHP() << endl;
			}
		}
		catch (runtime_error e)
		{
			cout << e.what() << endl;
		}

		// reset battle
		cout << "Do you want to continue? (Type 0 to continue)" << endl;
		getline(cin, confirm);
		cin.clear();
		if (confirm == "0")
		{
			one->reset();
			two->reset();
			battleState = 0;
		}
	}
}

void playerChoice(BattleCharacter *actor, BattleCharacter *receiver)
{
	string attack_name;
	int choice = 1;
	while (choice != 0)
	{
		cout << "It is " << actor->getName() << "'s turn!" << endl;
		cout << "What do you want to do?" << endl
			<< "0: Use an attack" << endl
			<< "1: Print your character information" << endl
			<< "2: Print the other character's information" << endl
			<< "3: Print your character's attack information" << endl
			<< "4: Forfeit" << endl;
		cin >> choice;
		cin.ignore(numeric_limits<std::streamsize>::max(), '\n'); //cleans input until \n
		cin.clear();
		switch (choice)
		{
		case 0:
			cout << "Enter attack name: ";
			getline(cin, attack_name);

			try
			{
				int damage = receiver->calculateDamage(actor, attack_name);
				cout << actor->getName() << " dealt " << damage << " damage to " << receiver->getName()
					<< endl;

				float time_to_delay = actor->delayTime(attack_name);
				cout << actor->getName() << " was delayed by " << time_to_delay << " seconds" << endl;
			}
			catch (runtime_error e)
			{
				cout << e.what() << endl;
				choice = 1;
			}
			break;
		case 1:
			cout << *actor;
			break;
		case 2:
			cout << *receiver;
			break;
		case 3:
			actor->printMoves();
			break;
		case 4:
			cout << actor->getName() << " forfeited!" << endl;
			actor->forfeit();
			choice = 0;
			break;
		default:
			break;
		}
	}
}

bool whoGoesNext(BattleCharacter *one, BattleCharacter *two)
{
	return one->getTimeToNextTurn() < two->getTimeToNextTurn();
}

int checkHP(BattleCharacter *one, BattleCharacter *two)
{
	if (one->getCurrentHP() == 0 && two->getCurrentHP() == 0)
	{
		//this should not happen
		//or can be used with try/catch as a third possibility... hmm...
		throw runtime_error("Both fighters have passed out");
	}
	if (two->getCurrentHP() == 0)
	{
		return 1;
	}
	else if (one->getCurrentHP() == 0)
	{
		return -1;
	}
	return 0;
}