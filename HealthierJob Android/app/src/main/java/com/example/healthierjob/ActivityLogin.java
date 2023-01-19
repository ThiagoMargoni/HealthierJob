package com.example.healthierjob;

import android.content.Intent;
import android.content.SharedPreferences;
import android.os.Bundle;
import android.preference.PreferenceManager;
import android.view.View;
import android.widget.Button;
import android.widget.EditText;
import android.widget.Toast;

import androidx.appcompat.app.AppCompatActivity;

import com.android.volley.Request;
import com.android.volley.RequestQueue;
import com.android.volley.Response;
import com.android.volley.VolleyError;
import com.android.volley.toolbox.JsonArrayRequest;
import com.android.volley.toolbox.JsonObjectRequest;
import com.android.volley.toolbox.Volley;
import com.example.healthierjob.MainActivity;
import com.example.healthierjob.R;

import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;

public class ActivityLogin extends AppCompatActivity {

    Button b;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_login);

        Button b = findViewById(R.id.btEntrar);

        b.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {

                EditText codigo = findViewById(R.id.tCodigo);
                EditText senha = findViewById(R.id.tSenha);

                String c = String.valueOf(codigo.getText()).replaceAll(" ", "");
                String s = String.valueOf(senha.getText()).replaceAll(" ", "");

                if(!(c.isEmpty() && s.isEmpty())) {

                    RequestQueue solicitacao = Volley.newRequestQueue(getContext());
                    String url = "http://10.0.2.2/api/Usuario/" + c + "/" + s;

                    JsonArrayRequest envio = new JsonArrayRequest(Request.Method.GET,
                        url, null,
                    new Response.Listener<JSONArray>() {
                        @Override
                        public void onResponse(JSONArray response) {
                            if(response != null) {

                                try {
                                    
                                    SharedPreferences preferences = PreferenceManager.getDefaultSharedPreferences(this);
                                    SharedPreferences.Editor editor = preferences.edit();
                                    editor.putString("c", c);
                                    editor.apply();

                                    Intent it = new Intent(getApplicationContext(), MainActivity.class);

                                    startActivity(it);

                                } catch (JSONException e) {
                                    e.printStackTrace();
                                }   

                            } else {
                                Toast.makeText(getApplicationContext(), "Usuário ou Senha Incorretos", Toast.LENGTH_LONG).show();
                            }
                        }
                    }, new Response.ErrorListener() {
                        @Override
                        public void onErrorResponse(VolleyError error) {
                            error.printStackTrace();
                            Toast.makeText(getContext(), String.valueOf(error), Toast.LENGTH_SHORT).show();
                        }
                    }
                    );

                    solicitacao.add(envio);
                } else {
                    Toast.makeText(getApplicationContext(), "Favor Não deixar Campos Vazios", Toast.LENGTH_LONG).show();
                }
            }
        });
    }
}