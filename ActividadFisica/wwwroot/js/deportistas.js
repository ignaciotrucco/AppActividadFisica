window.onload = ListadoDeportistas();

function ListadoDeportistas() {
    $.ajax({
        url: '../../Deportistas/ListadoDeportistas',
        data: {
        },
        type: 'POST',
        dataType: 'json',
        success: function (vistaPersonas) {

            let contenidoTabla = ``;

            $.each(vistaPersonas, function (index, persona) {
                contenidoTabla += `
                <tr>
                    <td class="text-center">${persona.nombreCompleto}</td>
                    <td class="text-center">${persona.fechaNacimientoString}</td>
                    <td class="text-center">${persona.generoString}</td>
                    <td class="text-center">${persona.peso}</td>
                    <td class="text-center">${persona.altura}</td>
                    <td class="text-center">${persona.email}</td>
                </tr>
                `;
            });
            document.getElementById("tbody-deportistas").innerHTML = contenidoTabla;
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