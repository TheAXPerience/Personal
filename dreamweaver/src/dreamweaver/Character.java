package dreamweaver;
import java.util.*;

public class Character {
	private int[] stats;
	private double[] elements; //fire, water, wind, earth
	private int currentHP;
	private int stagger;
	private String status;
	private boolean defend;
	private boolean away;
	private Map<String, double[]> attacks;
	
	public Character(Scanner in) {
		stats = new int[14];
		setStats(in);
		elements = new double[5];
		setElements(in);
		attacks = new HashMap<String, double[]>();
		reset();
		in.nextLine();
	}
	
	private void setStats(Scanner in) {
		for(int i = 0; i < stats.length; i++) {
			stats[i] = in.nextInt();
		}
	}
	
	private void setElements(Scanner in) {
		for(int i = 0; i < elements.length; i++) {
			elements[i] = in.nextDouble();
		}
	}
	
	public void addTech(Scanner in) {
		String name = in.nextLine();
		double[] parameters = new double[8];
		for(int i = 0; i < parameters.length; i++) {
			parameters[i] = in.nextDouble();
		}
		attacks.put(name, parameters);
		in.nextLine();
	}
	
	public void reset() {
		currentHP = stats[0];
		stagger = 0;
		status = "normal";
		away = false;
		defend = false;
		for(String s : attacks.keySet()) {
			attacks.get(s)[5] = 0;
		}
	}
	
	public int findStat(int stat) {
		return stats[stat];
	}
	
	public double findResistance(int element) {
		return elements[element];
	}
	
	public double[] getTech(String name) {
		return attacks.get(name);
	}
	
	public void attackRecover() {
		for(String s : attacks.keySet()) {
			double[] t = attacks.get(s);
			t[5] += (t[6] * stats[8] / 100);
			if(t[5] > 100) {
				t[5] = 100;
			}
		}
	}
	
	public void loseHP(int damage) {
		currentHP -= damage;
	}
	
	public int getHP() {
		return currentHP;
	}
	
	public int getStagger() {
		return stagger;
	}
	
	public void addStagger(int stag) {
		stagger += stag;
	}
	
	public boolean containsAttack(String name) {
		return attacks.keySet().contains(name);
	}
	
	public String actionsAvailable() {
		String r = "attack";
		for(String s : attacks.keySet()) {
			if(attacks.get(s)[5] >= 100) {
				r += ", " + s;
			}
		}
		return r + ", defend";
	}
	
	public String attackInfo() {
		String r = "";
		for(String s : attacks.keySet()) {
			double[] t = attacks.get(s);
			r += s + ":     " + t[1] + " POW     " + (int)(t[2]) + " ACC     " + (int)(t[3]) + " CRIT     " +
			(int)(t[4]) + " STAG     " + (int)(t[5]) + " CHARGE";
			if(t[7] == 0) {
				r += "     TECH";
			} else {
				r += "     SPELL";
			}
			r += "\n";
		}
		return r;
	}
	
	public String getStatus() {
		return status;
	}
	
	public void changeStatus(String other) {
		status = other;
	}
	
	public boolean isDefending() {
		return defend;
	}
	
	public void defending() {
		defend = !defend;
	}
	
	public boolean isAway() {
		return away;
	}
	
	public void disappear() {
		away = !away;
	}
}
