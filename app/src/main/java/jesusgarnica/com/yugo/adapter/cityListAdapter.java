package jesusgarnica.com.yugo.adapter;

import android.content.Context;
import android.support.v7.widget.RecyclerView;
import android.util.SparseBooleanArray;
import android.view.HapticFeedbackConstants;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.FrameLayout;
import android.widget.ImageView;
import android.widget.TextView;

import java.util.List;

import jesusgarnica.com.yugo.CityItem;
import jesusgarnica.com.yugo.R;


/**
 * Created by Jesus on 3/5/2018.
 */

public class cityListAdapter extends RecyclerView.Adapter<cityListAdapter.MyCityViewHolder>  {
    private Context cityContext;
    private List<CityItem> cityList;
    private cityListAdapter.cityAdapterListener listener;
    private SparseBooleanArray selectedItems;

    // array used to perform multiple animation at once
    private SparseBooleanArray animationItemsIndex;
    private boolean reverseAllAnimations = false;

    // index is used to animate only the selected row
    // dirty fix, find a better solution
    private static int currentSelectedIndex = -1;


    @Override
    public MyCityViewHolder onCreateViewHolder(ViewGroup parent, int viewType) {


        View itemView = LayoutInflater.from(parent.getContext())
                .inflate(R.layout.message_list_rows, parent, false);

        return new MyCityViewHolder(itemView);
    }

    @Override
    public void onBindViewHolder(MyCityViewHolder holder, int position) {
        CityItem city = cityList.get(position);

        // displaying text view data
        holder.cityName.setText(city.getCityName());


        // change the row state to activated
        holder.itemView.setActivated(selectedItems.get(position, false));


        // display profile image
        applyBackgroundImage(holder, city);

        // apply click events
        applyClickEvents(holder, position);
    }
    private void applyBackgroundImage(MyCityViewHolder h, CityItem city){

    }

    @Override
    public int getItemCount() {
        return 0;
    }

    public class MyCityViewHolder extends RecyclerView.ViewHolder implements View.OnClickListener {
        public TextView cityName;
        public ImageView cityBackground;
        public FrameLayout mainCitySelectContainer;

        public MyCityViewHolder(View view){
            super(view);
            cityName =(TextView) view.findViewById(R.id.city_name_TV);
            cityBackground = (ImageView)view.findViewById(R.id.city_image);
            mainCitySelectContainer = (FrameLayout)view.findViewById(R.id.city_container);
            view.setOnClickListener(this);

        }

        @Override
        public void onClick(View v) {
            listener.onCityClicked(getAdapterPosition());
            v.performHapticFeedback(HapticFeedbackConstants.KEYBOARD_TAP);

        }
    }


    public cityListAdapter(Context mContext, List<CityItem> cities, cityAdapterListener listener) {
        this.cityContext = mContext;
        this.cityList = cities;
        this.listener = listener;
        selectedItems = new SparseBooleanArray();
        animationItemsIndex = new SparseBooleanArray();
    }


    public interface cityAdapterListener {

        void onCityClicked(int position);


    }



}
