package jesusgarnica.com.yugo;

import android.content.Intent;
import android.graphics.drawable.AnimationDrawable;
import android.support.v7.app.AppCompatActivity;
import android.os.Bundle;
import android.support.v7.widget.Toolbar;
import android.view.View;
import android.view.Window;
import android.widget.Button;
import android.widget.RelativeLayout;

public class WelcomeScreen extends AppCompatActivity {

    private Button signInButton;
    @Override
    protected void onCreate(Bundle savedInstanceState) {

        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_welcome_screen);

        signInButton = (Button) findViewById(R.id.SignUp_button);


        RelativeLayout welcomeLayout = (RelativeLayout) findViewById(R.id.welcome_screenLayout);
        AnimationDrawable animationDrawable = (AnimationDrawable) welcomeLayout.getBackground();
        animationDrawable.setEnterFadeDuration(2000);
        animationDrawable.setExitFadeDuration(4000);
        animationDrawable.start();

        signInButton.setOnClickListener(new View.OnClickListener() {

            public void onClick(View arg0) {
                Intent nextScreen = new Intent(getApplicationContext(), SignUp.class); //this is us going to the next screen


                startActivity(nextScreen);


            }
        });
    }
}
