const pegarData = document.querySelector('.date-picker');
const selecionarData = document.querySelector('.date-picker .selected-date');
const elemento_data = document.querySelector('.date-picker .dates');
const elemento_mes = document.querySelector('.date-picker .dates .month .mth');
const proximo_mes = document.querySelector('.date-picker .dates .month .next-mth');
const mes_anterior = document.querySelector('.date-picker .dates .month .prev-mth');
const elemento_dias = document.querySelector('.date-picker .dates .days');

const meses = ['Janeiro', 'Fevereiro', 'Março', 'Abril', 'Maio', 'Junho', 'Julho', 'Agosto', 'Setembro', 'Outubro', 'Novembro', 'Dezembro'];

let data = new Date();
let dia = data.getDate();
let mes = data.getMonth() + 1;
let ano = data.getFullYear();

let dataSelecionada = data;
let diaSelecionado = dia;
let mesSelecionado = mes;
let anoSelecionado = ano;

elemento_mes.textContent = meses[mes - 1] + ' ' + ano;

selecionarData.textContent = `${addZero(dia)}/${addZero(mes)}/${ano}`;
selecionarData.dataset.value = dataSelecionada;

carregarData();

selecionarData.addEventListener('click', clicou);
proximo_mes.addEventListener('click', proximoMes);
mes_anterior.addEventListener('click', mesAnterior);

function clicou(e) {
	if (elemento_data.classList.toggle == 'active') {

		elemento_data.classList.toggle('disable');

	} else {

		elemento_data.classList.toggle('active');
	}
}

function proximoMes(e) {
	mes++;
	if (mes > 12) {
		mes = 1;
		ano++;
	}
	elemento_mes.textContent = meses[mes - 1] + ' ' + ano;
	carregarData();
}

function mesAnterior(e) {
	mes--;
	if (mes < 1) {
		mes = 12;
		ano--;
	}
	elemento_mes.textContent = meses[mes - 1] + ' ' + ano;
	carregarData();
}

function carregarData(e) {
	elemento_dias.innerHTML = '';
	let total_dias = 31;

	if (mes == 2) {
		if (ano % 400 == 0 || (ano % 4 == 0 && ano % 100 != 0)) {
			total_dias = 29;
		} else {
			total_dias = 28;
		}
	} else if (mes == 4 || mes == 6 || mes == 9 || mes == 11) {
		total_dias = 30;
	}

	for (let i = 0; i < total_dias; i++) {
		const elemento_dia = document.createElement('div');
		elemento_dia.classList.add('day');
		elemento_dia.textContent = i + 1;

		if (diaSelecionado == (i + 1) && anoSelecionado == ano && mesSelecionado == mes) {
			elemento_dia.classList.add('selected');
		}

		elemento_dia.addEventListener('click', function () {

			diaSelecionado = (i + 1);
			mesSelecionado = mes;
			anoSelecionado = ano;

			let data = `${addZero(i + 1)}/${addZero(mes)}/${ano}`;

			selecionarData.textContent = data;
			selecionarData.dataset.value = data;

			carregarData();
		});

		elemento_dias.appendChild(elemento_dia);
	}
}

function addZero(d) {

	let string = d < 10 ? "0" + d : d;

	return string

}

function enviar() {

	document.getElementById("d").value = `${diaSelecionado}/${mesSelecionado}/${anoSelecionado}`

}