window.onload = InformeGeneral();

function InformeGeneral() {

    $.ajax({
        url: '../../InformeGeneral/ListadoInformeGeneral',
        data: {},
        type: 'POST',
        dataType: 'json',
        success: function (EventoVista) {

            let contenidoTabla = ``;

            $.each(EventoVista, function (index, evento) {
                contenidoTabla += `
                <tr>
                    <td>${evento.nombre}</td>
                    <td colspan="8"></td>
                </tr>`;


                $.each(evento.vistaLugar, function (index, lugar) {
                    contenidoTabla += `
                    <tr>
                        <td></td>
                        <td>${lugar.nombre}</td>
                        <td colspan="7"></td>
                    </tr>`;


                    $.each(lugar.vistaTipoEjercicio, function (index, ejercicio) {
                        contenidoTabla += `
                        <tr>
                            <td colspan="2"></td>
                            <td>${ejercicio.descripcion}</td>
                            <td colspan="6"></td>
                        </tr>`;


                        $.each(ejercicio.vistaEjercicioFisico, function (index, ejercicioFisico) {
                            contenidoTabla += `
                <tr>
                    <td colspan="3"></td>
                    <td class="text-center">${ejercicioFisico.fechaInicioString}</td>
                    <td class="text-center">${ejercicioFisico.fechaFinString}</td>
                    <td class="text-center">${ejercicioFisico.intervaloEjercicio}</td>
                    <td class="text-center">${ejercicioFisico.observaciones}</td>
                    <td class="text-center">${ejercicioFisico.estadoEmocionalInicio}</td>
                    <td class="text-center">${ejercicioFisico.estadoEmocionalFin}</td>
                </tr>`;
                        });
                    });
                });
            });
            document.getElementById("tbody-informeGeneral").innerHTML = contenidoTabla;
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