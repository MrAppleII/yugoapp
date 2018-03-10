package jesusgarnica.com.yugo;

import android.content.Intent;
import android.support.v7.app.AppCompatActivity;
import android.os.Bundle;
import android.view.View;
import android.widget.Button;

public class SignUp extends AppCompatActivity {

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_sign_up);

        Button currentButton = (Button) this.findViewById(R.id.nextButton);


        Intent i = getIntent();
        currentButton.setOnClickListener(new View.OnClickListener() {

            public void onClick(View arg0) {

                Intent nextScreen = new Intent(getApplicationContext(), make_profile.class); //this is us going to the next screen

                startActivity(nextScreen);

            }
        });
    }
}
