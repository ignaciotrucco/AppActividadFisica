window.onload = ListadoInformeEjerciciosFisicos();

function ListadoInformeEjerciciosFisicos() {
    let fechadesde = document.getElementById("FechaDesdeBuscar").value;
    let fechahasta = document.getElementById("FechaHastaBuscar").value;
    $.ajax({
        url: '../../EjerciciosFisicos/ListadoInformeEjerciciosFisicos',
        data: {
            FechaDesde: fechadesde,
            FechaHasta: fechahasta,
        },
        type: 'POST',
        dataType: 'json',
        success: function (informeEjerciciosFisicosMostrar) {

            let contenidoTabla = ``;

            $.each(informeEjerciciosFisicosMostrar, function (index, tipoEjercicio) {
                contenidoTabla += `
                <tr>
                    <td class="text-center">${tipoEjercicio.tipoEjercicioDescripcion}</td>
                    <td></td>
                    <td></td>
                    <td></td>
                    <td></td>
                    <td></td>
                    <td></td>
                    <td></td>
                    <td></td>
                </tr>`;

                $.each(tipoEjercicio.vistaEjercicioFisico, function (index, ejercicio) {
                    contenidoTabla += `
                <tr>
                    <td></td>
                    <td class="text-center">${ejercicio.fechaInicioString}</td>
                    <td class="text-center">${ejercicio.estadoEmocionalInicio}</td>
                    <td class="text-center">${ejercicio.fechaFinString}</td>
                    <td class="text-center">${ejercicio.estadoEmocionalFin}</td>
                    <td class="text-center">${ejercicio.intervaloEjercicio}</td>
                    <td class="text-center">${ejercicio.observaciones}</td>
                    <td class="text-center">${ejercicio.lugarNombre}</td>
                    <td class="text-center">${ejercicio.caloriasQuemadas}</td>
                </tr>`;
                });
            });
            document.getElementById("tbody-informe").innerHTML = contenidoTabla;
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
    document.getElementById("FechaDesdeBuscar").value = "";
    document.getElementById("FechaHastaBuscar").value = "";
    ListadoInformeEjerciciosFisicos();
}