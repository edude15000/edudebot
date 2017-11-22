import com.google.gson.annotations.Expose;

public class BotUser {
	@Expose(serialize = true, deserialize = true)
	String username, rank;
	@Expose(serialize = true, deserialize = true)
	boolean follower, sub, mod, gaveSpot = false;
	@Expose(serialize = true, deserialize = true)
	int time, subCredits, points, numRequests;
	@Expose(serialize = true, deserialize = true)
	long gambleCoolDown, vipCoolDown;

	public BotUser(String username, int numRequests, boolean mod, boolean follower, boolean regular, int points,
			int time, String rank, long vipCoolDown, long gambleCoolDown, int subCredits) {
		this.username = username;
		this.numRequests = numRequests;
		this.mod = mod;
		this.follower = follower;
		this.sub = regular;
		this.points = points;
		this.time = time;
		this.rank = rank;
		this.vipCoolDown = vipCoolDown;
		this.gambleCoolDown = gambleCoolDown;
		this.subCredits = subCredits;
	}

}