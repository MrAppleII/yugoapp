package jesusgarnica.com.yugo.network;

/**
 * Created by Jesus on 2/26/2018.
 */

import java.util.List;

import jesusgarnica.com.yugo.Message;
import retrofit2.Call;
import retrofit2.http.GET;


public interface messageInterface {

    @GET("inbox.json") //this is the part at the end of the URL that gets changed. When you call the method, this gets passed in
    Call<List<Message>> getInbox();
}
