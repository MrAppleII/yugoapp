package jesusgarnica.com.yugo;

import android.content.Context;
import android.content.DialogInterface;
import android.net.Uri;
import android.os.Bundle;
import android.support.v4.app.Fragment;
import android.support.v7.app.AlertDialog;
import android.text.Layout;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.AdapterView;
import android.widget.ArrayAdapter;
import android.widget.Button;
import android.widget.Spinner;
import android.widget.TextView;
import android.widget.Toast;

import java.util.ArrayList;
import java.util.List;

import jesusgarnica.com.yugo.adapter.cityListAdapter;
import jesusgarnica.com.yugo.network.cityInterface;
import jesusgarnica.com.yugo.network.cityListClient;
import jesusgarnica.com.yugo.network.messageClient;
import jesusgarnica.com.yugo.network.messageInterface;
import retrofit2.Call;
import retrofit2.Callback;
import retrofit2.Response;


/**
 * A simple {@link Fragment} subclass.
 * Activities that contain this fragment must implement the
 * {@link destination_select.OnFragmentInteractionListener} interface
 * to handle interaction events.
 * Use the {@link destination_select#newInstance} factory method to
 * create an instance of this fragment.
 */
public class destination_select extends Fragment {
    // TODO: Rename parameter arguments, choose names that match
    // the fragment initialization parameters, e.g. ARG_ITEM_NUMBER
    private static final String ARG_PARAM1 = "param1";
    private static final String ARG_PARAM2 = "param2";
    private Button daysSelect;
    private Button budgetSelect;
    private Button travelersSelect;
    private Integer travalersTotal;
    private Integer budgetTotal;
    //CITIES LIST VARIABLES
    private List<CityItem> cities = new ArrayList<>();
    private cityListAdapter cAdapter;


    private Integer daysForTrip = 0;
    // TODO: Rename and change types of parameters
    private String mParam1;
    private String mParam2;

    private OnFragmentInteractionListener mListener;

    public destination_select() {
        // Required empty public constructor
    }

    /**
     * Use this factory method to create a new instance of
     * this fragment using the provided parameters.
     *
     * @param param1 Parameter 1.
     * @param param2 Parameter 2.
     * @return A new instance of fragment destination_select.
     */
    // TODO: Rename and change types and number of parameters
    public static destination_select newInstance(String param1, String param2) {
        destination_select fragment = new destination_select();
        Bundle args = new Bundle();
        args.putString(ARG_PARAM1, param1);
        args.putString(ARG_PARAM2, param2);
        fragment.setArguments(args);
        return fragment;
    }

    @Override
    public void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        if (getArguments() != null) {
            mParam1 = getArguments().getString(ARG_PARAM1);
            mParam2 = getArguments().getString(ARG_PARAM2);
        }
        // TODO  FIX BUTTONS final Button daysSelect = (Button) findViewById;



    }

    @Override
    public View onCreateView(LayoutInflater inflater, ViewGroup container,
                             Bundle savedInstanceState) {
        // Inflate the layout for this fragment
        final View destinationView = inflater.inflate(R.layout.fragment_destination_select, container, false);
        daysSelect = destinationView.findViewById(R.id.daysButton);
        budgetSelect = destinationView.findViewById(R.id.budgetButton);
        travelersSelect = destinationView.findViewById(R.id.travalersButton);
        /* Lets listen for the user to pick how many days*/
        daysSelect.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                AlertDialog.Builder builder = new AlertDialog.Builder(destinationView.getContext());
                builder.setTitle("How many days are you staying?");
                builder.setPositiveButton("Continue", new DialogInterface.OnClickListener() {
                    @Override
                    public void onClick(DialogInterface dialog, int which) {
                        daysSelect.setText("Days: "+daysForTrip);

                        dialog.dismiss();
                    }
                });
                LayoutInflater inflater = getLayoutInflater();
                View dialoglayout = inflater.inflate(R.layout.days_prompt, null);

                final Spinner daysSpinner = (Spinner) dialoglayout.findViewById(R.id.days_spinner);
                Integer[] daysNumbers = new Integer[]{1,2,3,4,5,6,7,8,9,10};
                ArrayAdapter<Integer> adapter = new ArrayAdapter<Integer>(getActivity().getApplicationContext(),android.R.layout.simple_spinner_dropdown_item, daysNumbers);
                daysSpinner.setAdapter(adapter);
                daysSpinner.setOnItemSelectedListener(new AdapterView.OnItemSelectedListener() {

                    @Override
                    public void onItemSelected(AdapterView<?> arg0, View arg1,
                                               int arg2, long arg3) {
                        daysForTrip =(Integer)daysSpinner.getSelectedItem();

                    }

                    @Override
                    public void onNothingSelected(AdapterView<?> arg0) {
                        // TODO Auto-generated method stub

                    }
                });

                builder.setView(dialoglayout);

                builder.show();

            }
        });
        budgetSelect.setOnClickListener(new View.OnClickListener(){


            @Override
            public void onClick(View views){
                AlertDialog.Builder builder = new AlertDialog.Builder(destinationView.getContext());

                builder.setTitle("What is your budget?");

                builder.setPositiveButton("Continue", new DialogInterface.OnClickListener() {
                    @Override
                    public void onClick(DialogInterface dialog, int which) {
                        budgetSelect.setText("Budget "+budgetTotal);

                        dialog.dismiss();
                    }
                });
                LayoutInflater inflater = getLayoutInflater();
                View dialoglayout = inflater.inflate(R.layout.days_prompt, null);
                TextView dpTV = dialoglayout.findViewById(R.id.daysPrompt_TV);
                dpTV.setText("Budget: ");

                final Spinner daysSpinner = (Spinner) dialoglayout.findViewById(R.id.days_spinner);
                Integer[] daysNumbers = new Integer[]{100,500,1000,2000,10000};
                ArrayAdapter<Integer> adapter = new ArrayAdapter<Integer>(getActivity().getApplicationContext(),android.R.layout.simple_spinner_dropdown_item, daysNumbers);
                daysSpinner.setAdapter(adapter);
                daysSpinner.setOnItemSelectedListener(new AdapterView.OnItemSelectedListener() {

                    @Override
                    public void onItemSelected(AdapterView<?> arg0, View arg1,
                                               int arg2, long arg3) {
                        budgetTotal =(Integer)daysSpinner.getSelectedItem();

                    }

                    @Override
                    public void onNothingSelected(AdapterView<?> arg0) {
                        // TODO Auto-generated method stub

                    }
                });

                builder.setView(dialoglayout);

                builder.show();

            }
        });
        travelersSelect.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                AlertDialog.Builder builder = new AlertDialog.Builder(destinationView.getContext());
                builder.setTitle("How many travalers?");
                builder.setPositiveButton("Continue", new DialogInterface.OnClickListener() {
                    @Override
                    public void onClick(DialogInterface dialog, int which) {
                        travelersSelect.setText("Travalers: "+travalersTotal);

                        dialog.dismiss();
                    }
                });
                LayoutInflater inflater = getLayoutInflater();
                View dialoglayout = inflater.inflate(R.layout.days_prompt, null);
                TextView dpTV = dialoglayout.findViewById(R.id.daysPrompt_TV);
                dpTV.setText("Travelers: ");
                final Spinner daysSpinner = (Spinner) dialoglayout.findViewById(R.id.days_spinner);
                Integer[] daysNumbers = new Integer[]{1,2,3,4,5};
                ArrayAdapter<Integer> adapter = new ArrayAdapter<Integer>(getActivity().getApplicationContext(),android.R.layout.simple_spinner_dropdown_item, daysNumbers);
                daysSpinner.setAdapter(adapter);
                daysSpinner.setOnItemSelectedListener(new AdapterView.OnItemSelectedListener() {

                    @Override
                    public void onItemSelected(AdapterView<?> arg0, View arg1,
                                               int arg2, long arg3) {
                        travalersTotal =(Integer)daysSpinner.getSelectedItem();

                    }

                    @Override
                    public void onNothingSelected(AdapterView<?> arg0) {
                        // TODO Auto-generated method stub

                    }
                });

                builder.setView(dialoglayout);

                builder.show();


            }
        });
        /*
        Ending code for the buttons

         */
        return destinationView;
    }
    private void getCities() {

        cityInterface apiService =
                cityListClient.getClient().create(cityInterface.class);

        Call<List<CityItem>> call = apiService.getCityList();
        call.enqueue(new Callback<List<CityItem>>() {
            @Override
            public void onResponse(Call<List<CityItem>> call, Response<List<CityItem>> response) {
                // clear the inbox
                cities.clear();

                // add all the messages
                // messages.addAll(response.body());

                // TODO - avoid looping
                // the loop was performed to add colors to each message
                for (CityItem city : response.body()) {
                    // generate a random color
                    cities.add(city);
                }

                cAdapter.notifyDataSetChanged();

            }

            @Override
            public void onFailure(Call<List<CityItem>> call, Throwable t) {
                Toast.makeText(getView().getContext(), "Unable to fetch json: " + t.getMessage(), Toast.LENGTH_LONG).show();

            }
        });
    }
    // TODO: Rename method, update argument and hook method into UI event
    public void onButtonPressed(Uri uri) {
        if (mListener != null) {
            mListener.onFragmentInteraction(uri);
        }
    }

    @Override
    public void onAttach(Context context) {
        super.onAttach(context);
        if (context instanceof OnFragmentInteractionListener) {
            mListener = (OnFragmentInteractionListener) context;
        } else {
            throw new RuntimeException(context.toString()
                    + " must implement OnFragmentInteractionListener");
        }
    }

    @Override
    public void onDetach() {
        super.onDetach();
        mListener = null;
    }

    /**
     * This interface must be implemented by activities that contain this
     * fragment to allow an interaction in this fragment to be communicated
     * to the activity and potentially other fragments contained in that
     * activity.
     * <p>
     * See the Android Training lesson <a href=
     * "http://developer.android.com/training/basics/fragments/communicating.html"
     * >Communicating with Other Fragments</a> for more information.
     */
    public interface OnFragmentInteractionListener {
        // TODO: Update argument type and name
        void onFragmentInteraction(Uri uri);
    }
}
