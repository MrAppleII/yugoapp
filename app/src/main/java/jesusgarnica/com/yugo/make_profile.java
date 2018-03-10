package jesusgarnica.com.yugo;

import android.content.Intent;
import android.support.v7.app.AppCompatActivity;
import android.os.Bundle;
import android.util.Log;
import android.view.View;
import android.widget.ArrayAdapter;
import android.widget.Button;
import android.widget.Spinner;

public class make_profile extends AppCompatActivity {

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_make_profile);

        Button finishbutton2 = (Button) this.findViewById(R.id.finish_button);
        Spinner genderSpinner = (Spinner) findViewById(R.id.gender_select);


        // Create an ArrayAdapter using the string array and a default spinner
        ArrayAdapter<CharSequence> staticAdapter = ArrayAdapter
                .createFromResource(this, R.array.gender_array,
                        android.R.layout.simple_spinner_item);

        // Specify the layout to use when the list of choices appears
        staticAdapter
                .setDropDownViewResource(android.R.layout.simple_spinner_dropdown_item);
        genderSpinner.setAdapter(staticAdapter);
        // applying the adapter to spinner
        Intent i = getIntent();
        finishbutton2.setOnClickListener(new View.OnClickListener() {

            public void onClick(View arg0) {

                Intent nextScreen = new Intent(getApplicationContext(), main_navigation.class); //this is us going to the next screen



                Log.i("n", "Next screen");

                startActivity(nextScreen);
                finishAffinity();


            }
        });


    }
}
