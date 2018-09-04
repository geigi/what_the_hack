import android.arch.persistence.room.Entity;
import android.arch.persistence.room.PrimaryKey;

@Entity
public class Timers {
    @PrimaryKey
    private int uid;

    private int time_saved;

    private int delay;

    private int message;

    public int getUid() {
        return uid;
    }

    public void setUid(int uid) {
        this.uid = uid;
    }

    public int getTime_saved() {
        return time_saved;
    }

    public void setTime_saved(int time_saved) {
        this.time_saved = time_saved;
    }

    public int getDelay() {
        return delay;
    }

    public void setDelay(int delay) {
        this.delay = delay;
    }

    public int getMessage() {
        return message;
    }

    public void setMessage(int message) {
        this.message = message;
    }
}
