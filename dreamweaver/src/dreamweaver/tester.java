package dreamweaver;
import java.util.*;
import java.io.*;

public class tester {

	public static void main(String[] args) throws FileNotFoundException {
		//statsGenerator();
		
		
		BattleSystem fighter = new BattleSystem();
		Scanner input = new Scanner(System.in);
		boolean again = true;
		while(again) {
			System.out.print("Choose your character: ");
			String player1 = input.nextLine();
			while(!fighter.checkForCharacter(player1)) {
				System.out.print("This character doesn't exist! Choose another one: ");
				player1 = input.nextLine();
			}
			System.out.print("Choose your opponent: ");
			String player2 = input.nextLine();
			while(!fighter.checkForCharacter(player2)) {
				System.out.print("This character doesn't exist! Choose another one: ");
				player2 = input.nextLine();
			}
			int count = 1;
			System.out.println("Turn " + count);
			while(!fighter.fight(player1, player2)) {
				count++;
				System.out.println("\nTurn " + (count));
			}
			System.out.print("Wanna play again? N for no: " );
			String yes = input.nextLine();
			if(yes.equalsIgnoreCase("N")) {
				again = false;
			} else {
				fighter.refresh();
			}
		} 
	}
	
	public static void statsGenerator() {
		Scanner input = new Scanner(System.in);
		int[] bases = new int[7];
		String[] base = {"Vitality", "Strength", "Spirit", "Endurance", "Skill", "Agility", "Luck"};
		
		for(int i = 0; i < bases.length; i++) {
			System.out.print(base[i] + ": ");
			bases[i] = input.nextInt();
		}
		System.out.println("Total: " + sum(bases));
		System.out.println("HP: " + ((int)(99.99 * bases[0])));
		System.out.println("Physical Attack: " + ((int)(6.26 * bases[1])));
		System.out.println("Physical Defense: " + ((int)(6.26 * (bases[1] * 0.5 + bases[3] * 0.5))));
		System.out.println("Magic Attack: " + ((int)(6.26 * bases[2])));
		System.out.println("Magic Defense: " + ((int)(6.26 * (bases[2] * 0.5 + bases[3] * 0.5))));
		System.out.println("Accuracy: " + ((int)(40 + (bases[4] * 0.85 + bases[6] * 0.15) * 0.6)));
		System.out.println("Evasion: " + ((int)(40 + (bases[5] * 0.75 + bases[6] * 0.25) * 0.6)));
		System.out.println("Movement Speed: " + ((int)(bases[5])));
		System.out.println("Attack Speed: " + ((int)(bases[0] * 0.1 + bases[4] * 0.3 + bases[5] * 0.6)));
		System.out.println("Casting Speed: " + ((int)(bases[2] * 0.7 + bases[4] * 0.1 + bases[5] * 0.2)));
		System.out.println("Critical Hit Rate: " + ((int)(bases[4] * 0.75 + bases[5] * 0.2 + bases[6] * 0.05)));
		System.out.println("Critical Avoid: " + ((int)(bases[6])));
		System.out.println("Physical Stagger Rate: " + ((int)(bases[1] * 0.7 + bases[4] * 0.3)));
		System.out.println("Magic Stagger Rate: " + ((int)(bases[2] * 0.7 + bases[4] * 0.3)));
		System.out.println("Physical Stagger Avoid: " + ((int)(bases[0] * 0.2 + bases[1] * 0.3 + bases[3] * 0.4 + bases[6] * 0.1)));
		System.out.println("Magic Stagger Avoid: " + ((int)(bases[0] * 0.2 + bases[2] * 0.3 + bases[3] * 0.4 + bases[6] * 0.1)));
	}
	
	public static int sum(int[] add) {
		int total = 0;
		for(int i = 0; i < add.length; i++) {
			total += add[i];
		}
		return total;
	}
	
	public static void characterStatsGenerator() {
		Scanner input = new Scanner(System.in);
		String[] stats = {"HP", "Attack", "Defense", "Magic", "Resistance", "Accuracy", "Evasion", "Speed", "Attack Speed", "Charge Speed",
				"Critical", "Stagger", "Luck", "Endurance"};
		int[] numbers = new int[stats.length];
		
		for(int i = 0; i < numbers.length; i++) {
			System.out.print(stats[i] + " % : ");
			if(i == 0) {
				numbers[i] = (input.nextInt() - 50) * 3999 / 50 + 6099;
			} else if(i > 0 && i < 5) {
				numbers[i] = (input.nextInt() - 50) * 290 / 50 + 336;
			} else {
				numbers[i] = (input.nextInt() - 50) * 4 / 5 + 60;
			}
		}
		
		System.out.println("\nResults!");
		for(int i = 0; i < numbers.length; i++) {
			System.out.println(stats[i] + ": " + numbers[i]);
		}
	}

}
