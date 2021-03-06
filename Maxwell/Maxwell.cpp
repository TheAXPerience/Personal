// Maxwell.cpp : Defines the entry point for the console application.

#include "stdafx.h"
#include "Character.h"
#include "BattleSystem.h"
#include <fstream>

#define characterFile "characters.bin"
#define attackFile "attacks.bin"
using namespace std;

Character* setup();
void setMoves(Character*);
void printCharacters();
void readFromFile(string character_file, string attack_file);
void writeToFile(string character_file, string attack_file);

int main()
{
	string name_temp1;
	string name_temp2;
	Character *temp = NULL;
	int choice = 0;

	do
	{
		cout << "What do you want to do?" << endl
			<< "0. Quit program" << endl
			<< "1. Read from a file" << endl
			<< "2. Save to a file" << endl
			<< "3. Make a character" << endl
			<< "4. Add moves to an existing character" << endl
			<< "5. Print a list of characters" << endl
			<< "6. Print a list of moves" << endl
			<< "7. Battle between two characters" << endl;
		cin >> choice;
		cin.ignore(numeric_limits<std::streamsize>::max(), '\n'); //cleans input until \n

		switch (choice)
		{
		case 0:
			break;
		case 1:
			readFromFile(characterFile, attackFile);
			break;
		case 2:
			writeToFile(characterFile, attackFile);
			break;
		case 3:
			temp = setup();
			name_temp1 = temp->getName();
			::list_of_characters[name_temp1] = temp;
			break;
		case 4:
			cout << "Which character? ";
			getline(cin, name_temp1);
			if (name_temp1.length() > 31)
			{
				name_temp1 = name_temp1.substr(0, 31);
			}

			if (::list_of_characters.find(name_temp1) == 
				::list_of_characters.end())
			{
				cout << "That character does not exist here" << endl;
			}
			else
			{
				temp = ::list_of_characters[name_temp1];
				setMoves(temp);
			}
			break;
		case 5:
			printCharacters();
			break;
		case 6:
			Character::printMoves();
			break;
		case 7:
			cout << "Enter the first character's name: ";
			getline(cin, name_temp1);
			cout << "Enter the second character's name: ";
			getline(cin, name_temp2);

			if (name_temp1.length() > 31)
			{
				name_temp1 = name_temp1.substr(0, 31);
			}
			if (name_temp2.length() > 31)
			{
				name_temp2 = name_temp2.substr(0, 31);
			}

			if (::list_of_characters.find(name_temp1) ==
				::list_of_characters.end())
			{
				cout << name_temp1 << " does not exist here" << endl;
			}
			else if (::list_of_characters.find(name_temp2) ==
				::list_of_characters.end())
			{
				cout << name_temp2 << " does not exist here" << endl;
			}
			else
			{
				battleSystem(::list_of_characters[name_temp1], ::list_of_characters[name_temp2]);
			}
			break;
		default:
			cout << "Please put 1 - 7 (0 to quit)" << endl;
			break;
		}
	} while (choice != 0);

	cin.clear();
	return 0;
}

Character *setup()
{
	//get stats
	char name[32];
	int stats[6];
	int elements[5];
	string n;
	cout << "What is your name? (Max 31 characters) ";
	getline(cin, n);
	if (n.length() > 31)
	{
		n = n.substr(0, 31);
	}
	strncpy_s(name, n.c_str(), 32);
	cout << "Set VIT (0 - 100): ";
	cin >> stats[0];
	cout << "Set STR (0 - 100): ";
	cin >> stats[1];
	cout << "Set SPR (0 - 100): ";
	cin >> stats[2];
	cout << "Set END (0 - 100): ";
	cin >> stats[3];
	cout << "Set MND (0 - 100): ";
	cin >> stats[4];
	cout << "Set AGL (0 - 100): ";
	cin >> stats[5];

	cout << "Set NON (1 - 200): ";
	cin >> elements[0];
	cout << "Set FIRE (1 - 200): ";
	cin >> elements[1];
	cout << "Set AIR (1 - 200): ";
	cin >> elements[2];
	cout << "Set WATER (1 - 200): ";
	cin >> elements[3];
	cout << "Set EARTH (1 - 200): ";
	cin >> elements[4];

	Character* chara = new Character(name, stats[0], stats[1], stats[2], stats[3], stats[4], stats[5],
		elements[0], elements[1], elements[2], elements[3], elements[4]);

	cin.ignore(numeric_limits<std::streamsize>::max(), '\n'); //cleans input until \n
	cin.clear();

	setMoves(chara);
	return chara;
}

void setMoves(Character *chara)
{
	//get moves
	bool cont = true;
	string continue_message = "";
	char name[32];
	float pow;
	float time;
	unsigned int moveinfo[4];

	while (cont)
	{
		// get move information
		cout << "Adding a move\nWhat is the name? (Max 31 characters) ";
		getline(cin, continue_message);
		if (continue_message.length() > 31)
		{
			continue_message = continue_message.substr(0, 31);
		}
		strncpy_s(name, continue_message.c_str(), 32);

		if (Character::containsMove(name))
		{
			chara->addExistingMove(name);
		}
		else
		{
			cout << "Power? (0.0 - 10.0) ";
			cin >> pow;
			cout << "Time? (5.0 - 60.0) ";
			cin >> time;
			cout << "Accuracy? (0 - 150) ";
			cin >> moveinfo[0];
			cout << "Critical? (0 - 100) ";
			cin >> moveinfo[1];
			cout << "Element? (Non = 0, Fire = 1, Air = 2, " <<
				"Water = 3, Earth = 4) ";
			cin >> moveinfo[2];
			cout << "Type? (Phys = 0, Mag = 1) ";
			cin >> moveinfo[3];

			// add the move to the character's list
			chara->addMove(name, pow, time, moveinfo[0], moveinfo[1], moveinfo[2], moveinfo[3]);
			cin.ignore(numeric_limits<std::streamsize>::max(), '\n'); //cleans input until \n
		}

		// continue prompt
		cout << "Continue? (to quit, press \"0\") ";
		getline(cin, continue_message);
		cin.clear();
		cont = (continue_message != "0");
	}
}

void printCharacters()
{
	map<string, Character*>::iterator it = ::list_of_characters.begin();
	while (it != ::list_of_characters.end())
	{
		cout << *(it->second) << endl;
		it++;
	}
}

void readFromFile(string character_file, string attack_file)
{
	char n[] = "Name";
	Character *char_temp = new Character(n);
	Move *move_temp = new Move;
	string name_temp = "";
	
	::list_of_characters.erase(::list_of_characters.begin(), ::list_of_characters.end());
	Character::list_of_moves.erase(Character::list_of_moves.begin(),
		Character::list_of_moves.end());

	cout << "Loading..." << endl;
	
	ifstream move_file(attack_file, ios::binary);
	if (move_file.is_open())
	{
		while (move_file.read((char*)(move_temp), sizeof(Move)))
		{
			name_temp = string(move_temp->name);
			Character::list_of_moves[name_temp] = *move_temp;
		}
		move_file.close();
	}
	else
	{
		cout << "Could not find file named " << attack_file << endl;
	}

	ifstream char_file(character_file, ios::binary);
	if (char_file.is_open())
	{
		while (char_file.read((char*)(char_temp), sizeof(Character)))
		{
			name_temp = string(char_temp->getName());
			::list_of_characters[name_temp] = new Character(*char_temp);
		}
		char_file.close();
	}
	else
	{
		cout << "Could not find file named " << character_file << endl;
	}

	delete move_temp;
	cout << "Your data was loaded" << endl;
}

void writeToFile(string character_file, string attack_file)
{
	cout << "Saving..." << endl;

	ofstream char_file(character_file, ios::binary);
	if (char_file.is_open())
	{
		map<string, Character*>::iterator it = ::list_of_characters.begin();
		while (it != ::list_of_characters.end())
		{
			char_file.write((char*)(it->second), sizeof(Character));
			it++;
		}
	}

	ofstream move_file(attack_file, ios::binary);
	if (move_file.is_open())
	{
		map<string, Move>::iterator it = Character::list_of_moves.begin();
		while (it != Character::list_of_moves.end())
		{
			move_file.write((char*)(&(it->second)), sizeof(Move));
			it++;
		}
	}

	char_file.close();
	move_file.close();
	cout << "Your data was saved" << endl;
}