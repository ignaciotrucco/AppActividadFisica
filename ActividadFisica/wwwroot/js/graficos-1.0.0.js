
window.onload = GraficoRedondo();

let graficoRedondo;
let graficoBarras;

function GraficoRedondo() {

    let buscarMesEjercicio = $("#BuscarMesEjercicio").val();
    let buscarAnioEjercicio = $("#BuscarAnioEjercicio").val();

    $.ajax({
        // la URL para la petición
        url: '../../PanelEjercicios/GraficoCircular',
        // la información a enviar
        // (también es posible utilizar una cadena de datos)
        data: {
            Mes: buscarMesEjercicio, Anio: buscarAnioEjercicio
        },
        // especifica si será una petición POST o GET
        type: 'POST',
        // código a ejecutar si la petición es satisfactoria;
        // la respuesta es pasada como argumento a la función
        success: function (vistaTipoEjercicios) {

            var labels = [];
            var data = [];

            $.each(vistaTipoEjercicios, function(index, vistaTipoEjercicio) {
                labels.push(vistaTipoEjercicio.descripcion);
                data.push(vistaTipoEjercicio.cantidadMinutos);
            });

            var ctx2 = document.getElementById("GraficoRedondo");
            graficoRedondo = new Chart(ctx2, {
                type: 'doughnut',
                data: {
                    labels: labels,
                    datasets: [{
                        data: data,
                        backgroundColor: [
                            'rgb(255, 99, 132)',
                            'rgb(54, 162, 235)',
                            'rgb(255, 205, 86)'
                        ],
                    }]
                },
            });

            GraficoBarraEjercicios()
        },

        error: function (xhr, status) {
            Swal.fire({
                icon: "error",
                title: "Oops...",
                text: "Algo salió mal",
            });
        }
    });
}

$("#TipoEjercicioID").change(function () {
    graficoBarras.destroy();
    GraficoBarraEjercicios();
})

$("#BuscarMesEjercicio, #BuscarAnioEjercicio").change(function () {
    graficoRedondo.destroy();
    graficoBarras.destroy();
    GraficoRedondo();
})

function GraficoBarraEjercicios() {

    let buscarTipoEjercicio = document.getElementById("TipoEjercicioID").value;
    let buscarMesEjercicio = document.getElementById("BuscarMesEjercicio").value;
    let buscarAnioEjercicio = document.getElementById("BuscarAnioEjercicio").value;

    $.ajax({
        // la URL para la petición
        url: '../../PanelEjercicios/GraficoBarraEjercicios',
        // la información a enviar
        // (también es posible utilizar una cadena de datos)
        data: {
            TipoEjercicioId: buscarTipoEjercicio, Mes: buscarMesEjercicio, Anio: buscarAnioEjercicio
        },
        // especifica si será una petición POST o GET
        type: 'POST',
        // el tipo de información que se espera de respuesta
        dataType: 'json',
        // código a ejecutar si la petición es satisfactoria;
        // la respuesta es pasada como argumento a la función
        success: function (ejerciciosPorDia) {

            let labels = [];
            let data = [];
            let diasConEjercicios = 0;
            let minutosTotales = 0;

            $.each(ejerciciosPorDia, function (index, ejercicioDia) {
                labels.push(ejercicioDia.dia + " DE " + ejercicioDia.mes);
                data.push(ejercicioDia.cantidadMinutos);

                minutosTotales += ejercicioDia.cantidadMinutos;

                if (ejercicioDia.cantidadMinutos > 0) {
                    diasConEjercicios += 1;
                }
            });

            //obtener el ID DEL SELECT
            var inputTipoEjercicioId = document.getElementById("TipoEjercicioID");

            //OBTENER EL TEXTO DE LA OPCION SELECCIONADA
            var ejerciciosNombre = inputTipoEjercicioId.options[inputTipoEjercicioId.selectedIndex].text;

            let diasSinEjercicios = ejerciciosPorDia.lenght - diasConEjercicios;
            // document.getElementById("").text(minutosTotales + " MINUTOS EN " + diasConEjercicios + " DÍAS");
            // document.getElementById("").text(diasSinEjercicios + " DÍAS SIN " + ejerciciosNombre);

            //INICIO GRAFICO DE BARRAS
            const ctx = document.getElementById('GraficoBarras');
            graficoBarras = new Chart(ctx, {
                type: 'bar',
                data: {
                    labels: labels,
                    datasets: [{
                        label: 'Cantidad de minutos',
                        data: data,
                        borderWidth: 2
                    }]
                },
                options: {
                    scales: {
                        y: {
                            beginAtZero: true
                        }
                    }
                }
            });
            //FIN GRAFICO DE BARRAS

        },

        error: function (xhr, status) {
            Swal.fire({
                icon: "error",
                title: "Oops...",
                text: "Algo salió mal",
            });
        }
    });
}