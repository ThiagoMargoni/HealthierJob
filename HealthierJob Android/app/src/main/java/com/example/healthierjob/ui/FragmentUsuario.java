package com.example.healthierjob.ui;

import android.content.Intent;
import android.content.SharedPreferences;
import android.os.Bundle;
import android.preference.PreferenceManager;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.ImageView;
import android.widget.TextView;
import android.widget.Toast;

import androidx.fragment.app.Fragment;

import com.android.volley.Request;
import com.android.volley.RequestQueue;
import com.android.volley.Response;
import com.android.volley.VolleyError;
import com.android.volley.toolbox.JsonArrayRequest;
import com.android.volley.toolbox.Volley;
import com.example.healthierjob.R;

import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;

/**
 * A simple {@link Fragment} subclass.
 * Use the {@link FragmentUsuario#newInstance} factory method to
 * create an instance of this fragment.
 */
public class FragmentUsuario extends Fragment {

    View view;

    // TODO: Rename parameter arguments, choose names that match
    // the fragment initialization parameters, e.g. ARG_ITEM_NUMBER
    private static final String ARG_PARAM1 = "param1";
    private static final String ARG_PARAM2 = "param2";

    // TODO: Rename and change types of parameters
    private String mParam1;
    private String mParam2;

    public FragmentUsuario() {
        // Required empty public constructor
    }

    /**
     * Use this factory method to create a new instance of
     * this fragment using the provided parameters.
     *
     * @param param1 Parameter 1.
     * @param param2 Parameter 2.
     * @return A new instance of fragment FragmentUsuario.
     */
    // TODO: Rename and change types and number of parameters
    public static FragmentUsuario newInstance(String param1, String param2) {
        FragmentUsuario fragment = new FragmentUsuario();
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
    }

    ImageView img;
    TextView codigo, nome, idade, dataE, areaAtua;

    @Override
    public View onCreateView(LayoutInflater inflater, ViewGroup container,
                             Bundle savedInstanceState) {
        // Inflate the layout for this fragment
        view = inflater.inflate(R.layout.fragment_usuario, container, false);

        img = view.findViewById(R.id.img_usuario);
        codigo = view.findViewById(R.id.codigo_usuario);
        nome = view.findViewById(R.id.nome_usuario);
        idade = view.findViewById(R.id.idade_usuario);
        dataE = view.findViewById(R.id.dataE);
        areaAtua = view.findViewById(R.id.areaAtua);

        SharedPreferences prefs = PreferenceManager.getDefaultSharedPreferences(view.getContext());
        String codigoUsu = prefs.getString("codigo", "");

        RequestQueue solicitacao = Volley.newRequestQueue(getContext());
        String url = "http://10.0.2.2/api/Usuario/" + codigoUsu;

        JsonArrayRequest envio = new JsonArrayRequest(Request.Method.GET,
                url, null,
                new Response.Listener<JSONArray>() {
                    @Override
                    public void onResponse(JSONArray response) {
                        if(response != null) {

                            try {
                                JSONObject ob = response.getJSONObject(0);

                                codigo.setText(ob.getString("codigo"));
                                nome.setText(ob.getString("nomeCompleto"));
                                idade.setText(ob.getInt("idade"));
                                dataE.setText(ob.getString("dataEntrada"));
                                areaAtua.setText(ob.getString("areaAtua"));
                                img.setImageResource(Integer.parseInt(ob.getString("img")));

                            } catch (JSONException e) {
                                e.printStackTrace();
                            }

                        } else {
                            Toast.makeText(getContext(), "Não foi Possível carregar os dados", Toast.LENGTH_LONG).show();
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

        return view;
    }
}