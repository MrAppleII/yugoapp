package jesusgarnica.com.yugo.network;

import java.util.List;

import jesusgarnica.com.yugo.CityItem;
import retrofit2.Call;
import retrofit2.http.GET;

/**
 * Created by Jesus on 3/5/2018.
 */

public interface cityInterface {

    @GET("inbox.json") //this is onlt for testing purposes, link will be updated soon
    Call<List<CityItem>> getCityList();
}
