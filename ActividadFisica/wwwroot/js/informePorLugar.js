window.onload = ListadoInformeEjerciciosFisicosPorLugar();

function ListadoInformeEjerciciosFisicosPorLugar() {
    let fechadesde = document.getElementById("FechaDesdeBuscar").value;
    let fechahasta = document.getElementById("FechaHastaBuscar").value;
    $.ajax({
        url: '../../EjerciciosFisicos/ListadoEjerciciosPorLugar',
        data: {
            FechaDesde: fechadesde,
            FechaHasta: fechahasta,
        },
        type: 'POST',
        dataType: 'json',
        success: function (vistaLugar) {

            let contenidoTabla = ``;

            $.each(vistaLugar, function (index, lugar) {
                contenidoTabla += `
                <tr>
                    <td class="text-center">${lugar.nombre}</td>
                    <td></td>
                    <td></td>
                    <td></td>
                    <td></td>
                    <td></td>
                </tr>`;

                $.each(lugar.vistaEjercicios, function (index, ejercicio) {
                    contenidoTabla += `
                <tr>
                    <td></td>
                    <td class="text-center">${ejercicio.fechaInicioString}</td>
                    <td class="text-center">${ejercicio.fechaFinString}</td>
                    <td class="text-center">${ejercicio.intervaloEjercicio}</td>
                    <td class="text-center">${ejercicio.tipoEjercicioDescripcion}</td>
                    <td class="text-center">${ejercicio.observaciones}</td>
                </tr>`;
                });
            });
            document.getElementById("tbody-informePorLugar").innerHTML = contenidoTabla;
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

function LimpiarFiltros() {
    $("#FechaDesdeBuscar").val("")
    $("#FechaHastaBuscar").val("")
    ListadoInformeEjerciciosFisicosPorLugar()
}
