var nomeSintomas = [];
var listaSintomas = [];

const response = fetch('https://localhost:7025/api/Api/', {
    method: 'GET'
})
    .then(data => data.json())
    .then(data => {
        console.log(data)
        data.forEach(element => {
            nomeSintomas.push(element["nomeSintoma"]);
        });
    })
    .catch((error) => console.log('ERRO!! ' + error.status));

$(function () {
    $("#tags").autocomplete({
        source: nomeSintomas,
        select: function (event, ui) {

            let texto = ui.item.value;

            const index = nomeSintomas.indexOf(ui.item.value)

            nomeSintomas.splice(index, 1)
            listaSintomas.push(texto)

            document.getElementById("box").innerHTML += ``;
        }
    });
});

function clicou(object, texto) {

    object.style.display = 'none'
    const index = listaSintomas.indexOf(texto)

    nomeSintomas.push(texto)

    listaSintomas.splice(index, 1)
}

function desfocou() {
    document.getElementById("tags").value = "";
    document.getElementById("tags").focus();
}

function enviar() {
    document.getElementById("lista").innerHTML += `<input name="sintomas" value="${listaSintomas}" type="hidden" />`;
}

function enviar2() {
    listaSintomas = [];
    listaSintomas.push("Nada");
    document.getElementById("lista").innerHTML += `<input name="sintomas" value="${listaSintomas}" type="hidden" />`;
}