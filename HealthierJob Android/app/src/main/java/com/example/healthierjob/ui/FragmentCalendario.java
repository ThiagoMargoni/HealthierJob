package com.example.healthierjob.ui;

import android.app.AlertDialog;
import android.app.DatePickerDialog;
import android.content.Intent;
import android.content.SharedPreferences;
import android.graphics.Color;
import android.graphics.drawable.ColorDrawable;
import android.os.Bundle;
import android.preference.PreferenceManager;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.Button;
import android.widget.DatePicker;
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

import java.util.Calendar;

/**
 * A simple {@link Fragment} subclass.
 * Use the {@link FragmentCalendario#newInstance} factory method to
 * create an instance of this fragment.
 */
public class FragmentCalendario extends Fragment {

    View view;

    // TODO: Rename parameter arguments, choose names that match
    // the fragment initialization parameters, e.g. ARG_ITEM_NUMBER
    private static final String ARG_PARAM1 = "param1";
    private static final String ARG_PARAM2 = "param2";

    // TODO: Rename and change types of parameters
    private String mParam1;
    private String mParam2;

    public FragmentCalendario() {
        // Required empty public constructor
    }

    /**
     * Use this factory method to create a new instance of
     * this fragment using the provided parameters.
     *
     * @param param1 Parameter 1.
     * @param param2 Parameter 2.
     * @return A new instance of fragment Calendario.
     */
    // TODO: Rename and change types and number of parameters
    public static FragmentCalendario newInstance(String param1, String param2) {
        FragmentCalendario fragment = new FragmentCalendario();
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

    private TextView botao;
    private TextView tv1, tv2, tv3, qtdPassos, recomendacao;
    private ImageView img, pesquisar;
    private DatePickerDialog.OnDateSetListener dpd;

    @Override
    public View onCreateView(LayoutInflater inflater, ViewGroup container,
                             Bundle savedInstanceState) {
        // Inflate the layout for this fragment
        view = inflater.inflate(R.layout.fragment_calendario, container, false);

        SharedPreferences prefs = PreferenceManager.getDefaultSharedPreferences(view.getContext());
        String codigo = prefs.getString("codigo", "");

        tv1 = view.findViewById(R.id.tv1);
        tv2 = view.findViewById(R.id.tv2);
        tv3 = view.findViewById(R.id.tv3);
        qtdPassos = view.findViewById(R.id.qtdPassos);
        recomendacao = view.findViewById(R.id.recomendacao);
        img = view.findViewById(R.id.img);

        tv1.setVisibility(View.INVISIBLE);
        tv2.setVisibility(View.INVISIBLE);
        tv3.setVisibility(View.INVISIBLE);
        qtdPassos.setVisibility(View.INVISIBLE);
        recomendacao.setVisibility(View.INVISIBLE);
        img.setVisibility(View.INVISIBLE);

        botao = view.findViewById(R.id.data);

        botao.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                Calendar calendario = Calendar.getInstance();
                int ano = calendario.get(Calendar.YEAR);
                int mes = calendario.get(Calendar.MONTH);
                int dia = calendario.get(Calendar.DAY_OF_MONTH);

                DatePickerDialog dp = new DatePickerDialog(getContext(),
                        androidx.appcompat.R.style.Theme_AppCompat_Light_Dialog,
                        dpd,
                        ano,mes,dia);
                dp.getWindow().setBackgroundDrawable(new ColorDrawable(Color.WHITE));
                dp.show();
            }
        });

        dpd = new DatePickerDialog.OnDateSetListener() {
            @Override
            public void onDateSet(DatePicker datePicker, int ano, int mes, int dia) {
                mes = mes + 1;

                String dataF = dia + "/" + mes + "/" + ano;
                botao.setText(dataF);
            }
        };

        pesquisar = view.findViewById(R.id.btPesquisar);

        pesquisar.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {

                String data = String.valueOf(botao.getText());

                RequestQueue solicitacao = Volley.newRequestQueue(getContext());
                String url = "http://10.0.2.2/api/Feedback/" + data + "/" + codigo;

                JsonArrayRequest envio = new JsonArrayRequest(Request.Method.GET,
                        url, null,
                        new Response.Listener<JSONArray>() {
                            @Override
                            public void onResponse(JSONArray response) {
                                if(response != null) {
                                    try {
                                        JSONObject ob = response.getJSONObject(0);

                                        tv1.setVisibility(View.VISIBLE);
                                        tv2.setVisibility(View.VISIBLE);
                                        qtdPassos.setVisibility(View.VISIBLE);
                                        recomendacao.setVisibility(View.VISIBLE);
                                        img.setVisibility(View.VISIBLE);

                                        if(ob.getInt("saude") == 1) {
                                            img.setImageResource(R.drawable.pessimo);
                                        } else if(ob.getInt("saude") == 2) {
                                            img.setImageResource(R.drawable.ruim);
                                        } else if(ob.getInt("saude") == 3) {
                                            img.setImageResource(R.drawable.medio);
                                        } else if(ob.getInt("saude") == 4) {
                                            img.setImageResource(R.drawable.bom);
                                        } else {
                                            img.setImageResource(R.drawable.otimo);
                                        }

                                        qtdPassos.setText(ob.getInt("qtdPassos"));
                                        recomendacao.setText(ob.getString("recomendacao"));

                                    } catch (JSONException e) {
                                        e.printStackTrace();
                                    }

                                } else {

                                    tv1.setVisibility(View.INVISIBLE);
                                    tv2.setVisibility(View.INVISIBLE);
                                    tv3.setVisibility(View.INVISIBLE);
                                    qtdPassos.setVisibility(View.INVISIBLE);
                                    recomendacao.setVisibility(View.INVISIBLE);
                                    img.setVisibility(View.INVISIBLE);

                                    Toast.makeText(getContext(), "Dados não Encontrados", Toast.LENGTH_LONG).show();
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
            }
        });

        return view;
    }
}