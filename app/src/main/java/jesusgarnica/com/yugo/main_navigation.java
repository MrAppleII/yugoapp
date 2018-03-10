package jesusgarnica.com.yugo;

import android.content.DialogInterface;
import android.net.Uri;
import android.nfc.Tag;
import android.os.Bundle;
import android.support.annotation.NonNull;
import android.support.design.widget.BottomNavigationView;

import android.support.v4.app.Fragment;
import android.support.v4.app.FragmentTransaction;
import android.support.v7.app.AppCompatActivity;
import android.support.v7.widget.Toolbar;
import android.util.Log;
import android.view.LayoutInflater;
import android.view.Menu;
import android.view.MenuItem;
import android.support.v7.app.ActionBar;
import android.widget.TextView;
import android.widget.Toast;


public class main_navigation extends AppCompatActivity implements destination_select.OnFragmentInteractionListener,user_inbox.OnFragmentInteractionListener {

    private TextView mTextMessage;

    private Toolbar navigationToolbar;
    /*
    Our Navigation Bar Information

     */
    private BottomNavigationView.OnNavigationItemSelectedListener mOnNavigationItemSelectedListener
            = new BottomNavigationView.OnNavigationItemSelectedListener() {

        @Override
        public boolean onNavigationItemSelected(@NonNull MenuItem item) {
            Fragment fragment;
            switch (item.getItemId()) {
                case R.id.navigation_home:
                    fragment = new destination_select();
                    loadFragment(fragment);
                    navigationToolbar.setTitle(R.string.title_home);
                    return true;
                case R.id.navigation_dashboard:
                    navigationToolbar.setTitle(R.string.title_dashboard);
                    return true;
                case R.id.navigation_notifications:
                    fragment = new user_inbox();
                    loadFragment(fragment);
                    navigationToolbar.setTitle(R.string.title_notifications);
                    return true;
            }
            return false;
        }
    };

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        setTitle("Yugo");

        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main_navigation);



        navigationToolbar = (Toolbar) findViewById(R.id.navigation_toolbar);
        setSupportActionBar(navigationToolbar);


        BottomNavigationView navigation = (BottomNavigationView) findViewById(R.id.navigation_bottom);
        navigation.setOnNavigationItemSelectedListener(mOnNavigationItemSelectedListener);
        loadFragment(new destination_select());
    }

    private void loadFragment(Fragment fragment) {
        // load fragment
        FragmentTransaction transaction = getSupportFragmentManager().beginTransaction();
        transaction.replace(R.id.frame_container, fragment);
        transaction.addToBackStack(null);
        transaction.commit();
    }

    /*
    Toolbar at the top
     */
    @Override
    public boolean onCreateOptionsMenu(Menu menu) {
        // Inflate the menu; this adds items to the action bar if it is present.
        getMenuInflater().inflate(R.menu.navigavation_main_toolbar, menu);

        return true;
    }



    @Override
    public boolean onOptionsItemSelected(MenuItem item) {
        // Handle action bar item clicks here. The action bar will
        // automatically handle clicks on the Home/Up button, so long
        // as you specify a parent activity in AndroidManifest.xml.
        int id = item.getItemId();

        //noinspection SimplifiableIfStatement
        if (id == R.id.logout_text) {
            Toast.makeText(main_navigation.this, "Action clicked", Toast.LENGTH_LONG).show();
            return true;
        }

        return super.onOptionsItemSelected(item);
    }
    @Override
    public void onFragmentInteraction(Uri uri){
        //you can leave it empty
    }

}
