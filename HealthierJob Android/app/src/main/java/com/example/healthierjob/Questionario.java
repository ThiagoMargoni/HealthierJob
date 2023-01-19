package com.example.healthierjob;

public class Questionario {
    int id_perg;
    String pergunta;

    public Questionario(int id_perg, String pergunta) {
        this.id_perg = id_perg;
        this.pergunta = pergunta;
    }

    public int getId_perg() {
        return id_perg;
    }

    public void setId_perg(int id_perg) {
        this.id_perg = id_perg;
    }

    public String getPergunta() {
        return pergunta;
    }

    public void setPergunta(String pergunta) {
        this.pergunta = pergunta;
    }
}
