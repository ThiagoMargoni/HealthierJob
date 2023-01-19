var sintDoenca = [];
var nomeSintomas = [];
var listaSintomas = [];

let id = document.getElementById("id").value;
let index = 0;

const resposta = fetch(`https://localhost:7025/api/Api/${id}/`, {
    method: 'GET'
})
    .then(data => data.json())
    .then(data => {
        data.forEach(element => {
            sintDoenca.push(element);
            listaSintomas.push(element);

            document.getElementById("teste").innerHTML += `<div class="chip"> ${element} <span class="closebtn" onclick="clicou(this.parentElement,'${element}')">&times;</span> </div>`;
        });
    })
    .catch((error) => console.log('ERRO!! ' + error.status));

const response = fetch('https://localhost:7025/api/Api/', {
    method: 'GET'
})
    .then(data => data.json())
    .then(data => {
        data.forEach(element => {
            if (!(sintDoenca[index] == element["nomeSintoma"])) {
                nomeSintomas.push(element["nomeSintoma"]);
            }

            index++;
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

            document.getElementById("teste").innerHTML += `<div class="chip"> ${texto} <span class="closebtn" onclick="clicou(this.parentElement,'${texto}')">&times;</span> </div>`;
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
