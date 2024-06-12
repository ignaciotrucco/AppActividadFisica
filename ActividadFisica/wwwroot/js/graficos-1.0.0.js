
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
            var fondo = [];

            $.each(vistaTipoEjercicios, function(index, vistaTipoEjercicio) {
                labels.push(vistaTipoEjercicio.descripcion);
                var color = GenerarColorRojo();
                fondo.push(color);
                data.push(vistaTipoEjercicio.cantidadMinutos);
            });

            var ctx2 = document.getElementById("GraficoRedondo");
            graficoRedondo = new Chart(ctx2, {
                type: 'doughnut',
                data: {
                    labels: labels,
                    datasets: [{
                        data: data,
                        backgroundColor: fondo,
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

            let diasSinEjercicios = ejerciciosPorDia.length - diasConEjercicios;
            $("#textoTotalEjercicios").html(minutosTotales + " MINUTOS EN " + diasConEjercicios + " DÍAS " + '       <i class="fa-solid fa-square-check"></i>');
            $("#textoSinEjercicios").html(diasSinEjercicios + " DÍAS SIN " + ejerciciosNombre + '         <i class="fa-solid fa-xmark"></i>');

            //INICIO GRAFICO DE BARRAS
            const ctx = document.getElementById('GraficoBarras');
            graficoBarras = new Chart(ctx, {
                type: 'line',
                data: {
                    labels: labels,
                    datasets: [{
                        label: 'CANTIDAD DE MINUTOS',
                        data: data,
                        borderWidth: 2,
                        borderColor: "rgb(255, 0, 0)",
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

function GenerarColorRojo() {
    // El valor de RR será alto (de 128 a 255) para garantizar que predomine el rojo.
    // Los valores de GG y BB serán muy bajos (de 0 a 63).

    let rr = Math.floor(Math.random() * 128) + 128; // 128 a 255
    let gg = Math.floor(Math.random() * 64); // 0 a 63
    let bb = Math.floor(Math.random() * 64); // 0 a 63

    // Convertimos a hexadecimal y formateamos para que tenga siempre dos dígitos.
    let colorHex = `#${rr.toString(16).padStart(2, '0')}${gg.toString(16).padStart(2, '0')}${bb.toString(16).padStart(2, '0')}`;
    return colorHex;
}

