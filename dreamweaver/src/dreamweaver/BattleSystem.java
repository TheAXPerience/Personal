package dreamweaver;
import java.io.*;
import java.util.*;

public class BattleSystem {
	private Map<String, Character> characters;
	private Scanner input;
	
	public BattleSystem() throws FileNotFoundException {
		characters = new HashMap<String, Character>();
		input = new Scanner(System.in);
		Scanner scan = new Scanner(new FileReader("mc"));
		while(scan.hasNextLine()) {
			String name = scan.nextLine();
			characters.put(name, new Character(scan));
		}
		scan = new Scanner(new FileReader("tech")); //element | power | accuracy | critical | stagger | charge | speed | 0 = tech
		while(scan.hasNextLine()) {
			String name = scan.nextLine();
			characters.get(name).addTech(scan);
		}
		scan = new Scanner(new FileReader("spell")); //element | power | accuracy | critical | stagger | charge | speed | 1 = spell
		while(scan.hasNextLine()) {
			String name = scan.nextLine();
			characters.get(name).addTech(scan);
		}
	}
	
	//true if target is dead, otherwise false
	public boolean fight(String party1, String party2) {
		Character char1 = characters.get(party1);
		Character char2 = characters.get(party2);
		System.out.println(party1 + ": " + char1.getHP() + "/" + char1.findStat(0) + " HP (" +
				((int)(100.0 * char1.getHP() / char1.findStat(0))) + "%)");
		System.out.println(party2 + ": " + char2.getHP() + "/" + char2.findStat(0) + " HP (" +
				((int)(100.0 * char2.getHP() / char2.findStat(0))) + "%)");
		
		System.out.print("\n" + party1 + "'s attacks\n" + char1.attackInfo() + "\n" + party2 + "'s attacks\n" + char2.attackInfo());
		
		String insert = "";
		if(char1.findStat(7) >= char2.findStat(7)) {
			if(char1.isAway()) {
				insert = "skydiver";
			} else {
				System.out.println("\nActions: " + char1.actionsAvailable());
				System.out.print(party1 + "'s turn! Choose an attack: ");
				insert = input.nextLine().toLowerCase();
				while(!checkAttack(char1, insert) && !insert.equalsIgnoreCase("defend")) {
					System.out.print("Choose a different one: ");
					insert = input.nextLine().toLowerCase();
				}
			}
			if(char1.isDefending()) {
				char1.defending();
			}
			if(insert.equalsIgnoreCase("defend")) {
				System.out.println(party1 + " defended!");
				char1.defending();
				char1.attackRecover();
			} else {
				System.out.println(party1 + " used " + insert + "!");
				calculate(char1, char2, party1, party2, insert);
			}
			
			if(char1.getHP() > 0 && char2.getHP() > 0) {
				if(char2.isAway()) {
					insert = "skydiver";
				} else {
					System.out.println("\nActions: " + char2.actionsAvailable());
					System.out.print(party2 + "'s turn! Choose an attack: ");
					insert = input.nextLine().toLowerCase();
					while(!checkAttack(char2, insert) && !insert.equalsIgnoreCase("defend")) {
						System.out.print("Choose a different one: ");
						insert = input.nextLine().toLowerCase();
					}
				}
				if(char2.isDefending()) {
					char2.defending();
				}
				if(insert.equalsIgnoreCase("defend")) {
					System.out.println(party2 + " defended!");
					char2.defending();
					char2.attackRecover();
				} else {
					System.out.println(party2 + " used " + insert + "!");
					calculate(char2, char1, party2, party1, insert);
				}
			}
		} else {
			if(char2.isAway()) {
				insert = "skydiver";
			} else {
				System.out.println("\nActions: " + char2.actionsAvailable());
				System.out.print(party2 + "'s turn! Choose an attack: ");
				insert = input.nextLine().toLowerCase();
				while(!checkAttack(char2, insert) && !insert.equalsIgnoreCase("defend")) {
					System.out.print("Choose a different one: ");
					insert = input.nextLine().toLowerCase();
				}
			}
			if(char2.isDefending()) {
				char2.defending();
			}
			if(insert.equalsIgnoreCase("defend")) {
				System.out.println(party2 + " defended!");
				char2.defending();
				char2.attackRecover();
			} else {
				System.out.println(party2 + " used " + insert + "!");
				calculate(char2, char1, party2, party1, insert);
			}
			
			if(char1.getHP() > 0 && char2.getHP() > 0) {
				if(char1.isAway()) {
					insert = "skydiver";
				} else {
					System.out.println("\nActions: " + char1.actionsAvailable());
					System.out.print(party1 + "'s turn! Choose an attack: ");
					insert = input.nextLine().toLowerCase();
					while(!checkAttack(char1, insert) && !insert.equalsIgnoreCase("defend")) {
						System.out.print("Choose a different one: ");
						insert = input.nextLine().toLowerCase();
					}
				}
				if(char1.isDefending()) {
					char1.defending();
				}
				if(insert.equalsIgnoreCase("defend")) {
					System.out.println(party1 + " defended!");
					char1.defending();
					char1.attackRecover();
				} else {
					System.out.println(party1 + " used " + insert + "!");
					calculate(char1, char2, party1, party2, insert);
				}
			}
		}
		
		if(char1.getHP() < 0) {
			System.out.println(party1 + " has fallen!");
		} else if(char2.getHP() < 0) {
			System.out.println(party2 + " has fallen!");
		}
		return char1.getHP() < 0 || char2.getHP() < 0;
	}
	
	private void calculate(Character attack, Character defend, String attacker, String defender, String skill) {
		double[] move = attack.getTech(skill);
		int hitRate = (int)(move[2] + attack.findStat(5) - defend.findStat(6));
		if(hitRate > 100) {
			hitRate = 100;
		}
		int staggerRate = (int)(move[4] + attack.findStat(11) - defend.findStat(13));
		int criticalRate = (int)(move[3] + attack.findStat(10) - defend.findStat(12));
		attack.attackRecover();
		move[5] = 0.0;
		
		double damage = 0;
		if(move[7] == 0.0) {
			int attackStat = attack.findStat(1);
			int defendStat = defend.findStat(2);
			if((attacker.equalsIgnoreCase("Wolf") || attacker.equalsIgnoreCase("archer")) 
					&& attack.getHP() < (attack.findStat(0) * 2 / 3)) {
				attackStat *= 2;
				staggerRate = (int)(move[4] + (attack.findStat(11) * 1.5) - defend.findStat(13));
				criticalRate = (int)(move[3] + (attack.findStat(10) * 1.5) - defend.findStat(12));
				System.out.println("\t" + attacker + " seems powered up!");
			}
			if(defender.equalsIgnoreCase("Wolf") && defend.getHP() < (defend.findStat(0) / 2)) {
				defendStat *= 2;
				System.out.println("\twolf seems powered up!");
			}
			if(skill.equalsIgnoreCase("skydiver") && !attack.isAway()) {
				attack.disappear();
			} else if(defend.isAway()) {
					System.out.println("\tThe target wasn't there!");
			} else if(attack.isAway()) {
				damage = calculateDamage(attacker, ((attackStat + 373) * move[1]) - (defendStat + 373) / 2, 
						defend, hitRate, criticalRate, staggerRate, (int)move[0], skill);
				attack.disappear();
			} else {
				damage = calculateDamage(attacker, ((attackStat + 373) * move[1]) - (defendStat + 373), 
						defend, hitRate, criticalRate, staggerRate, (int)move[0], skill);
			}
		} else if(move[7] == 1.0) {
			int attackStat = attack.findStat(3);
			int defendStat = defend.findStat(4);
			if((attacker.equalsIgnoreCase("Wolf") || attacker.equalsIgnoreCase("Archer")) && 
					attack.getHP() < (attack.findStat(0) * 2 / 3)) {
				attackStat *= 2;
				staggerRate = (int)(move[4] + (attack.findStat(11) * 1.5) - defend.findStat(13));
				criticalRate = (int)(move[3] + (attack.findStat(10) * 1.5) - defend.findStat(12));
				System.out.println("\t" + attacker + " seems powered up!");
			}
			if(defender.equalsIgnoreCase("Wolf") && defend.getHP() < (defend.findStat(0) / 2)) {
				defendStat *= 2;
				System.out.println("\tWolf seems powered up!");
			}
			if(defend.isAway()) {
				System.out.println("\tThe target wasn't there!");
			} else {
				damage = calculateDamage(attacker, ((attackStat + 373) * move[1]) - (defendStat + 373), 
						defend, hitRate, criticalRate, staggerRate, (int)move[0], skill);
			}
		}
		
		if(skill.equalsIgnoreCase("healing light")) {
			damage = attack.findStat(0) * -3 / 10;
			attack.loseHP((int)damage);
			System.out.println("\t" + attacker + " healed " + (int)(-1 * damage) + " HP!\n\t" + 
					defender + "'s Stagger: " + defend.getStagger());
		} else {
			defend.loseHP((int)damage);
			System.out.println("\t" + defender + " lost " + (int)(damage) + " HP!");
			if(skill.equalsIgnoreCase("drain fang")) {
				attack.loseHP((int)(damage / -3));
				System.out.println("\t" + attacker + " healed " + (int)(damage / 3) + " HP!");
			}
			System.out.println("\tHit Rate: " + hitRate + "\n\t" + defender + "'s Stagger: "+
					defend.getStagger() + "\n\tCritical Hit Rate: " + criticalRate);
		}
		if(attack.getHP() > attack.findStat(0)) {
			attack.loseHP(attack.getHP());
			attack.loseHP(-1 * attack.findStat(0));
		}
	}
	
	private double calculateDamage(String attacker, double damage, Character defend, int hitRate, 
			int crit, int stagger, int element, String skill) {
		int hit = (int)(Math.random() * 100 + 1);
		if(hit > hitRate) {
			System.out.println("\t" + attacker + " almost missed!(% = " + hit + ")");
			damage = damage * (100 - (hit - hitRate)) / 100;
		}
		if(stagger < 1) {
			stagger = 1;
		}
		if(damage < 1) {
			damage = 1;
		}
		
		//critical hit rate, critical damage
		hit = (int)(Math.random() * 100 + 1);
		if(hit <= crit) {
			System.out.println("\tCritical hit!(% = " + hit + ")");
			damage *= 2.5;
			stagger *= 2;
		}
		
		//stagger or not
		if(defend.getStagger() >= 100) {
			System.out.print("\tTarget is staggered!");
			damage *= 2;
			if(skill.equalsIgnoreCase("Backstab") || skill.equalsIgnoreCase("fang")) {
				damage *= 1.5;
				System.out.print(" " + skill + " is powered up!");
			}
			System.out.println();
			defend.addStagger(-1 * defend.getStagger() - stagger);
		}
		
		//elemental damage
		damage *= defend.findResistance(element);
		
		if(defend.isDefending()) {
			damage /= 2;
			System.out.println("\tThe enemy guarded!");
		}
		
		defend.addStagger(stagger);
		return damage;
	}
	
	public boolean checkForCharacter(String name) {
		return characters.keySet().contains(name);
	}
	
	//true if technique is able to be used
	private boolean checkAttack(Character character, String move) {
		if(character.containsAttack(move)) {
			if((move.equalsIgnoreCase("attack")) || character.getTech(move)[5] >= 100) {
				return true;
			} else {
				System.out.println("Cannot use this attack yet.");
			}
		} else if(!move.equalsIgnoreCase("defend")) {
			System.out.println("This character does not have this move.");
		}
		return false;
	}
	
	public void refresh() {
		for(String s : characters.keySet()) {
			characters.get(s).reset();
		}
	}
}