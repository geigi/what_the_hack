import android.arch.persistence.room.Dao;
import android.arch.persistence.room.Query;

import java.util.List;

@Dao
public interface DaoAccess {
    @Query("SELECT * FROM timers")
    List<Timers> getAll();
}
