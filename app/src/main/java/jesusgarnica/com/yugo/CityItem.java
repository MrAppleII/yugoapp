package jesusgarnica.com.yugo;

/**
 * Created by Jesus on 3/5/2018.
 */

public class CityItem {
    private String cityName;
    private String cityImage;

    private int id;
    public CityItem(){


    }
    public void setId(int id){
        this.id = id;
    }
    public int returnId(){
        return id;
    }
    public void setCityName(String cityName){
        this.cityName = cityName;
    }
    public String getCityName(){
        return cityName;
    }
    public void setCityImage(String picture){
        cityImage = picture;
    }
    public String returnCityImage(){
        return cityImage;
    }

}
