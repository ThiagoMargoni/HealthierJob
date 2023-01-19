package com.example.healthierjob.ui;

import android.content.Intent;
import android.content.SharedPreferences;
import android.os.Bundle;
import android.preference.PreferenceManager;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.AdapterView;
import android.widget.ArrayAdapter;
import android.widget.AutoCompleteTextView;
import android.widget.Button;
import android.widget.DatePicker;
import android.widget.EditText;
import android.widget.ImageView;
import android.widget.ListView;
import android.widget.TextView;
import android.widget.Toast;

import androidx.fragment.app.Fragment;

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

import java.util.ArrayList;
import java.util.Calendar;
import java.util.Date;
import java.util.List;

/**
 * A simple {@link Fragment} subclass.
 * Use the {@link FragmentQuestionario#newInstance} factory method to
 * create an instance of this fragment.
 */
public class FragmentQuestionario extends Fragment{

    View view;

    // TODO: Rename parameter arguments, choose names that match
    // the fragment initialization parameters, e.g. ARG_ITEM_NUMBER
    private static final String ARG_PARAM1 = "param1";
    private static final String ARG_PARAM2 = "param2";

    // TODO: Rename and change types of parameters
    private String mParam1;
    private String mParam2;

    public FragmentQuestionario() {
        // Required empty public constructor
    }

    /**
     * Use this factory method to create a new instance of
     * this fragment using the provided parameters.
     *
     * @param param1 Parameter 1.
     * @param param2 Parameter 2.
     * @return A new instance of fragment FragmentQuestionario.
     */
    // TODO: Rename and change types and number of parameters
    public static FragmentQuestionario newInstance(String param1, String param2) {
        FragmentQuestionario fragment = new FragmentQuestionario();
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

    Button btEnviar;
    String[] strings = new String[]{};
    ArrayAdapter<String> adapter;
    AutoCompleteTextView autoC;
    TextView listV;
    TextView perg;
    EditText qtdPassos;
    ImageView pessimo, ruim, medio, bom, otimo;
    List<String> q = new ArrayList<>();
    int pergAtual = 0, media = 0;

    @Override
    public View onCreateView(LayoutInflater inflater, ViewGroup container,
                             Bundle savedInstanceState) {
        // Inflate the layout for this fragment
        view = inflater.inflate(R.layout.fragment_questionario, container, false);

        pessimo = view.findViewById(R.id.pessimo);
        ruim = view.findViewById(R.id.ruim);
        medio = view.findViewById(R.id.medio);
        bom = view.findViewById(R.id.bom);
        otimo = view.findViewById(R.id.otimo);
        perg = view.findViewById(R.id.perg);

        pessimo.setImageResource(R.drawable.pessimo);
        ruim.setImageResource(R.drawable.ruim);
        medio.setImageResource(R.drawable.medio);
        bom.setImageResource(R.drawable.bom);
        otimo.setImageResource(R.drawable.otimo);

        autoC = view.findViewById(R.id.autoC);
        listV = view.findViewById(R.id.listV);
        btEnviar = view.findViewById(R.id.button);
        qtdPassos = view.findViewById(R.id.qtd);

        autoC.setVisibility(View.INVISIBLE);
        listV.setVisibility(View.INVISIBLE);
        btEnviar.setVisibility(View.INVISIBLE);
        qtdPassos.setVisibility(View.INVISIBLE);

        pessimo.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                media += 1;
                pergAtual++;
                carregarPergunta(q);
            }
        });

        ruim.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                media += 2;
                pergAtual++;
                carregarPergunta(q);
            }
        });

        medio.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                media += 3;
                pergAtual++;
                carregarPergunta(q);
            }
        });
        bom.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                media += 4;
                pergAtual++;
                carregarPergunta(q);
            }
        });

        otimo.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                media += 5;
                pergAtual++;
                carregarPergunta(q);
            }
        });

        String data = String.valueOf(Calendar. getInstance(). getTime());

        SharedPreferences prefs = PreferenceManager.getDefaultSharedPreferences(view.getContext());
        String codigo = prefs.getString("codigo", "");

        RequestQueue solicitacao1 = Volley.newRequestQueue(getContext());
        String url1 = "http://10.0.2.2/api/Feedback/" + data + "/" + codigo;

        JsonArrayRequest envio1 = new JsonArrayRequest(Request.Method.GET,
                url1, null,
                new Response.Listener<JSONArray>() {
                    @Override
                    public void onResponse(JSONArray response1) {
                        if(response1 == null) {

                            RequestQueue solicitacao2 = Volley.newRequestQueue(getContext());
                            String url2 = "http://10.0.2.2/api/Questionario/" ;

                            JsonArrayRequest envio2 = new JsonArrayRequest(Request.Method.GET,
                                    url2, null,
                                    new Response.Listener<JSONArray>() {
                                        @Override
                                        public void onResponse(JSONArray response2) {
                                            if(response2 != null) {

                                                for(int i = 0; i < response2.length(); i++) {

                                                    try {
                                                        JSONObject ob = response2.getJSONObject(i);

                                                        q.add( ob.getString("pergunta"));

                                                    } catch(JSONException e) {
                                                        e.printStackTrace();
                                                    }

                                                }

                                                carregarPergunta(q);

                                            } else {
                                                Toast.makeText(getContext(), "Não há Perguntas. Contate o administrador!", Toast.LENGTH_LONG).show();

                                                Intent it = new Intent(getContext(), MainActivity.class);

                                                startActivity(it);
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

                            solicitacao2.add(envio2);

                        } else {
                            Intent it = new Intent(getContext(), MainActivity.class);

                            startActivity(it);

                            Toast.makeText(getContext(), "Você Já Respondeu seu Questionário Hoje", Toast.LENGTH_LONG).show();
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

        solicitacao1.add(envio1);

        RequestQueue solicitacao3 = Volley.newRequestQueue(getContext());
        String url3 = "http://10.0.2.2/api/Sintomas/" ;

        JsonArrayRequest envio3 = new JsonArrayRequest(Request.Method.GET,
                url3, null,
                new Response.Listener<JSONArray>() {
                    @Override
                    public void onResponse(JSONArray response3) {
                        if(response3 != null) {

                            for(int i = 0; i < response3.length(); i++) {

                                try {
                                    JSONObject ob = response3.getJSONObject(i);

                                    String s = ob.getString("nomeSint");

                                    strings[i] = s;

                                } catch(JSONException e) {
                                    e.printStackTrace();
                                }

                            }

                            adapter = new ArrayAdapter<>(getContext(), androidx.appcompat.R.layout.support_simple_spinner_dropdown_item, strings);
                            autoC.setAdapter(adapter);

                            autoC.setOnItemClickListener(new AdapterView.OnItemClickListener() {
                                @Override
                                public void onItemClick(AdapterView<?> adapterView, View view, int i, long l) {
                                    String texto = (String) listV.getText();

                                    texto += adapter.getItem(i) + ";\n";

                                    listV.setText(texto);

                                    autoC.setText("");
                                }
                            });

                        } else {
                            Intent it = new Intent(getContext(), MainActivity.class);

                            startActivity(it);

                            Toast.makeText(getContext(), "Não há Sintomas. Contate o Amdminstrador!", Toast.LENGTH_LONG).show();
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

        solicitacao3.add(envio3);

        btEnviar.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {

                if(listV.getText() != null && listV.getText() != "" && qtdPassos.getText() != null) {

                    String sintomas = (String) listV.getText();

                    List<String> s = new ArrayList<>();

                    s.set(0, sintomas);
                    SharedPreferences prefs = PreferenceManager.getDefaultSharedPreferences(view.getContext());
                    String codigoUsuario = prefs.getString("codigo", "");

                    RequestQueue solicitacao4 = Volley.newRequestQueue(getContext());
                    String url4 = "http://10.0.2.2/api/Sintomas/" + s + "/" + codigoUsuario + "/" + "1000/10/10"; //data qualquer

                    JsonArrayRequest envio4 = new JsonArrayRequest(Request.Method.POST,
                            url4, null,
                            new Response.Listener<JSONArray>() {
                                @Override
                                public void onResponse(JSONArray response4) {
                                    if(response4 == null) {
                                        Toast.makeText(getContext(), "Erro", Toast.LENGTH_LONG).show();
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

                    solicitacao4.add(envio4);

                    RequestQueue solicitacao5 = Volley.newRequestQueue(getContext());
                    String url5 = "http://10.0.2.2/api/Sintomas/" + s + "/" + codigoUsuario + "/" + "1000/10/10";

                    JSONObject enviarDados = new JSONObject();
                    try{
                        enviarDados.put("codigoFunc", codigoUsuario);
                        enviarDados.put("id", 0);
                        enviarDados.put("codDoenca", 0);
                        enviarDados.put("qtdPassos", Integer.parseInt(qtdPassos.getText().toString()));
                        enviarDados.put("data", "1100/10/11");
                        enviarDados.put("saude", media);
                        enviarDados.put("recomendacao", sintomas);
                    } catch(JSONException jExc){
                        jExc.printStackTrace();
                    }

                    JsonObjectRequest envio5 = new JsonObjectRequest(Request.Method.POST,
                            url5, enviarDados,
                            new Response.Listener<JSONObject>() {
                                @Override
                                public void onResponse(JSONObject response) {
                                    Intent it = new Intent(getContext(), MainActivity.class);

                                    startActivity(it);

                                    Toast.makeText(getContext(), "Feedback Gerado com Sucesso. Você pode conferir selecionando a data de hoje no Calendário!", Toast.LENGTH_LONG).show();
                                }
                            }, new Response.ErrorListener() {
                        @Override
                        public void onErrorResponse(VolleyError error) {
                            error.printStackTrace();
                        }
                    }
                    );

                    solicitacao5.add(envio5);

                } else {
                    Toast.makeText(getContext(), "Favor Preencher todos os Campos", Toast.LENGTH_LONG).show();
                }
            }
        });

        return view;
    }

    void carregarPergunta(List<String> que) {

        if(pergAtual < que.size()) {
            List<String> lista = que;

            perg.setText(lista.get(pergAtual));
        } else {
            media = media/q.size();

            responderSintomas();
        }
    }

    void responderSintomas() {
        pessimo.setVisibility(View.INVISIBLE);
        ruim.setVisibility(View.INVISIBLE);
        medio.setVisibility(View.INVISIBLE);
        bom.setVisibility(View.INVISIBLE);
        otimo.setVisibility(View.INVISIBLE);
        perg.setVisibility(View.INVISIBLE);

        autoC.setVisibility(View.VISIBLE);
        listV.setVisibility(View.VISIBLE);
        btEnviar.setVisibility(View.VISIBLE);
        qtdPassos.setVisibility(View.VISIBLE);
    }
}